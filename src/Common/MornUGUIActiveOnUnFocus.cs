using UnityEngine;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/" + nameof(MornUGUIActiveOnUnFocus))]
    internal sealed class MornUGUIActiveOnUnFocus : MornUGUIMonoBase
    {
        public override void Initialize(MonoBehaviour owner)
        {
            if (!Application.isPlaying) return;
            gameObject.SetActive(true);
        }

        public override void OnSelect()
        {
            if (!Application.isPlaying) return;
            gameObject.SetActive(false);
        }

        public override void OnDeselect()
        {
            if (!Application.isPlaying) return;
            gameObject.SetActive(true);
        }
    }
}
