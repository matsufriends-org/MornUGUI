using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornLib
{
    public sealed class MornUGUIButton : MornUGUIBase, IMornUGUIObject, IMornUGUIInteractable
    {
        [Header("MornUGUIButton")]
        public bool IsLocked;
        public bool IsNegative;
        [Header("Modules")]
        [SerializeField] private MornUGUIActiveModule _activeModule = new();
        [SerializeField] private MornUGUIPointerModule _pointerModule = new();
        [SerializeField] private MornUGUIScaleModule _scalerModule = new();
        [SerializeField] private MornUGUISoundModule _soundModule = new();
        [SerializeField] private MornUGUIMirrorModule _mirrorModule = new();
        [SerializeField] private MornUGUIToggleModule _toggleModule = new();
        [SerializeField, Childrens(true)] private MornUGUIColorModule[] _colorModules;
        private List<MornUGUIModuleBase> _module;
        public MornUGUIToggleModule AsToggle => _toggleModule;
        bool IMornUGUIInteractable.IsLocked => IsLocked;
        bool IMornUGUIInteractable.IsNegative => IsNegative;
        Transform IMornUGUIObject.Transform => transform;
        GameObject IMornUGUIObject.GameObject => gameObject;
        CancellationToken IMornUGUIObject.DestroyCancellationToken => destroyCancellationToken;

        protected override void Awake()
        {
            foreach (var color in _colorModules)
            {
                color.Initialize(this);
            }

            base.Awake();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            if (!IsInteractable()) return;
            foreach (var color in _colorModules)
            {
                color.SetFocused(true);
            }
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            foreach (var color in _colorModules)
            {
                color.SetFocused(false);
            }
        }

        internal override List<MornUGUIModuleBase> CreateModules()
        {
            if (_module != null) return _module;
            _module = new List<MornUGUIModuleBase>();
            _activeModule.Initialize();
            _module.Add(_activeModule);
            _pointerModule.Initialize(this);
            _module.Add(_pointerModule);
            _scalerModule.Initialize(this);
            _module.Add(_scalerModule);
            _soundModule.Initialize(this);
            _module.Add(_soundModule);
            _mirrorModule.Initialize(this);
            _module.Add(_mirrorModule);
            _module.Add(_toggleModule);
            return _module;
        }
    }
}
