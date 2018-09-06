using UnityEngine;

namespace ClientLogic
{
    public class ModuleBase : UIBaseView
    {
        public ModuleID mModuleID { get; protected set; }
        public UILayer mLayer { get; protected set; }

        protected bool _blInited;
        protected string _modelResName;

        public bool mBlStack { get; protected set; }

        public ModuleBase(ModuleID id, UILayer layer)
        {
            mModuleID = id;
            mLayer = layer;
            mBlStack = true;
        }

        public override void Show(params object[] args)
        {
            if (!_blInited)
                LoadRes();
            if (_childrenViews.Count > 0)
            {
                for (int i = 0; i < _childrenViews.Count; i++)
                    _childrenViews[i].Show(args);
            }
            base.Show(args);
        }

        public override void Hide()
        {
            if (_childrenViews.Count > 0)
            {
                for (int i = 0; i < _childrenViews.Count; i++)
                    _childrenViews[i].Hide();
            }
            base.Hide();
        }

        protected void LoadRes()
        {
            _blInited = true;
            if (string.IsNullOrEmpty(_modelResName))
            {
                Logger.LogError("[ModuleBase.LoadRes() => module id:" + mModuleID + " prefabs name invalid!]");
                return;
            }
            GameObject obj = GameResMgr.Instance.LoadUIPrefab(_modelResName);
            SetDisplayObject(obj);
            GameUIMgr.Instance.AddModuleToStage(this);
        }

        public override void Dispose()
        {
            _blInited = false;
            if (_childrenViews != null)
            {
                for (int i = _childrenViews.Count - 1; i >= 0; i--)
                    _childrenViews[i].Dispose();
                _childrenViews.Clear();
                _childrenViews = null;
            }
            base.Dispose();
        }

        protected virtual void OnClose()
        {
            GameUIMgr.Instance.CloseModule(mModuleID);
        }
    }

}