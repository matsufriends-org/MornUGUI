using System;
using MornLib;

namespace MornUGUI
{
    [Serializable]
    public sealed class MornUGUIMaterialType : MornEnumBase
    {
        public override string[] Values => MornUGUIGlobal.I.MaterialNames;
        public override UnityEngine.Object PingTarget => MornUGUIGlobal.I;
    }
}