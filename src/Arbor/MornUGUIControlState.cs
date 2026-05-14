using System;
using System.Collections.Generic;
#if USE_MORNSTATE
using MornLib;
using StateBehaviour = MornLib.MornStateBehaviour;
#elif USE_ARBOR
using Arbor;
#endif
using UnityEngine;

#if USE_MORNSTATE || USE_ARBOR
namespace MornLib
{
    [Serializable]
    internal class MornUGUIControlState : StateBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private MornUGUIAnimationModule _animationModule;
        [SerializeField] private MornUGUIAutoFocusModule _autoFocusModule;
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
                _autoFocusModule.Initialize(this);
                _modules.Add(_autoFocusModule);
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

#if !USE_ARBOR
        public override void OnAwake()
#else
        private void Awake()
#endif
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

        [Button("Buttonの再取得")]
        public void RefreshButtons()
        {
            Execute(module => module.OnEditorInitialize());
#if USE_ARBOR
            RebuildStateLinkCache();
#endif
        }

        [Button("Buttonの復元")]
        public void RestoreButtons()
        {
            Execute(module => module.OnEditorRestore());
#if USE_ARBOR
            RebuildStateLinkCache();
#endif
        }
    }
}

#endif // USE_MORNSTATE || USE_ARBOR
