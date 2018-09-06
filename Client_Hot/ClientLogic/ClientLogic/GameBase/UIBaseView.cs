using UnityEngine;
using System.Collections.Generic;

namespace ClientLogic
{
    public abstract class UIBaseView
    {
        public GameObject mDisplayObject { get; protected set; }
        public RectTransform mRectTransform { get; protected set; }
        public bool mBlShow { get; protected set; }
        protected List<UIBaseView> _childrenViews;
        public UIBaseView()
        {
            mBlShow = false;
            _childrenViews = new List<UIBaseView>();
        }

        protected void AddChildren(UIBaseView children)
        {
            if (_childrenViews == null)
                _childrenViews = new List<UIBaseView>();
            if (_childrenViews.Contains(children))
                return;
            _childrenViews.Add(children);
        }

        protected void RemoveChildren(UIBaseView children)
        {
            if (_childrenViews == null || !_childrenViews.Contains(children))
                return;
            _childrenViews.Remove(children);
        }

        public void SetDisplayObject(GameObject obj)
        {
            if (obj == null)
            {
                Logger.LogWarning("[UIBaseView.SetDisplayObject() => display object is null!!]");
                return;
            }
            mDisplayObject = obj;
            mRectTransform = mDisplayObject.transform as RectTransform;
            ParseComponent();
        }


        public virtual void Show(params object[] args)
        {
            if (mDisplayObject == null)
            {
                Logger.LogWarning("[UIBaseView.Show() => show ui, but displayobject is null]");
                return;
            }

            if (!mBlShow)
            {
                AddEvent();
                if (!mDisplayObject.activeSelf)
                    mDisplayObject.SetActive(true);
            }
            Refresh(args);
            mBlShow = true;
        }

        public virtual void Hide()
        {
            if (mDisplayObject == null)
                return;
            if (mBlShow)
            {
                RemoveEvent();
                if (mDisplayObject.activeSelf)
                    mDisplayObject.SetActive(false);
                mBlShow = false;
            }
        }

        protected virtual void DiposeChildren()
        {
            if (_childrenViews != null)
            {
                for (int i = 0; i < _childrenViews.Count; i++)
                    _childrenViews[i].Dispose();
                _childrenViews.Clear();
                _childrenViews = null;
            }
        }

        public virtual void Dispose()
        {
            DiposeChildren();
            RemoveEvent();
            DisposeGameObject();
            mBlShow = false;
            mDisplayObject = null;
            mRectTransform = null;
        }

        protected virtual void DisposeGameObject()
        {
            if (mDisplayObject != null)
                GameObject.Destroy(mDisplayObject);
        }

        protected GameObject Find(string name)
        {
            return ObjectHelper.Find(name, mRectTransform);
        }

        protected T Find<T>(string name) where T : Object
        {
            return ObjectHelper.Find<T>(name, mRectTransform);
        }

        protected T FindOnSelf<T>() where T : Object
        {
            return ObjectHelper.FindOnSelf<T>(mRectTransform);
        }

        protected virtual void ParseComponent() { }
        protected virtual void AddEvent() { }
        protected virtual void RemoveEvent() { }
        protected virtual void Refresh(params object[] args) { }
    }
}
