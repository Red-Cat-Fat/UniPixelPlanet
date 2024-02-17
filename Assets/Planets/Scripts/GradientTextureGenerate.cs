using UnityEngine;
using UnityEngine.Serialization;

namespace Planets.Scripts
{
	[ExecuteInEditMode]
	public class GradientTextureGenerate : MonoBehaviour
	{
		[FormerlySerializedAs("gradient")] [SerializeField]
		private Gradient _gradient;

		[FormerlySerializedAs("targetMaterial")] [SerializeField]
		private Material _targetMaterial;

		[FormerlySerializedAs("Shader_key")] [SerializeField]
		private string _shader_key = "";

		private GradientColorKey[] _colorKeys;

		private void Awake()
		{
			//  targetMaterial = GetComponent<Image>().material;    
			//Material material = new Material(Shader.Find("Unlit/DryTerran"));
			_targetMaterial.SetTexture(_shader_key, CreateTexture());
			//GetComponent<Renderer>().material = material;
		}

		public void SetColors(
			GradientColorKey[] colorKey,
			GradientAlphaKey[] alphaKey,
			string key = ""
		)
		{
			if (string.IsNullOrEmpty(key))
				key = _shader_key;
			_gradient = new Gradient();
			_gradient.SetKeys(colorKey, alphaKey);
			_targetMaterial.SetTexture(key, CreateTexture());
			_colorKeys = new GradientColorKey[colorKey.Length];
			_colorKeys = colorKey;
		}

		public GradientColorKey[] GetColorKeys()
		{
			return _colorKeys;
		}

		private Texture2D CreateTexture()
		{
			var texture = new Texture2D(128, 1);
			for (var h = 0; h < texture.height; h++)
			{
				for (var w = 0; w < texture.width; w++)
					texture.SetPixel(
						w,
						h,
						_gradient.Evaluate((float)w / texture.width));
			}

			texture.Apply();
			texture.wrapMode = TextureWrapMode.Clamp;
			return texture;
		}
	}
}