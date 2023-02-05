using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Klonk.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        private Button _playButton;
        private Button _exitButton;
        private Button _settingsButton;
        
        private void OnEnable()
        {
            UIDocument menu = GetComponent<UIDocument>();
            VisualElement root = menu.rootVisualElement;

            _playButton = root.Q<Button>("PlayButton");
            _playButton.RegisterCallback<ClickEvent>(_ =>
            {
                SceneManager.LoadScene("Main");
            });
            
            _exitButton = root.Q<Button>("ExitButton");
            _exitButton.RegisterCallback<ClickEvent>(_ =>
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            });

            _settingsButton = root.Q<Button>("SettingsButton");
            _settingsButton.RegisterCallback<ClickEvent>(_ =>
            {
                SceneManager.LoadScene("Settings");
            });
        }
    }
}
