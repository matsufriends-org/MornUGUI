using UnityEngine;
using UnityEngine.EventSystems;

namespace MornLib
{
    public abstract class MornUGUIModuleBase
    {
        public virtual void Initialize(MonoBehaviour owner)
        {
        }

        public virtual void Awake()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnDisable()
        {
        }

        public virtual void OnSelect()
        {
        }

        public virtual void OnDeselect()
        {
        }

        public virtual void OnMove(AxisEventData eventData)
        {
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
        }

        public virtual void OnSubmit()
        {
        }

        public virtual void OnValueChanged()
        {
        }
    }
}
