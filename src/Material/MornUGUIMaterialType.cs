using System;

namespace MornLib
{
    [Serializable]
    public sealed class MornUGUIMaterialType : MornEnumBase
    {
        public override string[] Values => MornUGUIGlobal.I.MaterialNames;
        public override UnityEngine.Object PingTarget => MornUGUIGlobal.I;
    }
}