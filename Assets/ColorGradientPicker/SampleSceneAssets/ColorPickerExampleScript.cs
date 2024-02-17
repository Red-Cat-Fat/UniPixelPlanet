using ColorGradientPicker.Scripts;
using UnityEngine;

namespace ColorGradientPicker.SampleSceneAssets
{
    public class ColorPickerExampleScript : MonoBehaviour
    {
        private Renderer _r;
        void Start()
        {
            _r = GetComponent<Renderer>();
            _r.sharedMaterial = _r.material;
        }
        public void ChooseColorButtonClick()
        {
            ColorPicker.Create(_r.sharedMaterial.color, "Choose the cube's color!", SetColor, ColorFinished, true);
        }
        private void SetColor(Color currentColor)
        {
            _r.sharedMaterial.color = currentColor;
        }

        private void ColorFinished(Color finishedColor)
        {
            Debug.Log("You chose the color " + ColorUtility.ToHtmlStringRGBA(finishedColor));
        }
    }
}
