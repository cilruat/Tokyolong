using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public enum TWLoop
{
	Once,
	Loop,
	PingPong,
}

public enum TWCurve
{
	UseAnimationCurve,

	Linear,

	CurveLevel1, // Sine
	CurveLevel2, // Quad
	CurveLevel3, // Cubic
	CurveLevel4, // Quart
	CurveLevel5, // Quint
	CurveLevel6, // Expo
	Circ,
	Back,
	Elastic,
	Bounce,

	Punch,
	Shake,
	Spring,
}

public enum TWSpeed
{
	Slower,
	Faster,
	SlowerFaster,
}

public enum TWTag
{
	TagA,
	TagB,
	TagC,
	TagD,
}

[System.Serializable]
public class TWParam
{
	public float duration = 1f;
	public float delay = 0f;
	public TWLoop loopType = TWLoop.Once;
	public AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	public TWCurve curveType = TWCurve.Linear;
	public TWSpeed curveSpeed = TWSpeed.Slower;
	public bool initStartVal = true;
	public bool destroyOnFinish = false;
	public bool disableOnFinish = false;

	TWParam (float duration, float delay)
	{
		this.duration = duration;
		this.delay = delay;
	}

	static public TWParam New (float duration, float delay = 0f) { return new TWParam(duration, delay); }
	public TWParam Delay (float delay)			{ this.delay = delay; return this; }
	public TWParam Loop (TWLoop loopType)		{ this.loopType = loopType; return this; }
	public TWParam Curve (TWCurve curveType)	{ this.curveType = curveType; return this; }
	public TWParam Curve (AnimationCurve curve)	{ this.curveType = TWCurve.UseAnimationCurve; this.animationCurve = curve; return this; }
	public TWParam Speed (TWSpeed curveSpeed)	{ this.curveSpeed = curveSpeed; return this; }
	public TWParam DestroyOnFinish ()			{ this.destroyOnFinish = true; return this; }
	public TWParam DisableOnFinish ()			{ this.disableOnFinish = true; return this; }
}

public abstract class UITween : MonoBehaviour
{
	[HideInInspector] public bool isTweenGroup = false;

	[HideInInspector]public TWTag startTag = TWTag.TagA;
	public bool autoStart = false;
	public TWParam param = TWParam.New(1f);
	public UITween next;
	[HideInInspector] public UITween prev;
	public UnityEvent onCompleteFunc;
	bool reverse = false;

	protected LTDescr leanTween;

	protected Transform trans;
	protected RectTransform rectTrans;
	protected CanvasGroup canvasGroup;
	protected Text text;
	protected Graphic graphic;

	public float TotalLength	{ get { return param.delay + param.duration; } }
	public float Duration		{ get { return param.duration; } }
	public float Delay			{ get { return param.delay; } }

	protected virtual void Awake ()
	{
		Init();
	}

	bool alreadyInit = false;
	protected void Init ()
	{
		if (alreadyInit)
			return;

		isTweenGroup = (this.GetType() == typeof(UITweenGroup));

		trans = transform;
		rectTrans = GetComponent<RectTransform>();
		canvasGroup = GetComponent<CanvasGroup>();
		text = GetComponent<Text>();
		graphic = GetComponent<Graphic>();

		if (next != null)
			next.prev = this;

		alreadyInit = true;
	}

	void OnEnable ()
	{
		if (autoStart && prev == null)
			StartTween();
	}

	void OnDestroy ()
	{
		if (leanTween != null && leanTween.toggle)
			LeanTween.cancel(leanTween.id);
	}

	public virtual void CopyTo (bool toStart) {}

	protected abstract void SetVal (float ratio);

	protected void SetValReverse (float ratio)
	{
		SetVal(1f - ratio);
	}

	void SetStartVal () { SetTimeRatio(0f); }
	void SetEndVal () { SetTimeRatio(1f); }
	void SetTimeRatio (float timeRatio)
	{
		if (param.curveType == TWCurve.UseAnimationCurve)
			timeRatio = param.animationCurve.Evaluate(timeRatio);

		if (reverse)
			SetValReverse(timeRatio);
		else
			SetVal(timeRatio);
	}

	public void SetTime (float time)
	{
		SetTimeRatio(Mathf.Clamp01((time - Delay) / Duration));
	}

	public virtual void SetReverse (bool reverse)
	{
		this.reverse = reverse;
	}

	public void StartTween (TWTag startTag = TWTag.TagA)
	{
		gameObject.SetActive(true);
		enabled = (isTweenGroup || this.startTag == startTag) && prev == null;

		if (isTweenGroup)
			this.startTag = startTag;		

		if (leanTween != null && leanTween.toggle)
			LeanTween.cancel(leanTween.id);

		if (enabled)
		{
			if (param.initStartVal)
				SetStartVal();
			else
				CopyTo(true);
			
			Tween();
			AfterSetting();
		}
	}

