using UnityEngine;

namespace MornLib
{
    public abstract class MornUGUIColorBase : MornUGUIMonoBase
    {
        [SerializeField] private Color _focusedColor = Color.white;
        [SerializeField] private Color _unfocusedColor = Color.gray;
        [SerializeField] private Color _focusedColor2 = Color.white;
        [SerializeField] private Color _unfocusedColor2 = Color.gray;
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
            if (_parent == null) return;
            Color color;
            if (_isFocused) color = _parent.IsLocked ? _focusedColor2 : _focusedColor;
            else color = _parent.IsLocked ? _unfocusedColor2 : _unfocusedColor;
            ApplyColor(color);
        }
    }
}
