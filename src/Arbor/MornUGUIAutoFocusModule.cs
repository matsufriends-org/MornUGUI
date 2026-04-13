#if USE_ARBOR && USE_INPUTSYSTEM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal class MornUGUIAutoFocusModule : MornUGUIStateModuleBase
    {
        [SerializeField] private Selectable _target;
        [SerializeField, ShowIf(nameof(IsActive))] private bool _useCache = true;
        [SerializeField, ShowIf(nameof(IsActive))] private bool _findAdjacent;
        [SerializeField, ReadOnly] private Selectable _focusCache;
        private PlayerInput _cachedInput;
        private bool _isPointing;
        private Vector2? _cachedPointingPos;
        private MornUGUIControlState _parent;
        private bool IsActive => _target != null;

        public override void Initialize(MornUGUIControlState parent)
        {
            _parent = parent;
        }

        public override void OnStateBegin()
        {
            if (_target == null) return;
            var all = PlayerInput.all;
            if (all.Count == 0)
            {
                MornUGUIGlobal.Logger.LogWarning("PlayerInput is not found.");
                _cachedInput = null;
                return;
            }

            if (all.Count > 1)
            {
                MornUGUIGlobal.Logger.LogWarning("Multiple PlayerInput is found.");
                _cachedInput = null;
                return;
            }

            _cachedInput = all[0];
            if (_target != null && EventSystem.current.currentSelectedGameObject == _target.gameObject)
            {
                return;
            }

            // 初回の自動フォーカス
            AutoFocus();
        }

        private void AutoFocus()
        {
            if (_useCache && _focusCache != null && _focusCache.gameObject.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(_focusCache.gameObject);
                MornUGUIGlobal.Logger.Log("Focus on cache.");
            }
            else if (_target != null && _target.gameObject.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(_target.gameObject);
                MornUGUIGlobal.Logger.Log("Focus on target.");
            }
        }

        public override void OnStateUpdate()
        {
            if (_target == null || _cachedInput == null) return;

            // Navigate入力があった際にキャッシュを選択
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                var anyNavigate = _cachedInput.actions["Navigate"].controls.Any(x => x.IsPressed());
                var anySubmit = _cachedInput.actions["Submit"].controls.Any(x => x.IsPressed());
                var anyCancel = _cachedInput.actions["Cancel"].controls.Any(x => x.IsPressed());
                if (anyNavigate || anySubmit || anyCancel)
                {
                    // Navigateが動いてしまうため1F遅延
                    Observable.NextFrame().Subscribe(_ => AutoFocus()).AddTo(_parent);
                    _isPointing = false;
                }
            }

            if (_cachedInput.actions["Point"].WasPerformedThisFrame())
            {
                var newPoint = _cachedInput.actions["Point"].ReadValue<Vector2>();
                if (_isPointing)
                {
                    _cachedPointingPos = newPoint;
                }
                else
                {
                    _cachedPointingPos ??= newPoint;
                    if (Vector2.Distance(_cachedPointingPos.Value, newPoint) > 0.1f)
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        _isPointing = true;
                        _cachedPointingPos = newPoint;
                    }
                }
            }

            if (!_useCache)
            {
                return;
            }

            // キャッシュの更新処理
            var currentSelected = EventSystem.current.currentSelectedGameObject;
            var current = currentSelected == null ? null : currentSelected.GetComponent<Selectable>();
            if (current != null && IsFocusable(current))
            {
                _focusCache = current;
            }

            // キャッシュが非アクティブな場合、隣接を探す
            if (_findAdjacent && _focusCache != null && !_focusCache.gameObject.activeInHierarchy)
            {
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
                            if (distance < mostNearDistance && IsFocusable(near))
                            {
                                mostNearDistance = distance;
                                mostNear = near;
                            }
                        }
                    }

                    if (mostNear != null)
                    {
                        _focusCache = mostNear;
                        EventSystem.current.SetSelectedGameObject(_focusCache.gameObject);
                        MornUGUIGlobal.Logger.Log("Focus on cache near.");
                    }
                }
            }
        }

        private bool IsFocusable(Selectable selectable)
        {
            if (selectable.navigation.mode == Navigation.Mode.None) return false;
            return selectable.transform.IsChildOf(_parent.CanvasGroup.transform);
        }

        private async UniTaskVoid DelayAsync(Action action, CancellationToken cancellationToken)
        {
            await UniTask.Yield(cancellationToken);
            action();
        }

        public override void OnStateEnd()
        {
            if (_target == null) return;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
#endif
