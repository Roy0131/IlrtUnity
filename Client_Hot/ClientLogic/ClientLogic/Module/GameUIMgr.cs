using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ClientLogic
{
    public class GameUIMgr : Singleton<GameUIMgr>
    {
        private const int ScreenWidth = 1334;
        private const int ScreenHeight = 750;

        private RectTransform _uiModuleRoot;
        private RectTransform _uiWindowRoot;
        private RectTransform _uiPopupRoot;

        private Canvas _uiCanvas;
        public Camera mUICamera { get; private set; }

        private Dictionary<ModuleID, ModuleBase> _dictAllModule;
        private Dictionary<UILayer, ModuleBase> _dictShowModules;
        private Dictionary<UILayer, List<ModuleID>> _dictHideStackModules;


        public void Init()
        {
            Transform uiRoot = GameObject.Find("UIRoot").transform;
            if (uiRoot == null)
            {
                Logger.LogError("[GameUIMgr.Init() => can't find uiroot, init uimanage failed]");
                return;
            }

            _uiCanvas = ObjectHelper.Find<Canvas>("Canvas", uiRoot);
            mUICamera = ObjectHelper.Find<Camera>("UICamera", uiRoot);

            _dictAllModule = new Dictionary<ModuleID, ModuleBase>();
            _dictShowModules = new Dictionary<UILayer, ModuleBase>();
            _dictHideStackModules = new Dictionary<UILayer, List<ModuleID>>();

            _uiModuleRoot = ObjectHelper.Find<RectTransform>("Canvas/UIModuleRoot", uiRoot);
            _uiWindowRoot = ObjectHelper.Find<RectTransform>("Canvas/UIWindowRoot", uiRoot);
            _uiPopupRoot = ObjectHelper.Find<RectTransform>("Canvas/UIPopupRoot", uiRoot);
            RegistModule();

            ////Canvas Scaler
            int sw = Screen.width;
            int sh = Screen.height;
            float sr = sw / (float)sh;
            int rw = (int)(sr * ScreenHeight);
            int rh = ScreenHeight;
            var canvasScaler = _uiCanvas.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(rw, rh);
        }

        private void RegistModule()
        {
            _dictAllModule.Add(ModuleID.Login, new LoginModule());
            _dictAllModule.Add(ModuleID.Home, new HomeModule());
        }

        public void OpenModule(ModuleID id, params object[] args)
        {
            if (!_dictAllModule.ContainsKey(id))
            {
                Logger.LogError("[GameUIMgr.OpenModule() => open module:" + id + ", but this module unregisted!!!]");
                return;
            }

            ModuleBase module = _dictAllModule[id];
            if (_dictShowModules.ContainsKey(module.mLayer))
            {
                ModuleBase curModule = _dictShowModules[module.mLayer];
                if (curModule.mModuleID == id)
                {
                    curModule.Show(args);
                    return;
                }
                if (curModule.mBlStack)
                {
                    List<ModuleID> lst;
                    if (_dictHideStackModules.ContainsKey(curModule.mLayer))
                    {
                        lst = _dictHideStackModules[curModule.mLayer];
                    }
                    else
                    {
                        lst = new List<ModuleID>();
                        _dictHideStackModules.Add(curModule.mLayer, lst);
                    }
                    if (lst.IndexOf(curModule.mModuleID) == -1)
                        lst.Insert(0, curModule.mModuleID);
                }
                _dictShowModules[module.mLayer].Hide();
                _dictShowModules[module.mLayer] = module;
            }
            else
            {
                _dictShowModules.Add(module.mLayer, module);
            }
            module.Show(args);
        }

        public void CloseModule(ModuleID id, params object[] args)
        {
            if (!_dictAllModule.ContainsKey(id))
            {
                Logger.LogError("[GameUIMgr.CloseModule() => open module:" + id + ", but this module unregisted!!!]");
                return;
            }
            ModuleBase module = _dictAllModule[id];
            if (_dictShowModules.ContainsKey(module.mLayer))
                _dictShowModules.Remove(module.mLayer);
            module.Hide();
            if (module.mBlStack)
            {
                if (_dictHideStackModules.ContainsKey(module.mLayer))
                {
                    List<ModuleID> stacks = _dictHideStackModules[module.mLayer];
                    if (stacks.Count > 0)
                    {
                        id = stacks[0];
                        stacks.RemoveAt(0);
                        OpenModule(id);
                    }
                }
            }
        }

        public void CloseAllModule()
        {
            if (_dictShowModules.Count > 0)
            {
                Dictionary<UILayer, ModuleBase>.ValueCollection value = _dictShowModules.Values;
                foreach (ModuleBase moduleBase in value)
                    moduleBase.Hide();
                _dictShowModules.Clear();
            }
            _dictHideStackModules.Clear();
        }

        public void AddModuleToStage(ModuleBase module)
        {
            RectTransform parent = null;
            switch (module.mLayer)
            {
                case UILayer.Module:
                    parent = _uiModuleRoot;
                    break;
                case UILayer.Window:
                    parent = _uiWindowRoot;
                    break;
                case UILayer.Popup:
                    parent = _uiPopupRoot;
                    break;
            }
            ObjectHelper.AddChildToParent(module.mRectTransform, parent);
        }
    }
}
