#if USE_ARBOR
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal class MornUGUICancelModule : MornUGUIStateModuleBase
    {
        [SerializeField] private Selectable _target;
        private MornUGUIControlState _parent;
        private PlayerInput _cachedInput;

        public override void Initialize(MornUGUIControlState parent)
        {
            _parent = parent;
        }

        public override void OnStateBegin()
        {
            if (_target == null)
            {
                return;
            }

            MornInputProvider.OnPlayerInputsChanged += RefreshCachedInput;
            RefreshCachedInput();
        }

        public override void OnStateEnd()
        {
            MornInputProvider.OnPlayerInputsChanged -= RefreshCachedInput;
        }

        /// <summary>MornInputProvider の join/leave 通知でのみ再計算 (毎 frame チェックはしない)</summary>
        private void RefreshCachedInput()
        {
            _cachedInput = PlayerInput.all.OrderBy(p => p.playerIndex).FirstOrDefault();
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

            if (_cachedInput == null)
            {
                return;
            }

            var cancelAction = _cachedInput.FindAction(MornUGUIGlobal.I.Cancel);
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
