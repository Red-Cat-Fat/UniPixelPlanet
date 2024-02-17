using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Planets.Scripts
{
	public class MaterialSave : MonoBehaviour
	{
		private int _baseWidth = 100;
		private int _baseHeight = 100;

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);

    // Browser plugin should be called in OnPointerDown.
    //public void OnPointerDown(PointerEventData eventData) {
    //    DownloadFile(gameObject.name, "OnFileDownload", "sample.png", _textureBytes, _textureBytes.Length);
    //}

    // Called from browser
    public void OnFileDownload() {
        
    }
#endif
		public void SaveImage(List<Material> mats, string filename)
		{
			var texture = Texture2D.whiteTexture;

			var renderTexture = RenderTexture.GetTemporary(
				_baseWidth,
				_baseHeight,
				0);


			foreach (var mat in mats)
				Graphics.Blit(
					texture,
					renderTexture,
					mat);

			RenderTexture.active = renderTexture;
			var tex = new Texture2D(
				renderTexture.width,
				renderTexture.height,
				TextureFormat.RGBA32,
				false);
			tex.ReadPixels(
				new Rect(
					0f,
					0f,
					renderTexture.width,
					renderTexture.height),
				0,
				0);
			tex.Apply();
			var bytes = tex.EncodeToPNG();
			DestroyImmediate(tex);
			RenderTexture.ReleaseTemporary(renderTexture);

#if UNITY_WEBGL && !UNITY_EDITOR
        DownloadFile(gameObject.name, "OnFileDownload", filename + ".png",bytes, bytes.Length);
#else
			var path = StandaloneFileBrowser.StandaloneFileBrowser.SaveFilePanel(
				"Save Png",
				"",
				filename,
				"png");
			File.WriteAllBytes(path, bytes);
#endif
		}

		public void SaveSheets(
			List<Material> mats,
			string filename,
			int w,
			int h,
			IPlanet planet,
			int customSize
		)
		{
			StartCoroutine(
				SaveSheetCoroutine(
					mats,
					filename,
					w,
					h,
					planet,
					customSize));
		}

		private IEnumerator SaveSheetCoroutine(
			List<Material> mats,
			string filename,
			int w,
			int h,
			IPlanet planet,
			int size
		)
		{
			_baseWidth = _baseHeight = size;

			var renderTexture = RenderTexture.GetTemporary(
				_baseWidth,
				_baseHeight,
				0);

			var tex = new Texture2D(
				_baseWidth * w,
				_baseHeight * h,
				TextureFormat.RGBA32,
				false);

			var index = 1;
			for (var y = 0; y < h; y++)
			{
				for (var x = 0; x < w; x++)
				{
					var texture = Texture2D.whiteTexture;
					foreach (var mat in mats)
						Graphics.Blit(
							texture,
							renderTexture,
							mat);
					RenderTexture.active = renderTexture;

					tex.ReadPixels(
						new Rect(
							0f,
							0f,
							renderTexture.width,
							renderTexture.height),
						_baseWidth * x,
						_baseHeight * y);

					var atime = (float)index / (w * h);
					var t = Mathf.Lerp(
						0f,
						1f,
						atime);
					planet.SetCustomTime(t);

					index++;
				}
			}

			// apply tex and write png file
			tex.Apply();
			var bytes = tex.EncodeToPNG();
			DestroyImmediate(tex);
			RenderTexture.ReleaseTemporary(renderTexture);
			yield return new WaitForEndOfFrame();

#if UNITY_WEBGL && !UNITY_EDITOR
        DownloadFile(gameObject.name, "OnFileDownload", filename+ ".png",bytes, bytes.Length);
#else
			var path = StandaloneFileBrowser.StandaloneFileBrowser.SaveFilePanel(
				"Save Sprite Sheets",
				"",
				filename,
				"png");
			File.WriteAllBytes(path, bytes);
#endif
		}
	}
}