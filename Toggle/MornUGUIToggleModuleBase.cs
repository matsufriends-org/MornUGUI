using System;
using UnityEngine.EventSystems;

namespace MornUGUI
{
    [Serializable]
    internal abstract class MornUGUIToggleModuleBase
    {
        public virtual void OnValueChanged(MornUGUIToggle parent)
        {
            
        }
        
        public virtual void Awake(MornUGUIToggle parent)
        {
        }

        public virtual void Update(MornUGUIToggle parent)
        {
        }

        public virtual void OnSelect(MornUGUIToggle parent)
        {
        }

        public virtual void OnDeselect(MornUGUIToggle parent)
        {
        }

        public virtual void OnSubmit(MornUGUIToggle parent)
        {
        }

        public virtual void OnPointerEnter(MornUGUIToggle parent)
        {
        }

        public virtual void OnPointerExit(MornUGUIToggle parent)
        {
        }

        public virtual void OnPointerDown(MornUGUIToggle parent)
        {
        }

        public virtual void OnMove(MornUGUIToggle parent, AxisEventData axisEventData)
        {
        }
    }
}