#if USE_ARBOR
using System;
using System.Collections.Generic;
using Arbor;
using UnityEditor;
using UnityEngine;

namespace MornLib
{
    internal class MornUGUIControlState : StateBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private MornUGUIAnimationModule _animationModule;
#if USE_INPUTSYSTEM
        [SerializeField] private MornUGUIAutoFocusModule _autoFocusModule;
#endif
        [SerializeField] private MornUGUILinkModule _linkModule;
        [SerializeField] private MornUGUICancelModule _cancelModule;
        [SerializeField] private MornUGUICanvasFadeModule _canvasFadeModule;
        [SerializeField] private MornUGUICanvasInteractableModule _canvasInteractableModule;
        [SerializeField] private MornUGUISoundBlockStateModule _soundBlockModule;
        private List<MornUGUIStateModuleBase> _modules;
        public CanvasGroup CanvasGroup => _canvasGroup;
        private List<MornUGUIStateModuleBase> Modules
        {
            get
            {
                if (_modules != null) return _modules;
                _modules ??= new List<MornUGUIStateModuleBase>();
                // SoundBlockは、フォーカス時の音を防ぐために最初に初期化/登録
                _soundBlockModule.Initialize(this);
                _modules.Add(_soundBlockModule);
                _animationModule.Initialize(this);
                _modules.Add(_animationModule);
                // CanvasInteractableはAutoFocusより前に実行し、フォーカス時点でinteractableをtrueにしておく
                _canvasInteractableModule.Initialize(this);
                _modules.Add(_canvasInteractableModule);
#if USE_INPUTSYSTEM
                _autoFocusModule.Initialize(this);
                _modules.Add(_autoFocusModule);
#endif
                _linkModule.Initialize(this);
                _modules.Add(_linkModule);
                _cancelModule.Initialize(this);
                _modules.Add(_cancelModule);
                _canvasFadeModule.Initialize(this);
                _modules.Add(_canvasFadeModule);
                return _modules;
            }
        }

        public void Execute(Action<MornUGUIStateModuleBase> action)
        {
            foreach (var module in Modules)
            {
                action(module);
            }
        }

        private void Awake()
        {
            Execute(module => module.OnAwake());
        }

        public override void OnStateBegin()
        {
            Execute(module => module.OnStateBegin());
        }

        public override void OnStateUpdate()
        {
            Execute(module => module.OnStateUpdate());
        }

        public override void OnStateEnd()
        {
            Execute(module => module.OnStateEnd());
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MornUGUIControlState))]
    public sealed class MornUGUIButtonStateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var controlState = (MornUGUIControlState)target;
            if (GUILayout.Button("Buttonの再取得"))
            {
                controlState.Execute(module => module.OnEditorInitialize());
                controlState.RebuildStateLinkCache();
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Buttonの復元"))
            {
                controlState.Execute(module => module.OnEditorRestore());
                controlState.RebuildStateLinkCache();
                EditorUtility.SetDirty(target);
            }
        }
    }
#endif
}
#endif
