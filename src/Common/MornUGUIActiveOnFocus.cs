using UnityEngine;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/" + nameof(MornUGUIActiveOnFocus))]
    internal sealed class MornUGUIActiveOnFocus : MornUGUIMonoBase
    {
        public override void Initialize(MonoBehaviour owner)
        {
            if (!Application.isPlaying) return;
            gameObject.SetActive(false);
        }

        public override void OnSelect()
        {
            if (!Application.isPlaying) return;
            gameObject.SetActive(true);
        }

        public override void OnDeselect()
        {
            if (!Application.isPlaying) return;
            gameObject.SetActive(false);
        }
    }
}
