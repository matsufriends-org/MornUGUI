using System;
using System.Collections.Generic;
using System.Linq;
#if USE_ARBOR
using Arbor;
#endif
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal class MornUGUILinkModule : MornUGUIStateModuleBase
    {
        [Serializable]
        private class StateLinkSet
        {
            public Selectable Target;
            public StateLink StateLink;
        }

        [SerializeField, ReadOnly] private List<StateLinkSet> _stateLinkSets;
        private MornUGUIControlState _parent;

        public override void Initialize(MornUGUIControlState parent)
        {
            _parent = parent;
        }

        public override void OnStateBegin()
        {
            foreach (var set in _stateLinkSets)
            {
                var linkSet = set;
                if (linkSet.StateLink == null || linkSet.StateLink.stateID == 0) continue;
                linkSet.Target.OnSubmitAsObservable().Subscribe(_ => _parent.Transition(linkSet.StateLink))
                       .AddTo(_parent.CancellationTokenOnEnd);
            }
        }

        public override void OnEditorInitialize()
        {
            var selectables = _parent.CanvasGroup.transform.GetComponentsInChildren<Selectable>()
                .Where(IsButtonCandidate).ToList();
            foreach (var selectable in selectables)
            {
                var index = _stateLinkSets.FindIndex(x => x.Target == selectable);
                if (index != -1)
                {
                    var old = _stateLinkSets[index].StateLink;
#if USE_ARBOR
                    _stateLinkSets[index].StateLink = old == null
                        ? new StateLink { name = selectable.name }
                        : new StateLink
                        {
                            name = selectable.name,
                            stateID = old.stateID,
                            transitionTiming = old.transitionTiming,
                            lineColor = old.lineColor,
                            onTransitionCountChanged = old.onTransitionCountChanged,
                            transitionCount = old.transitionCount,
                        };
#else
                    if (old == null)
                    {
                        _stateLinkSets[index].StateLink = new StateLink { name = selectable.name };
                    }
                    else
                    {
                        old.name = selectable.name;
                    }
#endif
                }
                else
                {
                    _stateLinkSets.Add(
                        new StateLinkSet { Target = selectable, StateLink = new StateLink { name = selectable.name } });
                }
            }

            _stateLinkSets.RemoveAll(x =>
                selectables.All(y => y != x.Target) || !IsButtonCandidate(x.Target));
        }

        /// <summary>
        /// Target が None/Missing になった Set を StateLink.name と一致する Selectable に再バインドする。
        /// StateLink.stateID は保持するので、リンク先の State 遷移設定を失わずに参照だけ復旧できる。
        /// </summary>
        public override void OnEditorRestore()
        {
            if (_stateLinkSets == null)
            {
                return;
            }

            var selectables = _parent.CanvasGroup.transform
                .GetComponentsInChildren<Selectable>(includeInactive: true)
                .Where(IsButtonCandidate).ToList();
            foreach (var set in _stateLinkSets)
            {
                if (set.Target != null)
                {
                    continue;
                }

                if (set.StateLink == null || string.IsNullOrEmpty(set.StateLink.name))
                {
                    continue;
                }

                var matches = selectables.Where(s => s != null && s.name == set.StateLink.name).ToList();
                if (matches.Count == 0)
                {
                    continue;
                }

                if (matches.Count > 1)
                {
                    Debug.LogWarning(
                        $"[MornUGUILinkModule] Buttonの復元: '{set.StateLink.name}' に一致する Selectable が {matches.Count} 個見つかったため復元をスキップしました。手動で Target を設定してください。",
                        _parent.gameObject);
                    continue;
                }

                set.Target = matches[0];
            }
        }

        /// <summary>
        /// Button として扱うべき Selectable か判定する。
        /// Slider / Scrollbar などボタンでない Selectable と、MornUGUIIgnore が付いたものを除外する。
        /// </summary>
        private static bool IsButtonCandidate(Selectable selectable)
        {
            if (selectable == null)
            {
                return false;
            }

            if (selectable is Slider || selectable is Scrollbar)
            {
                return false;
            }

            if (selectable.GetComponent<MornUGUIIgnore>() != null)
            {
                return false;
            }

            return true;
        }
    }
}
