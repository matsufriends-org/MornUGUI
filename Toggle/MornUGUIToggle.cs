using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornUGUI
{
    [RequireComponent(typeof(Toggle))]
    public sealed class MornUGUIToggle : MonoBehaviour,
        ISelectHandler,
        IDeselectHandler,
        ISubmitHandler,
        IMoveHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private MornUGUIToggleColorModule _colorModule;
        [SerializeField] private MornUGUIToggleConvertPointerToSelectModule _convertPointerToSelectModule;
        [SerializeField] private MornUGUIToggleSoundModule _soundModule;
        public bool IsInteractable { get; set; }

        private IEnumerable<MornUGUIToggleModuleBase> GetModules()
        {
            yield return _colorModule;
            yield return _convertPointerToSelectModule;
            yield return _soundModule;
        }

        private void Execute(Action<MornUGUIToggleModuleBase, MornUGUIToggle> action)
        {
            foreach (var module in GetModules())
            {
                action(module, this);
            }
        }

        private void Awake()
        {
            _toggle.OnValueChangedAsObservable()
                   .Subscribe(_ => Execute((module, parent) => module.OnValueChanged(parent)))
                   .AddTo(this);
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

            Execute((module, parent) => module.OnMove(parent, eventData));
        }
    }
}