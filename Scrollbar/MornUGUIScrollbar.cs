using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornUGUI
{
    [RequireComponent(typeof(Scrollbar))]
    public sealed class MornUGUIScrollbar : MonoBehaviour,
        ISelectHandler,
        IDeselectHandler,
        ISubmitHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IDragHandler
    {
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private bool _isNegative;
        [SerializeField] private MornUGUIScrollbarActiveModule _activeModule;
        public float Value => _scrollbar.value;
        public float Size => _scrollbar.size;

        private IEnumerable<MornUGUIScrollbarModuleBase> GetModules()
        {
            yield return _activeModule;
        }

        private void Execute(Action<MornUGUIScrollbarModuleBase, MornUGUIScrollbar> action)
        {
            foreach (var module in GetModules())
            {
                action(module, this);
            }
        }

        private void OnEnable()
        {
            Execute((module, parent) => module.OnEnable(parent));
        }

        private void OnDisable()
        {
            Execute((module, parent) => module.OnDisable(parent));
        }

        private void Awake()
        {
            _scrollbar.OnValueChangedAsObservable().Subscribe(
                _ => Execute((module, parent) => module.OnValueChanged(parent))).AddTo(this);
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

        public void OnDrag(PointerEventData eventData)
        {
            Execute((module, parent) => module.OnDrag(parent));
        }
    }
}