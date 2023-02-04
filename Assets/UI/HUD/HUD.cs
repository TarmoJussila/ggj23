using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Klonk.UI.HUD
{
    public class HUD : MonoBehaviour
    {
        private const float UpdateRate = 5f;
        
        private VisualElement _dialogPanel;
        private Label _debugText;
        private float _fps;
        private int _frameCount;
        private float _deltaTime;

        private void OnEnable()
        {
            UIDocument menu = GetComponent<UIDocument>();
            VisualElement root = menu.rootVisualElement;

            _debugText = root.Q<Label>("debugText");
            _dialogPanel = root.Q<VisualElement>("exitConfirmDialog");
            HideDialog();

            root.Q<Button>("mainMenuButton").RegisterCallback<ClickEvent>(_ => { ShowDialog(); });

            root.Q<Button>("YesButton").RegisterCallback<ClickEvent>(_ => { SceneManager.LoadScene("MainMenu"); });

            root.Q<Button>("NoButton").RegisterCallback<ClickEvent>(_ => { HideDialog(); });
        }

        private void Update()
        {
            _frameCount++;
            _deltaTime += Time.deltaTime;
            if (_deltaTime > 1f / UpdateRate)
            {
                _fps = _frameCount / _deltaTime;
                _frameCount = 0;
                _deltaTime -= 1f / UpdateRate;
            }

            _debugText.text = $" FPS {_fps:#.##}";
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