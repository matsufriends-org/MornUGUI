using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Scrollbar;

namespace MornUGUI
{
    [RequireComponent(typeof(Scrollbar))]
    public sealed class MornUGUIScrollbar : MonoBehaviour, IMoveHandler
    {
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private bool _isNegative;
        [SerializeField] private MornUGUIScrollbarActiveModule _activeModule;
        [SerializeField] private MornUGUIScrollbarNavigationModule _navigationModule;
        public Direction Direction => _scrollbar.direction;
        public float Value => _scrollbar.value;
        public float Size => _scrollbar.size;

        private IEnumerable<MornUGUIScrollbarModuleBase> GetModules()
        {
            yield return _activeModule;
            yield return _navigationModule;
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

        public void OnMove(AxisEventData eventData)
        {
            Execute((module, parent) => module.OnMove(parent, eventData));
        }
    }
}