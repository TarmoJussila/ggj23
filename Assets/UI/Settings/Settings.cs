using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Klonk.UI.Settings
{
    public class MSettings : MonoBehaviour
    {
        private void OnEnable()
        {
            UIDocument menu = GetComponent<UIDocument>();
            VisualElement root = menu.rootVisualElement;

            root.Q<Button>("BackButton").RegisterCallback<ClickEvent>(_ =>
            {
                SceneManager.LoadScene("Menu");
            });
        }
    }
}