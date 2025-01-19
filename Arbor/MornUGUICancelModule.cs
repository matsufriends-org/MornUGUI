using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornUGUI
{
    [Serializable]
    internal class MornUGUICancelModule : MornUGUIModuleBase
    {
        [SerializeField] private GameObject _cancelTarget;

        public override void OnStateUpdate(MornUGUIControlState parent)
        {
            if (_cancelTarget == null)
            {
                return;
            }

            if (MornUGUIGlobal.I.InputCancel.WasPerformedThisFrame())
            {
                var current = EventSystem.current.currentSelectedGameObject;
                if (current != _cancelTarget)
                {
                    EventSystem.current.SetSelectedGameObject(_cancelTarget);
                }
                else
                {
                    ExecuteEvents.Execute(
                        _cancelTarget,
                        new BaseEventData(EventSystem.current),
                        ExecuteEvents.submitHandler);
                }
            }
        }
    }
}