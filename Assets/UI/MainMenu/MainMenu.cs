using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {

        private Button _playButton;
        private Button _exitButton;
        
        private void OnEnable()
        {
            UIDocument menu = GetComponent<UIDocument>();
            VisualElement root = menu.rootVisualElement;

            _playButton = root.Q<Button>("PlayButton");
            _playButton.RegisterCallback<ClickEvent>(evt =>
            {
                SceneManager.LoadScene("Main");
            });
            
            _exitButton = root.Q<Button>("ExitButton");
            _exitButton.RegisterCallback<ClickEvent>(evt =>
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            });
        }
    }
}
