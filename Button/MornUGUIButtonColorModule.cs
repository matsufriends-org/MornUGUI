using System;
using UnityEngine;
using UnityEngine.UI;

namespace MornUGUI
{
    [Serializable]
    public sealed class MornUGUIButtonColorModule : MornUGUIButtonModuleBase
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _focusedColor;
        [SerializeField] private Color _unfocusedColor;

        public override void Awake()
        {
            if (_image == null)
            {
                return;
            }

            _image.color = _focusedColor;
        }

        public override void OnSelect()
        {
            if (_image == null)
            {
                return;
            }

            _image.color = _unfocusedColor;
        }

        public override void OnDeselect()
        {
            if (_image == null)
            {
                return;
            }

            _image.color = _focusedColor;
        }
    }
}