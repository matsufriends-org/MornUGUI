using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/Slider")]
    [RequireComponent(typeof(Slider))]
    internal sealed class MornUGUISlider : MonoBehaviour,
        ISelectHandler,
        IDeselectHandler,
        ISubmitHandler,
        IMoveHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerClickHandler,
        IMornUGUIObject,
        IMornUGUIInteractable
    {
        [SerializeField, Me] private Slider _slider;
        [Header("Modules")]
        [SerializeField] private MornUGUIPointerModule _pointerModule = new();
        [SerializeField] private MornUGUISliderNavigationModule _navigationModule = new();
        [SerializeField] private MornUGUISliderSoundModule _sliderSoundModule = new();
        [SerializeField, Childrens(true, true)] private MornUGUIMonoBase[] _monoModules;
        private MornUGUIModuleHost _host;
        public bool IsInteractable { get; set; }
        public Slider.Direction Direction => _slider.direction;
        public float Value => _slider.value;
        public float MinValue => _slider.minValue;
        public float MaxValue => _slider.maxValue;
        bool IMornUGUIInteractable.IsLocked => !IsInteractable;
        bool IMornUGUIInteractable.IsNegative => false;
        Transform IMornUGUIObject.Transform => transform;
        GameObject IMornUGUIObject.GameObject => gameObject;
        CancellationToken IMornUGUIObject.DestroyCancellationToken => destroyCancellationToken;
        private MornUGUIModuleHost Host => _host ??= new MornUGUIModuleHost(this, BuildModules);

        private MornUGUIModuleBase[] BuildModules()
        {
            return new MornUGUIModuleBase[]
            {
                _pointerModule,
                _navigationModule,
                _sliderSoundModule,
            };
        }

        private void Awake()
        {
            foreach (var module in _monoModules)
            {
                module.Initialize(this);
            }

            _slider.onValueChanged.AddListener(_ => Host.Execute(module => module.OnValueChanged()));
            Host.Execute(module => module.Awake());
        }

        private void Update()
        {
            Host.Execute(module => module.Update());
        }

        private void OnEnable()
        {
            Host.Execute(module => module.OnEnable());
        }

        private void OnDisable()
        {
            Host.Execute(module => module.OnDisable());
        }

        public void OnSelect(BaseEventData eventData)
        {
            Host.Execute(module => module.OnSelect());
            foreach (var module in _monoModules)
            {
                module.OnSelect();
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Host.Execute(module => module.OnDeselect());
            foreach (var module in _monoModules)
            {
                module.OnDeselect();
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Host.Execute(module => module.OnSubmit());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Host.Execute(module => module.OnPointerEnter(eventData));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Host.Execute(module => module.OnPointerExit(eventData));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Host.Execute(module => module.OnPointerDown(eventData));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Host.Execute(module => module.OnPointerClick(eventData));
        }

        public void OnMove(AxisEventData eventData)
        {
            if (eventData.selectedObject != gameObject)
            {
                return;
            }

            Host.Execute(module => module.OnMove(eventData));
        }
    }
}
