using System;
using UnityEngine;

#if USE_MORNSTATE || USE_ARBOR
namespace MornLib
{
    [Serializable]
    internal class MornUGUICanvasInteractableModule : MornUGUIStateModuleBase
    {
        [SerializeField] private bool _isActive = true;
        private MornUGUIControlState _parent;

        public override void Initialize(MornUGUIControlState parent)
        {
            _parent = parent;
        }

        public override void OnAwake()
        {
            if (!_isActive) return;
            _parent.CanvasGroup.interactable = false;
            _parent.CanvasGroup.blocksRaycasts = false;
        }

        public override void OnStateBegin()
        {
            if (!_isActive) return;
            _parent.CanvasGroup.interactable = true;
            _parent.CanvasGroup.blocksRaycasts = true;
        }

        public override void OnStateEnd()
        {
            if (!_isActive) return;
            _parent.CanvasGroup.interactable = false;
            _parent.CanvasGroup.blocksRaycasts = false;
        }
    }
}
#endif
