#if UNITY_EDITOR
using MornEnum;
using UnityEditor;

namespace MornUGUI
{
    [CustomPropertyDrawer(typeof(MornUGUIMaterialType))]
    public class MornUGUIMaterialTypeDrawer : MornEnumDrawerBase<MornUGUIMaterialTypeGlobal>
    {
        protected override MornUGUIMaterialTypeGlobal Global => MornUGUIMaterialTypeGlobal.I;
    }
}
#endif