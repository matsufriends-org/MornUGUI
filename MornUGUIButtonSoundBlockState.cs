using Arbor;
using MornFlag;
using UnityEngine;
using VContainer;

namespace MornUGUI
{
    public class MornUGUIButtonSoundBlockState : StateBehaviour
    {
        [Inject] private IMornFlagSetter _flagSetter;
        [SerializeField] private int _blockFrame = 10;
        private int _leftFrame;

        public override void OnStateBegin()
        {
            if (_blockFrame > 0)
            {
                _leftFrame = _blockFrame;
                _flagSetter.FlagOn(MornUGUIUtil.BlockSelectSoundFlag);
            }
        }

        public override void OnStateUpdate()
        {
            if (_leftFrame > 0)
            {
                _leftFrame--;
                if (_leftFrame == 0)
                {
                    _flagSetter.FlagOff(MornUGUIUtil.BlockSelectSoundFlag);
                }
            }
        }

        public override void OnStateEnd()
        {
            _flagSetter.FlagOff(MornUGUIUtil.BlockSelectSoundFlag);
        }
    }
}