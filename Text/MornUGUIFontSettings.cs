using TMPro;
using UnityEngine;

namespace MornUGUI
{
    [CreateAssetMenu(fileName = nameof(MornUGUIFontSettings), menuName = "Morn/" + nameof(MornUGUIFontSettings))]
    public sealed class MornUGUIFontSettings : ScriptableObject
    {
        [SerializeField] public TMP_FontAsset Font;
        [SerializeField] public Material[] Materials;
    }
}