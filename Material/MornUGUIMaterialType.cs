using System;
using MornEnum;

namespace MornUGUI
{
    [Serializable]
    public sealed class MornUGUIMaterialType : MornEnumBase
    {
        public override string[] Values => MornUGUIGlobal.I.MaterialNames;
    }
}