    public void StopTween()
    {
        if (leanTween == null)
            return;

        LeanTween.cancel(leanTween.id); 
    }

	protected virtual void Tween ()
	{
		leanTween = LeanTween.value(0f, 1f, isTweenGroup ? 0f : param.duration);

		if (leanTween != null)
		{
			if (reverse)
				leanTween.setOnUpdate(SetValReverse);
			else
				leanTween.setOnUpdate(SetVal);
		}

		AddCallback(SetEndVal);
	}

	void AfterSetting ()
	{
		if (leanTween == null)
			return;
			
		leanTween.setLoopType(ToLeanTweenType(param.loopType)).setIgnoreTimeScale(true);

		if (param.curveType == TWCurve.UseAnimationCurve)
			leanTween.setEase(param.animationCurve);
		else
			leanTween.setEase(ToLeanTweenType(param.curveType, param.curveSpeed));

		if (param.delay > 0f)
			leanTween.setDelay(param.delay);

		leanTween.setOnComplete(OnTweenFinish);
	}

	void OnTweenFinish()
	{
		if (this == null)
			return;
		
		if (next != null)
			next.StartTween();

		if (onCompleteFunc != null)
		{
			onCompleteFunc.Invoke();
			onCompleteFunc.RemoveAllListeners();
		}

		leanTween = null;
		enabled = false;

		if (param.disableOnFinish)
			gameObject.SetActive(false);

		if (param.destroyOnFinish)
			Destroy(gameObject);
	}

	public virtual bool IsTweening ()
	{
		return enabled && leanTween != null;
	}

	public UITween AddCallback (UnityAction func)
	{
		if (onCompleteFunc == null)
			onCompleteFunc = new UnityEvent();

		onCompleteFunc.AddListener(func);

		return this;
	}

	static protected T GetTween<T>(GameObject obj, bool addComponent = true) where T : UITween
	{
		T tween = obj.GetComponent<T>();
		if (tween == null && addComponent)
			tween = obj.AddComponent<T>();

		tween.Init();

		return tween;
	}

	static protected LeanTweenType ToLeanTweenType (TWLoop type)
	{
		switch (type)
		{
			case TWLoop.Once:		return LeanTweenType.once;
			case TWLoop.Loop:		return LeanTweenType.clamp;
			case TWLoop.PingPong:	return LeanTweenType.pingPong;
		}
		return LeanTweenType.notUsed;
	}

	static protected LeanTweenType ToLeanTweenType (TWCurve type, TWSpeed speed)
	{
		switch (type)
		{
			case TWCurve.UseAnimationCurve:	return LeanTweenType.animationCurve;
			case TWCurve.Linear:			return LeanTweenType.linear;
			case TWCurve.Punch:				return LeanTweenType.punch;
			case TWCurve.Shake:				return LeanTweenType.easeShake;
			case TWCurve.Spring:			return LeanTweenType.easeSpring;
			default:
			{
				StringBuilder sb = new StringBuilder();
				switch (speed)
				{
					case TWSpeed.Slower:		sb.Append("easeOut");	break;
					case TWSpeed.Faster:		sb.Append("easeIn");		break;
					case TWSpeed.SlowerFaster:	sb.Append("easeInOut");	break;
				}
				
				switch (type)
				{
					case TWCurve.CurveLevel1:	sb.Append("Sine");	break;
					case TWCurve.CurveLevel2:	sb.Append("Quad");	break;
					case TWCurve.CurveLevel3:	sb.Append("Cubic");	break;
					case TWCurve.CurveLevel4:	sb.Append("Quart");	break;
					case TWCurve.CurveLevel5:	sb.Append("Quint");	break;
					case TWCurve.CurveLevel6:	sb.Append("Expo");	break;
					default:					sb.Append(type.ToString());	break;
				}

				return Parse.ToEnum<LeanTweenType>(sb.ToString());
			}
		}
	}

#if UNITY_EDITOR
	// For Editor
	[System.NonSerialized] public bool foldout;
	[System.NonSerialized] public float elapsed;
	[System.NonSerialized] public UITweenGroup upper;
	LTDescr temp = new LTDescr();
	bool isPong;

	public virtual bool ShowCopyButton() { return false; }
	public void InitForInspector ()
	{
		Awake();
		temp.reset();
		temp.to = Vector3.one;
		temp.setDiff(Vector3.one);
	}

