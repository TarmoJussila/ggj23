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

            DropdownField resolutionDropdown = root.Q<DropdownField>("ResolutionsDropDown");
            resolutionDropdown.choices.Clear();
            Resolution[] resolutions = Screen.resolutions;

            foreach (Resolution resolution in resolutions)
            {
                resolutionDropdown.choices.Add(resolution.width + "x" + resolution.height + " " + resolution.refreshRate + "Hz");
            }
            
            root.Q<Button>("BackButton").RegisterCallback<ClickEvent>(_ =>
            {
                SceneManager.LoadScene("Menu");
            });
            
            root.Q<Button>("FullScreenButton").RegisterCallback<ClickEvent>(_ =>
            {
                Screen.fullScreen = !Screen.fullScreen;
            });
        }
    }
}