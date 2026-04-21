#if USE_ARBOR
using System;

namespace MornLib
{
    [Serializable]
    internal abstract class MornUGUIStateModuleBase
    {
        public abstract void Initialize(MornUGUIControlState parent);

        public virtual void OnAwake()
        {
        }

        public virtual void OnStateBegin()
        {
        }

        public virtual void OnStateUpdate()
        {
        }

        public virtual void OnStateEnd()
        {
        }

        public virtual void OnEditorInitialize()
        {
        }

        public virtual void OnEditorRestore()
        {
        }
    }
}
#endif
