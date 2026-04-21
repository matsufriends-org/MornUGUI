#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MornLib
{
    /// <summary>
    /// MornUGUIControlState の特定フィールドが Missing/None の箇所を全 Prefab/Scene から探す
    /// EditorWindow の共通基底。結果はアセット単位にグループ化し Ping ボタンを添える。
    /// 派生側は <see cref="ScanControlState"/> でモジュール固有のチェックを実装する。
    /// </summary>
    internal abstract class MornUGUIMissingSearchWindowBase : EditorWindow
    {
        protected sealed class Entry
        {
            public string HierarchyPath;
            public string Detail;
        }

        private sealed class AssetGroup
        {
            public string AssetPath;
            public readonly List<Entry> Entries = new();
        }

        private readonly List<AssetGroup> _groups = new();
        private readonly Dictionary<string, AssetGroup> _groupIndex = new();
        private Vector2 _scrollPosition;
        private int _scannedCount;
        private string _filter = "";
        private bool _isSearching;

#if USE_ARBOR
        protected abstract void ScanControlState(MornUGUIControlState controlState, string assetPath);
#endif

        protected void AddEntry(string assetPath, Entry entry)
        {
            if (!_groupIndex.TryGetValue(assetPath, out var group))
            {
                group = new AssetGroup { AssetPath = assetPath };
                _groupIndex[assetPath] = group;
                _groups.Add(group);
            }

            group.Entries.Add(entry);
        }

        protected static string BuildHierarchyPath(Transform t)
        {
            var stack = new Stack<string>();
            var cursor = t;
            while (cursor != null)
            {
                stack.Push(cursor.name);
                cursor = cursor.parent;
            }

            return string.Join("/", stack);
        }

        protected virtual void OnGUI()
        {
            EditorGUILayout.Space();
            using (new EditorGUI.DisabledScope(_isSearching))
            {
                if (GUILayout.Button("検索実行", GUILayout.Height(30)))
                {
                    Run();
                }
            }

            if (_isSearching)
            {
                EditorGUILayout.HelpBox("検索中...", MessageType.Info);
                return;
            }

            EditorGUILayout.Space();
            using (new EditorGUILayout.HorizontalScope())
            {
                _filter = EditorGUILayout.TextField("フィルター:", _filter);
                if (GUILayout.Button("クリア", GUILayout.Width(60)))
                {
                    _filter = "";
                }
            }

            EditorGUILayout.Space();
            var totalEntries = 0;
            foreach (var g in _groups)
            {
                totalEntries += g.Entries.Count;
            }

            EditorGUILayout.LabelField($"{_scannedCount} 件スキャン / {_groups.Count} アセットに {totalEntries} 件検出");
            EditorGUILayout.Space();

            using (var scroll = new EditorGUILayout.ScrollViewScope(_scrollPosition))
            {
                _scrollPosition = scroll.scrollPosition;
                var filterLower = string.IsNullOrEmpty(_filter) ? null : _filter.ToLower();
                foreach (var group in _groups)
                {
                    if (filterLower != null && !GroupMatchesFilter(group, filterLower))
                    {
                        continue;
                    }

                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField($"{group.AssetPath}  ({group.Entries.Count} 件)", EditorStyles.miniBoldLabel);
                            if (GUILayout.Button("Ping", GUILayout.Width(60)))
                            {
                                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(group.AssetPath);
                                if (asset != null)
                                {
                                    EditorGUIUtility.PingObject(asset);
                                }
                            }
                        }

                        foreach (var entry in group.Entries)
                        {
                            EditorGUILayout.LabelField($"  • {entry.HierarchyPath}  {entry.Detail}");
                        }
                    }
                }
            }
        }

        private static bool GroupMatchesFilter(AssetGroup group, string filterLower)
        {
            if (group.AssetPath.ToLower().Contains(filterLower))
            {
                return true;
            }

            foreach (var e in group.Entries)
            {
                if (e.HierarchyPath.ToLower().Contains(filterLower) || e.Detail.ToLower().Contains(filterLower))
                {
                    return true;
                }
            }

            return false;
        }

        private void Run()
        {
            _groups.Clear();
            _groupIndex.Clear();
            _scannedCount = 0;
            _isSearching = true;

            try
            {
                ScanAll("t:Prefab", ScanPrefab);
                ScanAll("t:SceneAsset", ScanScene);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                _isSearching = false;
                Repaint();
            }
        }

        private void ScanAll(string filter, Func<string, bool> scanner)
        {
            var guids = AssetDatabase.FindAssets(filter);
            var total = guids.Length;
            for (var i = 0; i < total; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (ShouldSkip(path))
                {
                    continue;
                }

                if (EditorUtility.DisplayCancelableProgressBar("Missing 検索", $"{path} ({i + 1}/{total})", (float)i / total))
                {
                    break;
                }

                if (scanner(path))
                {
                    _scannedCount++;
                }
            }
        }

        private static bool ShouldSkip(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return true;
            }

            if (assetPath.StartsWith("Packages/", StringComparison.Ordinal))
            {
                return true;
            }

            if (assetPath.Contains("/~") || assetPath.Contains("/."))
            {
                return true;
            }

            return false;
        }

        private bool ScanPrefab(string path)
        {
            GameObject root;
            try
            {
                root = PrefabUtility.LoadPrefabContents(path);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Missing 検索] {path} のロード失敗: {e.Message}");
                return false;
            }

            if (root == null)
            {
                return false;
            }

            try
            {
                ScanRootObject(root, path);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Missing 検索] {path} のスキャン失敗: {e.Message}");
                return false;
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(root);
            }
        }

        private bool ScanScene(string path)
        {
            UnityEngine.SceneManagement.Scene scene;
            try
            {
                scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Missing 検索] {path} のロード失敗: {e.Message}");
                return false;
            }

            try
            {
                foreach (var rootGo in scene.GetRootGameObjects())
                {
                    ScanRootObject(rootGo, path);
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Missing 検索] {path} のスキャン失敗: {e.Message}");
                return false;
            }
            finally
            {
                EditorSceneManager.CloseScene(scene, removeScene: true);
            }
        }

        private void ScanRootObject(GameObject root, string assetPath)
        {
#if USE_ARBOR
            foreach (var cs in root.GetComponentsInChildren<MornUGUIControlState>(includeInactive: true))
            {
                if (cs == null)
                {
                    continue;
                }

                ScanControlState(cs, assetPath);
            }
#endif
        }
    }
}
#endif
