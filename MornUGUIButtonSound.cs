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
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _onSelectedClip;
        [SerializeField] private AudioClip _onSubmitClip;

        private void Awake()
        {
            var container = VContainerSettings.Instance.GetOrCreateRootLifetimeScopeInstance().Container;
            _flagGetter = container.Resolve<IMornFlagGetter>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (_flagGetter.GetFlag(MornUGUIUtil.BlockSelectSoundFlag))
            {
                return;
            }

            _audioSource.PlayOneShot(_onSelectedClip);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (_flagGetter.GetFlag(MornUGUIUtil.BlockSelectSoundFlag))
            {
                return;
            }

            _audioSource.PlayOneShot(_onSubmitClip);
        }
    }
}