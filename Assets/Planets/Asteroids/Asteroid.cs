using Planets.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Planets.Asteroids
{
	public class Asteroid : MonoBehaviour, IPlanet
	{
		[FormerlySerializedAs("g_Asteroid")] [SerializeField]
		private GameObject _asteroid;

		private Material _asteroidMaterial;

		private readonly string[] _colorVars =
		{
			"_Color1",
			"_Color2",
			"_Color3"
		};

		private readonly string[] _initColors =
		{
			"#a3a7c2",
			"#4c6885",
			"#3a3f5e"
		};

		private void Awake()
		{
			_asteroidMaterial = _asteroid.GetComponent<Image>().material;
			SetInitialColors();
		}

		public void SetPixel(float amount)
		{
			_asteroidMaterial.SetFloat(ShaderProperties.KeyPixels, amount);
		}

		public void SetLight(Vector2 pos)
		{
			_asteroidMaterial.SetVector(ShaderProperties.KeyLightOrigin, pos);
		}

		public void SetSeed(float seed)
		{
			var convertedSeed = seed % 1000f / 100f;
			_asteroidMaterial.SetFloat(ShaderProperties.KeySeed, convertedSeed);
		}

		public void SetRotate(float r)
		{
			_asteroidMaterial.SetFloat(ShaderProperties.KeyRotation, r);
		}

		public void UpdateTime(float time)
		{
			return;
		}

		public void SetCustomTime(float time)
		{
			var dt = time * 6.28f;
			time = Mathf.Clamp(
				dt,
				0.1f,
				6.28f);
			_asteroidMaterial.SetFloat(ShaderProperties.KeyRotation, time);
		}

		public void SetInitialColors()
		{
			for (var i = 0; i < _colorVars.Length; i++)
				_asteroidMaterial.SetColor(_colorVars[i], ColorUtil.FromRGB(_initColors[i]));
		}

		public Color[] GetColors()
		{
			var colors = new Color[3];
			for (var i = 0; i < _colorVars.Length; i++)
				colors[i] = _asteroidMaterial.GetColor(_colorVars[i]);
			return colors;
		}

		public void SetColors(Color[] colors)
		{
			for (var i = 0; i < colors.Length; i++)
				_asteroidMaterial.SetColor(_colorVars[i], colors[i]);
		}
	}
}