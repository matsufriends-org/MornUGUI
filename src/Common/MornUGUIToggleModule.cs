using System;
using UniRx;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    public sealed class MornUGUIToggleModule : MornUGUIModuleBase
    {
        [SerializeField] private GameObject _selectedOn;
        [SerializeField] private GameObject _unSelectedOn;
        [SerializeField] private GameObject _selectedOff;
        [SerializeField] private GameObject _unSelectedOff;
        [SerializeField] private bool _isOn;
        private readonly Subject<bool> _toggleSubject = new();
        private bool _isSelected;

        public bool IsOn
        {
            get => _isOn;
            set
            {
                _isOn = value;
                ApplyVisual();
                _toggleSubject.OnNext(_isOn);
            }
        }

        public IObservable<bool> OnValueChanged => _toggleSubject;

        public override void Awake()
        {
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

        public override void OnSubmit()
        {
            _isOn = !_isOn;
            ApplyVisual();
            _toggleSubject.OnNext(_isOn);
        }

        private void ApplyVisual()
        {
            if (_selectedOn != null)
            {
                _selectedOn.SetActive(_isSelected && _isOn);
            }

            if (_unSelectedOn != null)
            {
                _unSelectedOn.SetActive(!_isSelected && _isOn);
            }

            if (_selectedOff != null)
            {
                _selectedOff.SetActive(_isSelected && !_isOn);
            }

            if (_unSelectedOff != null)
            {
                _unSelectedOff.SetActive(!_isSelected && !_isOn);
            }
        }
    }
}
