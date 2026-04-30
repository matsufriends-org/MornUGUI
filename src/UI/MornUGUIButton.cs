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
        [SerializeField, Childrens(true, true)] private MornUGUIMonoBase[] _monoModules;
        public MornUGUIToggleModule AsToggle => _toggleModule;
        bool IMornUGUIInteractable.IsLocked => IsLocked;
        bool IMornUGUIInteractable.IsNegative => IsNegative;
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

        internal override MornUGUIModuleBase[] BuildModules()
        {
            return new MornUGUIModuleBase[]
            {
                _activeModule,
                _pointerModule,
                _scalerModule,
                _soundModule,
                _mirrorModule,
                _toggleModule,
            };
        }
    }
}
