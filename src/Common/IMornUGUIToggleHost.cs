using System;

namespace MornLib
{
    public interface IMornUGUIToggleHost
    {
        bool IsToggleOn { get; }
        IObservable<bool> OnToggleChanged { get; }
    }
}
