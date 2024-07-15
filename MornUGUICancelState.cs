using Arbor;
using MornInput;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace MornUGUI
{
    public class MornUGUICancelState : StateBehaviour
    {
        [SerializeField] private GameObject _target;
        [Inject] private IMornInput _input;

        public override void OnStateUpdate()
        {
            if (_input.IsPressStart("Cancel"))
            {
                var current = EventSystem.current.currentSelectedGameObject;
                if (current != _target) EventSystem.current.SetSelectedGameObject(_target);
                else
                    ExecuteEvents.Execute(_target, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }
        }
    }
}