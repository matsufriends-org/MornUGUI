using UnityEngine;

namespace MornLib
{
    public abstract class MornUGUIColorBase : MornUGUIMonoBase
    {
        [SerializeField] private MornUGUIColorSettings _override;
        private bool _isFocused;
        private IMornUGUIInteractable _parent;
        private MornUGUIColorSettings Settings => _override != null ? _override : MornUGUIGlobal.I.DefaultColorSettings;

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
            if (_parent == null) return;
            var settings = Settings;
            if (settings == null) return;
            Color color;
            if (_isFocused) color = _parent.IsLocked ? settings.FocusedColor2 : settings.FocusedColor;
            else color = _parent.IsLocked ? settings.UnfocusedColor2 : settings.UnfocusedColor;
            ApplyColor(color);
        }
    }
}
