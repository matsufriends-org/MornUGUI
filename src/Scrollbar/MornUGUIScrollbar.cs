using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Scrollbar;

namespace MornLib
{
    [RequireComponent(typeof(Scrollbar))]
    internal sealed class MornUGUIScrollbar : MonoBehaviour, IMoveHandler, ISelectHandler, ISubmitHandler
    {
        [SerializeField, Me] private Scrollbar _scrollbar;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private MornUGUIScrollbarActiveModule _activeModule;
        [SerializeField] private MornUGUIScrollbarNavigationModule _navigationModule;
        [SerializeField] private MornUGUIScrollbarSoundModule _soundModule;
        public Direction Direction => _scrollbar.direction;
        public float Value => _scrollbar.value;
        public float Size => _scrollbar.size;

        private IEnumerable<MornUGUIScrollbarModuleBase> GetModules()
        {
            yield return _activeModule;
            yield return _navigationModule;
            yield return _soundModule;
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
            _scrollbar.OnValueChangedAsObservable().Subscribe(_ => Execute((module, parent) => module.OnValueChanged(parent))).AddTo(this);
            Execute((module, parent) => module.Awake(parent));
        }

        public void ToUp()
        {
            SimulateScroll(true);
        }

        public void ToBottom()
        {
            SimulateScroll(false);
        }

        private void SimulateScroll(bool toUp)
        {
            if (_scrollRect == null) return;
            var delta = toUp ? _scrollRect.scrollSensitivity : -_scrollRect.scrollSensitivity;
            delta *= 6;
            var eventData = new PointerEventData(EventSystem.current)
            {
                scrollDelta = Direction == Direction.BottomToTop || Direction == Direction.TopToBottom ? new Vector2(0, delta) : new Vector2(delta, 0)
            };
            ExecuteEvents.Execute(_scrollRect.gameObject, eventData, ExecuteEvents.scrollHandler);
        }

        void IMoveHandler.OnMove(AxisEventData eventData)
        {
            Execute((module, parent) => module.OnMove(parent, eventData));
        }

        void ISelectHandler.OnSelect(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnSelect(parent));
        }

        void ISubmitHandler.OnSubmit(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnSubmit(parent));
        }
    }
}