using System;
using MornEnum;

namespace MornUGUI
{
    [Serializable]
    public sealed class MornUGUIMaterialType : MornEnumBase<MornUGUIMaterialTypeGlobal>
    {
        protected override MornUGUIMaterialTypeGlobal Global => MornUGUIMaterialTypeGlobal.I;
    }
}