	public virtual bool AnimateEditor (float elapsedTo, bool forceUpdate, TWTag startTag)
	{
		switch (param.loopType)
		{
			case TWLoop.Once:
			{
				elapsedTo = Mathf.Min(elapsedTo, param.duration);
				break;
			}
			default:
			{
				if (param.duration > 0)
				{
					isPong = param.loopType == TWLoop.PingPong && (Mathf.FloorToInt(elapsedTo / param.duration) % 2 == 1);

					if (isPong)
						elapsedTo = param.duration - Mathf.Repeat(elapsedTo, param.duration);
					else
						elapsedTo = Mathf.Repeat(elapsedTo, param.duration);
				}

				break;
			}
		}
		
		if (elapsed == elapsedTo && forceUpdate == false)
			return false;

		if (gameObject.activeSelf == false)
			gameObject.SetActive(true);

		elapsed = elapsedTo;

		if (param.curveType == TWCurve.UseAnimationCurve)
			temp.setEase(param.animationCurve);
		else
			temp.setEase(ToLeanTweenType(param.curveType, param.curveSpeed));

		temp.ratioPassed = (param.duration <= 0f ? 0f : Mathf.Clamp01(elapsed / param.duration));
		SetVal(temp.easeMethod().x);

		return true;
	}
#endif
}

public abstract class UITweenFloatType : UITween
{
#if UNITY_EDITOR
	public override bool ShowCopyButton() { return true; }
#endif

	public float start = 0f;
	public float end = 1f;

	protected float GetVal (float ratio)
	{
		return Mathf.LerpUnclamped(start, end, ratio);
	}

	public static T Start<T> (GameObject obj, float start, float end, TWParam param) where T : UITweenFloatType
	{
		T tween = GetTween<T>(obj);
		tween.start = start;
		tween.end = end;
		tween.param = param;
		tween.StartTween();
		return tween;
	}

	public static T Start<T> (GameObject obj, float end, TWParam param) where T : UITweenFloatType
	{
		param.initStartVal = false;
		return Start<T>(obj, 0f, end, param);
	}

	protected virtual float GetValFromObject () { return 0f; }
	public override void CopyTo (bool toStart)
	{
		if (toStart) start = GetValFromObject();
		else end = GetValFromObject();
	}
}

public abstract class UITweenRatioType : UITween
{
#if UNITY_EDITOR
	public override bool ShowCopyButton() { return true; }
#endif

	[Range(0f, 1f)]
	public float start = 0f;
	[Range(0f, 1f)]
	public float end = 1f;

	protected float GetVal (float ratio)
	{
		return Mathf.LerpUnclamped(start, end, ratio);
	}

	public static T Start<T> (GameObject obj, float start, float end, TWParam param) where T : UITweenRatioType
	{
		T tween = GetTween<T>(obj);
		tween.start = start;
		tween.end = end;
		tween.param = param;
		tween.StartTween();
		return tween;
	}

	public static T Start<T> (GameObject obj, float end, TWParam param) where T : UITweenRatioType
	{
		param.initStartVal = false;
		return Start<T>(obj, 0f, end, param);
	}

	protected virtual float GetValFromObject () { return 0f; }
	public override void CopyTo (bool toStart)
	{
		if (toStart) start = GetValFromObject();
		else end = GetValFromObject();
	}
}

public abstract class UITweenVector3Type : UITween
{
#if UNITY_EDITOR
	public override bool ShowCopyButton() { return true; }
#endif

	public Vector3 start = Vector3.zero;
	public Vector3 end = Vector3.zero;
	protected bool _use_rectTrans = true;

	protected Vector3 GetVal (float ratio)
	{
		return Vector3.LerpUnclamped(start, end, ratio);
	}

	public static T Start<T> (GameObject obj, Vector3 start, Vector3 end, TWParam param, bool use_rectTrans = true) where T : UITweenVector3Type
	{
		T tween = GetTween<T>(obj);
		tween.start = start;
		tween.end = end;
		tween.param = param;
		tween._use_rectTrans = use_rectTrans;
		tween.StartTween();
		return tween;
	}

	public static T Start<T> (GameObject obj, Vector3 end, TWParam param, bool use_rectTrans = true) where T : UITweenVector3Type
	{
		param.initStartVal = false;
		return Start<T>(obj, Vector3.zero, end, param, use_rectTrans);
	}

	protected virtual Vector3 GetValFromObject () { return Vector3.zero; }
	public override void CopyTo (bool toStart)
	{
		if (toStart) start = GetValFromObject();
		else end = GetValFromObject();
	}
}

public abstract class UITweenColorType : UITween
{
#if UNITY_EDITOR
	public override bool ShowCopyButton() { return true; }
#endif

	public Color start = Color.white;
	public Color end = Color.white;

	protected Color GetVal (float ratio)
	{
		return Color.LerpUnclamped(start, end, ratio);
	}

	public static T Start<T> (GameObject obj, Color start, Color end, TWParam param) where T : UITweenColorType
	{
		T tween = GetTween<T>(obj);
		tween.start = start;
		tween.end = end;
		tween.param = param;
		tween.StartTween();
		return tween;
	}

	public static T Start<T> (GameObject obj, Color end, TWParam param) where T : UITweenColorType
	{
		param.initStartVal = false;
		return Start<T>(obj, Color.white, end, param);
	}

	protected virtual Color GetValFromObject () { return Color.white; }
	public override void CopyTo (bool toStart)
	{
		if (toStart) start = GetValFromObject();
		else end = GetValFromObject();
	}
}