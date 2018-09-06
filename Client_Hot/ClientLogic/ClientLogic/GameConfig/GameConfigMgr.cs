using System;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;


namespace ClientLogic
{
    public class GameConfigMgr
    {
        private static GameConfigMgr _instance;
        public static GameConfigMgr Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameConfigMgr();
                return _instance;
            }
        }

        private Dictionary<string, Type> _dictAllCfgType = new Dictionary<string, Type>();
        public void Init()
        {
            RegConfig<ActiveStageConfig>();

            foreach (KeyValuePair<string, Type> kv in _dictAllCfgType)
            {
                try
                {
                    string xmlName = kv.Key.ToString();
                    XmlDocument doc = GameResMgr.Instance.LoadXml(xmlName);
                    if (doc == null)
                    {
                        Debug.LogError("[Config name:" + xmlName + " can't find!!!]");
                        continue;
                    }
                    XmlNode node = doc.SelectSingleNode("Config");
                    kv.Value.GetMethod("Parse").Invoke(null, new object[] { node });
                }
                catch (Exception ex)
                {
                    Debug.LogError("[parse configs error, ex:" + ex + "]");
                }
            }
        }

        private void RegConfig<T>()
        {
            Type t = typeof(T);
            _dictAllCfgType.Add(t.GetField("urlKey").GetValue(null) as string, t);
        }

        public ActiveStageConfig GetStageConfig(int id)
        {
            return ActiveStageConfig.Get(id);
        }
    }
}
