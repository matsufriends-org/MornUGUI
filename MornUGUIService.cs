using UnityEngine;

namespace MornUGUI
{
    public class MornUGUIService : MonoBehaviour
    {
        private static MornUGUIService _instance;
        public static MornUGUIService I
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<MornUGUIService>(FindObjectsInactive.Include);
                }
                
                if (_instance == null)
                {
                    var go = new GameObject(nameof(MornUGUIService));
                    _instance = go.AddComponent<MornUGUIService>();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public bool IsBlocking { get; private set; }

        public void BlockOn()
        {
            IsBlocking = true;
        }

        public void BlockOff()
        {
            IsBlocking = false;
        }
    }
}