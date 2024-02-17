using Planets.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Planets.LavaWorld
{
	public class LavaWorld : MonoBehaviour, IPlanet
	{
		[FormerlySerializedAs("PlanetUnder")] [SerializeField] private GameObject _planetUnder;
		[FormerlySerializedAs("Craters")] [SerializeField] private GameObject _craters;
		[FormerlySerializedAs("LavaRivers")] [SerializeField] private GameObject _lavaRivers;
		private Material _planetMaterial;
		private Material _cratersMaterial;
		private Material _riversMaterial;

		private readonly string[] _colorVars1 =
		{
			"_Color1",
			"_Color2",
			"_Color3"
		};

		private readonly string[] _initColors1 =
		{
			"#8f4d57",
			"#52333f",
			"#3d2936"
		};

		private readonly string[] _colorVars2 =
		{
			"_Color1",
			"_Color2"
		};

		private readonly string[] _initColors2 =
		{
			"#52333f",
			"#3d2936"
		};

		private readonly string[] _colorVars3 =
		{
			"_Color1",
			"_Color2",
			"_Color3"
		};

		private readonly string[] _initColors3 =
		{
			"#ff8933",
			"#e64539",
			"#ad2f45"
		};

		private void Awake()
		{
			_planetMaterial = _planetUnder.GetComponent<Image>().material;
			_cratersMaterial = _craters.GetComponent<Image>().material;
			_riversMaterial = _lavaRivers.GetComponent<Image>().material;
			SetInitialColors();
		}

		public void SetPixel(float amount)
		{
			_planetMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
			_cratersMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
			_riversMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
		}

		public void SetLight(Vector2 pos)
		{
			_planetMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
			_cratersMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
			_riversMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
		}

		public void SetSeed(float seed)
		{
			var convertedSeed = seed % 1000f / 100f;
			_planetMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_cratersMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_riversMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
		}

		public void SetRotate(float r)
		{
			_planetMaterial.SetFloat(ShaderProperties.KeyRotation, r);
			_cratersMaterial.SetFloat(ShaderProperties.KeyRotation, r);
			_riversMaterial.SetFloat(ShaderProperties.KeyRotation, r);
		}

		public void UpdateTime(float time)
		{
			_planetMaterial.SetFloat(ShaderProperties.KeyTime, time * 0.5f);
			_cratersMaterial.SetFloat(ShaderProperties.KeyTime, time * 0.5f);
			_riversMaterial.SetFloat(ShaderProperties.KeyTime, time * 0.5f);
		}

		public void SetCustomTime(float time)
		{
			var dt = 10f + time * 60f;
			_planetMaterial.SetFloat(ShaderProperties.KeyTime, dt * 0.5f);
			_cratersMaterial.SetFloat(ShaderProperties.KeyTime, dt * 0.5f);
			_riversMaterial.SetFloat(ShaderProperties.KeyTime, dt * 0.5f);
		}

		public void SetInitialColors()
		{
			for (var i = 0; i < _colorVars1.Length; i++)
				_planetMaterial.SetColor(_colorVars1[i], ColorUtil.FromRGB(_initColors1[i]));
			for (var i = 0; i < _colorVars2.Length; i++)
				_cratersMaterial.SetColor(_colorVars2[i], ColorUtil.FromRGB(_initColors2[i]));
			for (var i = 0; i < _colorVars3.Length; i++)
				_riversMaterial.SetColor(_colorVars3[i], ColorUtil.FromRGB(_initColors3[i]));
		}

		public Color[] GetColors()
		{
			var colors = new Color[8];
			var pos = 0;
			for (var i = 0; i < _colorVars1.Length; i++)
				colors[i] = _planetMaterial.GetColor(_colorVars1[i]);
			pos = _colorVars1.Length;
			for (var i = 0; i < _colorVars2.Length; i++)
				colors[i + pos] = _cratersMaterial.GetColor(_colorVars2[i]);
			pos = _colorVars1.Length + _colorVars2.Length;
			for (var i = 0; i < _colorVars3.Length; i++)
				colors[i + pos] = _riversMaterial.GetColor(_colorVars3[i]);
			return colors;
		}

		public void SetColors(Color[] colors)
		{
			for (var i = 0; i < _colorVars1.Length; i++)
				_planetMaterial.SetColor(_colorVars1[i], colors[i]);
			for (var i = 0; i < _colorVars2.Length; i++)
				_cratersMaterial.SetColor(_colorVars2[i], colors[i + _colorVars1.Length]);
			for (var i = 0; i < _colorVars3.Length; i++)
				_riversMaterial.SetColor(_colorVars3[i], colors[i + _colorVars1.Length + _colorVars2.Length]);
		}
	}
}