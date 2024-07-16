using System;
using MornFlag;
using MornSound;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace MornUGUI
{
    [RequireComponent(typeof(Button))]
    public sealed class MornUGUIButtonSound : MonoBehaviour, ISelectHandler, ISubmitHandler
    {
        private IMornFlagGetter _flagGetter;
        private IMornSoundSimple _player;
        [SerializeField] private string _onSelected;
        [SerializeField] private string _onSubmit;

        private void Awake()
        {
            var container = VContainerSettings.Instance.GetOrCreateRootLifetimeScopeInstance().Container;
            _flagGetter = container.Resolve<IMornFlagGetter>();
            _player = container.Resolve<IMornSoundSimple>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (_flagGetter.GetFlag(MornUGUIUtil.BlockSelectSoundFlag))
                return;
            _player.Play(_onSelected);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (_flagGetter.GetFlag(MornUGUIUtil.BlockSelectSoundFlag))
                return;
            _player.Play(_onSubmit);
        }
    }
}