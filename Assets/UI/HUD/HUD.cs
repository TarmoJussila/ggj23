using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI.HUD
{
    public class HUD : MonoBehaviour
    {
        private VisualElement _dialogPanel;
        
        private void OnEnable()
        {
            UIDocument menu = GetComponent<UIDocument>();
            VisualElement root = menu.rootVisualElement;

            _dialogPanel = root.Q<VisualElement>("exitConfirmDialog");
            HideDialog();

            root.Q<Button>("mainMenuButton").RegisterCallback<ClickEvent>(evt =>
            {
                ShowDialog();
            });
            
            root.Q<Button>("YesButton").RegisterCallback<ClickEvent>(evt =>
            {
                SceneManager.LoadScene("MainMenu");
            });
            
            root.Q<Button>("NoButton").RegisterCallback<ClickEvent>(evt =>
            {
                HideDialog();
            });
        }

        private void HideDialog()
        {
            _dialogPanel.AddToClassList("hiddenDialog");
        }

        private void ShowDialog()
        {
            _dialogPanel.RemoveFromClassList("hiddenDialog");
        }
    }
}
