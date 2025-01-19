using System;
using System.Collections.Generic;
using Arbor;
using MornFlag;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace MornUGUI
{
    internal class MornUGUIControlState : StateBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private MornUGUICanvasInteractableModule _canvasInteractableModule;
        [SerializeField] private MornUGUICanvasFadeModule _canvasFadeModule;
        [SerializeField] private MornUGUIButtonModule _buttonModule;
        [SerializeField] private MornUGUIFocusModule _focusModule;
        [SerializeField] private MornUGUICancelModule _cancelModule;
        [SerializeField] private MornUGUISoundBlockModule _soundBlockModule;
        [Inject] private IMornFlagSetter _flagSetter;
        public IMornFlagSetter FlagSetter => _flagSetter;
        public CanvasGroup CanvasGroup => _canvasGroup;

        private IEnumerable<MornUGUIModuleBase> GetModules()
        {
            // SoundBlockModuleは最初に実行する
            yield return _soundBlockModule;
            yield return _canvasInteractableModule;
            yield return _canvasFadeModule;
            yield return _buttonModule;
            yield return _focusModule;
            yield return _cancelModule;
        }

        public void Execute(Action<MornUGUIModuleBase, MornUGUIControlState> action)
        {
            foreach (var module in GetModules())
            {
                action(module, this);
            }
        }

        public override void OnStateBegin()
        {
            Execute((module, parent) => module.OnStateBegin(parent));
        }

        public override void OnStateUpdate()
        {
            Execute((module, parent) => module.OnStateUpdate(parent));
        }

        public override void OnStateEnd()
        {
            Execute((module, parent) => module.OnStateEnd(parent));
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
            if (GUILayout.Button("(Editor)Initialize"))
            {
                controlState.Execute((module, parent) => module.OnEditorInitialize(parent));
                controlState.RebuildFields();
                EditorUtility.SetDirty(target);
            }
        }
    }
#endif
}