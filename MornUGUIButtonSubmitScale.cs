using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornUGUI
{
    [RequireComponent(typeof(Button))]
    [ExecuteAlways]
    public class MornUGUIButtonSubmitScale : MonoBehaviour, ISubmitHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _submitScale = 1.1f;
        [SerializeField] private float _lerpSpeed = 10f;
        private Vector3? _defaultScale;

        private void Update()
        {
            if (_defaultScale != null)
            {
                var a = _rectTransform.localScale;
                var b = _defaultScale.Value;
                var t = Time.deltaTime * _lerpSpeed;
                _rectTransform.localScale = Vector3.Lerp(a, b, t);
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            DoScale();
        }

        private void DoScale()
        {
            if (_defaultScale == null) _defaultScale = _rectTransform.localScale;
            _rectTransform.localScale = _defaultScale.Value * _submitScale;
        }
    }
}