using UnityEngine;
using UnityEngine.Serialization;

namespace Planets.Scripts
{
	[ExecuteInEditMode]
	public class GradientTextureMultiGenerate : MonoBehaviour
	{
		[FormerlySerializedAs("gradient1")] [SerializeField] private Gradient _gradient1;
		[FormerlySerializedAs("gradient2")] [SerializeField] private Gradient _gradient2;
		[FormerlySerializedAs("mat1")] [SerializeField] private Material _mat1;
		[FormerlySerializedAs("mat2")] [SerializeField] private Material _mat2;

		private void Awake()
		{
			//Material material = new Material(Shader.Find("Unlit/DryTerran"));
			_mat1.SetTexture(ShaderProperties.KeyTextureKeyword1, CreateTexture1());
			_mat1.SetTexture(ShaderProperties.KeyTextureKeyword2, CreateTexture2());
			_mat2.SetTexture(ShaderProperties.KeyTextureKeyword1, CreateTexture1());
			_mat2.SetTexture(ShaderProperties.KeyTextureKeyword2, CreateTexture2());
			//GetComponent<Renderer>().material = material;
		}

		private Texture2D CreateTexture1()
		{
			var texture = new Texture2D(128, 1);
			for (var h = 0; h < texture.height; h++)
			{
				for (var w = 0; w < texture.width; w++)
					texture.SetPixel(
						w,
						h,
						_gradient1.Evaluate((float)w / texture.width));
			}

			texture.Apply();
			texture.wrapMode = TextureWrapMode.Clamp;
			return texture;
		}

		private Texture2D CreateTexture2()
		{
			var texture = new Texture2D(128, 1);
			for (var h = 0; h < texture.height; h++)
			{
				for (var w = 0; w < texture.width; w++)
					texture.SetPixel(
						w,
						h,
						_gradient2.Evaluate((float)w / texture.width));
			}

			texture.Apply();
			texture.wrapMode = TextureWrapMode.Clamp;
			return texture;
		}
	}
}