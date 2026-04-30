using UnityEngine;

namespace MornLib
{
    [AddComponentMenu("MornUGUI/Active")]
    internal sealed class MornUGUIActive : MornUGUIMonoBase
    {
        [SerializeField] private GameObject _focused;
        [SerializeField] private GameObject _unfocused;

        public override void OnSelect()
        {
            if (_focused != null) _focused.SetActive(true);
            if (_unfocused != null) _unfocused.SetActive(false);
        }

        public override void OnDeselect()
        {
            ApplyUnfocused();
        }

        private void Awake()
        {
            ApplyUnfocused();
        }

        private void OnDisable()
        {
            ApplyUnfocused();
        }

        private void ApplyUnfocused()
        {
            if (_focused != null) _focused.SetActive(false);
            if (_unfocused != null) _unfocused.SetActive(true);
        }
    }
}
