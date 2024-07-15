using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornUGUI
{
    [RequireComponent(typeof(Button))]
    public class MornUGUIButtonGameObjectChanger : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private GameObject _focused;
        [SerializeField] private GameObject _unfocused;

        private void Awake()
        {
            _focused.SetActive(false);
            _unfocused.SetActive(true);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _focused.SetActive(false);
            _unfocused.SetActive(true);
        }

        public void OnSelect(BaseEventData eventData)
        {
            _focused.SetActive(true);
            _unfocused.SetActive(false);
        }
    }
}