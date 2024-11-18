using System;
using System.Collections.Generic;
using System.Linq;
using Arbor;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MornUGUI
{
    [Serializable]
    internal class ButtonStateLinkSet
    {
        public Button Button;
        public StateLink StateLink;
    }

    public class MornUGUIButtonState : StateBehaviour
    {
        [SerializeField] internal Transform Parent;
        [SerializeField] [ReadOnly] internal List<ButtonStateLinkSet> ButtonStateLinkSets;

        public override void OnStateBegin()
        {
            foreach (var buttonStateLinkSet in ButtonStateLinkSets)
                buttonStateLinkSet.Button.OnClickAsObservable().Subscribe(_ =>
                {
                    Transition(buttonStateLinkSet.StateLink);
                }).AddTo(CancellationTokenOnEnd);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MornUGUIButtonState))]
    public class MornUGUIButtonStateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var buttonState = (MornUGUIButtonState)target;
            if (GUILayout.Button("Gather"))
            {
                buttonState.ButtonStateLinkSets = buttonState.Parent.GetComponentsInChildren<Button>().Select(button =>
                {
                    var stateLinkSet = new ButtonStateLinkSet
                    {
                        Button = button, StateLink = new StateLink { name = button.name }
                    };
                    return stateLinkSet;
                }).ToList();
                buttonState.RebuildFields();
                EditorUtility.SetDirty(target);
            }
        }
    }
#endif
}
