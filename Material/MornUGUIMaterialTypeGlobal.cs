using MornEnum;
using UnityEngine;

namespace MornUGUI
{
    [CreateAssetMenu(fileName = nameof(MornUGUIMaterialTypeGlobal), menuName = "Morn/" + nameof(MornUGUIMaterialTypeGlobal))]
    public sealed class MornUGUIMaterialTypeGlobal : MornEnumGlobalBase<MornUGUIMaterialTypeGlobal>
    {
        protected override bool ShowLog => true;
        protected override string ModuleName => nameof(MornUGUI);
    }
}