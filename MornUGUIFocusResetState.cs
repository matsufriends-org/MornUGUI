using Arbor;
using Cysharp.Threading.Tasks;
using MornInput;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace MornUGUI
{
    public class MornUGUIFocusResetState : StateBehaviour
    {
        [SerializeField] private GameObject _focusObject;
        [Inject] private IMornInput _inputController;

        public override void OnStateBegin()
        {
            if (_inputController.CurrentScheme != "Mouse") EventSystem.current.SetSelectedGameObject(_focusObject);
            _inputController.OnSchemeChanged.Subscribe(pair =>
            {
                var (prev, next) = pair;
                if (next == "Mouse") EventSystem.current.SetSelectedGameObject(null);
                else if (prev == "Mouse") EventSystem.current.SetSelectedGameObject(_focusObject);
            }).AddTo(CancellationTokenOnEnd);
        }
    }
}