using ILRuntime.Runtime.Enviorment;
using System.Collections;
using UnityEngine;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;

public class GameDriver : MonoBehaviour
{
    public static GameDriver Instance;

    public AppDomain mAppDomain { get; private set; }
    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 30;
        DontDestroyOnLoad(gameObject);
        gameObject.AddComponent<Loom>();
    }

    private void Start()
    {
        StartCoroutine(LoadMainDLL());
    }

    private IEnumerator LoadMainDLL()
    {
        mAppDomain = new AppDomain();
        WWW w;
#if UNITY_ANDROID
        w = new WWW(Application.streamingAssetsPath + "/ClientLogic.dll");
#else
        w = new WWW("file:///" + Application.streamingAssetsPath + "/ClientLogic.dll");
#endif
        while (!w.isDone)
            yield return null;
        byte[] dllBytes = null;
        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.LogError(w.error);
        }
        else
        {
            dllBytes = w.bytes;
        }
        w.Dispose();

#if UNITY_ANDROID
        w = new WWW(Application.streamingAssetsPath + "/ClientLogic.pdb");
#else
        w = new WWW("file:///" + Application.streamingAssetsPath + "/ClientLogic.pdb");
#endif
        while (!w.isDone)
            yield return null;
        byte[] pdbBytes = w.bytes;

        using (System.IO.MemoryStream msDll = new System.IO.MemoryStream(dllBytes))
        {
            using (System.IO.MemoryStream pdbDll = new System.IO.MemoryStream(pdbBytes))
            {
                mAppDomain.LoadAssembly(msDll, pdbDll, new Mono.Cecil.Pdb.PdbReaderProvider());
            }
        }
        ILHelper.InitILRunTime(mAppDomain);
        yield return new WaitForSeconds(0.5f);
        OnRunGame();
    }

    private IMethod _upDateMethod;
    private IMethod _quitMethod;
    private void OnRunGame()
    {
        //预先获得IMethod，可以减低每次调用查找方法耗用的时间
        IType type = mAppDomain.LoadedTypes["ClientLogic.GameEntry"];
        _upDateMethod = type.GetMethod("Update", 0);
        _quitMethod = type.GetMethod("ApplicationQuit", 0);

        mAppDomain.Invoke("ClientLogic.GameEntry", "RunGame", null, null);
    }

    private void Update()
    {
        if (_upDateMethod != null)
            mAppDomain.Invoke(_upDateMethod, null, null);
    }

    private void OnApplicationQuit()
    {
        if (_quitMethod != null)
            mAppDomain.Invoke(_quitMethod, null, null);
    }
}
