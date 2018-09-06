using UnityEngine.UI;

namespace ClientLogic
{
    public class LoginModule : ModuleBase
    {
        private InputField _accoutInput;
        private Button _loginBtn;

        public LoginModule()
            : base(ModuleID.Login, UILayer.Module)
        {
            _modelResName = UIModuleResName.UI_Login;
        }

        protected override void ParseComponent()
        {
            base.ParseComponent();
            _accoutInput = Find<InputField>("AccoutInput");
            _loginBtn = Find<Button>("LoginBtn");

            _loginBtn.onClick.Add(OnLogin);
        }

        private void OnLogin()
        {
            if (string.IsNullOrWhiteSpace(_accoutInput.text))
            {
                Logger.LogError("[LoginModule.OnLogin() => Accout invailde!!!]");
                return;
            }
            HomeModule.TmpAccount = _accoutInput.text;
            GameStageMgr.Instance.ChangeStage(StageType.Home);
        }
    }
}
