using System;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUISliderSoundModule : MornUGUIModuleBase
    {
        [SerializeField] private bool _ignoreCursor;
        [SerializeField] private AudioClip _overrideCursorClip;
        private float _lastValueChangedSoundTime;

        public override void OnSelect()
        {
            if (_ignoreCursor) return;
            var clip = _overrideCursorClip ? _overrideCursorClip : MornUGUIGlobal.I.ButtonCursorClip;
            clip.PlayOneShotOnMornUGUI();
        }

        public override void OnValueChanged()
        {
            if (_ignoreCursor) return;
            if (Time.time - _lastValueChangedSoundTime < MornUGUIGlobal.I.SliderSoundInterval) return;
            _lastValueChangedSoundTime = Time.time;
            var clip = _overrideCursorClip ? _overrideCursorClip : MornUGUIGlobal.I.ButtonCursorClip;
            clip.PlayOneShotOnMornUGUI();
        }
    }
}
