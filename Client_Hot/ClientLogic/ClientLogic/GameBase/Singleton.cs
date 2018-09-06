﻿
namespace ClientLogic
{
    public class Singleton<T> where T : new()
    {
        private static T _instance;
        protected bool _blInited = false;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new T();
                return _instance;
            }
        }
    }
}