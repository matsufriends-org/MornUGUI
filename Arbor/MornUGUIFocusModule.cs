using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MornUGUI
{
    [Serializable]
    internal class MornUGUIFocusModule : MornUGUIModuleBase
    {
        [SerializeField] private bool _useCache = true;
        [SerializeField] private GameObject _autoFocusTarget;
        [SerializeField] [ReadOnly] private GameObject _focusCache;
        private PlayerInput _cachedInput;
        private string _cachedScheme;

        public override void OnStateBegin(MornUGUIControlState parent)
        {
            if (_autoFocusTarget == null)
            {
                return;
            }

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
                    MornUGUIGlobal.I.Log("Focus on cache.");
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(_autoFocusTarget);
                    MornUGUIGlobal.I.Log("Focus on target.");
                }
            }
        }

        public override void OnStateUpdate(MornUGUIControlState parent)
        {
            if (_autoFocusTarget == null || _cachedInput == null)
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
                    MornUGUIGlobal.I.Log("Focus off by mouse.");
                }
                else if (_cachedScheme == "Mouse")
                {
                    // マウスからそれ以外へ変化する場合はフォーカスを設定
                    // そのキー入力がボタンに反応してしまうため、1F待機する
                    DelayAsync(
                        () =>
                        {
                            EventSystem.current.SetSelectedGameObject(_autoFocusTarget);
                            MornUGUIGlobal.I.Log("Auto Focus on target by mouse.");
                        },
                        parent.destroyCancellationToken).Forget();
                }

                _cachedScheme = nextScheme;
            }

            var current = EventSystem.current.currentSelectedGameObject;
            if (current != null && _useCache)
            {
                _focusCache = EventSystem.current.currentSelectedGameObject;
            }

            if (!_focusCache.activeInHierarchy &&_useCache)
            {
                // キャッシュの隣接を探す
                var selectable = _focusCache.GetComponent<Selectable>();

                if (selectable != null)
                {
                    var list = new List<Selectable>()
                    {
                        selectable.FindSelectableOnUp(),
                        selectable.FindSelectableOnDown(),
                        selectable.FindSelectableOnLeft(),
                        selectable.FindSelectableOnRight()
                    };

                    var mostNearDistance = float.MaxValue;
                    Selectable mostNear = null;

                    foreach (var near in list)
                    {
                        if (near != null && near.gameObject.activeInHierarchy)
                        {
                            var distance = Vector3.Distance(near.transform.position, _focusCache.transform.position);
                            if (distance < mostNearDistance)
                            {
                                mostNearDistance = distance;
                                mostNear = near;
                            }
                        }
                    }
                    
                    if (mostNear != null)
                    {
                        _focusCache = mostNear.gameObject;
                        EventSystem.current.SetSelectedGameObject(_focusCache);
                        MornUGUIGlobal.I.Log("Focus on cache near.");
                    }
                }
            }
        }

        private async UniTaskVoid DelayAsync(Action action, CancellationToken cancellationToken)
        {
            await UniTask.Yield(cancellationToken);
            action();
        }

        public override void OnStateEnd(MornUGUIControlState parent)
        {
            if (_autoFocusTarget == null)
            {
                return;
            }

            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}