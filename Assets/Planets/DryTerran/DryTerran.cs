using Planets.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Planets.DryTerran
{
	public class DryTerran : MonoBehaviour, IPlanet
	{
		[FormerlySerializedAs("Land")] [SerializeField]
		private GameObject _land;

		[SerializeField] private GradientTextureGenerate _gradientLand;
		private Material _landMaterial;
		private readonly GradientColorKey[] _colorKey = new GradientColorKey[5];
		private readonly GradientAlphaKey[] _alphaKey = new GradientAlphaKey[5];

		private readonly string[] _colors1 =
		{
			"#ff8933",
			"#e64539",
			"#ad2f45",
			"#52333f",
			"#3d2936"
		};

		private readonly float[] _colorTimes =
		{
			0,
			0.2f,
			0.4f,
			0.6f,
			0.8f,
			1.0f
		};

		private void Awake()
		{
			_landMaterial = _land.GetComponent<Image>().material;
			SetGragientColor();
		}

		public void SetPixel(float amount)
		{
			_landMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
		}

		public void SetLight(Vector2 pos)
		{
			_landMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
		}

		public void SetSeed(float seed)
		{
			var convertedSeed = seed % 1000f / 100f;
			_landMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
		}

		public void SetRotate(float r)
		{
			_landMaterial.SetFloat(ShaderProperties.KeyRotation, r);
		}

		public void UpdateTime(float time)
		{
			_landMaterial.SetFloat(ShaderProperties.KeyTime, time);
		}

		public void SetCustomTime(float time)
		{
			var dt = 10f + time * 60f;
			_landMaterial.SetFloat(ShaderProperties.KeyTime, dt);
		}

		public void SetInitialColors()
		{
			SetGragientColor();
		}

		private void SetGragientColor()
		{
			for (var i = 0; i < _colorKey.Length; i++)
			{
				_colorKey[i].color = default;
				ColorUtility.TryParseHtmlString(_colors1[i], out _colorKey[i].color);

				_colorKey[i].time = _colorTimes[i];
				_alphaKey[i].alpha = 1.0f;
				_alphaKey[i].time = _colorTimes[i];
			}

			_gradientLand.SetColors(_colorKey, _alphaKey);
		}

		public Color[] GetColors()
		{
			var colors = new Color[5];
			var gradColors = _gradientLand.GetColorKeys();
			for (var i = 0; i < gradColors.Length; i++)
				colors[i] = gradColors[i].color;
			return colors;
		}

		public void SetColors(Color[] colors)
		{
			for (var i = 0; i < _colorKey.Length; i++)
			{
				_colorKey[i].color = colors[i];
				_colorKey[i].time = _colorTimes[i];
				_alphaKey[i].alpha = 1.0f;
				_alphaKey[i].time = _colorTimes[i];
			}

			_gradientLand.SetColors(_colorKey, _alphaKey);
		}
	}
}