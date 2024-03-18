using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Game;
using Game.Scripts.Patterns;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Settings
{
    public enum OptionsMenuState
    {
        Hidden,
        Show,
    }
    
    public class OptionsMenuUI: MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown qualityDropdown;

        private Resolution[] _resolutions;
        
        private void Awake()
        {
            SubscribeToEvents();
            InitializeUIComponents();
            LoadResolutions();
            LoadCurrentUISettings();
            
            ToggleOptionsMenuVisibility(false);
        }

        private void InitializeUIComponents()
        {
            backButton.onClick.AddListener(() =>
            {
                GameManager.Instance.RollbackState();
            });
            
            fullscreenToggle.onValueChanged.AddListener(fullscreen =>
            {
                OptionsManager.Instance.SaveIsFullscreen(fullscreen);
            });
            
            masterVolumeSlider.onValueChanged.AddListener(master =>
            {
                OptionsManager.Instance.SaveAudioSettings(master, musicVolumeSlider.value, sfxVolumeSlider.value);
            });
            
            musicVolumeSlider.onValueChanged.AddListener(music =>
            {
                OptionsManager.Instance.SaveAudioSettings(masterVolumeSlider.value, music, sfxVolumeSlider.value);
            });
            
            sfxVolumeSlider.onValueChanged.AddListener(sfx =>
            {
                OptionsManager.Instance.SaveAudioSettings(masterVolumeSlider.value, musicVolumeSlider.value, sfx);
            });
            
            resolutionDropdown.onValueChanged.AddListener(index =>
            {
                OptionsManager.Instance.SaveResolution(
                    _resolutions[index].width,
                    _resolutions[index].height
                    );
            });
            
            qualityDropdown.onValueChanged.AddListener(quality =>
            {
                OptionsManager.Instance.SaveQualityLevel(quality);
            });
        }

        public void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            EventBus<OptionsMenuState>.Subscribe(OptionsMenuState.Hidden, OnHideOptionsMenu);
            EventBus<OptionsMenuState>.Subscribe(OptionsMenuState.Show, OnShowOptionsMenu);
        }
        
        private void UnsubscribeFromEvents()
        {
            EventBus<OptionsMenuState>.Unsubscribe(OptionsMenuState.Hidden, OnHideOptionsMenu);
            EventBus<OptionsMenuState>.Unsubscribe(OptionsMenuState.Show, OnShowOptionsMenu);
        }

        private void OnHideOptionsMenu()
        {
            ToggleOptionsMenuVisibility(false);
        }
        private void OnShowOptionsMenu()
        {
            ToggleOptionsMenuVisibility(true);
        }

        private void LoadCurrentUISettings()
        {
            masterVolumeSlider.value = OptionsManager.Instance.MasterVolume;
            musicVolumeSlider.value = OptionsManager.Instance.MusicVolume;
            sfxVolumeSlider.value = OptionsManager.Instance.SfxVolume;

            fullscreenToggle.isOn = OptionsManager.Instance.IsFullscreen;

            var currentResolutionIndex = Array.FindIndex(_resolutions, resolution => 
                resolution.width == OptionsManager.Instance.GameResolution.width && 
                    resolution.height == OptionsManager.Instance.GameResolution.height);

            resolutionDropdown.value = currentResolutionIndex;
            qualityDropdown.value = OptionsManager.Instance.QualityLevel;
        }

        private void LoadResolutions()
        {
            _resolutions = Screen.resolutions.Select(
                    resolution => new Resolution { width = resolution.width, height = resolution.height })
                .DistinctBy(resolution => new { resolution.width, resolution.height })
                .Reverse()
                .ToArray();

            var options = new List<string>();
            var currentResolutionIndex = 0;

            var index = 0;
            foreach (var resolution in _resolutions)
            {
                options.Add($"{resolution.width}x{resolution.height}");
                
                if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = index;
                }

                index++;
            }
            
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void ToggleOptionsMenuVisibility(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}