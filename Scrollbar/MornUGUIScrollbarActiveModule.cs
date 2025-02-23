using System;
using UnityEngine;

namespace MornUGUI
{
    [Serializable]
    internal sealed class MornUGUIScrollbarActiveModule : MornUGUIScrollbarModuleBase
    {
        [SerializeField] private GameObject _upperArrow;
        [SerializeField] private GameObject _bottomArrow;

        public override void OnEnable(MornUGUIScrollbar parent)
        {
            UpdateArrow(parent);
        }

        public override void OnDisable(MornUGUIScrollbar parent)
        {
            if (_upperArrow != null)
            {
                _upperArrow.SetActive(false);
            }

            if (_bottomArrow != null)
            {
                _bottomArrow.SetActive(false);
            }
        }

        public override void Awake(MornUGUIScrollbar parent)
        {
            UpdateArrow(parent);
        }

        public override void OnValueChanged(MornUGUIScrollbar parent)
        {
            UpdateArrow(parent);
        }

        private void UpdateArrow(MornUGUIScrollbar parent)
        {
            var canMove = parent.Size < 1;
            var canUpper = canMove && !Mathf.Approximately(parent.Value, 1);
            var canBottom = canMove && !Mathf.Approximately(parent.Value, 0);
            if (_upperArrow != null)
            {
                _upperArrow.SetActive(canUpper);
            }

            if (_bottomArrow != null)
            {
                _bottomArrow.SetActive(canBottom);
            }
        }
    }
}