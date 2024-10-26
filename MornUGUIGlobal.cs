using MornGlobal;
using UnityEngine;

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
        [SerializeField] private AudioClip _buttonCursorClip;
        [SerializeField] private AudioClip _buttonSubmitClip;
        [SerializeField] private AudioClip _buttonCancelClip;
        public AudioClip ButtonCursorClip => _buttonCursorClip;
        public AudioClip ButtonSubmitClip => _buttonSubmitClip;
        public AudioClip ButtonCancelClip => _buttonCancelClip;
    }
}