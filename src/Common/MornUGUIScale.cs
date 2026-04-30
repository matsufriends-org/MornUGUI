using UnityEngine;

namespace MornLib
{
    internal sealed class MornUGUIScale : MornUGUIMonoBase
    {
        [SerializeField] private bool _isActive;
        [SerializeField, ShowIf(nameof(IsActive))] private float _focusedScale = 1.1f;
        [SerializeField, ShowIf(nameof(IsActive))] private float _unfocusedScale = 1.0f;
        [SerializeField, ShowIf(nameof(IsActive))] private float _lerpT = 100;
        private float _aimScale;
        private bool IsActive => _isActive;

        public override void OnSelect()
        {
            _aimScale = _focusedScale;
        }

        public override void OnDeselect()
        {
            _aimScale = _unfocusedScale;
        }

        private void Awake()
        {
            _aimScale = _unfocusedScale;
        }

        private void OnDisable()
        {
            _aimScale = _unfocusedScale;
        }

        private void Update()
        {
            if (!_isActive) return;
            var currentScale = transform.localScale.x;
            var newScale = Mathf.Lerp(currentScale, _aimScale, Time.unscaledDeltaTime * _lerpT);
            transform.localScale = Vector3.one * newScale;
        }
    }
}
