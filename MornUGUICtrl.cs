using MornSound;
using UnityEngine;

namespace MornUGUI
{
    public class MornUGUICtrl : MonoBehaviour
    {
        [SerializeField] private AudioSource _seSource;
        public bool IsBlocking { get; private set; }

        public void BlockOn()
        {
            IsBlocking = true;
        }

        public void BlockOff()
        {
            IsBlocking = false;
        }
        
        public void PlayOneShot(AudioClip clip)
        {
            if (_seSource != null && clip != null)
            {
                _seSource.MornPlayOneShot(clip);
            }
        }
    }
}