using UnityEngine;

namespace MornLib
{
    [CreateAssetMenu(fileName = nameof(MornUGUIRectSizeSettings), menuName = "MornUGUI/" + nameof(MornUGUIRectSizeSettings))]
    internal sealed class MornUGUIRectSizeSettings : ScriptableObject
    {
        public Vector2 Size = new(160, 90);
    }
}