using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Selectable))]
public class UIButton : MonoBehaviour,
						IPointerDownHandler,
						IPointerUpHandler,
						IPointerEnterHandler,
						IPointerExitHandler
{
	Selectable selectable;
	Text text;
	RawImage icon;
	Transform selected;

	float internalScaling = 1f;
	bool inner = false;
	bool down = false;
	public bool IsDown { get { return down; } }

	public bool scalingOnTouch = true;
	public MaskableGraphic[] shinings;
	public List<MaskableGraphic> exceptWhenUnable = new List<MaskableGraphic> ();
	Dictionary<MaskableGraphic, Material> backup = new Dictionary<MaskableGraphic, Material> ();

	bool isRegisterd = false;

	void Awake ()
	{
		_register();
	}

	void _register()
	{
		if (isRegisterd)
			return;

		isRegisterd = true;
		
		selectable = GetComponent<Selectable>();
		text = Any.Child<Text>(transform, "Text", false);
		icon = Any.Child<RawImage>(transform, "Icon", false);
		selected = Any.Child(transform, "Selected", false);

		if (selected != null)
			selected.gameObject.SetActive(false);
	}

	public void SetButtonEnable (bool val, bool changeInteractable = false)
	{
		enabled = val;

		if (changeInteractable)
			selectable.interactable = val;
		else
			selectable.enabled = val;
	}

	protected virtual void OnDisable ()
	{
		OnPointerUp();

		Transform check = transform.Find("BigButton");
		if( check != null )
			Destroy(check.gameObject);
	}

	public Color textColor{ set{ if(text != null) text.color = value; } }
	public Color imgColor{ set{ if(selectable != null) selectable.image.color = value; } }

	public void SetText (string val)
	{		
		if (text != null)
			text.text = val;
	}

	public void SetSelect (bool isSelected)
	{
		if (selected != null)
			selected.gameObject.SetActive(isSelected);

		if (text != null)
			UITweenColor.Start(text.gameObject, isSelected ? Color.white : Color.black, TWParam.New(.05f));

		if (icon != null)
			UITweenColor.Start(icon.gameObject, isSelected ? Color.white : Color.black, TWParam.New(.05f));
	}

	public void AddListener (UnityAction func)
	{
		if (isRegisterd == false)
			_register();
		
		if (selectable.GetType () != typeof(Button)) {
			Debug.LogError ("selectable type is not Button(type = " + selectable.GetType ().ToString ());
			return;
		}

		((Button)selectable).onClick.AddListener(func);		
	}

	/*
	public void AddToggleListener (UnityAction<bool> func)
	{
		if (selectable.GetType () == typeof(Toggle)) {
			Debug.LogError ("selectable type is not Button(type = " + selectable.GetType ().ToString ());
			return;
		}

		((Toggle)selectable).onValueChanged.AddListener(func);
	}
	*/

	public void OnPointerDown (PointerEventData data)
	{		
		OnPointerDown ();
	}

	public virtual void OnPointerDown ()
	{
		Init();

		if (!down && scalingOnTouch)
			UITweenScale.Start(gameObject, internalScaling, TWParam.New(.1f));

		for (int i = 0; i < shinings.Length; i++)
			ShiningGraphic.Start (shinings [i]);

		down = true;
	}

	public void OnPointerUp (PointerEventData data)
	{
		OnPointerUp ();
	}

	public virtual void OnPointerUp()
	{
		if (down && inner && scalingOnTouch)
			UITweenScale.Start(gameObject, 1f, TWParam.New(.3f).Curve(TWCurve.Elastic));

		down = false;
	}

	public void OnPointerEnter (PointerEventData data)
	{
		OnPointerEnter ();
	}

	public void OnPointerEnter()
	{
		inner = true;

		if (down && scalingOnTouch)
			UITweenScale.Start(gameObject, internalScaling, TWParam.New(.1f));
	}

	public void OnPointerExit (PointerEventData data)
	{
		OnPointerExit ();
	}

	public void OnPointerExit()
	{
		inner = false;

		if (down && scalingOnTouch)
			UITweenScale.Start(gameObject, 1f, TWParam.New(.1f));
	}

	bool alreadyInit = false;
	void Init ()
	{
		if (alreadyInit)
			return;

		alreadyInit = true;

		//MakeBigButton();

		Selectable sel = GetComponent<Selectable>();
		if (sel != null)
			sel.transition = Selectable.Transition.ColorTint;

		RectTransform rt = (RectTransform)transform;
		float size = Mathf.Max(rt.rect.width, rt.rect.height);
		float deltaSize = Mathf.Clamp ((size * .1f), 8f, 15f);
		internalScaling = (size + deltaSize) / size;
	}

	const float MIN_BUTTON_LENGTH = 180f;
	void MakeBigButton ()
	{
		Transform check = transform.Find("BigButton");
		if (check != null)
			return;

		RectTransform rtOrigin = GetComponent<RectTransform>();
		if (rtOrigin == false)
			return;

		float length = rtOrigin.rect.width + rtOrigin.rect.height;
		if (length >= MIN_BUTTON_LENGTH)
			return;

		float rate = MIN_BUTTON_LENGTH / length;

		GameObject obj = Any.NewUIGameObject(transform, "BigButton");
		obj.transform.SetAsFirstSibling();

		Image img = obj.AddComponent<Image>();
		img.sprite = null;
		img.color = new Color(0f, 0f, 0f, 0f);

		RectTransform rt = (RectTransform)obj.transform;
		rt.anchoredPosition = Vector2.zero;
		rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, rtOrigin.rect.width * rate);
		rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, rtOrigin.rect.height * rate);
	}

	public void EnableAndRecover () {
		if (enabled)
			return;

		MaskableGraphic[] _mgs = GetComponentsInChildren<MaskableGraphic> (false);

		foreach (MaskableGraphic _mg in _mgs) {
			if (exceptWhenUnable.Contains (_mg))
				continue;

			Material backupMaterial;
			if (backup.TryGetValue (_mg, out backupMaterial))
				_mg.material = backupMaterial;
			else
				_mg.material = null;
		}

		backup.Clear ();
		enabled = true;

		if (selectable == null)
			selectable = GetComponent<Selectable> ();

		selectable.enabled = true;
	}

	public void DisableAndGreyScale () {
		if (enabled == false)
			return;

		backup.Clear ();

		MaskableGraphic[] _mgs = GetComponentsInChildren<MaskableGraphic> (false);
		Material m = new Material (Shader.Find ("UI/Greyscale"));
		foreach (MaskableGraphic _mg in _mgs) {
			if (exceptWhenUnable.Contains (_mg))
				continue;

			if (_mg.material)
				backup.Add (_mg, _mg.material);			

			_mg.material = m;
		}

		enabled = false;

		if (selectable == null)
			selectable = GetComponent<Selectable> ();

		selectable.enabled = false;
	}
}
