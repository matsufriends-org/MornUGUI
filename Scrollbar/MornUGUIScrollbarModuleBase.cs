using System;

namespace MornUGUI
{
    [Serializable]
    internal abstract class MornUGUIScrollbarModuleBase
    {
        public virtual void Awake(MornUGUIScrollbar parent)
        {
        }

        public virtual void OnEnable(MornUGUIScrollbar parent)
        {
        }

        public virtual void OnDisable(MornUGUIScrollbar parent)
        {
        }

        public virtual void Update(MornUGUIScrollbar parent)
        {
        }

        public virtual void OnSelect(MornUGUIScrollbar parent)
        {
        }

        public virtual void OnDeselect(MornUGUIScrollbar parent)
        {
        }

        public virtual void OnSubmit(MornUGUIScrollbar parent)
        {
        }

        public virtual void OnPointerEnter(MornUGUIScrollbar parent)
        {
        }

        public virtual void OnPointerExit(MornUGUIScrollbar parent)
        {
        }

        public virtual void OnPointerDown(MornUGUIScrollbar parent)
        {
        }

        public virtual void OnDrag(MornUGUIScrollbar parent)
        {
        }

        public virtual void OnValueChanged(MornUGUIScrollbar parent)
        {
        }
    }
}