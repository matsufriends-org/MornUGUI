using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornUGUI
{
    [RequireComponent(typeof(Button))]
    internal class MornUGUIConvertPointerToSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler
    {
        private bool _isExist;

        private void Update()
        {
            if (_isExist && EventSystem.current.currentSelectedGameObject != gameObject)
                EventSystem.current.SetSelectedGameObject(gameObject);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            ExecuteEvents.Execute(gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _isExist = true;
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _isExist = false;
            if (EventSystem.current.currentSelectedGameObject == gameObject)
                EventSystem.current.SetSelectedGameObject(null);
        }
    }
}