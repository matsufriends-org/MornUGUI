#if USE_ARBOR
using System;
using System.Collections.Generic;
using System.Linq;
using Arbor;
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
                    _stateLinkSets[index].StateLink.name = selectable.name;
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

                var matched = selectables.FirstOrDefault(s => s != null && s.name == set.StateLink.name);
                if (matched != null)
                {
                    set.Target = matched;
                }
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
#endif
