using UnityEngine;
using UnityEngine.EventSystems;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/" + nameof(MornUGUIActiveOnFocus))]
    internal sealed class MornUGUIActiveOnFocus : MornUGUIMonoBase
    {
        public override void Initialize(MonoBehaviour owner)
        {
            if (!Application.isPlaying) return;
            // 動的 Instantiate 直後に SetSelectedGameObject(this) されているケースを救済するため、
            // 既に owner が選択中なら active にしておく。 通常の再生開始時は false 側に倒れる。
            var es = EventSystem.current;
            var ownerGo = owner != null ? owner.gameObject : null;
            var isSelected = es != null && ownerGo != null && es.currentSelectedGameObject == ownerGo;
            if (gameObject.activeSelf != isSelected)
            {
                gameObject.SetActive(isSelected);
            }
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
