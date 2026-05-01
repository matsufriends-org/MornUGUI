using System;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/" + nameof(MornUGUIButton))]
    public sealed class MornUGUIButton : MornUGUIBase, IMornUGUIObject, IMornUGUIInteractable, IMornUGUIToggleHost, IMornUGUIMonoOwner
    {
        [Header("MornUGUIButton")]
        public bool IsLocked;
        public bool IsNegative;
        public bool IsToggleOn { get; set; }
        [Header("Modules")]
        [SerializeField] private MornUGUIPointerModule _pointerModule = new();
        [SerializeField] private MornUGUISoundModule _soundModule = new();
        [SerializeField] private MornUGUIMirrorModule _mirrorModule = new();
        [SerializeField, Childrens(true, true)] private MornUGUIMonoBase[] _monoModules;
        private readonly Subject<bool> _toggleSubject = new();
        public IObservable<bool> OnToggleChanged => _toggleSubject;
        bool IMornUGUIInteractable.IsLocked => IsLocked;
        bool IMornUGUIInteractable.IsNegative => IsNegative;
        Transform IMornUGUIObject.Transform => transform;
        GameObject IMornUGUIObject.GameObject => gameObject;
        CancellationToken IMornUGUIObject.DestroyCancellationToken => destroyCancellationToken;

        [OnMornInject]
        private void FilterMonoModules()
        {
            _monoModules = MornUGUIMonoOwnerUtil.FilterDirectlyOwned(this, _monoModules);
        }

        protected override void Awake()
        {
            _monoModules = MornUGUIMonoOwnerUtil.FilterDirectlyOwned(this, _monoModules);
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
