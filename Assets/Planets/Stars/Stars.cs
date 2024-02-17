using Planets.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Planets.Stars
{
	public class Stars : MonoBehaviour, IPlanet
	{
		[SerializeField] private GameObject StarBackground;
		[SerializeField] private GameObject Star;
		[SerializeField] private GameObject StarFlares;
		private Material _mStarbackground;
		private Material _mStar;
		private Material _mStarFlares;
		[SerializeField] private GradientTextureGenerate _gradientStar;
		[SerializeField] private GradientTextureGenerate _gradientStarFlare;

		private readonly string _gradientVars = "_GradientTex";

		private readonly GradientColorKey[] _colorKey1 = new GradientColorKey[4];
		private readonly GradientColorKey[] _colorKey2 = new GradientColorKey[2];
		private readonly GradientAlphaKey[] _alphaKey1 = new GradientAlphaKey[4];
		private readonly GradientAlphaKey[] _alphaKey2 = new GradientAlphaKey[2];

		private readonly string[] _colorVars1 = { "_Color1" };
		private readonly string[] _initColors1 = { "#ffffe4" };

		private readonly string[] _colors1 =
		{
			"#f5ffe8",
			"#77d6c1",
			"#1c92a7",
			"#033e5e"
		};

		private readonly string[] _colors2 =
		{
			"#77d6c1",
			"#ffffe4"
		};

		private readonly float[] _colorTimes1 = new float[4]
		{
			0f,
			0.33f,
			0.66f,
			1.0f
		};

		private readonly float[] _colorTimes2 = new float[2]
		{
			0f,
			1.0f
		};

		private void Awake()
		{
			_mStarbackground = StarBackground.GetComponent<Image>().material;
			_mStar = Star.GetComponent<Image>().material;
			_mStarFlares = StarFlares.GetComponent<Image>().material;
			SetInitialColors();
		}

		public void SetPixel(float amount)
		{
			_mStarbackground.SetFloat(ShaderProperties.KeyPixels, amount * 2);
			_mStar.SetFloat(ShaderProperties.KeyPixels, amount);
			_mStarFlares.SetFloat(ShaderProperties.KeyPixels, amount * 2);
		}

		public void SetLight(Vector2 pos)
		{
			return;
		}

		public void SetSeed(float seed)
		{
			var convertedSeed = seed % 1000f / 100f;
			_mStarbackground.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_mStar.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			_mStarFlares.SetFloat(ShaderProperties.KeySeed, convertedSeed);
			// setGragientColor(seed);
		}

		public void SetRotate(float r)
		{
			_mStarbackground.SetFloat(ShaderProperties.KeyRotation, r);
			_mStar.SetFloat(ShaderProperties.KeyRotation, r);
			_mStarFlares.SetFloat(ShaderProperties.KeyRotation, r);
		}

		public void UpdateTime(float time)
		{
			_mStarbackground.SetFloat(ShaderProperties.KeyTime, time);
			_mStar.SetFloat(ShaderProperties.KeyTime, time * 0.1f);
			_mStarFlares.SetFloat(ShaderProperties.KeyTime, time);
		}

		public void SetCustomTime(float time)
		{
			_mStarbackground.SetFloat(ShaderProperties.KeyTime, time);
			_mStar.SetFloat(ShaderProperties.KeyTime, time);
			_mStarFlares.SetFloat(ShaderProperties.KeyTime, time);
		}

		public void SetInitialColors()
		{
			for (var i = 0; i < _colorVars1.Length; i++)
				_mStarbackground.SetColor(_colorVars1[i], ColorUtil.FromRGB(_initColors1[i]));
			SetGragientColor();
		}

		private void SetGragientColor()
		{
			for (var i = 0; i < _colorKey1.Length; i++)
			{
				_colorKey1[i].color = default;
				ColorUtility.TryParseHtmlString(_colors1[i], out _colorKey1[i].color);

				_colorKey1[i].time = _colorTimes1[i];
				_alphaKey1[i].alpha = 1.0f;
				_alphaKey1[i].time = _colorTimes1[i];
			}


			for (var i = 0; i < _colorKey2.Length; i++)
			{
				_colorKey2[i].color = default;
				ColorUtility.TryParseHtmlString(_colors2[i], out _colorKey2[i].color);

				_colorKey2[i].time = _colorTimes2[i];
				_alphaKey2[i].alpha = 1.0f;
				_colorKey2[i].time = _colorTimes2[i];
			}

			_gradientStar.SetColors(
				_colorKey1,
				_alphaKey1,
				_gradientVars);
			_gradientStarFlare.SetColors(
				_colorKey2,
				_alphaKey2,
				_gradientVars);
		}

		public Color[] GetColors()
		{
			var colors = new Color[7];
			for (var i = 0; i < _colorVars1.Length; i++)
				colors[i] = _mStarbackground.GetColor(_colorVars1[i]);
			var size = _colorVars1.Length;

			var gradColors = _gradientStar.GetColorKeys();
			for (var i = 0; i < gradColors.Length; i++)
				colors[i + size] = gradColors[i].color;
			size += gradColors.Length;

			var gradColors2 = _gradientStarFlare.GetColorKeys();
			for (var i = 0; i < gradColors2.Length; i++)
				colors[i + size] = gradColors2[i].color;

			return colors;
		}

		public void SetColors(Color[] colors)
		{
			for (var i = 0; i < _colorVars1.Length; i++)
				_mStarbackground.SetColor(_colorVars1[i], colors[i]);
			var size = _colorVars1.Length;

			for (var i = 0; i < _colorKey1.Length; i++)
			{
				_colorKey1[i].color = colors[i + size];
				_colorKey1[i].time = _colorTimes1[i];
				_alphaKey1[i].alpha = 1.0f;
				_alphaKey1[i].time = _colorTimes1[i];
			}

			_gradientStar.SetColors(
				_colorKey1,
				_alphaKey1,
				_gradientVars);
			size += _colorKey1.Length;

			for (var i = 0; i < _colorKey2.Length; i++)
			{
				_colorKey2[i].color = colors[i + size];
				_colorKey2[i].time = _colorTimes2[i];
				_alphaKey2[i].alpha = 1.0f;
				_alphaKey2[i].time = _colorTimes2[i];
			}

			_gradientStarFlare.SetColors(
				_colorKey2,
				_alphaKey2,
				_gradientVars);
		}
	}
}