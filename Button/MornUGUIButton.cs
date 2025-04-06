using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace MornUGUI
{
    [RequireComponent(typeof(Button))]
    public class MornUGUIButton : MonoBehaviour,
        ISelectHandler,
        IDeselectHandler,
        ISubmitHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private bool _isNegative;
        [SerializeField] private MornUGUIButtonActiveModule _activeModule;
        [SerializeField] private MornUGUIButtonColorModule _colorModule;
        [SerializeField] private MornUGUIButtonConvertPointerToSelectModule _convertPointerToSelectModule;
        [SerializeField] private MornUGUIButtonSoundModule _soundModule;
        [SerializeField] private MornUGUIButtonToggleModule _toggleModule;
        [Inject] private MornUGUICtrl _uguiCtrl; 
        public MornUGUICtrl UGUICtrl => _uguiCtrl;
        public bool IsInteractable { get; set; }
        public bool IsNegative => _isNegative;
        public IObservable<Unit>　OnButtonSelected => _button.OnSelectAsObservable().Select(_ => Unit.Default);
        public IObservable<Unit> OnButtonSubmit => _button.OnSubmitAsObservable().Select(_ => Unit.Default);
        public MornUGUIButtonToggleModule AsToggle => _toggleModule;
        public MornUGUIButtonActiveModule AsActive => _activeModule;
        
        private IEnumerable<MornUGUIButtonModuleBase> GetModules()
        {
            yield return _activeModule;
            yield return _colorModule;
            yield return _convertPointerToSelectModule;
            yield return _soundModule;
            yield return _toggleModule;
        }

        private void Execute(Action<MornUGUIButtonModuleBase, MornUGUIButton> action)
        {
            foreach (var module in GetModules())
            {
                action(module, this);
            }
        }

        private void Awake()
        {
            Execute((module, parent) => module.Awake(parent));
        }

        private void Update()
        {
            Execute((module, parent) => module.Update(parent));
        }

        public void OnSelect(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnSelect(parent));
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnDeselect(parent));
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnSubmit(parent));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Execute((module, parent) => module.OnPointerEnter(parent));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Execute((module, parent) => module.OnPointerExit(parent));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Execute((module, parent) => module.OnPointerDown(parent));
        }
    }
}