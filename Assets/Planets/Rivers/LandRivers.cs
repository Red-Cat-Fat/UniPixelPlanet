using Planets.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Planets.Rivers
{
	public class LandRivers : MonoBehaviour, IPlanet
	{
		[FormerlySerializedAs("Land")] [SerializeField] private GameObject _land;
		[FormerlySerializedAs("Cloud")] [SerializeField] private GameObject _cloud;

		private Material _landMaterial;
		private Material _cloudMaterial;

		private readonly string[] _colorVars1 =
		{
			"_Color1",
			"_Color2",
			"_Color3",
			"_Color4",
			"_River_color",
			"_River_color_dark"
		};

		private readonly string[] _initColors1 =
		{
			"#63AB3F",
			"#3B7D4F",
			"#2F5753",
			"#283540",
			"#4FA4B8",
			"#404973"
		};

		private readonly string[] _colorVars2 =
		{
			"_Base_color",
			"_Outline_color",
			"_Shadow_Base_color",
			"_Shadow_Outline_color"
		};

		private readonly string[] _initColors2 =
		{
			"#FFFFFF",
			"#DFE0E8",
			"#686F99",
			"#404973"
		};

		private void Awake()
		{
			_landMaterial = _land.GetComponent<Image>().material;
			_cloudMaterial = _cloud.GetComponent<Image>().material;
			SetInitialColors();
		}

		public void SetPixel(float amount)
		{
			_landMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
			_cloudMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
		}

		public void SetLight(Vector2 pos)
		{
			_landMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
			_cloudMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
		}

		public void SetSeed(float seed)
		{
			var convertedSeed = seed % 1000f / 100f;
			_landMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_cloudMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_cloudMaterial.SetFloat(ShaderProperties.KeyCloudCover, Random.Range(0.35f, 0.6f));
		}

		public void SetRotate(float r)
		{
			_landMaterial.SetFloat(ShaderProperties.KeyRotation, r);
			_cloudMaterial.SetFloat(ShaderProperties.KeyRotation, r);
		}

		public void UpdateTime(float time)
		{
			_cloudMaterial.SetFloat(ShaderProperties.KeyTime, time * 0.25f);
			_landMaterial.SetFloat(ShaderProperties.KeyTime, time * 0.5f);
		}

		public void SetCustomTime(float time)
		{
			var dt = 10f + time * 60f;
			_cloudMaterial.SetFloat(ShaderProperties.KeyTime, dt * 0.25f);
			_landMaterial.SetFloat(ShaderProperties.KeyTime, dt * 0.5f);
		}

		public void SetInitialColors()
		{
			for (var i = 0; i < _colorVars1.Length; i++)
				_landMaterial.SetColor(_colorVars1[i], ColorUtil.FromRGB(_initColors1[i]));
			for (var i = 0; i < _colorVars2.Length; i++)
				_cloudMaterial.SetColor(_colorVars2[i], ColorUtil.FromRGB(_initColors2[i]));
		}

		public Color[] GetColors()
		{
			var colors = new Color[10];
			var pos = 0;
			for (var i = 0; i < _colorVars1.Length; i++)
				colors[i] = _landMaterial.GetColor(_colorVars1[i]);
			pos = _colorVars1.Length;
			for (var i = 0; i < _colorVars2.Length; i++)
				colors[i + pos] = _cloudMaterial.GetColor(_colorVars2[i]);
			return colors;
		}

		public void SetColors(Color[] colors)
		{
			for (var i = 0; i < _colorVars1.Length; i++)
				_landMaterial.SetColor(_colorVars1[i], colors[i]);
			for (var i = 0; i < _colorVars2.Length; i++)
				_cloudMaterial.SetColor(_colorVars2[i], colors[i + _colorVars1.Length]);
		}
	}
}