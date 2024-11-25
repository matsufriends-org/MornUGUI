using System;
using UnityEngine;

namespace MornUGUI
{
    [Serializable]
    public sealed class MornUGUIButtonActiveModule : MornUGUIButtonModuleBase
    {
        [SerializeField] private GameObject _focused;
        [SerializeField] private GameObject _unfocused;

        public override void Awake()
        {
            if (_focused != null)
            {
                _focused?.SetActive(false);
            }

            if (_unfocused != null)
            {
                _unfocused?.SetActive(true);
            }
        }

        public override void OnDeselect()
        {
            if (_focused != null)
            {
                _focused?.SetActive(false);
            }

            if (_unfocused != null)
            {
                _unfocused?.SetActive(true);
            }
        }

        public override void OnSelect()
        {
            if (_focused != null)
            {
                _focused?.SetActive(true);
            }

            if (_unfocused != null)
            {
                _unfocused?.SetActive(false);
            }
        }
    }
}