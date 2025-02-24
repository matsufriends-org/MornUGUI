using System;
using UnityEngine;
using UnityEngine.UI;

namespace MornUGUI
{
    [Serializable]
    internal sealed class MornUGUIToggleColorModule : MornUGUIToggleModuleBase
    {
        [SerializeField] private Image _image;
        [Header("interactable")]
        [SerializeField] private Color _focusedColor = Color.white;
        [SerializeField] private Color _unfocusedColor = Color.gray;
        [Header("not interactable")]
        [SerializeField] private Color _focusedColor2 = Color.white;
        [SerializeField] private Color _unfocusedColor2 = Color.gray;
        private bool _cachedIsFocused;

        public override void Awake(MornUGUIToggle parent)
        {
            Update(parent);
        }

        public override void Update(MornUGUIToggle parent)
        {
            if (_image == null)
            {
                return;
            }

            if (_cachedIsFocused)
            {
                _image.color = parent.IsInteractable ? _focusedColor : _focusedColor2;
            }
            else
            {
                _image.color = parent.IsInteractable ? _unfocusedColor : _unfocusedColor2;
            }
        }

        public override void OnSelect(MornUGUIToggle parent)
        {
            _cachedIsFocused = true;
            Update(parent);
        }

        public override void OnDeselect(MornUGUIToggle parent)
        {
            _cachedIsFocused = false;
            Update(parent);
        }
    }
}