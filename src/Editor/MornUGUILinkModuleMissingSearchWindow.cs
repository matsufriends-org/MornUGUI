#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MornLib
{
    /// <summary>LinkModule の StateLinkSet.Target が None/Missing のものを全 Prefab/Scene から探す。</summary>
    internal sealed class MornUGUILinkModuleMissingSearchWindow : MornUGUIMissingSearchWindowBase
    {
        private const string StateLinkSetsPath = "_linkModule._stateLinkSets";

        [MenuItem("Tools/MornUGUI/LinkModule の Missing 検索")]
        private static void ShowWindow()
        {
            var window = GetWindow<MornUGUILinkModuleMissingSearchWindow>("LinkModule Missing 検索");
            window.minSize = new Vector2(600, 400);
            window.Show();
        }

#if USE_ARBOR
        protected override void ScanControlState(MornUGUIControlState controlState, string assetPath)
        {
            var so = new SerializedObject(controlState);
            var sets = so.FindProperty(StateLinkSetsPath);
            if (sets == null || !sets.isArray)
            {
                return;
            }

            for (var i = 0; i < sets.arraySize; i++)
            {
                var element = sets.GetArrayElementAtIndex(i);
                var target = element.FindPropertyRelative("Target");
                if (target == null || target.objectReferenceValue != null)
                {
                    continue;
                }

                var stateLink = element.FindPropertyRelative("StateLink");
                var nameProp = stateLink?.FindPropertyRelative("name");
                var linkName = nameProp != null ? nameProp.stringValue : "?";
                var kind = target.objectReferenceInstanceIDValue == 0 ? "None" : "Missing";
                AddEntry(assetPath, new Entry
                {
                    HierarchyPath = BuildHierarchyPath(controlState.transform),
                    Detail = $"#{i}  name='{linkName}'  ({kind})",
                });
            }
        }
#endif
    }
}
#endif
