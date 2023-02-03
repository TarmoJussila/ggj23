using UnityEngine;
using UnityEngine.UIElements;

namespace UI.TestMenu
{
    public class TestMenu : MonoBehaviour
    {
        private TestMenuController _controller;

        private void OnEnable()
        {
            UIDocument menu = GetComponent<UIDocument>();
            VisualElement root = menu.rootVisualElement;

            _controller = new(root);
            _controller.RegisterCallbacks();
        }
    }
}
