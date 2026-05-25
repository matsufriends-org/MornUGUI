using UnityEngine;
using UnityEngine.EventSystems;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/" + nameof(MornUGUIActiveOnFocus))]
    internal sealed class MornUGUIActiveOnFocus : MornUGUIMonoBase
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
