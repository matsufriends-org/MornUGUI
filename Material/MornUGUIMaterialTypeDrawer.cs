#if UNITY_EDITOR
using MornEnum;
using UnityEditor;

namespace MornUGUI
{
    [CustomPropertyDrawer(typeof(MornUGUIMaterialType))]
    public class MornUGUIMaterialTypeDrawer : MornEnumDrawerBase
    {
        protected override string[] Values => MornUGUIGlobal.I.MaterialNames;
    }
}
#endif