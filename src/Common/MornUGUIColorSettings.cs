using UnityEngine;

namespace MornLib
{
    [CreateAssetMenu(fileName = nameof(MornUGUIColorSettings), menuName = "Morn/" + nameof(MornUGUIColorSettings))]
    public sealed class MornUGUIColorSettings : ScriptableObject
    {
        [SerializeField] private Color _focusedColor = Color.white;
        [SerializeField] private Color _unfocusedColor = Color.gray;
        [SerializeField] private Color _focusedColor2 = Color.white;
        [SerializeField] private Color _unfocusedColor2 = Color.gray;
        public Color FocusedColor => _focusedColor;
        public Color UnfocusedColor => _unfocusedColor;
        public Color FocusedColor2 => _focusedColor2;
        public Color UnfocusedColor2 => _unfocusedColor2;
    }
}
