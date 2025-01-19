using System;
using UnityEngine;
using UnityEngine.UI;

namespace MornUGUI
{
    [Serializable]
    internal sealed class MornUGUIButtonColorModule : MornUGUIButtonModuleBase
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _focusedColor = Color.white;
        [SerializeField] private Color _unfocusedColor = Color.gray;

        public override void Awake(MornUGUIButton parent)
        {
            if (_image == null)
            {
                return;
            }

            _image.color = _unfocusedColor;
        }

        public override void OnSelect(MornUGUIButton parent)
        {
            if (_image == null)
            {
                return;
            }

            _image.color = _focusedColor;
        }

        public override void OnDeselect(MornUGUIButton parent)
        {
            if (_image == null)
            {
                return;
            }

            _image.color = _unfocusedColor;
        }
    }
}