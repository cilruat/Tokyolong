using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

public class Any
{
	static public void SetStatic (GameObject obj)
	{
		SetStatic (obj.transform);
	}

	static public void SetStatic (Transform transform)
	{
		transform.gameObject.isStatic = true;
		foreach (Transform tr in transform)
			SetStatic (tr);
	}

	static public void SetLayer (MonoBehaviour parent, int layer)
	{
		SetLayer(parent.transform, layer);
	}

	static public void SetLayer (GameObject parent, int layer)
	{
		SetLayer (parent.transform, layer);
	}

	static public void SetLayer (Transform parent, int layer)
	{
		parent.gameObject.layer = layer;
		foreach (Transform tr in parent)
			SetLayer (tr, layer);
	}

	static public void SetActive (Transform parent, bool show)
	{
		parent.gameObject.SetActive(show);
		foreach (Transform tr in parent)
			SetActive (tr, show);
	}

	static public GameObject NewGameObject (Transform parent, string name)
	{
		return NewGameObject(parent, name, false);
	}

	static public GameObject NewUIGameObject (Transform parent, string name)
	{
		return NewGameObject(parent, name, true);
	}

	static GameObject NewGameObject (Transform parent, string name, bool rectTransform)
	{
		GameObject obj = null;
		if (rectTransform)
			obj = new GameObject(name, typeof(RectTransform));
		else
			obj = new GameObject(name);
		
		if (parent)
		{
			obj.transform.SetParent(parent);
			obj.layer = parent.gameObject.layer;
		}

		obj.transform.InitTransform();

		return obj;
	}

	static public Transform Child (Transform tr, string name, bool showErrorLog = true)
	{
		return Child<Transform>(tr, name, showErrorLog);
	}

	static public T Child<T> (GameObject obj, string name, bool showErrorLog = true) where T: Component
	{
		return Child<T>(obj.transform, name, showErrorLog);
	}

	static public T Child<T> (MonoBehaviour mono, string name, bool showErrorLog = true) where T: Component
	{
		return Child<T>(mono.transform, name, showErrorLog);
	}

	static public T Child<T> (Transform tr, string name, bool showErrorLog = true) where T: Component
	{
		string[] names = name.Split('/');

		foreach (string n in names)
		{
			tr = tr.Find(n);
			if (tr == false)
			{
				if (showErrorLog)
					Debug.Log(STR.RED("Can not Found '" + n + "'"));
				return default(T);
			}
		}

		return tr.GetComponent<T>();
	}

	static public string GetObjPath(GameObject obj)
	{
		string path = obj.name;

		Transform parent = obj.transform.parent;
		while (parent != null)
		{
			path = parent.name + "/" + path;
			parent = parent.parent;
		}

		return path;
	}

    static public string fieldName(Expression<Func<object>> expr)
    {
        var body = ((MemberExpression)expr.Body);
        return body.Member.Name;
    }

    static Type[] systemTypes = {
        typeof(System.Boolean),
        typeof(System.String),
        typeof(System.Byte),
        typeof(System.SByte),
        typeof(System.Char),
        typeof(System.Decimal),
        typeof(System.Double),
        typeof(System.Single),
        typeof(System.Int32),
        typeof(System.UInt32),
        typeof(System.Int64),
        typeof(System.UInt64),
        typeof(System.Int16),
        typeof(System.UInt16),
    };

    static bool isSystemType( System.Type type )
    {
        for( int i = 0; i < systemTypes.Length; i++ )
            if( type.Equals( systemTypes[i] ) )
                return true;

        return false;
    }

    static public string GetFieldValue( object obj )
	{
		StringBuilder sb = new StringBuilder();

		System.Type type = obj.GetType();
		if (isSystemType(type))
		{
			sb.Append(obj.ToString());
		}
		else
		{
			FieldInfo[] f = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			for (int i = 0; i < f.Length; i++)
			{
				if (sb.Length > 0)
					sb.Append(", ");
				
				if (isSystemType(f[i].FieldType) == false)
					sb.Append(f[i].Name);
				else
					sb.Append(string.Format("{0}: <color='#ffff00'>{1}</color>", f[i].Name, f[i].GetValue(obj)));
			}
		}

		return sb.ToString();
	}		
}

public static class AnyStatic
{
	public static string RemoveLast(this string s, string del )
    {
        if( s.EndsWith( del ) )
            return s.Substring( 0, s.Length - del.Length );

        return s;
    }
    
    public static Transform InitTransform (this Transform tran)
    {
        tran.localPosition = Vector3.zero;
        tran.localRotation = Quaternion.identity;
        tran.localScale = Vector3.one;
        return tran;
    }

	public static RectTransform InitTransform (this RectTransform tran)
    {
		tran.anchoredPosition3D = Vector3.zero;
        tran.localRotation = Quaternion.identity;
        tran.localScale = Vector3.one;
        return tran;
    }

	public static RectTransform InitTransform (this RectTransform tran, RectTransform right, bool initPosition = false)
    {
		if (initPosition)
			tran.anchoredPosition3D = Vector3.zero;
		else
			tran.anchoredPosition3D = right.anchoredPosition3D;
        tran.localRotation = right.localRotation;
        tran.localScale = right.localScale;

        tran.anchorMin = right.anchorMin;
        tran.anchorMax = right.anchorMax;
        tran.pivot = right.pivot;
        tran.sizeDelta = right.sizeDelta;

        return tran;
    }

    public static T ToEnum<T>(this string value)
    {
        return (T)System.Enum.Parse(typeof(T), value, true);
    }

    public static string GetName<T>(Expression<Func<T>> expr)
    {
        if( expr == null )
            return string.Empty;

        var param = (MemberExpression)expr.Body;
        return param.Member.Name;
    }

    public static IEnumerator FadeIn (this GameObject obj, float t, float fadeTo = 1f)
    {
		UITween tween = UITweenAlpha.Start(obj, fadeTo, TWParam.New(t));
		while (tween.enabled)
			yield return null;
    }

    public static IEnumerator FadeOut (this GameObject obj, float t)
    {
		UITween tween = UITweenAlpha.Start(obj, 0f, TWParam.New(t));
		while (tween.enabled)
			yield return null;
    }

	public static UITween TweenSecond(this Text txt, float end, float dur, float delay = 0)
	{
		return UITweenCount.Start (txt.gameObject, 0, end, TWParam.New (dur, delay), UITweenCount.Type.MilliSecond);
	}

	public static UITween TweenCount(this Text txt, float end, float dur, string format = "{0}")
	{
		return UITweenCount.Start (txt.gameObject, 0, end, TWParam.New (dur), UITweenCount.Type.Count, format);
	}

	public static Image SetAlpha(this Image img, float f)
	{
		img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Clamp01(f) );
		return img;
	}

    public static float Degrees (this Vector2 v2 )
    {
        if( v2.x < 0)
            return 360 - (Mathf.Atan2(v2.x,v2.y) * Mathf.Rad2Deg * -1);
        else
            return Mathf.Atan2(v2.x, v2.y) * Mathf.Rad2Deg;
    }
}