using UniRx;
using UnityEngine;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/Toggle")]
    internal sealed class MornUGUIToggle : MornUGUIMonoBase
    {
        [SerializeField] private GameObject _selectedOn;
        [SerializeField] private GameObject _unSelectedOn;
        [SerializeField] private GameObject _selectedOff;
        [SerializeField] private GameObject _unSelectedOff;
        private IMornUGUIToggleHost _host;
        private bool _isSelected;

        public override void Initialize(MonoBehaviour owner)
        {
            _host = (IMornUGUIToggleHost)owner;
            _host.OnToggleChanged.Subscribe(_ => ApplyVisual()).AddTo(this);
            ApplyVisual();
        }

        public override void OnSelect()
        {
            _isSelected = true;
            ApplyVisual();
        }

        public override void OnDeselect()
        {
            _isSelected = false;
            ApplyVisual();
        }

        private void ApplyVisual()
        {
            if (_host == null) return;
            var isOn = _host.IsToggleOn;
            if (_selectedOn != null) _selectedOn.SetActive(_isSelected && isOn);
            if (_unSelectedOn != null) _unSelectedOn.SetActive(!_isSelected && isOn);
            if (_selectedOff != null) _selectedOff.SetActive(_isSelected && !isOn);
            if (_unSelectedOff != null) _unSelectedOff.SetActive(!_isSelected && !isOn);
        }
    }
}
