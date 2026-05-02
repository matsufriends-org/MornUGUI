using UnityEngine;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/" + nameof(MornUGUIActiveOnUnFocus))]
    internal sealed class MornUGUIActiveOnUnFocus : MornUGUIMonoBase
    {
        public override void Initialize(MonoBehaviour owner)
        {
            gameObject.SetActive(true);
        }

        public override void OnSelect()
        {
            gameObject.SetActive(false);
        }

        public override void OnDeselect()
        {
            gameObject.SetActive(true);
        }
    }
}
