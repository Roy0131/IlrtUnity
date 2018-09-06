namespace ClientLogic
{
    public class HomeStage : BaseStage
    {
        public HomeStage()
            : base(StageType.Home)
        {
            _sceneName = "HomeScene";
        }

        protected override void LoadSceneFinish()
        {
            base.LoadSceneFinish();
            GameUIMgr.Instance.OpenModule(ModuleID.Home);
        }
    }
}
