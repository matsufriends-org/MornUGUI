using MornGlobal;
using UnityEngine;

namespace MornUGUI
{
    [CreateAssetMenu(fileName = nameof(MornUGUIGlobal), menuName = "Morn/" + nameof(MornUGUIGlobal))]
    internal sealed class MornUGUIGlobal : MornGlobalBase<MornUGUIGlobal>
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
        internal AudioClip ButtonCursorClip => _buttonCursorClip;
        internal AudioClip ButtonSubmitClip => _buttonSubmitClip;
        internal AudioClip ButtonCancelClip => _buttonCancelClip;
    }
}