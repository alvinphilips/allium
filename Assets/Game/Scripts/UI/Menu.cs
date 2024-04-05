using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Scripts.UI
{
    [RequireComponent(typeof(UIDocument))]
    public abstract class Menu : MonoBehaviour
    {
        protected UIDocument Document;
        public bool RefreshUI { get; set; }
        
        // Start is called before the first frame update
        private void Start()
        {
            StartCoroutine(Generate());
        }

        private void Awake()
        {
            Document = GetComponent<UIDocument>();
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;

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
    }
}
