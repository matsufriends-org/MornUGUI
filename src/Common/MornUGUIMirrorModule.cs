using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUIMirrorModule : MornUGUIModuleBase
    {
        [SerializeField] private bool _left;
        [SerializeField] private bool _right;
        [SerializeField] private bool _up;
        [SerializeField] private bool _down;
        private Selectable _parent;
        private Selectable _cameFrom;
        private Navigation _originalNav;
        private bool _overridden;
        private static Selectable s_trackedSelection;

        public void Initialize(Selectable parent)
        {
            _parent = parent;
        }

        public override void Update()
        {
            if (EventSystem.current == null) return;
            var currentGo = EventSystem.current.currentSelectedGameObject;
            if (currentGo == null) return;
            var current = currentGo.GetComponent<Selectable>();
            if (current != null)
            {
                s_trackedSelection = current;
            }
        }

        public override void OnSelect()
        {
            _overridden = false;
            _cameFrom = s_trackedSelection != _parent ? s_trackedSelection : null;
            if (_cameFrom == null) return;
            if (!_left && !_right && !_up && !_down) return;
            _originalNav = _parent.navigation;
            _overridden = true;
            var nav = _originalNav;
            if (nav.mode == Navigation.Mode.Automatic)
            {
                nav.selectOnLeft = _parent.FindSelectableOnLeft();
                nav.selectOnRight = _parent.FindSelectableOnRight();
                nav.selectOnUp = _parent.FindSelectableOnUp();
                nav.selectOnDown = _parent.FindSelectableOnDown();
            }

            nav.mode = Navigation.Mode.Explicit;
            if (_left) nav.selectOnLeft = _cameFrom;
            if (_right) nav.selectOnRight = _cameFrom;
            if (_up) nav.selectOnUp = _cameFrom;
            if (_down) nav.selectOnDown = _cameFrom;
            _parent.navigation = nav;
        }

        public override void OnDeselect()
        {
            if (!_overridden) return;
            _parent.navigation = _originalNav;
            _overridden = false;
        }
    }
}
