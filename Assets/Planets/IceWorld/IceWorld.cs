using Planets.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Planets.IceWorld
{
	public class IceWorld : MonoBehaviour, IPlanet
	{
		[FormerlySerializedAs("PlanetUnder")] [SerializeField]
		private GameObject _planetUnder;

		[FormerlySerializedAs("Lakes")] [SerializeField]
		private GameObject _lakes;

		[FormerlySerializedAs("Clouds")] [SerializeField]
		private GameObject _clouds;

		private Material _planetUnderMaterial;
		private Material _lakesMaterial;
		private Material _cloudsMaterial;

		private readonly string[] _colorVars1 =
		{
			"_Color1",
			"_Color2",
			"_Color3"
		};

		private readonly string[] _initColors1 =
		{
			"#faffff",
			"#c7d4e1",
			"#928fb8"
		};

		private readonly string[] _colorVars2 =
		{
			"_Color1",
			"_Color2",
			"_Color3"
		};

		private readonly string[] _initColors2 =
		{
			"#4fa4b8",
			"#4c6885",
			"#3a3f5e"
		};

		private readonly string[] _colorVars3 =
		{
			"_Base_color",
			"_Outline_color",
			"_Shadow_Base_color",
			"_Shadow_Outline_color"
		};

		private readonly string[] _initColors3 =
		{
			"#e1f2ff",
			"#c0e3ff",
			"#5e70a5",
			"#404973"
		};

		private void Awake()
		{
			_planetUnderMaterial = _planetUnder.GetComponent<Image>().material;
			_lakesMaterial = _lakes.GetComponent<Image>().material;
			_cloudsMaterial = _clouds.GetComponent<Image>().material;
			SetInitialColors();
		}

		public void SetPixel(float amount)
		{
			_planetUnderMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
			_lakesMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
			_cloudsMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
		}

		public void SetLight(Vector2 pos)
		{
			_planetUnderMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
			_lakesMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
			_cloudsMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
		}

		public void SetSeed(float seed)
		{
			var convertedSeed = seed % 1000f / 100f;
			_planetUnderMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_lakesMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_cloudsMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
		}

		public void SetRotate(float r)
		{
			_planetUnderMaterial.SetFloat(ShaderProperties.KeyRotation, r);
			_lakesMaterial.SetFloat(ShaderProperties.KeyRotation, r);
			_cloudsMaterial.SetFloat(ShaderProperties.KeyRotation, r);
		}

		public void UpdateTime(float time)
		{
			_cloudsMaterial.SetFloat(ShaderProperties.KeyTime, time * 0.5f);
			_planetUnderMaterial.SetFloat(ShaderProperties.KeyTime, time);
			_lakesMaterial.SetFloat(ShaderProperties.KeyTime, time);
		}

		public void SetCustomTime(float time)
		{
			var dt = 10f + time * 60f;
			_cloudsMaterial.SetFloat(ShaderProperties.KeyTime, dt * 0.5f);
			_planetUnderMaterial.SetFloat(ShaderProperties.KeyTime, dt);
			_lakesMaterial.SetFloat(ShaderProperties.KeyTime, dt);
		}

		public void SetInitialColors()
		{
			for (var i = 0; i < _colorVars1.Length; i++)
				_planetUnderMaterial.SetColor(_colorVars1[i], ColorUtil.FromRGB(_initColors1[i]));
			for (var i = 0; i < _colorVars2.Length; i++)
				_lakesMaterial.SetColor(_colorVars2[i], ColorUtil.FromRGB(_initColors2[i]));
			for (var i = 0; i < _colorVars3.Length; i++)
				_cloudsMaterial.SetColor(_colorVars3[i], ColorUtil.FromRGB(_initColors3[i]));
		}

		public Color[] GetColors()
		{
			var colors = new Color[10];
			var pos = 0;
			for (var i = 0; i < _colorVars1.Length; i++)
				colors[i] = _planetUnderMaterial.GetColor(_colorVars1[i]);
			pos = _colorVars1.Length;
			for (var i = 0; i < _colorVars2.Length; i++)
				colors[i + pos] = _lakesMaterial.GetColor(_colorVars2[i]);
			pos = _colorVars1.Length + _colorVars2.Length;
			for (var i = 0; i < _colorVars3.Length; i++)
				colors[i + pos] = _cloudsMaterial.GetColor(_colorVars3[i]);
			return colors;
		}

		public void SetColors(Color[] colors)
		{
			for (var i = 0; i < _colorVars1.Length; i++)
				_planetUnderMaterial.SetColor(_colorVars1[i], colors[i]);
			for (var i = 0; i < _colorVars2.Length; i++)
				_lakesMaterial.SetColor(_colorVars2[i], colors[i + _colorVars1.Length]);
			for (var i = 0; i < _colorVars3.Length; i++)
				_cloudsMaterial.SetColor(_colorVars3[i], colors[i + _colorVars1.Length + _colorVars2.Length]);
		}
	}
}