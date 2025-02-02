using System;
using UnityEngine;
using UnityEngine.UI;

namespace MornUGUI
{
    [Serializable]
    internal sealed class MornUGUIButtonColorModule : MornUGUIButtonModuleBase
    {
        [SerializeField] private Image _image;
        [Header("interactable")]
        [SerializeField] private Color _focusedColor = Color.white;
        [SerializeField] private Color _unfocusedColor = Color.gray;
        [Header("not interactable")]
        [SerializeField] private Color _focusedColor2 = Color.white;
        [SerializeField] private Color _unfocusedColor2 = Color.gray;
        
        public override void Awake(MornUGUIButton parent)
        {
            if (_image == null)
            {
                return;
            }

            _image.color = parent.IsInteractable ? _unfocusedColor : _unfocusedColor2;
        }

        public override void OnSelect(MornUGUIButton parent)
        {
            if (_image == null)
            {
                return;
            }

            _image.color = parent.IsInteractable ? _focusedColor : _focusedColor2;
        }

        public override void OnDeselect(MornUGUIButton parent)
        {
            if (_image == null)
            {
                return;
            }

            _image.color = parent.IsInteractable ? _unfocusedColor : _unfocusedColor2;
        }
    }
}