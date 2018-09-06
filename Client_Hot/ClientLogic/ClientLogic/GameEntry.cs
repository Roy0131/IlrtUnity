namespace ClientLogic
{
    public class GameEntry
    {
        #region unity主工程调用入口
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
        }

        public static void RunGame()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            Logger.Log("GameEntry.RunGame() => start run game!!!");
            Instance.Init();

            sw.Stop();
            Logger.Log(string.Format("time = {0}ms", sw.ElapsedMilliseconds));
        }

        public static void FiexedUpdate()
        {

        }

        public static void Update()
        {
            GameStageMgr.Instance.Update();
        }

        public static void ApplicationQuit()
        {

        }
        #endregion

        private void Init()
        {
            GameConfigMgr.Instance.Init();
            GameUIMgr.Instance.Init();
            GameStageMgr.Instance.Init();
            GameStageMgr.Instance.ChangeStage(StageType.Login);
        }

    }
}