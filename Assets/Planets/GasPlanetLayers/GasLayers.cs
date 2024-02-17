using Planets.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Planets.GasPlanetLayers
{
	public class GasLayers : MonoBehaviour, IPlanet
	{
		private const string GradientVars = "_ColorScheme";
		private const string GradientDarkVars = "_Dark_ColorScheme";
		private readonly GradientColorKey[] _colorKey1 = new GradientColorKey[3];
		private readonly GradientColorKey[] _colorKey2 = new GradientColorKey[3];
		private readonly GradientAlphaKey[] _alphaKey = new GradientAlphaKey[3];

		[FormerlySerializedAs("_GasLayers")] [SerializeField]
		private GameObject _gasLayers;

		[FormerlySerializedAs("_Ring")] [SerializeField]
		private GameObject _ring;

		[SerializeField] private GradientTextureGenerate _gradientGas1;
		[SerializeField] private GradientTextureGenerate _gradientGas2;
		[SerializeField] private GradientTextureGenerate _gradientRing1;
		[SerializeField] private GradientTextureGenerate _gradientRing2;

		private Material _gasLayersMaterial;
		private Material _ringMaterial;

		private readonly string[] _colors1 =
		{
			"#eec39a",
			"#d9a066",
			"#8f563b"
		};

		private readonly string[] _colors2 =
		{
			"#663931",
			"#45283c",
			"#222034"
		};

		private readonly float[] _colorTimes =
		{
			0,
			0.5f,
			1.0f
		};

		private void Awake()
		{
			_gasLayersMaterial = _gasLayers.GetComponent<Image>().material;
			_ringMaterial = _ring.GetComponent<Image>().material;
			SetGradientColor();
		}

		public void SetPixel(float amount)
		{
			_gasLayersMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
			_ringMaterial.SetFloat(ShaderProperties.KeyPixels, amount * 3f);
		}

		public void SetLight(Vector2 pos)
		{
			_gasLayersMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos * 1.3f);
			_ringMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos * 1.3f);
		}

		public void SetSeed(float seed)
		{
			var convertedSeed = seed % 1000f / 100f;
			_gasLayersMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_ringMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			//  _Ring.SetFloat("cloud_cover", Random.Range(0.28f, 0.5f));
		}

		public void SetRotate(float r)
		{
			_gasLayersMaterial.SetFloat(ShaderProperties.KeyRotation, r);
			_ringMaterial.SetFloat(ShaderProperties.KeyRotation, r + 0.7f);
		}

		public void UpdateTime(float time)
		{
			_gasLayersMaterial.SetFloat(ShaderProperties.KeyTime, time * 0.5f);
			_ringMaterial.SetFloat(ShaderProperties.KeyTime, time * 0.5f * -3f);
		}

		public void SetCustomTime(float time)
		{
			var dt = 10f + time * 60f;
			_gasLayersMaterial.SetFloat(ShaderProperties.KeyTime, dt * 0.5f);
			_ringMaterial.SetFloat(ShaderProperties.KeyTime, dt * 0.5f * -3f);
		}

		public void SetInitialColors()
		{
			SetGradientColor();
		}

		private void SetGradientColor()
		{
			for (var i = 0; i < _colorKey1.Length; i++)
			{
				_colorKey1[i].color = default;
				ColorUtility.TryParseHtmlString(_colors1[i], out _colorKey1[i].color);

				_colorKey1[i].time = _colorTimes[i];
				_alphaKey[i].alpha = 1.0f;
				_alphaKey[i].time = _colorTimes[i];
			}


			for (var i = 0; i < _colorKey2.Length; i++)
			{
				_colorKey2[i].color = default;
				ColorUtility.TryParseHtmlString(_colors2[i], out _colorKey2[i].color);

				_colorKey2[i].time = _colorTimes[i];
				_alphaKey[i].alpha = 1.0f;
				_colorKey2[i].time = _colorTimes[i];
			}

			_gradientGas1.SetColors(
				_colorKey1,
				_alphaKey,
				GradientVars);
			_gradientGas2.SetColors(
				_colorKey2,
				_alphaKey,
				GradientDarkVars);

			_gradientRing1.SetColors(
				_colorKey1,
				_alphaKey,
				GradientVars);
			_gradientRing2.SetColors(
				_colorKey2,
				_alphaKey,
				GradientDarkVars);
		}

		public Color[] GetColors()
		{
			var colors = new Color[12];
			var gradColors = _gradientGas1.GetColorKeys();
			for (var i = 0; i < gradColors.Length; i++)
				colors[i] = gradColors[i].color;
			var size = gradColors.Length;

			var gradColors2 = _gradientGas2.GetColorKeys();
			for (var i = 0; i < gradColors2.Length; i++)
				colors[i + size] = gradColors2[i].color;

			size += gradColors2.Length;

			var gradColors3 = _gradientRing1.GetColorKeys();
			for (var i = 0; i < gradColors3.Length; i++)
				colors[i + size] = gradColors3[i].color;

			size += gradColors3.Length;

			var gradColors4 = _gradientRing2.GetColorKeys();
			for (var i = 0; i < gradColors4.Length; i++)
				colors[i + size] = gradColors4[i].color;

			return colors;
		}

		public void SetColors(Color[] colors)
		{
			for (var i = 0; i < _colorKey1.Length; i++)
			{
				_colorKey1[i].color = colors[i];
				_colorKey1[i].time = _colorTimes[i];
				_alphaKey[i].alpha = 1.0f;
				_alphaKey[i].time = _colorTimes[i];
			}

			_gradientGas1.SetColors(
				_colorKey1,
				_alphaKey,
				GradientVars);
			var size = _colorKey1.Length;

			for (var i = 0; i < _colorKey2.Length; i++)
			{
				_colorKey2[i].color = colors[i + size];
				_colorKey2[i].time = _colorTimes[i];
				_alphaKey[i].alpha = 1.0f;
				_alphaKey[i].time = _colorTimes[i];
			}

			_gradientGas1.SetColors(
				_colorKey2,
				_alphaKey,
				GradientDarkVars);
			size += _colorKey2.Length;

			for (var i = 0; i < _colorKey1.Length; i++)
			{
				_colorKey1[i].color = colors[i + size];
				_colorKey1[i].time = _colorTimes[i];
				_alphaKey[i].alpha = 1.0f;
				_alphaKey[i].time = _colorTimes[i];
			}

			_gradientRing1.SetColors(
				_colorKey1,
				_alphaKey,
				GradientVars);
			size += _colorKey1.Length;

			for (var i = 0; i < _colorKey2.Length; i++)
			{
				_colorKey2[i].color = colors[i + size];
				_colorKey2[i].time = _colorTimes[i];
				_alphaKey[i].alpha = 1.0f;
				_alphaKey[i].time = _colorTimes[i];
			}

			_gradientRing2.SetColors(
				_colorKey2,
				_alphaKey,
				GradientDarkVars);
		}
	}
}