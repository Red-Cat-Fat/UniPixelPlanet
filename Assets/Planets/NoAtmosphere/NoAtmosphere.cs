using Planets.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Planets.NoAtmosphere
{
	public class NoAtmosphere : MonoBehaviour, IPlanet
	{
		[FormerlySerializedAs("Land")] [SerializeField] private GameObject _land;
		[FormerlySerializedAs("Craters")] [SerializeField] private GameObject _craters;
		private Material _landMaterial;
		private Material _cratersMaterial;

		private readonly string[] _colorVars1 =
		{
			"_Color1",
			"_Color2",
			"_Color3"
		};

		private readonly string[] _initColors1 =
		{
			"#A3A7C2",
			"#4C6885",
			"#3A3F5E"
		};

		private readonly string[] _colorVars2 =
		{
			"_Color1",
			"_Color2"
		};

		private readonly string[] _initColors2 =
		{
			"#4C6885",
			"#3A3F5E"
		};

		private void Awake()
		{
			_landMaterial = _land.GetComponent<Image>().material;
			_cratersMaterial = _craters.GetComponent<Image>().material;
			SetInitialColors();
		}

		public void SetPixel(float amount)
		{
			_landMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
			_cratersMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
		}

		public void SetLight(Vector2 pos)
		{
			_landMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
			_cratersMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
		}

		public void SetSeed(float seed)
		{
			var convertedSeed = seed % 1000f / 100f;
			_landMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_cratersMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
		}

		public void SetRotate(float r)
		{
			_landMaterial.SetFloat(ShaderProperties.KeyRotation, r);
			_cratersMaterial.SetFloat(ShaderProperties.KeyRotation, r);
		}

		public void UpdateTime(float time)
		{
			_landMaterial.SetFloat(ShaderProperties.KeyTime, time * 0.5f);
			_cratersMaterial.SetFloat(ShaderProperties.KeyTime, time * 0.5f);
		}

		public void SetCustomTime(float time)
		{
			var dt = 10f + time * 60f;
			_landMaterial.SetFloat(ShaderProperties.KeyTime, dt * 0.5f);
			_cratersMaterial.SetFloat(ShaderProperties.KeyTime, dt * 0.5f);
		}

		public void SetInitialColors()
		{
			for (var i = 0; i < _colorVars1.Length; i++)
				_landMaterial.SetColor(_colorVars1[i], ColorUtil.FromRGB(_initColors1[i]));
			for (var i = 0; i < _colorVars2.Length; i++)
				_cratersMaterial.SetColor(_colorVars2[i], ColorUtil.FromRGB(_initColors2[i]));
		}

		public Color[] GetColors()
		{
			var colors = new Color[10];
			var pos = 0;
			for (var i = 0; i < _colorVars1.Length; i++)
				colors[i] = _landMaterial.GetColor(_colorVars1[i]);
			pos = _colorVars1.Length;
			for (var i = 0; i < _colorVars2.Length; i++)
				colors[i + pos] = _cratersMaterial.GetColor(_colorVars2[i]);
			return colors;
		}

		public void SetColors(Color[] colors)
		{
			for (var i = 0; i < _colorVars1.Length; i++)
				_landMaterial.SetColor(_colorVars1[i], colors[i]);
			for (var i = 0; i < _colorVars2.Length; i++)
				_cratersMaterial.SetColor(_colorVars2[i], colors[i + _colorVars1.Length]);
		}
	}
}