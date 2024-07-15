using MornSound;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace MornUGUI
{
    [RequireComponent(typeof(Button))]
    public class MornUGUIButtonSound : MonoBehaviour, ISelectHandler, ISubmitHandler
    {
        [SerializeField] private string _onSelected;
        [SerializeField] private string _onSubmit;
        private IMornSoundSimple _player;

        private void Awake()
        {
            _player = VContainerSettings.Instance.GetOrCreateRootLifetimeScopeInstance().Container
                .Resolve<IMornSoundSimple>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            _player.Play(_onSelected);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            _player.Play(_onSubmit);
        }
    }
}