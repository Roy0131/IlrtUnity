using System.Xml;
using UnityEngine;
public class GameResMgr
{
    private static GameResMgr _instance;
    public static GameResMgr Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameResMgr();
            return _instance;
        }
    }

    public XmlDocument LoadXml(string fileName)
    {
        TextAsset ta = null;
        ta = Resources.Load<TextAsset>("config/" + fileName);
        if (ta == null)
            return null;
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(ta.text);
        return doc;
    }

    private GameObject InstanceObject(GameObject original)
    {
        return GameObject.Instantiate<GameObject>(original);
    }

    public GameObject LoadUIPrefab(string name)
    {
        GameObject originalObj = Resources.Load<GameObject>("ui/" + name);
        if (originalObj == null)
            return null;
        return InstanceObject(originalObj);
    }
}