using UnityEngine;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/" + nameof(MornUGUIActiveOnFocus))]
    internal sealed class MornUGUIActiveOnFocus : MornUGUIMonoBase
    {
        public override void Initialize(MonoBehaviour owner)
        {
            gameObject.SetActive(false);
        }

        public override void OnSelect()
        {
            gameObject.SetActive(true);
        }

        public override void OnDeselect()
        {
            gameObject.SetActive(false);
        }
    }
}
