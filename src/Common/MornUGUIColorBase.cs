using UnityEngine;

namespace MornLib
{
    public abstract class MornUGUIColorBase : MornUGUIMonoBase
    {
        [SerializeField] private MornUGUIColorSettings _settings;
        private bool _isFocused;
        private IMornUGUIInteractable _parent;

        protected abstract void ApplyColor(Color color);

        public override void Initialize(MonoBehaviour owner)
        {
            _parent = owner as IMornUGUIInteractable;
            Refresh();
        }

        public override void OnSelect()
        {
            _isFocused = true;
            Refresh();
        }

        public override void OnDeselect()
        {
            _isFocused = false;
            Refresh();
        }

        private void Update()
        {
            Refresh();
        }

        private void OnDisable()
        {
            _isFocused = false;
            Refresh();
        }

        private void Refresh()
        {
            if (_parent == null || _settings == null) return;
            Color color;
            if (_isFocused) color = _parent.IsLocked ? _settings.FocusedColor2 : _settings.FocusedColor;
            else color = _parent.IsLocked ? _settings.UnfocusedColor2 : _settings.UnfocusedColor;
            ApplyColor(color);
        }
    }
}
