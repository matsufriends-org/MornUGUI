using System;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Scrollbar;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/" + nameof(MornUGUISelector))]
    public sealed class MornUGUISelector : MornUGUIBase,
        IMornUGUIObject,
        IMornUGUISelector,
        IMornUGUIInteractable,
        IMornUGUIMovable,
        IMornUGUIArrow
    {
        [Header("MornUGUISelector")]
        [SerializeField] private Direction _direction;
        [SerializeField] private IntReactiveProperty _value;
        [SerializeField] private Vector2Int _valueRange;
        [Header("Modules")]
        [SerializeField] private MornUGUIArrowModule _arrowModule;
        [SerializeField] private MornUGUILimitNavigationModule _limitNavigationModule;
        [SerializeField] private MornUGUIArrowSoundModule _soundModule;
        [SerializeField] private MornUGUIMirrorModule _mirrorModule;
#if USE_MORN_LOCALIZE
        [SerializeField] private MornUGUISelectorTextModule _textModule;
#endif
        [SerializeField, Childrens(true, true)] private MornUGUIMonoBase[] _monoModules;
        public int Value
        {
            get => _value.Value;
            set => _value.Value = Mathf.Clamp(value, _valueRange.x, _valueRange.y);
        }
        public IObservable<int> OnValueChanged => _value;

        internal override MornUGUIModuleBase[] BuildModules()
        {
            return new MornUGUIModuleBase[]
            {
                _arrowModule,
                _limitNavigationModule,
                _soundModule,
                _mirrorModule,
#if USE_MORN_LOCALIZE
                _textModule,
#endif
            };
        }

        private bool IsAtMin => Value <= _valueRange.x;
        private bool IsAtMax => Value >= _valueRange.y;
        Vector2Int IMornUGUISelector.ValueRange => _valueRange;
        int IMornUGUISelector.Value => Value;
        Transform IMornUGUIObject.Transform => transform;
        GameObject IMornUGUIObject.GameObject => gameObject;
        CancellationToken IMornUGUIObject.DestroyCancellationToken => destroyCancellationToken;
        bool IMornUGUIInteractable.IsLocked => false;
        bool IMornUGUIInteractable.IsNegative => false;
        bool IMornUGUIMovable.IsHorizontal => _direction is Direction.LeftToRight or Direction.RightToLeft;
        bool IMornUGUIMovable.IsVertical => _direction is Direction.BottomToTop or Direction.TopToBottom;
        bool IMornUGUIMovable.CanUpper => _direction == Direction.BottomToTop ? !IsAtMax : !IsAtMin;
        bool IMornUGUIMovable.CanBottom => _direction == Direction.BottomToTop ? !IsAtMin : !IsAtMax;
        bool IMornUGUIMovable.CanLeft => _direction == Direction.LeftToRight ? !IsAtMin : !IsAtMax;
        bool IMornUGUIMovable.CanRight => _direction == Direction.LeftToRight ? !IsAtMax : !IsAtMin;
        Selectable IMornUGUIMovable.UpNavigationTarget => FindSelectableOnUp();
        Selectable IMornUGUIMovable.DownNavigationTarget => FindSelectableOnDown();
        Selectable IMornUGUIMovable.LeftNavigationTarget => FindSelectableOnLeft();
        Selectable IMornUGUIMovable.RightNavigationTarget => FindSelectableOnRight();

        protected override void Awake()
        {
            foreach (var module in _monoModules)
            {
                module.Initialize(this);
            }

            base.Awake();
            _value.Subscribe(_ => ValueChanged());
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            if (!IsInteractable()) return;
            foreach (var module in _monoModules)
            {
                module.OnSelect();
            }
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            foreach (var module in _monoModules)
            {
                module.OnDeselect();
            }
        }

        public void OnUpSubmit()
        {
            if (_direction == Direction.BottomToTop && !IsAtMax) Value++;
            else if (_direction == Direction.TopToBottom && !IsAtMin) Value--;
        }

        public void OnBottomSubmit()
        {
            if (_direction == Direction.BottomToTop && !IsAtMin) Value--;
            else if (_direction == Direction.TopToBottom && !IsAtMax) Value++;
        }

        public void OnLeftSubmit()
        {
            if (_direction == Direction.LeftToRight && !IsAtMin) Value--;
            else if (_direction == Direction.RightToLeft && !IsAtMax) Value++;
        }

        public void OnRightSubmit()
        {
            if (_direction == Direction.LeftToRight && !IsAtMax) Value++;
            else if (_direction == Direction.RightToLeft && !IsAtMin) Value--;
        }
    }
}