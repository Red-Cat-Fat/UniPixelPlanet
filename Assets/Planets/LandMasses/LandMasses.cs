using System.Linq;
using Planets.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Planets.LandMasses
{
	public class LandMasses : MonoBehaviour, IPlanet
	{
		[FormerlySerializedAs("Water")] [SerializeField]
		private GameObject _water;

		[FormerlySerializedAs("Land")] [SerializeField]
		private GameObject _land;

		[FormerlySerializedAs("Cloud")] [SerializeField]
		private GameObject _cloud;

		private Material _waterMaterial;
		private Material _landMaterial;
		private Material _cloudsMaterial;

		private readonly string[] _colorVars1 =
		{
			"_Color1",
			"_Color2",
			"_Color3"
		};

		private readonly string[] _initColors1 =
		{
			"#92E8C0",
			"#4FA4B8",
			"#2C354D"
		};

		private readonly string[] _colorVars2 =
		{
			"_Color1",
			"_Color2",
			"_Color3",
			"_Color4"
		};

		private readonly string[] _initColors2 =
		{
			"#C8D45D",
			"#63AB3F",
			"#2F5753",
			"#283540"
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
			"#DFE0E8",
			"#A3A7C2",
			"#686F99",
			"#404973"
		};

		private void Awake()
		{
			_waterMaterial = _water.GetComponent<Image>().material;
			_landMaterial = _land.GetComponent<Image>().material;
			_cloudsMaterial = _cloud.GetComponent<Image>().material;
			SetInitialColors();
		}

		public void SetPixel(float amount)
		{
			_waterMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
			_landMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
			_cloudsMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
		}

		public void SetLight(Vector2 pos)
		{
			_waterMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
			_landMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
			_cloudsMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
		}

		public void SetSeed(float seed)
		{
			var convertedSeed = seed % 1000f / 100f;
			_waterMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_landMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_cloudsMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_cloudsMaterial.SetFloat(ShaderProperties.KeyCloudCover, Random.Range(0.35f, 0.6f));
		}

		public void SetRotate(float r)
		{
			_waterMaterial.SetFloat(ShaderProperties.KeyRotation, r);
			_landMaterial.SetFloat(ShaderProperties.KeyRotation, r);
			_cloudsMaterial.SetFloat(ShaderProperties.KeyRotation, r);
		}

		public void UpdateTime(float time)
		{
			_cloudsMaterial.SetFloat(ShaderProperties.KeyTime, time * 0.5f);
			_waterMaterial.SetFloat(ShaderProperties.KeyTime, time);
			_landMaterial.SetFloat(ShaderProperties.KeyTime, time);
		}

		public void SetCustomTime(float time)
		{
			var dt = 10f + time * 60f;
			_cloudsMaterial.SetFloat(ShaderProperties.KeyTime, dt * 0.5f);
			_waterMaterial.SetFloat(ShaderProperties.KeyTime, dt);
			_landMaterial.SetFloat(ShaderProperties.KeyTime, dt);
		}

		public void SetInitialColors()
		{
			for (var i = 0; i < _colorVars1.Length; i++)
				_waterMaterial.SetColor(_colorVars1[i], ColorUtil.FromRGB(_initColors1[i]));
			for (var i = 0; i < _colorVars2.Length; i++)
				_landMaterial.SetColor(_colorVars2[i], ColorUtil.FromRGB(_initColors2[i]));
			for (var i = 0; i < _colorVars3.Length; i++)
				_cloudsMaterial.SetColor(_colorVars3[i], ColorUtil.FromRGB(_initColors3[i]));
		}

		public Color[] GetColors()
		{
			var colors = new Color[11];
			var pos = 0;
			for (var i = 0; i < _colorVars1.Length; i++)
				colors[i] = _waterMaterial.GetColor(_colorVars1[i]);
			pos = _colorVars1.Length;
			for (var i = 0; i < _colorVars2.Length; i++)
				colors[i + pos] = _landMaterial.GetColor(_colorVars2[i]);
			pos = _colorVars1.Length + _colorVars2.Length;
			for (var i = 0; i < _colorVars3.Length; i++)
				colors[i + pos] = _cloudsMaterial.GetColor(_colorVars3[i]);
			return colors;
		}

		public void SetColors(Color[] colors)
		{
			var colorArray = colors.ToList();

			for (var i = 0; i < _colorVars1.Length; i++)
			{
				var key = _colorVars1[i];
				var val = colorArray.GetRange(i, 1).FirstOrDefault();
				_waterMaterial.SetColor(key, val);
			}

			for (var i = 0; i < _colorVars2.Length; i++)
			{
				var key = _colorVars2[i];
				var val = colorArray.GetRange(i + _colorVars1.Length, 1).FirstOrDefault();
				_landMaterial.SetColor(key, val);
			}

			for (var i = 0; i < _colorVars3.Length; i++)
			{
				var key = _colorVars3[i];
				var val = colorArray.GetRange(i + _colorVars1.Length + _colorVars2.Length, 1).FirstOrDefault();
				_cloudsMaterial.SetColor(key, val);
			}
		}
	}
}