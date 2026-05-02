using UnityEngine;

namespace MornLib
{
    /// <summary>
    /// 再生開始ごとに Bump される version カウンタ。
    /// Enter Play Mode Options で Reload Domain/Scene が OFF の場合、 MonoBehaviour の非シリアライズフィールドが play session 跨ぎで保持されるため、
    /// 「初回初期化済みフラグ」 を bool で持つと前セッションの true が残る。
    /// 代わりにこの version と比較することで「再生開始時に確実に再初期化」を実現する。
    /// </summary>
    internal static class MornUGUIPlayModeVersion
    {
        public static int Current { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnEnterPlayMode()
        {
            Current++;
        }
    }
}
