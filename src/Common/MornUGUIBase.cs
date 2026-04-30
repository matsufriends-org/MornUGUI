using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    public abstract class MornUGUIBase : Selectable, ISubmitHandler, IPointerClickHandler
    {
        [SerializeField] private bool _unfocusOnNotInteractable = true;
        private MornUGUIModuleHost _host;
        private MornUGUIModuleHost Host => _host ??= new MornUGUIModuleHost(this, BuildModules);

        internal abstract MornUGUIModuleBase[] BuildModules();

        protected override void Awake()
        {
            base.Awake();
            Host.Execute(module => module.Awake());
        }

        protected virtual void Update()
        {
            if (_unfocusOnNotInteractable && !IsInteractable() && EventSystem.current != null &&
                EventSystem.current.currentSelectedGameObject == gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }

            Host.Execute(module => module.Update());
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Host.Execute(module => module.OnEnable());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Host.Execute(module => module.OnDisable());
        }

        public override void OnSelect(BaseEventData eventData)
        {
            if (!IsInteractable()) return;
            base.OnSelect(eventData);
            Host.Execute(module => module.OnSelect());
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            // interactable=falseによる非フォーカス化を望まない場合、モジュールへのOnDeselect伝搬も抑制して見た目を維持
            if (!_unfocusOnNotInteractable && !IsInteractable()) return;
            Host.Execute(module => module.OnDeselect());
        }

        public override void OnMove(AxisEventData eventData)
        {
            if (!IsInteractable()) return;
            base.OnMove(eventData);
            Host.Execute(module => module.OnMove(eventData));
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsInteractable()) return;
            base.OnPointerEnter(eventData);
            Host.Execute(module => module.OnPointerEnter(eventData));
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            Host.Execute(module => module.OnPointerExit(eventData));
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!IsInteractable()) return;
            base.OnPointerDown(eventData);
            Host.Execute(module => module.OnPointerDown(eventData));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsInteractable()) return;
            Host.Execute(module => module.OnPointerClick(eventData));
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            if (!IsInteractable()) return;
            Host.Execute(module => module.OnSubmit());
        }

        protected virtual void ValueChanged()
        {
            Host.Execute(module => module.OnValueChanged());
        }
    }
}
