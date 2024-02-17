using ColorGradientPicker.Scripts;
using UnityEngine;

namespace ColorGradientPicker.SampleSceneAssets
{
    public class GradientPickerExample : MonoBehaviour
    {
        private Renderer _r;
        private Gradient _myGradient;
        void Start()
        {
            _r = GetComponent<Renderer>();
            _r.sharedMaterial = _r.material;
            _myGradient = new Gradient();
        }
        private void Update()
        {
            _r.sharedMaterial.color = _myGradient.Evaluate(0.5f + Mathf.Sin(Time.time * 2f) * 0.5f);
        }
        public void ChooseGradientButtonClick()
        {
            GradientPicker.Create(_myGradient, "Choose the sphere's color!", SetGradient, GradientFinished);
        }
        private void SetGradient(Gradient currentGradient)
        {
            _myGradient = currentGradient;
        }

        private void GradientFinished(Gradient finishedGradient)
        {
            Debug.Log("You chose a Gradient with " + finishedGradient.colorKeys.Length + " Color keys");
        }
    }
}
