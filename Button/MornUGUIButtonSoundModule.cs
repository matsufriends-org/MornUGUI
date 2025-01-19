using System;
using UnityEngine;

namespace MornUGUI
{
    [Serializable]
    internal sealed class MornUGUIButtonSoundModule : MornUGUIButtonModuleBase
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _overrideCursorClip;
        [SerializeField] private AudioClip _overrideSubmitClip;
        [SerializeField] private AudioClip _overrideCancelClip;

        public override void OnSelect(MornUGUIButton parent)
        {
            if (parent.FlagGetter.GetFlag(MornUGUIGlobal.I.FlagNameBlockingSoundOnFirstFocus))
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

        public override void OnSubmit(MornUGUIButton parent)
        {
            if (parent.FlagGetter.GetFlag(MornUGUIGlobal.I.FlagNameBlockingSoundOnFirstFocus))
            {
                return;
            }

            AudioClip clip;
            if (parent.IsNegative)
            {
                clip = _overrideCancelClip ? _overrideCancelClip : MornUGUIGlobal.I.ButtonCancelClip;
            }
            else
            {
                clip = _overrideSubmitClip ? _overrideSubmitClip : MornUGUIGlobal.I.ButtonSubmitClip;
            }

            if (clip == null || _audioSource == null)
            {
                return;
            }

            _audioSource.PlayOneShot(clip);
        }
    }
}