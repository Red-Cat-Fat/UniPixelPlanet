using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Planets.Scripts
{
	public class ExportControl : MonoBehaviour
	{
		[FormerlySerializedAs("inputWidth")] [SerializeField]
		private InputField _inputWidth;

		[FormerlySerializedAs("inputHeight")] [SerializeField]
		private InputField _inputHeight;

		[FormerlySerializedAs("textFrame")] [SerializeField]
		private Text _textFrame;

		[FormerlySerializedAs("textResolution")] [SerializeField]
		private Text _textResolution;

		private int _width = 5;
		private int _height = 2;
		private int _frames;
		private const int MaxFrame = 100;

		private void Start()
		{
			UpdateFrame();
		}

		public int GetWidth()
		{
			return _width;
		}

		public int GetHeight()
		{
			return _height;
		}

		public void OnUpdateFrames()
		{
			int.TryParse(_inputWidth.text, out var w);
			int.TryParse(_inputHeight.text, out var h);
			if (w < 1 || w > MaxFrame)
			{
				_inputWidth.text = _width.ToString();
				return;
			}

			if (h < 1 || h > MaxFrame)
			{
				_inputHeight.text = _height.ToString();
				return;
			}

			_width = w;
			_height = h;
			UpdateFrame();
		}

		private void UpdateFrame()
		{
			_frames = _width * _height;

			_inputWidth.text = _width.ToString();
			_textFrame.text = _frames.ToString();
			_textResolution.text = (_width * 100).ToString() + "x" + (_height * 100).ToString();
		}

		public void OnWidthUp()
		{
			if (_width < MaxFrame)
				_width++;
			UpdateFrame();
		}

		public void OnWidthDown()
		{
			if (_width > 1)
				_width--;
			UpdateFrame();
		}

		public void OnHeightUp()
		{
			if (_height < MaxFrame)
				_height++;
			UpdateFrame();
		}

		public void OnHeightDown()
		{
			if (_height > 1)
				_height--;
			UpdateFrame();
		}
	}
}