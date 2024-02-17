using System;
using UnityEngine;
using UnityEngine.UI;

namespace ColorGradientPicker.Scripts
{
    public class ColorPicker : MonoBehaviour
    {
        /// <summary>
        /// Event that gets called by the ColorPicker
        /// </summary>
        /// <param name="c">received Color</param>
        public delegate void Color(UnityEngine.Color c);

        private static ColorPicker _instance;
        /// <returns>
        /// True when the ColorPicker is closed
        /// </returns>
        public static bool done = true;

        //onColorChanged event
        private static Color _onCc;
        //onColorSelected event
        private static Color _onCs;

        //Color before editing
        private static Color32 _originalColor;
        //current Color
        private static Color32 _modifiedColor;
        private static HSV _modifiedHsv;

        //useAlpha bool
        private static bool _useA;

        private bool _interact;

        // these can only work with the prefab and its children
        public RectTransform positionIndicator;
        public Slider mainComponent;
        public Slider rComponent;
        public Slider gComponent;
        public Slider bComponent;
        public Slider aComponent;
        public InputField hexaComponent;
        public RawImage colorComponent;

        private void Awake()
        {
            _instance = this;
            gameObject.SetActive(false);
        }


        /// <summary>
        /// Creates a new Colorpicker
        /// </summary>
        /// <param name="original">Color before editing</param>
        /// <param name="message">Display message</param>
        /// <param name="onColorChanged">Event that gets called when the color gets modified</param>
        /// <param name="onColorSelected">Event that gets called when one of the buttons done or cancel get pressed</param>
        /// <param name="useAlpha">When set to false the colors used don't have an alpha channel</param>
        /// <returns>
        /// False if the instance is already running
        /// </returns>
        public static bool Create(UnityEngine.Color original, string message, Color onColorChanged, Color onColorSelected, bool useAlpha = false)
        {   
            if(_instance is null)
            {
                Debug.LogError("No Colorpicker prefab active on 'Start' in scene");
                return false;
            }
            if(done)
            {
                done = false;
                _originalColor = original;
                _modifiedColor = original;
                _onCc = onColorChanged;
                _onCs = onColorSelected;
                _useA = useAlpha;
                _instance.gameObject.SetActive(true);
                _instance.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = message;
                _instance.aComponent.gameObject.SetActive(useAlpha);
                _instance.RecalculateMenu(true);
                _instance.hexaComponent.placeholder.GetComponent<Text>().text = "RRGGBB" + (useAlpha ? "AA" : "");
                return true;
            }
            else
            {
                Done();
                return false;
            }
        }

        //called when color is modified, to update other UI components
        private void RecalculateMenu(bool recalculateHSV)
        {
            _interact = false;
            if(recalculateHSV)
            {
                _modifiedHsv = new HSV(_modifiedColor);
            }
            else
            {
                _modifiedColor = _modifiedHsv.ToColor();
            }
            rComponent.value = _modifiedColor.r;
            rComponent.transform.GetChild(3).GetComponent<InputField>().text = _modifiedColor.r.ToString();
            gComponent.value = _modifiedColor.g;
            gComponent.transform.GetChild(3).GetComponent<InputField>().text = _modifiedColor.g.ToString();
            bComponent.value = _modifiedColor.b;
            bComponent.transform.GetChild(3).GetComponent<InputField>().text = _modifiedColor.b.ToString();
            if (_useA)
            {
                aComponent.value = _modifiedColor.a;
                aComponent.transform.GetChild(3).GetComponent<InputField>().text = _modifiedColor.a.ToString();
            }
            mainComponent.value = (float)_modifiedHsv.H;
            rComponent.transform.GetChild(0).GetComponent<RawImage>().color = new Color32(255, _modifiedColor.g, _modifiedColor.b, 255);
            rComponent.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = new Color32(0, _modifiedColor.g, _modifiedColor.b, 255);
            gComponent.transform.GetChild(0).GetComponent<RawImage>().color = new Color32(_modifiedColor.r, 255, _modifiedColor.b, 255);
            gComponent.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = new Color32(_modifiedColor.r, 0, _modifiedColor.b, 255);
            bComponent.transform.GetChild(0).GetComponent<RawImage>().color = new Color32(_modifiedColor.r, _modifiedColor.g, 255, 255);
            bComponent.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = new Color32(_modifiedColor.r, _modifiedColor.g, 0, 255);
            if (_useA) aComponent.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = new Color32(_modifiedColor.r, _modifiedColor.g, _modifiedColor.b, 255);
            positionIndicator.parent.GetChild(0).GetComponent<RawImage>().color = new HSV(_modifiedHsv.H, 1d, 1d).ToColor();
            positionIndicator.anchorMin = new Vector2((float)_modifiedHsv.S, (float)_modifiedHsv.V);
            positionIndicator.anchorMax = positionIndicator.anchorMin;
            hexaComponent.text = _useA ? ColorUtility.ToHtmlStringRGBA(_modifiedColor) : ColorUtility.ToHtmlStringRGB(_modifiedColor);
            colorComponent.color = _modifiedColor;
            _onCc?.Invoke(_modifiedColor);
            _interact = true;
        }

