using UnityEngine;
using UnityEngine.UI;

namespace ClientLogic
{
    public class GameEntry
    {
        private static GameEntry _instance;
        private static GameEntry Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameEntry();
                return _instance;
            }
        }
        public GameEntry()
        {
            _instance = this;
            Debug.Log("[GameEntry.Constructor..]");
        }

        private void Init()
        {
            GameConfigMgr.Instance.Init();
            ActiveStageConfig config = GameConfigMgr.Instance.GetStageConfig(1);
            Debug.Log("stageID:" + config.StageID);

            GameObject obj = GameObject.Find("UIRoot");
            if (obj == null)
            {
                Debug.LogError("can't find uiRoot");
            }
            else
            {
                Debug.Log("childCount:" + obj.transform.childCount);
            }

            Transform uiRoot = obj.transform.Find("Canvas/UIModuleRoot");
            GameObject uiHomeObj = GameResMgr.Instance.LoadUIPrefab("uiHome");
            uiHomeObj.transform.SetParent(uiRoot, false);

            Button btn = uiHomeObj.transform.Find("Button").GetComponent<Button>();
            if (btn == null)
            {
                Debug.Log("button not found!!!");
            }
            else
            {
                Debug.Log("button founded");
            }
            btn.onClick.Add(OnClick);
        }


        private void OnClick()
        {
            Debug.Log("button clicked");
        }

        public static void RunGame()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            Debug.Log("GameEntry.RunGame() => start run game!!!");

            Instance.Init();

            sw.Stop();

            Debug.LogFormat("time = {0}ms", sw.ElapsedMilliseconds);
        }
    }
}