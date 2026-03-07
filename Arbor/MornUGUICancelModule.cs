#if USE_ARBOR
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal class MornUGUICancelModule : MornUGUIStateModuleBase
    {
        [SerializeField] private bool _isActive;
        [SerializeField, ShowIf(nameof(IsActive))] private Selectable _target;
        private MornUGUIControlState _parent;
        private bool IsActive => _isActive;

        public override void Initialize(MornUGUIControlState parent)
        {
            _parent = parent;
        }

        public override void OnStateUpdate()
        {
            if (!_isActive || _target == null || !_target.IsInteractable()) return;
            var current = EventSystem.current.currentSelectedGameObject;
            // AutoFocusModule側でまずはフォーカスが合うため、nullの時は処理しない
            if (current == null) return;
            if (MornUGUIGlobal.I.InputCancel.WasPerformedThisFrame())
            {
                if (current != _target.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(_target.gameObject);
                }
                else
                {
                    ExecuteEvents.Execute(
                        _target.gameObject,
                        new BaseEventData(EventSystem.current),
                        ExecuteEvents.submitHandler);
                }
            }
        }
    }
}
#endif
