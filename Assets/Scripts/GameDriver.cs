using ILRuntime.Runtime.Enviorment;
using System.Collections;
using UnityEngine;

public class GameDriver : MonoBehaviour
{
    public static GameDriver Instance;

    public AppDomain mAppDomain { get; private set; }
    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 30;
        DontDestroyOnLoad(gameObject);
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
            //mAppDomain.LoadAssembly(msDll, null, null);
            using (System.IO.MemoryStream pdbDll = new System.IO.MemoryStream(pdbBytes))
            {
                mAppDomain.LoadAssembly(msDll, pdbDll, new Mono.Cecil.Pdb.PdbReaderProvider());
            }
        }
        InitILRunTime();
        yield return new WaitForSeconds(0.5f);
        OnRunGame();
    }

    private void InitILRunTime()
    {
        //mAppDomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
        ILRuntime.Runtime.Generated.CLRBindings.Initialize(mAppDomain);

        //mAppDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((action) =>
        //{
        //    return new UnityEngine.Events.UnityAction(() =>
        //    {
        //        ((System.Action)action).Invoke();
        //    });
        //});

        LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(mAppDomain);
    }

    private void OnRunGame()
    {
        mAppDomain.Invoke("ClientLogic.GameEntry", "RunGame", null, null);
    }
}
