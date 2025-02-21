using TMPro;
using UnityEngine;

namespace MornUGUI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    [ExecuteAlways]
    public sealed class MornUGUITextSetter : MonoBehaviour
    {
        [SerializeField, ReadOnly] public TextMeshProUGUI Text;
        [SerializeField] public MornUGUITextSizeSettings SizeSettings;
        [SerializeField] public MornUGUIFontSettings FontSettings;
        [SerializeField] public MornUGUIMaterialType MaterialType;

        private void Awake()
        {
            if (Application.isPlaying)
            {
                Adjust();
            }
        }

        private void Reset()
        {
            Text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                Adjust();
            }
        }

        public void Adjust()
        {
            var global = MornUGUIGlobal.I;
            if (global == null || SizeSettings == null || FontSettings == null || MaterialType == null)
            {
                return;
            }

            var fontChanged = Text.font != FontSettings.Font;
            var autoSizeChanged = Text.enableAutoSizing == false;
            var maxFontSizeChanged = !Mathf.Approximately(Text.fontSizeMax, SizeSettings.FontSize);
            var minFontSizeChanged = !Mathf.Approximately(Text.fontSizeMin, 0);
            var characterSpacingChanged = !Mathf.Approximately(Text.characterSpacing, SizeSettings.CharacterSpacing);
            var lineSpacingChanged = !Mathf.Approximately(Text.lineSpacing, SizeSettings.LineSpacing);
            var materialChanged = Text.fontSharedMaterial != FontSettings.Materials[MaterialType.Index];
            var anyChanged = fontChanged
                             || autoSizeChanged
                             || maxFontSizeChanged
                             || minFontSizeChanged
                             || characterSpacingChanged
                             || lineSpacingChanged
                             || materialChanged;
            if (Text != null && anyChanged)
            {
                Text.font = FontSettings.Font;
                Text.enableAutoSizing = true;
                Text.fontSizeMax = SizeSettings.FontSize;
                Text.fontSizeMin = 0;
                Text.characterSpacing = SizeSettings.CharacterSpacing;
                Text.lineSpacing = SizeSettings.LineSpacing;
                Text.fontMaterial = FontSettings.Materials[MaterialType.Index];
                global.Log("Text Adjusted");
                global.SetDirty(Text);
            }
        }
    }
}