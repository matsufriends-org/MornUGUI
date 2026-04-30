using System;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornLib
{
    public sealed class MornUGUIButton : MornUGUIBase, IMornUGUIObject, IMornUGUIInteractable, IMornUGUIToggleHost
    {
        [Header("MornUGUIButton")]
        public bool IsLocked;
        public bool IsNegative;
        public bool IsToggleOn;
        [Header("Modules")]
        [SerializeField] private MornUGUIPointerModule _pointerModule = new();
        [SerializeField] private MornUGUISoundModule _soundModule = new();
        [SerializeField] private MornUGUIMirrorModule _mirrorModule = new();
        [SerializeField, Childrens(true, true)] private MornUGUIMonoBase[] _monoModules;
        private readonly Subject<bool> _toggleSubject = new();
        bool IMornUGUIInteractable.IsLocked => IsLocked;
        bool IMornUGUIInteractable.IsNegative => IsNegative;
        bool IMornUGUIToggleHost.IsToggleOn => IsToggleOn;
        IObservable<bool> IMornUGUIToggleHost.OnToggleChanged => _toggleSubject;
        Transform IMornUGUIObject.Transform => transform;
        GameObject IMornUGUIObject.GameObject => gameObject;
        CancellationToken IMornUGUIObject.DestroyCancellationToken => destroyCancellationToken;

        protected override void Awake()
        {
            foreach (var module in _monoModules)
            {
                module.Initialize(this);
            }

            base.Awake();
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

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            if (!IsInteractable()) return;
            IsToggleOn = !IsToggleOn;
            _toggleSubject.OnNext(IsToggleOn);
        }

        internal override MornUGUIModuleBase[] BuildModules()
        {
            return new MornUGUIModuleBase[]
            {
                _pointerModule,
                _soundModule,
                _mirrorModule,
            };
        }
    }
}
