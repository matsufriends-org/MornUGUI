using UnityEngine;
using UnityEngine.Audio;
#if USE_INPUTSYSTEM
using UnityEngine.InputSystem;
#endif

namespace MornLib
{
    [CreateAssetMenu(fileName = nameof(MornUGUIGlobal), menuName = "Morn/" + nameof(MornUGUIGlobal))]
    public sealed class MornUGUIGlobal : MornGlobalBase<MornUGUIGlobal>
    {
        protected override string ModuleName => "MornUGUI";
#if USE_INPUTSYSTEM
        [Header("Input")]
        [SerializeField] private InputActionReference _submit;
        [SerializeField] private InputActionReference _cancel;
#endif
        [Header("Audio")]
        [SerializeField] private AudioMixerGroup _seMixerGroup;
        [SerializeField] private AudioClip _buttonCursorClip;
        [SerializeField] private AudioClip _buttonSubmitClip;
        [SerializeField] private AudioClip _buttonCancelClip;
        [Header("SoundBlock")]
        [SerializeField] private int _soundBlockFrame = 3;
        [SerializeField] private float _sliderSoundInterval = 0.3f;
        [Header("Materials")]
        [SerializeField] private string[] _materialNames;
#if USE_INPUTSYSTEM
        public InputAction InputSubmit => _submit.action;
        public InputAction InputCancel => _cancel.action;
#endif
        public AudioMixerGroup SeMixerGroup => _seMixerGroup;
        public AudioClip ButtonCursorClip => _buttonCursorClip;
        public AudioClip ButtonSubmitClip => _buttonSubmitClip;
        public AudioClip ButtonCancelClip => _buttonCancelClip;
        public int BlockFrame => _soundBlockFrame;
        public float SliderSoundInterval => _sliderSoundInterval;
        public string[] MaterialNames => _materialNames;
    }
}
