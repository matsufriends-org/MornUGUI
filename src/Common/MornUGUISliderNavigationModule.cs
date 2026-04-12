using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MornLib
{
    [Serializable]
    internal sealed class MornUGUISliderNavigationModule : MornUGUIModuleBase
    {
        [SerializeField] private Selectable _up;
        [SerializeField] private Selectable _down;
        [SerializeField] private Selectable _left;
        [SerializeField] private Selectable _right;
        private MornUGUISlider _slider;
        private float _lastValue;

        public void Initialize(MornUGUISlider slider)
        {
            _slider = slider;
        }

        public override void OnValueChanged()
        {
            _lastValue = _slider.Value;
        }

        public override void OnMove(AxisEventData axisEventData)
        {
            switch (_slider.Direction)
            {
                case Slider.Direction.LeftToRight:
                    MoveHorizontal(axisEventData, true);
                    break;
                case Slider.Direction.RightToLeft:
                    MoveHorizontal(axisEventData, false);
                    break;
                case Slider.Direction.BottomToTop:
                    MoveVertical(axisEventData, true);
                    break;
                case Slider.Direction.TopToBottom:
                    MoveVertical(axisEventData, false);
                    break;
            }
        }

        private void MoveHorizontal(AxisEventData axisEventData, bool isToRight)
        {
            var toLeft = axisEventData.moveDir == MoveDirection.Left && _left != null;
            var toRight = axisEventData.moveDir == MoveDirection.Right && _right != null;
            if (toLeft || toRight)
            {
                var atMin = Mathf.Approximately(_slider.Value, _slider.MinValue);
                var atMax = Mathf.Approximately(_slider.Value, _slider.MaxValue);
                var atLeft = isToRight ? atMin : atMax;
                var atRight = isToRight ? atMax : atMin;
                var canLeft = toLeft && atLeft;
                var canRight = toRight && atRight;
                if ((canLeft || canRight) && Mathf.Approximately(_slider.Value, _lastValue))
                {
                    var nextSelectable = canLeft ? _left : _right;
                    EventSystem.current.SetSelectedGameObject(nextSelectable.gameObject);
                }
            }
        }

        private void MoveVertical(AxisEventData axisEventData, bool isTop)
        {
            var toUp = axisEventData.moveDir == MoveDirection.Up && _up != null;
            var toDown = axisEventData.moveDir == MoveDirection.Down && _down != null;
            if (toUp || toDown)
            {
                var atMin = Mathf.Approximately(_slider.Value, _slider.MinValue);
                var atMax = Mathf.Approximately(_slider.Value, _slider.MaxValue);
                var atDown = isTop ? atMin : atMax;
                var atUp = isTop ? atMax : atMin;
                var canUp = toUp && atUp;
                var canDown = toDown && atDown;
                if ((canUp || canDown) && Mathf.Approximately(_slider.Value, _lastValue))
                {
                    var nextSelectable = canUp ? _up : _down;
                    EventSystem.current.SetSelectedGameObject(nextSelectable.gameObject);
                }
            }
        }
    }
}