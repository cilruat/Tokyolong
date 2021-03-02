using System;
using UnityEngine;
using UnityEngine.UI;

namespace ScratchCardAsset.Demo
{
	public class ScratchDemoUI : MonoBehaviour
	{
		public ScratchCardManager CardManager;
		public Texture[] Brushes;
		public Toggle[] BrushToggles;
		public Toggle ProgressToggle;
		public Text ProgressText;
		public Dropdown ScratchModeDropdown;
		public Slider BrushScaleSlider;
		public Text BrushScaleText;
		public EraseProgress EraseProgress;

		private string ToggleKey = "Toggle";
		private string BrushKey = "Brush";
		private string ScaleKey = "Scale";

		void Start()
		{
			Application.targetFrameRate = 60;
			ProgressToggle.isOn = PlayerPrefs.GetInt(ToggleKey, 0) == 0;
			EraseProgress.OnProgress += OnEraseProgress;
			BrushScaleSlider.onValueChanged.AddListener(OnSlider);
			ScratchModeDropdown.onValueChanged.AddListener(OnDropdown);
			BrushScaleSlider.value = PlayerPrefs.GetFloat(ScaleKey, 1f);
			foreach (var brushToggle in BrushToggles)
			{
				brushToggle.onValueChanged.AddListener(OnChange);
			}

			BrushToggles[PlayerPrefs.GetInt(BrushKey)].isOn = true;
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Restart();
			}
		}

		private void OnDropdown(int id)
		{
			var mode = (ScratchCard.ScratchMode) id;
			CardManager.Card.Mode = mode;
		}

		private void OnSlider(float val)
		{
			CardManager.Card.BrushScale = Vector2.one * val;
			BrushScaleText.text = Math.Round(val, 2).ToString();
			PlayerPrefs.SetFloat(ScaleKey, val);
		}

		private void OnChange(bool val)
		{
			for (var i = 0; i < BrushToggles.Length; i++)
			{
				if (BrushToggles[i].isOn)
				{
					CardManager.SetEraseTexture(Brushes[i]);
					PlayerPrefs.SetInt(BrushKey, i);
					break;
				}
			}
		}

		private void OnEraseProgress(float progress)
		{
			var value = Mathf.Round(progress * 100f).ToString();
			ProgressText.text = string.Format("Progress: {0}%", value);
		}

		public void OnCheck(bool check)
		{
			EraseProgress.enabled = ProgressToggle.isOn;
			PlayerPrefs.SetInt(ToggleKey, ProgressToggle.isOn ? 0 : 1);
		}

		public void Restart()
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		}
	}
}