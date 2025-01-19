using MornGlobal;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MornUGUI
{
    [CreateAssetMenu(fileName = nameof(MornUGUIGlobal), menuName = "Morn/" + nameof(MornUGUIGlobal))]
    public sealed class MornUGUIGlobal : MornGlobalBase<MornUGUIGlobal>
    {
#if DISABLE_MORN_ASPECT_LOG
        protected override bool ShowLog => false;
#else
        protected override bool ShowLog => true;
#endif
        protected override string ModuleName => nameof(MornUGUI);
        [Header("Input")]
        [SerializeField] private InputActionReference _cancel;
        [Header("Audio")]
        [SerializeField] private AudioClip _buttonCursorClip;
        [SerializeField] private AudioClip _buttonSubmitClip;
        [SerializeField] private AudioClip _buttonCancelClip;
        [Header("SoundBlock")]
        [SerializeField] private int _soundBlockFrame = 3;
        [SerializeField] private string _flagNameBlockingSoundOnFirstFocus = "MornUGUI.ButtonFocus";
        public InputAction InputCancel => _cancel.action;
        public AudioClip ButtonCursorClip => _buttonCursorClip;
        public AudioClip ButtonSubmitClip => _buttonSubmitClip;
        public AudioClip ButtonCancelClip => _buttonCancelClip;
        public int BlockFrame => _soundBlockFrame;
        public string FlagNameBlockingSoundOnFirstFocus => _flagNameBlockingSoundOnFirstFocus;
    }
}