using Planets.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Planets.GasPlanet
{
	public class GasPlanet : MonoBehaviour, IPlanet
	{
		[FormerlySerializedAs("Cloud1")] [SerializeField]
		private GameObject _cloud1;

		[FormerlySerializedAs("Cloud2")] [SerializeField]
		private GameObject _cloud2;

		private Material _cloudMaterial1;
		private Material _cloudMaterial2;

		private readonly string[] _colorVars1 =
		{
			"_Base_color",
			"_Outline_color",
			"_Shadow_base_color",
			"_Shadow_outline_color"
		};

		private readonly string[] _initColors1 =
		{
			"#3b2027",
			"#3b2027",
			"#21181b",
			"#21181b"
		};

		private readonly string[] _colorVars2 =
		{
			"_Base_color",
			"_Outline_color",
			"_Shadow_base_color",
			"_Shadow_outline_color"
		};

		private readonly string[] _initColors2 =
		{
			"#f0b541",
			"#cf752b",
			"#ab5130",
			"#7d3833"
		};

		private void Awake()
		{
			_cloudMaterial1 = _cloud1.GetComponent<Image>().material;
			_cloudMaterial2 = _cloud2.GetComponent<Image>().material;
			SetInitialColors();
		}

		public void SetPixel(float amount)
		{
			_cloudMaterial1.SetFloat(ShaderProperties.KeyPixels, amount);
			_cloudMaterial2.SetFloat(ShaderProperties.KeyPixels, amount);
		}

		public void SetLight(Vector2 pos)
		{
			_cloudMaterial1.SetVector(ShaderProperties.KeyLightOrigin, pos);
			_cloudMaterial2.SetVector(ShaderProperties.KeyLightOrigin, pos);
		}

		public void SetSeed(float seed)
		{
			var convertedSeed = seed % 1000f / 100f;
			_cloudMaterial1.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_cloudMaterial2.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_cloudMaterial2.SetFloat(ShaderProperties.KeyCloudCover, Random.Range(0.28f, 0.5f));
		}

		public void SetRotate(float r)
		{
			_cloudMaterial1.SetFloat(ShaderProperties.KeyRotation, r);
			_cloudMaterial2.SetFloat(ShaderProperties.KeyRotation, r);
		}

		public void UpdateTime(float time)
		{
			_cloudMaterial1.SetFloat(ShaderProperties.KeyTime, time * 0.5f);
			_cloudMaterial2.SetFloat(ShaderProperties.KeyTime, time * 0.5f);
		}

		public void SetCustomTime(float time)
		{
			var dt = 10f + time * 60f;
			_cloudMaterial1.SetFloat(ShaderProperties.KeyTime, dt * 0.5f);
			_cloudMaterial2.SetFloat(ShaderProperties.KeyTime, dt * 0.5f);
		}

		public void SetInitialColors()
		{
			for (var i = 0; i < _colorVars1.Length; i++)
				_cloudMaterial1.SetColor(_colorVars1[i], ColorUtil.FromRGB(_initColors1[i]));
			for (var i = 0; i < _colorVars2.Length; i++)
				_cloudMaterial2.SetColor(_colorVars2[i], ColorUtil.FromRGB(_initColors2[i]));
		}

		public Color[] GetColors()
		{
			var colors = new Color[10];
			var pos = 0;
			for (var i = 0; i < _colorVars1.Length; i++)
				colors[i] = _cloudMaterial1.GetColor(_colorVars1[i]);
			pos = _colorVars1.Length;
			for (var i = 0; i < _colorVars2.Length; i++)
				colors[i + pos] = _cloudMaterial2.GetColor(_colorVars2[i]);
			return colors;
		}

		public void SetColors(Color[] colors)
		{
			for (var i = 0; i < _colorVars1.Length; i++)
				_cloudMaterial1.SetColor(_colorVars1[i], colors[i]);
			for (var i = 0; i < _colorVars2.Length; i++)
				_cloudMaterial2.SetColor(_colorVars2[i], colors[i + _colorVars1.Length]);
		}
	}
}