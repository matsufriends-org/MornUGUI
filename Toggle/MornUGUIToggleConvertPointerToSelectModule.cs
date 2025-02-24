using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornUGUI
{
    [Serializable]
    internal sealed class MornUGUIToggleConvertPointerToSelectModule : MornUGUIToggleModuleBase
    {
        [SerializeField] [ReadOnly] private bool _isExist;

        public override void Update(MornUGUIToggle parent)
        {
            if (_isExist && EventSystem.current.currentSelectedGameObject != parent.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(parent.gameObject);
            }
        }

        public override void OnPointerDown(MornUGUIToggle parent)
        {
            ExecuteEvents.Execute(
                parent.gameObject,
                new BaseEventData(EventSystem.current),
                ExecuteEvents.submitHandler);
        }

        public override void OnPointerEnter(MornUGUIToggle parent)
        {
            _isExist = true;
            Update(parent);
        }

        public override void OnPointerExit(MornUGUIToggle parent)
        {
            _isExist = false;
            if (EventSystem.current.currentSelectedGameObject == parent.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}