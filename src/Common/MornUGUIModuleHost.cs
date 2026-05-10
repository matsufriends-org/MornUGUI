using System;
using System.Collections.Generic;
using UnityEngine;

namespace MornLib
{
    internal sealed class MornUGUIModuleHost
    {
        private readonly MonoBehaviour _owner;
        private readonly Func<MornUGUIModuleBase[]> _factory;
        private readonly HashSet<MornUGUIModuleBase> _initialized = new();

        public MornUGUIModuleHost(MonoBehaviour owner, Func<MornUGUIModuleBase[]> factory)
        {
            _owner = owner;
            _factory = factory;
        }

        public void Execute(Action<MornUGUIModuleBase> action)
        {
            var modules = _factory();
            if (modules == null) return;
            foreach (var module in modules)
            {
                if (module == null) continue;
                if (_initialized.Add(module)) module.Initialize(_owner);
                action(module);
            }
        }
    }
}
