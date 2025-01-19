using System;

namespace MornUGUI
{
    [Serializable]
    internal abstract class MornUGUIModuleBase
    {
        public virtual void OnStateBegin(MornUGUIControlState parent)
        {
        }

        public virtual void OnStateUpdate(MornUGUIControlState parent)
        {
        }

        public virtual void OnStateEnd(MornUGUIControlState parent)
        {
        }

        public virtual void OnEditorInitialize(MornUGUIControlState parent)
        {
        }
    }
}