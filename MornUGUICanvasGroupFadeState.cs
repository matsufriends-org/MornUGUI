using Arbor;
using DG.Tweening;
using UnityEngine;

namespace MornUGUI
{
    public class MornUGUICanvasGroupFadeState : StateBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeInDuration = 0.3f;
        [SerializeField] private float _fadeOutDuration = 0.6f;
        private Tween _cachedTween;

        public override void OnStateBegin()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 0;
            _cachedTween?.Kill();
            _cachedTween = DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1, _fadeInDuration);
        }

        public override void OnStateEnd()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _cachedTween?.Kill();
            _cachedTween = DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0, _fadeOutDuration);
        }
    }
}