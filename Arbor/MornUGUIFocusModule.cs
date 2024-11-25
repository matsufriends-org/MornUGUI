using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace MornUGUI
{
    [Serializable]
    public class MornUGUIFocusModule : MornUGUIModuleBase
    {
        [SerializeField] private bool _useCache = true;
        [SerializeField] private GameObject _autoFocusTarget;
        [SerializeField] [ReadOnly] private GameObject _focusCache;
        private PlayerInput _cachedInput;
        private string _cachedScheme;

        public override void OnStateBegin()
        {
            var all = PlayerInput.all;
            if (all.Count == 0)
            {
                MornUGUIGlobal.I.LogWarning("PlayerInput is not found.");
                _cachedInput = null;
                return;
            }

            if (all.Count > 1)
            {
                MornUGUIGlobal.I.LogWarning("Multiple PlayerInput is found.");
                _cachedInput = null;
                return;
            }

            _cachedInput = all[0];
            if (EventSystem.current.currentSelectedGameObject == _autoFocusTarget)
            {
                return;
            }

            // 現在のPlayerInputを取得
            var currentScheme = _cachedInput.currentControlScheme;
            _cachedScheme = currentScheme;
            if (currentScheme.Length > 0 && currentScheme != "Mouse")
            {
                if (_useCache && _focusCache != null)
                {
                    EventSystem.current.SetSelectedGameObject(_focusCache);
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(_autoFocusTarget);
                }
            }
        }

        public override void OnStateUpdate()
        {
            if (_cachedInput == null)
            {
                return;
            }

            var nextScheme = _cachedInput.currentControlScheme;
            if (nextScheme != _cachedScheme)
            {
                if (nextScheme == "Mouse")
                {
                    // マウスに変化する場合はフォーカスを外す
                    EventSystem.current.SetSelectedGameObject(null);
                }
                else if (_cachedScheme == "Mouse")
                {
                    // マウスからそれ以外へ変化する場合はフォーカスを設定
                    EventSystem.current.SetSelectedGameObject(_autoFocusTarget);
                }

                _cachedScheme = nextScheme;
            }

            var current = EventSystem.current.currentSelectedGameObject;
            if (current != null && _useCache)
            {
                _focusCache = EventSystem.current.currentSelectedGameObject;
            }
        }

        public override void OnStateEnd()
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}