#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MornLib
{
    /// <summary>CancelModule._target が Missing (壊れ参照) のものを全 Prefab/Scene から探す。None は未設定として許容。</summary>
    internal sealed class MornUGUICancelModuleMissingSearchWindow : MornUGUIMissingSearchWindowBase
    {
        private const string TargetPath = "_cancelModule._target";

        [MenuItem("Tools/MornUGUI/CancelModule の Missing 検索")]
        private static void ShowWindow()
        {
            var window = GetWindow<MornUGUICancelModuleMissingSearchWindow>("CancelModule Missing 検索");
            window.minSize = new Vector2(600, 400);
            window.Show();
        }

#if USE_ARBOR
        protected override void ScanControlState(MornUGUIControlState controlState, string assetPath)
        {
            var so = new SerializedObject(controlState);
            var target = so.FindProperty(TargetPath);
            if (target == null)
            {
                return;
            }

            if (target.objectReferenceValue != null)
            {
                return;
            }

            // None (instanceID == 0) は未設定として許容。Missing (instanceID != 0) のみ報告する
            if (target.objectReferenceInstanceIDValue == 0)
            {
                return;
            }

            AddEntry(assetPath, new Entry
            {
                HierarchyPath = BuildHierarchyPath(controlState.transform),
                Detail = "_target (Missing)",
            });
        }
#endif
    }
}
#endif