        //used by EventTrigger to calculate the chosen value in color box
        public void SetChooser()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(positionIndicator.parent as RectTransform, Input.mousePosition, GetComponentInParent<Canvas>().worldCamera, out Vector2 localpoint);
            localpoint = Rect.PointToNormalized((positionIndicator.parent as RectTransform).rect, localpoint);
            if (positionIndicator.anchorMin != localpoint)
            {
                positionIndicator.anchorMin = localpoint;
                positionIndicator.anchorMax = localpoint;
                _modifiedHsv.S = localpoint.x;
                _modifiedHsv.V = localpoint.y;
                RecalculateMenu(false);
            }
        }

        //gets main Slider value
        public void SetMain(float value)
        {
            if (_interact)
            {
                _modifiedHsv.H = value;
                RecalculateMenu(false);
            }
        }

        //gets r Slider value
        public void SetR(float value)
        {
            if (_interact)
            {
                _modifiedColor.r = (byte)value;
                RecalculateMenu(true);
            }
        }
        //gets r InputField value
        public void SetR(string value)
        {
            if(_interact)
            {
                _modifiedColor.r = (byte)Mathf.Clamp(int.Parse(value), 0, 255);
                RecalculateMenu(true);
            }
        }
        //gets g Slider value
        public void SetG(float value)
        {
            if(_interact)
            {
                _modifiedColor.g = (byte)value;
                RecalculateMenu(true);
            }
        }
        //gets g InputField value
        public void SetG(string value)
        {
            if (_interact)
            {
                _modifiedColor.g = (byte)Mathf.Clamp(int.Parse(value), 0, 255);
                RecalculateMenu(true);
            }
        }
        //gets b Slider value
        public void SetB(float value)
        {
            if (_interact)
            {
                _modifiedColor.b = (byte)value;
                RecalculateMenu(true);
            }
        }
        //gets b InputField value
        public void SetB(string value)
        {
            if (_interact)
            {
                _modifiedColor.b = (byte)Mathf.Clamp(int.Parse(value), 0, 255);
                RecalculateMenu(true);
            }
        }
        //gets a Slider value
        public void SetA(float value)
        {
            if (_interact)
            {
                _modifiedHsv.A = (byte)value;
                RecalculateMenu(false);
            }
        }
        //gets a InputField value
        public void SetA(string value)
        {
            if (_interact)
            {
                _modifiedHsv.A = (byte)Mathf.Clamp(int.Parse(value), 0, 255);
                RecalculateMenu(false);
            }
        }
        //gets hexa InputField value
        public void SetHexa(string value)
        {
            if (_interact)
            {
                if (ColorUtility.TryParseHtmlString("#" + value, out UnityEngine.Color c))
                {
                    if (!_useA) c.a = 1;
                    _modifiedColor = c;
                    RecalculateMenu(true);
                }
                else
                {
                    hexaComponent.text = _useA ? ColorUtility.ToHtmlStringRGBA(_modifiedColor) : ColorUtility.ToHtmlStringRGB(_modifiedColor);
                }
            }
        }
        //cancel button call
        public void CCancel()
        {
            Cancel();
        }
        /// <summary>
        /// Manually cancel the ColorPicker and recover the default value
        /// </summary>
        public static void Cancel()
        {
            _modifiedColor = _originalColor;
            Done();
        }
        //done button call
        public void CDone()
        {
            Done();
        }
        /// <summary>
        /// Manually close the ColorPicker and apply the selected color
        /// </summary>
        public static void Done()
        {
            done = true;
            _onCc?.Invoke(_modifiedColor);
            _onCs?.Invoke(_modifiedColor);
            _instance.transform.gameObject.SetActive(false);
        }
        //HSV helper class
        private sealed class HSV
        {
            public double H = 0, S = 1, V = 1;
            public byte A = 255;
            public HSV () { }
            public HSV (double h, double s, double v)
            {
                H = h;
                S = s;
                V = v;
            }
            public HSV (UnityEngine.Color color)
            {
                float max = Mathf.Max(color.r, Mathf.Max(color.g, color.b));
                float min = Mathf.Min(color.r, Mathf.Min(color.g, color.b));

                float hue = (float)H;
                if (min != max)
                {
                    if (max == color.r)
                    {
                        hue = (color.g - color.b) / (max - min);

                    }
                    else if (max == color.g)
                    {
                        hue = 2f + (color.b - color.r) / (max - min);

                    }
                    else
                    {
                        hue = 4f + (color.r - color.g) / (max - min);
                    }

                    hue *= 60;
                    if (hue < 0) hue += 360;
                }

                H = hue;
                S = (max == 0) ? 0 : 1d - ((double)min / max);
                V = max;
                A = (byte)(color.a * 255);
            }
            public Color32 ToColor()
            {
                int hi = Convert.ToInt32(Math.Floor(H / 60)) % 6;
                double f = H / 60 - Math.Floor(H / 60);

                double value = V * 255;
                byte v = (byte)Convert.ToInt32(value);
                byte p = (byte)Convert.ToInt32(value * (1 - S));
                byte q = (byte)Convert.ToInt32(value * (1 - f * S));
                byte t = (byte)Convert.ToInt32(value * (1 - (1 - f) * S));

                switch(hi)
                {
                    case 0:
                        return new Color32(v, t, p, A);
                    case 1:
                        return new Color32(q, v, p, A);
                    case 2:
                        return new Color32(p, v, t, A);
                    case 3:
                        return new Color32(p, q, v, A);
                    case 4:
                        return new Color32(t, p, v, A);
                    case 5:
                        return new Color32(v, p, q, A);
                    default:
                        return new Color32();
                }
            }
        }
    }
}