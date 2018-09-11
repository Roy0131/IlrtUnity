namespace ClientLogic
{
    public class GameNetMgr : Singleton<GameNetMgr>
    {
        #region Server Url
        public static string GAME_LOGIC_URL;
        private static string _serverUrl;
        public static string LoginServer
        {
            get
            {
                return _serverUrl + "login";
            }
        }

        public static string SelectServerUrl
        {
            get
            {
                return _serverUrl + "select_server";
            }
        }
        #endregion

        private LoginServer _loginServer;
        public GameServer mGameServer { get; private set; }

        public void Init()
        {
            GAME_LOGIC_URL = "https://192.168.1.16:30100/client_msg";
            _serverUrl = "https://192.168.1.16:35000/";
            mGameServer = new GameServer();
            _loginServer = new LoginServer();
        }

        public void DoGameLogin(string account = "222", string password = "")
        {
            _loginServer.DoLogin(account, password);
        }

        public void DoSelectServer(string acc, string token, int sid)
        {
            _loginServer.DoSelectServer(acc, token, sid);
        }

        public void Update()
        {
            if (mGameServer != null)
                mGameServer.Update();
        }

        public void Dispose()
        {
            if (mGameServer != null)
            {
                mGameServer.Dispose();
                mGameServer = null;
            }
        }
    }
}
