using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornUGUI
{
    [RequireComponent(typeof(Button))]
    [ExecuteAlways]
    public class MornUGUIButtonGameObjectChanger : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
    {
        [SerializeField] private GameObject _focused;
        [SerializeField] private GameObject _unfocused;
        private bool _isSelect;

        public void OnDeselect(BaseEventData eventData)
        {
            _isSelect = false;
            _focused.SetActive(false);
            _unfocused.SetActive(true);
        }

        public void OnSelect(BaseEventData eventData)
        {
            _isSelect = true;
            _focused.SetActive(true);
            _unfocused.SetActive(false);
        }

        public void OnSubmit(BaseEventData eventData)
        {
        }
    }
}