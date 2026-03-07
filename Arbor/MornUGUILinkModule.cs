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
            var selectables = _parent.CanvasGroup.transform.GetComponentsInChildren<Selectable>().ToList();
            foreach (var selectable in selectables)
            {
                if (selectable.GetComponent<MornUGUIIgnore>() != null) continue;
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
                selectables.All(y => y != x.Target) || x.Target.GetComponent<MornUGUIIgnore>() != null);
        }
    }
}
#endif
