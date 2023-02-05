using System;
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
        private VisualElement _weaponIcon;
        private Label _weaponName;
        private Label _scoreLabel;
        private float _fps;
        private int _frameCount;
        private int _score;
        private float _deltaTime;

        private void OnEnable()
        {
            UIDocument menu = GetComponent<UIDocument>();
            VisualElement root = menu.rootVisualElement;

            WeaponHandler.OnWeaponChange += OnWeaponChange;
            CharacterHealth.OnEnemyDead += OnEnemyDead;

            _debugText = root.Q<Label>("debugText");
            _dialogPanel = root.Q<VisualElement>("exitConfirmDialog");
            _weaponIcon = root.Q<VisualElement>("weaponIcon");
            _weaponName = root.Q<Label>("weaponName");
            _scoreLabel = root.Q<Label>("scoreLabel");
            HideDialog();

            root.Q<Button>("mainMenuButton").RegisterCallback<ClickEvent>(_ => { ShowDialog(); });

            root.Q<Button>("YesButton").RegisterCallback<ClickEvent>(_ => { SceneManager.LoadScene("Menu"); });

            root.Q<Button>("NoButton").RegisterCallback<ClickEvent>(_ => { HideDialog(); });
        }

        private void OnDisable()
        {
            WeaponHandler.OnWeaponChange -= OnWeaponChange;
            CharacterHealth.OnEnemyDead += OnEnemyDead;
        }

        private void OnWeaponChange(Weapon weapon)
        {
            _weaponName.text = weapon.Name;
            _weaponIcon.style.backgroundImage = new StyleBackground(weapon.Sprite);
        }

        private void OnEnemyDead()
        {
            _score++;
            _scoreLabel.text = "Score: " + _score;
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