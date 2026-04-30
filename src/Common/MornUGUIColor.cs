using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MornLib
{
    internal sealed class MornUGUIColor : MornUGUIMonoBase
    {
        [SerializeField] private List<Image> _images = new();
        [SerializeField] private TMP_Text _text;
        [SerializeField, ShowIf(nameof(HasAny))] private Color _focusedColor = Color.white;
        [SerializeField, ShowIf(nameof(HasAny))] private Color _unfocusedColor = Color.gray;
        [SerializeField, ShowIf(nameof(HasAny))] private Color _focusedColor2 = Color.white;
        [SerializeField, ShowIf(nameof(HasAny))] private Color _unfocusedColor2 = Color.gray;
        private bool _isFocused;
        private IMornUGUIInteractable _parent;
        private bool HasAny => _images.Count > 0 || _text != null;

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
            foreach (var image in _images)
            {
                if (image != null) image.color = color;
            }

            if (_text != null) _text.color = color;
        }
    }
}
