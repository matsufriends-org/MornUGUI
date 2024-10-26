using MornFlag;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace MornUGUI
{
    [RequireComponent(typeof(Button))]
    public sealed class MornUGUIButtonSound : MonoBehaviour, ISelectHandler, ISubmitHandler
    {
        [Inject] private IMornFlagGetter _flagGetter;
        [SerializeField] private AudioSource _audioSource;

        public void OnSelect(BaseEventData eventData)
        {
            if (_flagGetter.GetFlag(MornUGUIUtil.BlockSelectSoundFlag))
            {
                return;
            }

            _audioSource.PlayOneShot(MornUGUIGlobal.I.ButtonCursorClip);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (_flagGetter.GetFlag(MornUGUIUtil.BlockSelectSoundFlag))
            {
                return;
            }

            _audioSource.PlayOneShot(MornUGUIGlobal.I.ButtonSubmitClip);
        }
    }
}