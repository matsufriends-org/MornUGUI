using UnityEngine;
using UnityEngine.EventSystems;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/" + nameof(MornUGUIActiveOnUnFocus))]
    internal sealed class MornUGUIActiveOnUnFocus : MornUGUIMonoBase
    {
        private MonoBehaviour _owner;

        public override void Initialize(MonoBehaviour owner)
        {
            _owner = owner;
            if (!Application.isPlaying) return;
            SyncActive();
        }

        private void OnEnable()
        {
            if (!Application.isPlaying) return;
            SyncActive();
        }

        private void SyncActive()
        {
            var es = EventSystem.current;
            var ownerGo = _owner != null ? _owner.gameObject : null;
            var isSelected = es != null && ownerGo != null && es.currentSelectedGameObject == ownerGo;
            var desired = !isSelected;
            if (gameObject.activeSelf != desired)
            {
                gameObject.SetActive(desired);
            }
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
