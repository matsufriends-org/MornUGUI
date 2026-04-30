using TMPro;
using UnityEngine;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/" + nameof(MornUGUIColorText))]
    internal sealed class MornUGUIColorText : MornUGUIColorBase
    {
        [SerializeField, Me] private TMP_Text _text;

        protected override void ApplyColor(Color color)
        {
            _text.color = color;
        }
    }
}
