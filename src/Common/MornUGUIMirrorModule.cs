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

            // _cameFrom の navigation を逆引きし、どの方向から _parent へ遷移してきたかを判定する。
            // 例: _cameFrom.selectOnUp == _parent なら、_cameFrom が下側にあって上入力で _parent に来た＝下から来た
            var fromLeft = SelectOnRightOf(_cameFrom) == _parent;
            var fromRight = SelectOnLeftOf(_cameFrom) == _parent;
            var fromUp = SelectOnDownOf(_cameFrom) == _parent;
            var fromDown = SelectOnUpOf(_cameFrom) == _parent;
            var applyLeft = _left && fromLeft;
            var applyRight = _right && fromRight;
            var applyUp = _up && fromUp;
            var applyDown = _down && fromDown;
            if (!applyLeft && !applyRight && !applyUp && !applyDown) return;

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
            if (applyLeft) nav.selectOnLeft = _cameFrom;
            if (applyRight) nav.selectOnRight = _cameFrom;
            if (applyUp) nav.selectOnUp = _cameFrom;
            if (applyDown) nav.selectOnDown = _cameFrom;
            _parent.navigation = nav;
        }

        private static Selectable SelectOnLeftOf(Selectable s) =>
            s.navigation.mode == Navigation.Mode.Automatic ? s.FindSelectableOnLeft() : s.navigation.selectOnLeft;

        private static Selectable SelectOnRightOf(Selectable s) =>
            s.navigation.mode == Navigation.Mode.Automatic ? s.FindSelectableOnRight() : s.navigation.selectOnRight;

        private static Selectable SelectOnUpOf(Selectable s) =>
            s.navigation.mode == Navigation.Mode.Automatic ? s.FindSelectableOnUp() : s.navigation.selectOnUp;

        private static Selectable SelectOnDownOf(Selectable s) =>
            s.navigation.mode == Navigation.Mode.Automatic ? s.FindSelectableOnDown() : s.navigation.selectOnDown;

        public override void OnDeselect()
        {
            if (!_overridden) return;
            _parent.navigation = _originalNav;
            _overridden = false;
        }
    }
}
