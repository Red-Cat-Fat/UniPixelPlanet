using System;
using System.Collections.Generic;
using System.Linq;
using ColorGradientPicker.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Planets.Scripts
{
	public class PlanetControl : MonoBehaviour
	{
		[FormerlySerializedAs("sliderPixel")] [SerializeField]
		private Slider _sliderPixel;

		[FormerlySerializedAs("textPixel")] [SerializeField]
		private Text _textPixel;

		[FormerlySerializedAs("sliderRotation")] [SerializeField]
		private Slider _sliderRotation;

		[FormerlySerializedAs("inputSeed")] [SerializeField]
		private InputField _inputSeed;

		[FormerlySerializedAs("planets")] [SerializeField]
		private GameObject[] _planets;

		[FormerlySerializedAs("dd_planets")] [SerializeField]
		private Dropdown _dropDownPlanets;

		[SerializeField] private ExportControl _exportControl;

		[FormerlySerializedAs("exportPanel")] [SerializeField]
		private GameObject _exportPanel;

		[SerializeField] private MaterialSave _materialSave;

		[FormerlySerializedAs("pref_colorButton")] [SerializeField]
		private GameObject _colorButtonPrfab;

		[FormerlySerializedAs("colorButtonHolder")] [SerializeField]
		private RectTransform _colorButtonHolder;

		private float _time;
		private float _pixels = 100;
		private int _seed;
		private bool _overrideTime;
		private List<Color> _colors = new();
		private readonly List<GameObject> _colorButtons = new();
		private int _selectedColorButtonID;
		private GameObject _selectedColorButton;

		private int _selectedPlanet;

		private void Start()
		{
			OnChangeSeedRandom();
			GetColors();
			MakeColorButtons();
		}

		public void OnClickChooseColor()
		{
			_selectedColorButton = EventSystem.current.currentSelectedGameObject;
			_selectedColorButtonID =
				EventSystem.current.currentSelectedGameObject.GetComponent<ColorChooserButton>().ButtonID;
			ColorPicker.Create(
				_colors[_selectedColorButtonID],
				"Choose color",
				OnColorChanged,
				OnColorSelected);
		}

		private void OnColorChanged(Color currentColor)
		{
			_colors[_selectedColorButtonID] = currentColor;
			SetColor();
		}

		private void OnColorSelected(Color finishedColor)
		{
			_colors[_selectedColorButtonID] = finishedColor;
			SetColor();
		}

		private void MakeColorButtons()
		{
			for (var i = 0; i < _colors.Count; i++)
			{
				var btn = Instantiate(_colorButtonPrfab, _colorButtonHolder);
				_colorButtons.Add(btn);
				btn.GetComponent<Image>().color = _colors[i];
				btn.GetComponent<ColorChooserButton>().ButtonID = i;
				btn.GetComponent<Button>().onClick.AddListener(() => OnClickChooseColor());
			}
		}

		private void GetColors()
		{
			foreach (var btn in _colorButtons)
				DestroyImmediate(btn);

			_colors.Clear();
			_colorButtons.Clear();
			_colors = _planets[_selectedPlanet].GetComponent<IPlanet>().GetColors().ToList();
		}

		private void SetColor()
		{
			//Debug.Log(selected_planet + ":"+planets[selected_planet]);
			_selectedColorButton.GetComponent<Image>().color = _colors[_selectedColorButtonID];
			_planets[_selectedPlanet].GetComponent<IPlanet>().SetColors(_colors.ToArray());
		}

		public void OnSelectPlanet()
		{
			_selectedPlanet = _dropDownPlanets.value;
			for (var i = 0; i < _planets.Length; i++)
				if (i == _selectedPlanet)
					_planets[i].SetActive(true);
				else
					_planets[i].SetActive(false);

			GetColors();
			MakeColorButtons();
		}

		public void OnSliderPixelChanged()
		{
			_pixels = _sliderPixel.value;
			_planets[_selectedPlanet].GetComponent<IPlanet>().SetPixel(_sliderPixel.value);
			_textPixel.text = _pixels.ToString("F0") + "x" + _pixels.ToString("F0");
		}

		public void OnSliderRotationChanged()
		{
			_planets[_selectedPlanet].GetComponent<IPlanet>().SetRotate(_sliderRotation.value);
		}

		public void OnLightPositionChanged(Vector2 pos)
		{
			_planets[_selectedPlanet].GetComponent<IPlanet>().SetLight(pos);
		}

		private void UpdateTime(float time)
		{
			_planets[_selectedPlanet].GetComponent<IPlanet>().UpdateTime(time);
		}

		public void OnChangeSeedInput()
		{
			if (int.TryParse(_inputSeed.text, out _seed))
				_planets[_selectedPlanet].GetComponent<IPlanet>().SetSeed(_seed);
		}

		public void OnChangeSeedRandom()
		{
			SeedRandom();
			_planets[_selectedPlanet].GetComponent<IPlanet>().SetSeed(_seed);
		}

		private void SeedRandom()
		{
			UnityEngine.Random.InitState(DateTime.Now.Millisecond);
			_seed = UnityEngine.Random.Range(0, int.MaxValue);
			_inputSeed.text = _seed.ToString();
		}

		private void Update()
		{
			if (isOnGui())
				return;

			if (Input.GetMouseButton(0))
			{
				var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
				OnLightPositionChanged(pos);
			}

			_time += Time.deltaTime;
			if (!_overrideTime)
				UpdateTime(_time);
		}

		private bool isOnGui()
		{
			var eventData = new PointerEventData(EventSystem.current);
			eventData.position = Input.mousePosition;
			var result = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventData, result);

			if (result.Count(x => x.gameObject.GetComponent<Selectable>()) > 0)
				return true;

			return false;
		}

		public void OnExportPng()
		{
			var mats = new List<Material>();
			var images = _planets[_selectedPlanet].GetComponentsInChildren<Image>();
			foreach (var img in images)
				mats.Add(img.material);
			_materialSave.SaveImage(mats, _seed.ToString());
			mats.Clear();
		}

		public void OnExportSheets()
		{
			_overrideTime = true;
			var customeSize = 100;
			if (_dropDownPlanets.value == 9)
				customeSize = 200;
			var mats = new List<Material>();
			var images = _planets[_selectedPlanet].GetComponentsInChildren<Image>();
			foreach (var img in images)
			{
				mats.Add(img.material);
				Debug.Log(img.gameObject.name);
			}

			var iplanet = _planets[_selectedPlanet].GetComponent<IPlanet>();
			_materialSave.SaveSheets(
				mats,
				_seed.ToString(),
				_exportControl.GetWidth(),
				_exportControl.GetHeight(),
				iplanet,
				customeSize);
			mats.Clear();
			_overrideTime = false;
		}

		public void ShowExport()
		{
			_exportPanel.SetActive(true);
		}

		public void HideExport()
		{
			_exportPanel.SetActive(false);
		}
	}
}