using UnityEngine;

namespace Game.Scripts.Patterns
{
    public abstract class Singleton<T> : MonoBehaviour where T: Component
    {
        private static T _instance;
        public static T Instance {
            get
            {
                _instance ??= FindAnyObjectByType<T>();

                if (_instance != null) return _instance;
                var go = new GameObject(typeof(T).Name);
                _instance = go.AddComponent<T>();

                return _instance;
            }
        }

        public static bool HasInstance()
        {
            return _instance != null;
        }

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
