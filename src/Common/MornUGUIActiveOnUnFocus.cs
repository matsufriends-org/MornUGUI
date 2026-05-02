using UnityEngine;
using UnityEngine.EventSystems;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/" + nameof(MornUGUIActiveOnUnFocus))]
    internal sealed class MornUGUIActiveOnUnFocus : MornUGUIMonoBase
    {
        public override void Initialize(MonoBehaviour owner)
        {
            if (!Application.isPlaying) return;
            // Awake 時点で EventSystem が既に owner を選択済みのケース (動的生成直後の SetSelectedGameObject 等)
            // OnSelect コールバックは再発行されないので、 ここで現在の選択状態を反映させる必要がある。
            gameObject.SetActive(!IsOwnerSelected(owner));
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

        private static bool IsOwnerSelected(MonoBehaviour owner)
        {
            var es = EventSystem.current;
            return es != null && owner != null && es.currentSelectedGameObject == owner.gameObject;
        }
    }
}
