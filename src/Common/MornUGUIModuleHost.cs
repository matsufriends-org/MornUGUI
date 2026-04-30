using System;
using UnityEngine;

namespace MornLib
{
    internal sealed class MornUGUIModuleHost
    {
        private readonly MonoBehaviour _owner;
        private readonly Func<MornUGUIModuleBase[]> _factory;
        private MornUGUIModuleBase[] _modules;

        public MornUGUIModuleHost(MonoBehaviour owner, Func<MornUGUIModuleBase[]> factory)
        {
            _owner = owner;
            _factory = factory;
        }

        public void Execute(Action<MornUGUIModuleBase> action)
        {
            EnsureInitialized();
            foreach (var module in _modules)
            {
                action(module);
            }
        }

        private void EnsureInitialized()
        {
            if (_modules != null) return;
            _modules = _factory();
            foreach (var module in _modules)
            {
                module.Initialize(_owner);
            }
        }
    }
}
