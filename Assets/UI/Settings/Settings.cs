using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Klonk.UI.Settings
{
    public class MSettings : MonoBehaviour
    {
        private DropdownField _resolutionDropdown;
        
        private void OnEnable()
        {
            UIDocument menu = GetComponent<UIDocument>();
            VisualElement root = menu.rootVisualElement;

            _resolutionDropdown = root.Q<DropdownField>("ResolutionsDropDown");
            _resolutionDropdown.choices.Clear();
            Resolution[] resolutions = Screen.resolutions;

            foreach (Resolution resolution in resolutions)
            {
                _resolutionDropdown.choices.Add(resolution.width + "x" + resolution.height + " " + resolution.refreshRate + "Hz");
            }

            root.Q<Button>("BackButton").RegisterCallback<ClickEvent>(_ =>
            {
                SceneManager.LoadScene("Menu");
            });
            
            root.Q<Button>("FullScreenButton").RegisterCallback<ClickEvent>(_ =>
            {
                Screen.fullScreen = !Screen.fullScreen;
            });
            
            root.Q<Button>("ChangeButton").RegisterCallback<ClickEvent>(_ =>
            {
                int index = _resolutionDropdown.index;
                if (index < Screen.resolutions.Length)
                {
                    Resolution resolution = Screen.resolutions[_resolutionDropdown.index];
                    Debug.Log($"Switching to resolution: {resolution}");
                    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
                }
                else
                {
                    Debug.LogWarning("Failed to switch resolution");
                }
            });
        }
    }
}