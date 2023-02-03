using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Klonk.UI.HUD
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

            root.Q<Button>("mainMenuButton").RegisterCallback<ClickEvent>(_ =>
            {
                ShowDialog();
            });
            
            root.Q<Button>("YesButton").RegisterCallback<ClickEvent>(_ =>
            {
                SceneManager.LoadScene("MainMenu");
            });
            
            root.Q<Button>("NoButton").RegisterCallback<ClickEvent>(_ =>
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
