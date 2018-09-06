using UnityEngine;

public class ObjectHelper
{
    public static GameObject Find(string name, Transform transform)
    {
        Transform tf = transform.Find(name);
        if (tf == null)
            return null;
        return tf.gameObject;
    }

    public static T Find<T>(string name, Transform transform) where T : UnityEngine.Object
    {
        GameObject obj = Find(name, transform);
        if (obj == null)
            return null;
        return obj.GetComponent<T>();
    }


    public static GameObject Find(string name, RectTransform transform)
    {
        Transform tf = transform.Find(name);
        if (tf == null)
            return null;
        return tf.gameObject;
    }

    public static T Find<T>(string name, RectTransform transform) where T : UnityEngine.Object
    {
        GameObject obj = Find(name, transform);
        if (obj == null)
            return null;
        return obj.GetComponent<T>();
    }

    public static T FindOnSelf<T>(RectTransform transform) where T : UnityEngine.Object
    {
        return transform.GetComponent<T>();
    }

    public static void DestoryObject(GameObject obj)
    {
        GameObject.Destroy(obj);
    }

    public static void AddChildToParent(RectTransform child, RectTransform parent, bool blResetPos = false)
    {
        Vector2 achoredPos = blResetPos ? Vector2.zero : child.anchoredPosition;
        Vector2 sizeDelta = child.sizeDelta;
        Vector2 scale = child.localScale;
        child.SetParent(parent, false);
        child.localScale = scale;
        child.anchoredPosition = achoredPos;
        child.sizeDelta = sizeDelta;
    }
}
