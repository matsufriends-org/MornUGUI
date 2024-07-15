using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornUGUI
{
    [RequireComponent(typeof(Button))]
    [ExecuteAlways]
    public class MornUGUIButtonColorChanger : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private float _lerpSpeed;
        private bool _isSelect;

        private void Reset()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            var aimColor = _isSelect ? _selectedColor : _normalColor;
            _image.color = Color.Lerp(_image.color, aimColor, _lerpSpeed * Time.deltaTime);
        }

        private void OnValidate()
        {
            _image.color = _normalColor;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _isSelect = false;
            _image.color = _normalColor;
        }

        public void OnSelect(BaseEventData eventData)
        {
            _isSelect = true;
            _image.color = _selectedColor;
        }

        public void OnSubmit(BaseEventData eventData)
        {
            var color = _image.color;
            color.a = 0.3f;
            _image.color = color;
        }
    }
}