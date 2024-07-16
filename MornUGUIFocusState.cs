using Arbor;
using Cysharp.Threading.Tasks;
using MornAttribute;
using MornInput;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace MornUGUI
{
    public class MornUGUIFocusState : StateBehaviour
    {
        [SerializeField] private GameObject _focusObject;
        [SerializeField] private bool _useCache = true;
        [SerializeField] [ReadOnly] private GameObject _focusCache;
        [Inject] private IMornInput _inputController;

        public override void OnStateBegin()
        {
            if (EventSystem.current.currentSelectedGameObject == _focusObject)
            {
                return;
            }
            
            var currentScheme = _inputController.CurrentScheme;
            if (currentScheme.Length > 0 && currentScheme != "Mouse")
            {
                if (_useCache && _focusCache != null) EventSystem.current.SetSelectedGameObject(_focusCache);
                else EventSystem.current.SetSelectedGameObject(_focusObject);
            }

            _inputController.OnSchemeChanged.Subscribe(pair =>
            {
                var (prev, next) = pair;
                if (next == "Mouse") EventSystem.current.SetSelectedGameObject(null);
                else if (prev == "Mouse") EventSystem.current.SetSelectedGameObject(_focusObject);
            }).AddTo(CancellationTokenOnEnd);
        }

        public override void OnStateUpdate()
        {
            var current = EventSystem.current.currentSelectedGameObject;
            if (current != null && _useCache) _focusCache = EventSystem.current.currentSelectedGameObject;
        }

        public override void OnStateEnd()
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}