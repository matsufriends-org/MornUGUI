using UnityEngine;

namespace MornUGUI
{
    [CreateAssetMenu(fileName = nameof(MornUGUIRectSizeSettings), menuName = "Morn/" + nameof(MornUGUIRectSizeSettings))]
    public sealed class MornUGUIRectSizeSettings : ScriptableObject
    {
        public Vector2 Size = new(160, 90);
    }
}