using System;
using UnityEngine;

namespace MornUGUI
{
    [Serializable]
    internal class MornUGUISoundBlockModule : MornUGUIModuleBase
    {
        [SerializeField] private bool _ignore;
        [SerializeField] [ReadOnly] private int _leftFrame;

        public override void OnStateBegin(MornUGUIControlState parent)
        {
            if (_ignore)
            {
                return;
            }

            var blockFrame = MornUGUIGlobal.I.BlockFrame;
            if (blockFrame > 0)
            {
                _leftFrame = blockFrame;
                parent.FlagSetter.FlagOn(MornUGUIGlobal.I.FlagNameBlockingSoundOnFirstFocus);
            }
        }

        public override void OnStateUpdate(MornUGUIControlState parent)
        {
            if (_ignore)
            {
                return;
            }

            if (_leftFrame > 0)
            {
                _leftFrame--;
                if (_leftFrame == 0)
                {
                    parent.FlagSetter.FlagOff(MornUGUIGlobal.I.FlagNameBlockingSoundOnFirstFocus);
                }
            }
        }

        public override void OnStateEnd(MornUGUIControlState parent)
        {
            if (_ignore)
            {
                return;
            }

            parent.FlagSetter.FlagOff(MornUGUIGlobal.I.FlagNameBlockingSoundOnFirstFocus);
        }
    }
}