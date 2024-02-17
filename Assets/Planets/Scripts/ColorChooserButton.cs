using UnityEngine;

namespace Planets.Scripts
{
	public class ColorChooserButton : MonoBehaviour
	{
		private int _buttonID;

		public int ButtonID
		{
			get => _buttonID;
			set => _buttonID = value;
		}
	}
}