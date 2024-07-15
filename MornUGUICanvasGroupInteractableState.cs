using Arbor;
using UnityEngine;

namespace MornUGUI
{
    public class MornUGUICanvasGroupInteractableState : StateBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public override void OnStateBegin()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public override void OnStateEnd()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}