using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ColorGradientPicker.Scripts
{
    public class GradientPicker : MonoBehaviour
    {
        /// <summary>
        /// Event that gets called by the GradientPicker.
        /// </summary>
        /// <param name="g">received Gradient</param>
        public delegate void Gradient(UnityEngine.Gradient g);

        private static GradientPicker _instance;
        /// <summary>
        /// True when the GradientPicker is closed
        /// </summary>
        public static bool done = true;


        //onGradientChanged Event
        private static Gradient _onGC;
        //onGradientSelected Event
        private static Gradient _onGs;

        //Gradient before editing
        private static UnityEngine.Gradient _originalGradient;
        //current Gradient
        private static UnityEngine.Gradient _modifiedGradient;

        //key template
        private GameObject _key;

        private static bool _interact;


        //all these objects only work on Prefab
        private InputField _positionComponent;
        private Image _colorComponent;
        private Transform _alphaComponent;

        private List<Slider> _colorKeyObjects;
        private List<GradientColorKey> _colorKeys;
        private int _selectedColorKey;
        private List<Slider> _alphaKeyObjects;
        private List<GradientAlphaKey> _alphaKeys;
        private int _selectedAlphaKey;

        private void Awake()
        {
            _instance = this;
            _key = transform.GetChild(2).gameObject;
            _positionComponent = transform.parent.GetChild(3).GetComponent<InputField>();
            _colorComponent = transform.parent.GetChild(4).GetComponent<Image>();
            _alphaComponent = transform.parent.GetChild(5);
            transform.parent.gameObject.SetActive(false);
        }
        /// <summary>
        /// Creates a new GradiantPicker
        /// </summary>
        /// <param name="original">Color before editing</param>
        /// <param name="message">Display message</param>
        /// <param name="onGradientChanged">Event that gets called when the gradient gets modified</param>
        /// <param name="onGradientSelected">Event that gets called when one of the buttons done or cancel gets pressed</param>
        /// <returns>False if the instance is already running</returns>
        public static bool Create(UnityEngine.Gradient original, string message, Gradient onGradientChanged, Gradient onGradientSelected)
        {
            if (_instance is null)
            {
                Debug.LogError("No Gradientpicker prefab active on 'Start' in scene");
                return false;
            }
            if (done)
            {
                done = false;
                _originalGradient = new UnityEngine.Gradient();
                _originalGradient.SetKeys(original.colorKeys, original.alphaKeys);
                _modifiedGradient = new UnityEngine.Gradient();
                _modifiedGradient.SetKeys(original.colorKeys, original.alphaKeys);
                _onGC = onGradientChanged;
                _onGs = onGradientSelected;
                _instance.transform.parent.gameObject.SetActive(true);
                _instance.transform.parent.GetChild(0).GetChild(0).GetComponent<Text>().text = message;
                _instance.Setup();
                return true;
            }
            else
            {
                Done();
                return false;
            }
        }
        //Setup new GradientPicker
        private void Setup()
        {
            _interact = false;
            _colorKeyObjects = new List<Slider>();
            _colorKeys = new List<GradientColorKey>();
            _alphaKeyObjects = new List<Slider>();
            _alphaKeys = new List<GradientAlphaKey>();
            foreach (GradientColorKey k in _originalGradient.colorKeys)
            {
                CreateColorKey(k);
            }
            foreach (GradientAlphaKey k in _originalGradient.alphaKeys)
            {
                CreateAlphaKey(k);
            }
            CalculateTexture();
            _interact = true;
        }
        //creates a ColorKey UI object
        private void CreateColorKey(GradientColorKey k)
        {
            if (_colorKeys.Count < 8)
            {
                Slider s = Instantiate(_key, transform.position, new Quaternion(), transform).GetComponent<Slider>();
                ((RectTransform)s.transform).anchoredPosition = new Vector2(0, -29f);
                s.name = "ColorKey";
                s.gameObject.SetActive(true);
                s.value = k.time;
                s.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = k.color;
                _colorKeyObjects.Add(s);
                _colorKeys.Add(k);
                ChangeSelectedColorKey(_colorKeys.Count - 1);
            }
        }
        //checks if new ColorKey should be created
        public void CreateNewColorKey(float time)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _interact = false;
                CreateColorKey(new GradientColorKey(_modifiedGradient.Evaluate(time), time));
                _interact = true;
            }
        }
        //creates a AlphaKey UI object
        private void CreateAlphaKey(GradientAlphaKey k)
        {
            if (_alphaKeys.Count < 8)
            {
                Slider s = Instantiate(_key, transform.position, new Quaternion(), transform).GetComponent<Slider>();
                ((RectTransform)s.transform).anchoredPosition = new Vector2(0, 25f);
                s.transform.GetChild(0).GetChild(0).rotation = new Quaternion();
                s.name = "AlphaKey";
                s.gameObject.SetActive(true);
                s.value = k.time;
                s.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(k.alpha, k.alpha, k.alpha, 1f);
                _alphaKeyObjects.Add(s);
                _alphaKeys.Add(k);
                ChangeSelectedAlphaKey(_alphaKeys.Count - 1);
            }
        }
        //checks if new AlphaKey should be created
        public void CreateNewAlphaKey(float time)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _interact = false;
                CreateAlphaKey(new GradientAlphaKey(_modifiedGradient.Evaluate(time).a, time));
                _interact = true;
            }
        }

        private void CalculateTexture()
        {
            Color[] g = new Color[325];
            for (int i = 0; i < g.Length; i++)
            {
                g[i] = _modifiedGradient.Evaluate(i / (float)g.Length);
            }
            Texture2D tex = new Texture2D(g.Length, 1)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear
            };
            tex.SetPixels(g);
            tex.Apply();
            GetComponent<RawImage>().texture = tex;
            _onGC?.Invoke(_modifiedGradient);
        }
        //accessed by alpha Slider
        public void SetAlpha(float value)
        {
            if (_interact)
            {
                _alphaKeys[_selectedAlphaKey] = new GradientAlphaKey(value, _alphaKeys[_selectedAlphaKey].time);
                _modifiedGradient.SetKeys(_colorKeys.ToArray(), _alphaKeys.ToArray());
                CalculateTexture();
                _alphaComponent.GetChild(4).GetComponent<InputField>().text = Mathf.RoundToInt(value * 255f).ToString();
                _alphaKeyObjects[_selectedAlphaKey].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(value, value, value, 1f);
            }
        }
        //accessed by alpha InputField
        public void SetAlpha(string value)
        {
            _alphaComponent.GetComponent<Slider>().value = Mathf.Clamp(int.Parse(value), 0, 255) / 255f;
            CalculateTexture();
        }

        private void ChangeSelectedColorKey(int value)
        {
            if (_colorKeyObjects.Count() > _selectedColorKey)
            {
                _colorKeyObjects[_selectedColorKey].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.gray;
            }
            if (_alphaKeyObjects.Count() > 0)
            {
                _alphaKeyObjects[_selectedAlphaKey].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.gray;
            }
            _colorKeyObjects[value].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.green;
            if (_selectedColorKey != value && !ColorPicker.done)
            {
                ColorPicker.Done();
            }
            _selectedColorKey = value;
            _colorKeyObjects[value].Select();
        }

        private void ChangeSelectedAlphaKey(int value)
        {
            if (_alphaKeyObjects.Count > _selectedAlphaKey)
            {
                _alphaKeyObjects[_selectedAlphaKey].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.gray;
            }
            if (_colorKeyObjects.Count > 0)
            {
                _colorKeyObjects[_selectedColorKey].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.gray;
            }
            _alphaKeyObjects[value].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.green;
            _selectedAlphaKey = value;
            _alphaKeyObjects[value].Select();
        }
        //checks if Key can be deleted
        public void CheckDeleteKey(Slider s)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (s.name == "ColorKey" && _colorKeys.Count > 2)
                {
                    if (!ColorPicker.done)
                    {
                        ColorPicker.Done();
                        return;
                    }
                    int index = _colorKeyObjects.IndexOf(s);
                    Destroy(_colorKeyObjects[index].gameObject);
                    _colorKeyObjects.RemoveAt(index);
                    _colorKeys.RemoveAt(index);
                    if (index <= _selectedColorKey)
                    {
                        ChangeSelectedColorKey(_selectedColorKey - 1);
                    }
                    _modifiedGradient.SetKeys(_colorKeys.ToArray(), _alphaKeys.ToArray());
                    CalculateTexture();
                }
                if(s.name == "AlphaKey" && _alphaKeys.Count > 2)
                {
                    int index = _alphaKeyObjects.IndexOf(s);
                    Destroy(_alphaKeyObjects[index].gameObject);
                    _alphaKeyObjects.RemoveAt(index);
                    _alphaKeys.RemoveAt(index);
                    if (index <= _selectedAlphaKey)
                    {
                        ChangeSelectedAlphaKey(_selectedAlphaKey - 1);
                    }
                    _modifiedGradient.SetKeys(_colorKeys.ToArray(), _alphaKeys.ToArray());
                    CalculateTexture();
                }
            }
        }
        //changes Selected Key
        public void Select()
        {
            Slider s = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();
            s.transform.SetAsLastSibling();
            if (s.name == "ColorKey")
            {
                ChangeSelectedColorKey(_colorKeyObjects.IndexOf(s));
                _alphaComponent.gameObject.SetActive(false);
                _colorComponent.gameObject.SetActive(true);
                _positionComponent.text = Mathf.RoundToInt(_colorKeys[_selectedColorKey].time * 100f).ToString();
                _colorComponent.GetComponent<Image>().color = _colorKeys[_selectedColorKey].color;
            }
            else
            {
                ChangeSelectedAlphaKey(_alphaKeyObjects.IndexOf(s));
                _colorComponent.gameObject.SetActive(false);
                _alphaComponent.gameObject.SetActive(true);
                _positionComponent.text = Mathf.RoundToInt(_alphaKeys[_selectedAlphaKey].time * 100f).ToString();
                _alphaComponent.GetComponent<Slider>().value = _alphaKeys[_selectedAlphaKey].alpha;
                _alphaComponent.GetChild(4).GetComponent<InputField>().text = Mathf.RoundToInt(_alphaKeys[_selectedAlphaKey].alpha * 255f).ToString();
            }
        }
        //accessed by position Slider
        public void SetTime(float time)
        {
            if (_interact)
            {
                Slider s = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();
                if (s.name == "ColorKey")
                {
                    int index = _colorKeyObjects.IndexOf(s);
                    _colorKeys[index] = new GradientColorKey(_colorKeys[index].color, time);
                }
                else
                {
                    int index = _alphaKeyObjects.IndexOf(s);
                    _alphaKeys[index] = new GradientAlphaKey(_alphaKeys[index].alpha, time);
                }
                _modifiedGradient.SetKeys(_colorKeys.ToArray(), _alphaKeys.ToArray());
                CalculateTexture();
                _positionComponent.text = Mathf.RoundToInt(time * 100f).ToString();
            }
        }
        //accessed by position InputField
        public void SetTime(string time)
        {
            _interact = false;
            float t = Mathf.Clamp(int.Parse(time), 0, 100) * 0.01f;
            if (_colorComponent.gameObject.activeSelf)
            {
                _colorKeyObjects[_selectedColorKey].value = t;
                _colorKeys[_selectedColorKey] = new GradientColorKey(_colorKeys[_selectedColorKey].color, t);
            }
            else
            {
                _alphaKeyObjects[_selectedAlphaKey].value = t;
                _alphaKeys[_selectedAlphaKey] = new GradientAlphaKey(_alphaKeys[_selectedAlphaKey].alpha, t);
            }
            _modifiedGradient.SetKeys(_colorKeys.ToArray(), _alphaKeys.ToArray());
            CalculateTexture();
            _interact = true;
        }
        //choose color button call
        public void ChooseColor()
        {
            ColorPicker.Create(_colorKeys[_selectedColorKey].color, "Gradient Color Key", (c) => UpdateColor(_selectedColorKey, c), null);
        }

        private void UpdateColor(int index, Color c)
        {
            _interact = false;
            _colorKeys[index] = new GradientColorKey(c, _colorKeys[index].time);
            _colorKeyObjects[index].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = c;
            _colorComponent.color = c;
            _modifiedGradient.SetKeys(_colorKeys.ToArray(), _alphaKeys.ToArray());
            CalculateTexture();
            _interact = true;
        }
        //cancel button call
        public void CCancel()
        {
            Cancel();
        }
        /// <summary>
        /// Manually cancel the GradientPicker and recovers the default value
        /// </summary>
        public static void Cancel()
        {
            _modifiedGradient = _originalGradient;
            Done();
        }
        //done button call
        public void CDone()
        {
            Done();
        }
        /// <summary>
        /// Manually close the GradientPicker and apply the selected color
        /// </summary>
        public static void Done()
        {
            if(!ColorPicker.done)
                ColorPicker.Done();
            foreach (Slider s in _instance._colorKeyObjects)
            {
                Destroy(s.gameObject);
            }
            foreach (Slider s in _instance._alphaKeyObjects)
            {
                Destroy(s.gameObject);
            }
            _instance._colorKeyObjects = null;
            _instance._colorKeys = null;
            _instance._alphaKeyObjects = null;
            _instance._alphaKeys = null;
            done = true;
            _onGC?.Invoke(_modifiedGradient);
            _onGs?.Invoke(_modifiedGradient);
            _instance.transform.parent.gameObject.SetActive(false);
        }
    }
}
