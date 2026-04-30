#if USE_MORN_LOCALIZE
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal class MornUGUISelectorTextModule : MornUGUIModuleBase
    {
        [SerializeField] private List<MornLocalizeString> _texts = new();
        [SerializeField] private TMP_Text _text;
        private IMornUGUISelector _parent;

        public override void Initialize(MonoBehaviour owner)
        {
            _parent = (IMornUGUISelector)owner;
            var obj = (IMornUGUIObject)owner;
            MornLocalizeCore.OnLanguageChanged.Subscribe(_ => OnValueChanged()).AddTo(obj.DestroyCancellationToken);
        }

        public override void OnEnable()
        {
            OnValueChanged();
        }

        public override void OnValueChanged()
        {
            if (_text == null || _texts.Count == 0)
            {
                return;
            }

            var range = _parent.ValueRange;
            var index = Mathf.Clamp(_parent.Value - range.x, 0, _texts.Count - 1);
            _text.text = _texts[index].Get();
        }
    }
}
#endif