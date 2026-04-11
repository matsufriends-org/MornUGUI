using System;
using MornLib;

namespace MornUGUI
{
    [Serializable]
    public sealed class MornUGUIMaterialType : MornEnumBase
    {
        protected override string[] Values => MornUGUIGlobal.I.MaterialNames;
    }
}