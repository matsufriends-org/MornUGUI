using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornUGUI
{
    [Serializable]
    internal sealed class MornUGUIScrollbarSoundModule : MornUGUIScrollbarModuleBase
    {
        [SerializeField] private bool _ignoreCursor;
        [SerializeField] private bool _ignoreSubmit;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _overrideCursorClip;
        [SerializeField] private AudioClip _overrideSubmitClip;

        public override void OnSelect(MornUGUIScrollbar parent)
        {
            if (_ignoreCursor || MornUGUIService.I.IsBlocking)
            {
                return;
            }

            var clip = _overrideCursorClip ? _overrideCursorClip : MornUGUIGlobal.I.ButtonCursorClip;
            if (clip == null || _audioSource == null)
            {
                return;
            }

            _audioSource.PlayOneShot(MornUGUIGlobal.I.ButtonCursorClip);
        }
        
        public override void OnMove(MornUGUIScrollbar parent, AxisEventData axis)
        {
            if (_ignoreCursor || MornUGUIService.I.IsBlocking)
            {
                return;
            }

            var clip = _overrideCursorClip ? _overrideCursorClip : MornUGUIGlobal.I.ButtonCursorClip;
            if (clip == null || _audioSource == null)
            {
                return;
            }

            _audioSource.PlayOneShot(MornUGUIGlobal.I.ButtonCursorClip);
        }


        public override void OnSubmit(MornUGUIScrollbar parent)
        {
            if (_ignoreSubmit)
            {
                return;
            }

            var clip = _overrideSubmitClip ? _overrideSubmitClip : MornUGUIGlobal.I.ButtonSubmitClip;
            if (clip == null || _audioSource == null)
            {
                return;
            }

            _audioSource.PlayOneShot(clip);
        }
    }
}