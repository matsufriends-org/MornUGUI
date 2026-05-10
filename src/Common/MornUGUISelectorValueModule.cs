using System;
using TMPro;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUISelectorValueModule : MornUGUIModuleBase
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _format = "{0}";
        private IMornUGUISelector _parent;

        public override void Initialize(MonoBehaviour owner)
        {
            _parent = (IMornUGUISelector)owner;
        }

        public override void OnEnable()
        {
            Apply();
        }

        public override void OnValueChanged()
        {
            Apply();
        }

        private void Apply()
        {
            if (_text == null) return;
            var format = string.IsNullOrEmpty(_format) ? "{0}" : _format;
            _text.text = string.Format(format, _parent.Value);
        }
    }
}
