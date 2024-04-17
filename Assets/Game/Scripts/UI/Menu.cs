using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Scripts.UI
{
    [RequireComponent(typeof(UIDocument))]
    public abstract class Menu : MonoBehaviour
    {
        [SerializeField] protected StyleSheet style;
        
        public enum DeviceType
        {
            Desktop,
            Mobile
        }
        protected UIDocument Document;
        [SerializeField] protected DeviceType deviceType = DeviceType.Desktop;
        public bool RefreshUI { get; set; }
        public DeviceType Device => deviceType;
        public bool IsMobile => deviceType == DeviceType.Mobile;
        
        protected void Start()
        {
            Debug.Log("Awoogs");
            StartCoroutine(Generate());

            if (Application.isPlaying && Application.platform == RuntimePlatform.IPhonePlayer)
            {
                deviceType = DeviceType.Mobile;
            }
        }

        private void Awake()
        {
            Document = GetComponent<UIDocument>();
            MenuManager.Instance.RegisterMenu(this);
        }

        private void OnValidate()
        {
            if (Application.isPlaying || !gameObject.activeInHierarchy) return;

            Document = GetComponent<UIDocument>();
            StartCoroutine(Generate());
        }
        
        private void Update()
        {
            if (!RefreshUI && !Input.GetKey(KeyCode.R)) return;
            RefreshUI = false;
            StartCoroutine(Generate());
        }

        protected abstract IEnumerator Generate();

        protected VisualElement Create(params string[] classNames)
        {
            return Create<VisualElement>(classNames);
        }

        protected static T Create<T>(params string[] classNames) where T: VisualElement, new()
        {
            var element = new T();
            foreach (var className in classNames)
            {
                element.AddToClassList(className);
            }

            return element;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
