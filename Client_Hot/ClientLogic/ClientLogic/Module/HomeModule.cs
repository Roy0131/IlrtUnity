using UnityEngine.UI;

namespace ClientLogic
{
    public class HomeModule : ModuleBase
    {
        public static string TmpAccount { get; set; } = ""; //demo测试，为了方便

        private Text _accoutText;
        public HomeModule()
            : base(ModuleID.Home, UILayer.Module)
        {
            _modelResName = UIModuleResName.UI_Home;
        }

        protected override void ParseComponent()
        {
            base.ParseComponent();
            _accoutText = Find<Text>("Account");

            _accoutText.text = TmpAccount;
        }
    }
}
