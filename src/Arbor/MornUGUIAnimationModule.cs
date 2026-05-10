using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MornLib
{
    [Serializable]
    internal class MornUGUIAnimationModule : MornUGUIStateModuleBase
    {
        [SerializeField] private bool _isActive;
        [SerializeField, ShowIf(nameof(IsActive))] private BindAnimatorClip _inAnimation;
        [SerializeField, ShowIf(nameof(IsActive))] private BindAnimatorClip _outAnimation;
        private bool _isIn;
        private bool IsActive => _isActive;
        private CancellationTokenSource _cancellationTokenSource;
        private MornUGUIControlState _parent;

        public override void Initialize(MornUGUIControlState parent)
        {
            _parent = parent;
        }

        public override void OnStateBegin()
        {
            PlayInAnimationAsync(_parent.destroyCancellationToken).Forget();
        }

        public override void OnStateEnd()
        {
            PlayOutAnimationAsync(_parent.destroyCancellationToken).Forget();
        }

        public async UniTask PlayInAnimationAsync(CancellationToken ct)
        {
            if (!_isActive || !_inAnimation.IsValid || _isIn)
            {
                return;
            }

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
            _isIn = true;
            await PlayAnimation(_inAnimation, _cancellationTokenSource.Token);
        }

        public async UniTask PlayOutAnimationAsync(CancellationToken ct)
        {
            if (!_isActive || !_outAnimation.IsValid || !_isIn)
            {
                return;
            }

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct);
            _isIn = false;
            await PlayAnimation(_outAnimation, _cancellationTokenSource.Token);
        }

        private async UniTask PlayAnimation(BindAnimatorClip bindAnimatorClip, CancellationToken ct)
        {
            if (!bindAnimatorClip.IsValid)
            {
                return;
            }

            var animator = bindAnimatorClip.Animator;
            var clip = bindAnimatorClip.Clip;
            animator.Play(clip.name, 0, 0f);
            var normalizedTime = 0f;
            while (normalizedTime < 1f)
            {
                ct.ThrowIfCancellationRequested();
                var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName(clip.name))
                {
                    normalizedTime = stateInfo.normalizedTime;
                }

                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }
        }
    }
}
