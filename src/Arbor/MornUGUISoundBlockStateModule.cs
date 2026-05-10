using System;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal class MornUGUISoundBlockStateModule : MornUGUIStateModuleBase
    {
        [SerializeField] private bool _isActive = true;
        private int _leftFrame;
        private string _factorName;

        public override void Initialize(MornUGUIControlState parent)
        {
            _factorName = $"MornUGUISoundBlockStateModule_{parent.GetInstanceID()}";
        }

        public override void OnStateBegin()
        {
            if (!_isActive) return;
            var blockFrame = MornUGUIGlobal.I.BlockFrame;
            if (blockFrame > 0)
            {
                _leftFrame = blockFrame;
                MornUGUIUtil.AddSoundBlockFactor(_factorName);
            }
        }

        public override void OnStateUpdate()
        {
            if (!_isActive) return;
            if (_leftFrame > 0)
            {
                _leftFrame--;
                if (_leftFrame == 0)
                {
                    MornUGUIUtil.RemoveSoundBlockFactor(_factorName);
                }
            }
        }

        public override void OnStateEnd()
        {
            if (!_isActive) return;
            MornUGUIUtil.RemoveSoundBlockFactor(_factorName);
        }
    }
}
