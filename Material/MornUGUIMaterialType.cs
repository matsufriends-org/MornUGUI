using System;
using MornEnum;

namespace MornUGUI
{
    [Serializable]
    public sealed class MornUGUIMaterialType : MornEnumBase
    {
        protected override string[] Values => MornUGUIGlobal.I.MaterialNames;
    }
}