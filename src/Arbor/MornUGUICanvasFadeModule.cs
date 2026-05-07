#if USE_ARBOR || USE_MORNSTATE
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal class MornUGUICanvasFadeModule : MornUGUIStateModuleBase
    {
        [SerializeField] private bool _isActive;
        [SerializeField, ShowIf(nameof(IsActive))] private float _fadeInDuration = 0.3f;
        [SerializeField, ShowIf(nameof(IsActive))] private float _fadeOutDuration = 0.6f;
        private CancellationTokenSource _cts;
        private MornUGUIControlState _parent;
        private bool IsActive => _isActive;

        public override void Initialize(MornUGUIControlState parent)
        {
            _parent = parent;
        }

        public override void OnAwake()
        {
            if (!_isActive)
            {
                return;
            }

            _parent.CanvasGroup.alpha = 0;
        }

        public override void OnStateBegin()
        {
            if (!_isActive)
            {
                return;
            }

            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(_parent.destroyCancellationToken);
            _parent.CanvasGroup.alpha = 0;
            FadeCanvas(_parent.CanvasGroup, 1, _fadeInDuration, _cts.Token).Forget();
        }

        public override void OnStateEnd()
        {
            if (!_isActive)
            {
                return;
            }

            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(_parent.destroyCancellationToken);
            FadeCanvas(_parent.CanvasGroup, 0, _fadeOutDuration, _cts.Token).Forget();
        }

        private async static UniTaskVoid FadeCanvas(CanvasGroup target, float to, float duration, CancellationToken ct)
        {
            var from = target.alpha;
            var startTime = Time.time;
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime = Time.time - startTime;
                var alpha = Mathf.Lerp(from, to, elapsedTime / duration);
                target.alpha = alpha;
                if (Mathf.Approximately(target.alpha, to))
                {
                    break;
                }

                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }

            ct.ThrowIfCancellationRequested();
            target.alpha = to;
        }
    }
}
#endif
