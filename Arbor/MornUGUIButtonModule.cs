using System;
using System.Collections.Generic;
using System.Linq;
using Arbor;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MornUGUI
{
    [Serializable]
    internal class MornUGUIButtonModule : MornUGUIModuleBase
    {
        [Serializable]
        private class ButtonStateLinkSet
        {
            public Button Button;
            public StateLink StateLink;
        }

        [SerializeField] [ReadOnly] private List<ButtonStateLinkSet> _buttonStateLinkSets;

        public override void OnStateBegin(MornUGUIControlState parent)
        {
            foreach (var buttonStateLinkSet in _buttonStateLinkSets)
            {
                buttonStateLinkSet.Button.OnClickAsObservable().Subscribe(
                    _ =>
                    {
                        parent.Transition(buttonStateLinkSet.StateLink);
                    }).AddTo(parent.CancellationTokenOnEnd);
            }
        }

        public override void OnEditorInitialize(MornUGUIControlState parent)
        {
            _buttonStateLinkSets = parent.CanvasGroup.transform.GetComponentsInChildren<Button>().Select(
                button =>
                {
                    var stateLinkSet = new ButtonStateLinkSet
                    {
                        Button = button,
                        StateLink = new StateLink
                        {
                            name = button.name,
                        }
                    };
                    return stateLinkSet;
                }).ToList();
        }
    }
}