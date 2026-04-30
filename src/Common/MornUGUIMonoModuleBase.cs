using UnityEngine;

namespace MornLib
{
    public abstract class MornUGUIMonoModuleBase : MonoBehaviour
    {
        public virtual void Initialize(MonoBehaviour owner)
        {
        }

        public virtual void OnSelect()
        {
        }

        public virtual void OnDeselect()
        {
        }
    }
}
