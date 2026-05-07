#if USE_ARBOR || USE_MORNSTATE
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal class MornUGUICancelModule : MornUGUIStateModuleBase
    {
        [SerializeField] private Selectable _target;
        private MornUGUIControlState _parent;

        public override void Initialize(MornUGUIControlState parent)
        {
            _parent = parent;
        }

        public override void OnStateUpdate()
        {
            if (_target == null || !_target.IsInteractable())
            {
                return;
            }

            var current = EventSystem.current.currentSelectedGameObject;
            // AutoFocusModule側でまずはフォーカスが合うため、nullの時は処理しない
            if (current == null)
            {
                return;
            }

            // グローバル UI の Cancel Action を直接参照。
            // _globalUIActions.devices が MornInputProvider 側で 1P (最若 playerIndex) のデバイスに絞られているため、
            // ここでは PlayerInput を経由せずとも自動的に 1P のみ反応する。
            var cancelAction = MornUGUIGlobal.I.InputCancel;
            if (cancelAction != null && cancelAction.WasPerformedThisFrame())
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
