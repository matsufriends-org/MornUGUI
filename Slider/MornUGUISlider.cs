using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornUGUI
{
    [RequireComponent(typeof(Slider))]
    public sealed class MornUGUISlider : MonoBehaviour,
        ISelectHandler,
        IDeselectHandler,
        ISubmitHandler,
        IMoveHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private MornUGUISliderActiveModule _activeModule;
        [SerializeField] private MornUGUISliderColorModule _colorModule;
        [SerializeField] private MornUGUISliderConvertPointerToSelectModule _convertPointerToSelectModule;
        [SerializeField] private MornUGUISliderSoundModule _soundModule;
        public bool IsInteractable { get; set; }
        public IObservable<Unit>　OnSliderSelected => _slider.OnSelectAsObservable().Select(_ => Unit.Default);
        public IObservable<Unit> OnSliderSubmit => _slider.OnSubmitAsObservable().Select(_ => Unit.Default);
        public IObservable<float> OnSliderChanged => _slider.OnValueChangedAsObservable();

        private IEnumerable<MornUGUISliderModuleBase> GetModules()
        {
            yield return _activeModule;
            yield return _colorModule;
            yield return _convertPointerToSelectModule;
            yield return _soundModule;
        }

        private void Execute(Action<MornUGUISliderModuleBase, MornUGUISlider> action)
        {
            foreach (var module in GetModules())
            {
                action(module, this);
            }
        }

        private void Awake()
        {
            Execute((module, parent) => module.Awake(parent));
        }

        private void Update()
        {
            Execute((module, parent) => module.Update(parent));
        }

        public void OnSelect(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnSelect(parent));
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnDeselect(parent));
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnSubmit(parent));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Execute((module, parent) => module.OnPointerEnter(parent));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Execute((module, parent) => module.OnPointerExit(parent));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Execute((module, parent) => module.OnPointerDown(parent));
        }

        public void OnMove(AxisEventData eventData)
        {
            // 自分から自分への移動のみ
            if (eventData.selectedObject != gameObject)
            {
                return;
            }
            
            Execute((module, parent) => module.OnMove(parent));
        }
    }
}