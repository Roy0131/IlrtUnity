namespace ClientLogic
{
    public class LoginStage : BaseStage
    {
        public LoginStage()
            : base(StageType.Login)
        {
            _sceneName = "LoginScene";
        }

        protected override void LoadSceneFinish()
        {
            base.LoadSceneFinish();
            GameUIMgr.Instance.OpenModule(ModuleID.Login);
        }
    }
}
