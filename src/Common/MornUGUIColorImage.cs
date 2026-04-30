using UnityEngine;
using UnityEngine.UI;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/" + nameof(MornUGUIColorImage))]
    internal sealed class MornUGUIColorImage : MornUGUIColorBase
    {
        [SerializeField, Me] private Image _image;

        protected override void ApplyColor(Color color)
        {
            _image.color = color;
        }
    }
}
