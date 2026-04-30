using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    [RequireComponent(typeof(Slider))]
    internal sealed class MornUGUISlider : MonoBehaviour,
        ISelectHandler,
        IDeselectHandler,
        ISubmitHandler,
        IMoveHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerClickHandler,
        IMornUGUIObject,
        IMornUGUIInteractable
    {
        [SerializeField, Me] private Slider _slider;
        [Header("Modules")]
        [SerializeField] private MornUGUIActiveModule _activeModule = new();
        [SerializeField] private MornUGUIPointerModule _pointerModule = new();
        [SerializeField] private MornUGUISliderNavigationModule _navigationModule = new();
        [SerializeField] private MornUGUISliderSoundModule _sliderSoundModule = new();
        [SerializeField, Childrens(true)] private MornUGUIColorModule[] _colorModules;
        private List<MornUGUIModuleBase> _modules;
        public bool IsInteractable { get; set; }
        public Slider.Direction Direction => _slider.direction;
        public float Value => _slider.value;
        public float MinValue => _slider.minValue;
        public float MaxValue => _slider.maxValue;
        bool IMornUGUIInteractable.IsLocked => !IsInteractable;
        bool IMornUGUIInteractable.IsNegative => false;
        Transform IMornUGUIObject.Transform => transform;
        GameObject IMornUGUIObject.GameObject => gameObject;
        CancellationToken IMornUGUIObject.DestroyCancellationToken => destroyCancellationToken;
        private List<MornUGUIModuleBase> Modules
        {
            get
            {
                if (_modules != null) return _modules;
                _modules = new List<MornUGUIModuleBase>();
                _activeModule.Initialize();
                _modules.Add(_activeModule);
                _pointerModule.Initialize(this);
                _modules.Add(_pointerModule);
                _navigationModule.Initialize(this);
                _modules.Add(_navigationModule);
                _modules.Add(_sliderSoundModule);
                return _modules;
            }
        }

        private void Execute(Action<MornUGUIModuleBase> action)
        {
            foreach (var module in Modules)
            {
                action(module);
            }
        }

        private void Awake()
        {
            foreach (var color in _colorModules)
            {
                color.Initialize(this);
            }

            _slider.onValueChanged.AddListener(_ => Execute(module => module.OnValueChanged()));
            Execute(module => module.Awake());
        }

        private void Update()
        {
            Execute(module => module.Update());
        }

        private void OnEnable()
        {
            Execute(module => module.OnEnable());
        }

        private void OnDisable()
        {
            Execute(module => module.OnDisable());
        }

        public void OnSelect(BaseEventData eventData)
        {
            Execute(module => module.OnSelect());
            foreach (var color in _colorModules)
            {
                color.SetFocused(true);
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Execute(module => module.OnDeselect());
            foreach (var color in _colorModules)
            {
                color.SetFocused(false);
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Execute(module => module.OnSubmit());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Execute(module => module.OnPointerEnter(eventData));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Execute(module => module.OnPointerExit(eventData));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Execute(module => module.OnPointerDown(eventData));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Execute(module => module.OnPointerClick(eventData));
        }

        public void OnMove(AxisEventData eventData)
        {
            if (eventData.selectedObject != gameObject)
            {
                return;
            }

            Execute(module => module.OnMove(eventData));
        }
    }
}