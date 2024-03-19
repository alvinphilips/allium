using System;
using Game.Scripts.Audio;
using Game.Scripts.Patterns;
using UnityEngine;

namespace Game.Scripts.Game
{
    public class OptionsManager : Singleton<OptionsManager>
    {
        private void Start()
        {
            LoadAudioSettings();
            LoadResolution();
            LoadIsFullscreen();
            LoadQualityLevel();
        }

        private void OnDestroy()
        {
            SaveResolution(GameResolution.width, GameResolution.height);
            SaveAudioSettings(MasterVolume, MusicVolume, SfxVolume);
            SaveIsFullscreen(IsFullscreen);
            SaveQualityLevel(QualityLevel);
        }

        public float MasterVolume { get; private set; } = 1;
        public float MusicVolume { get; private set; } = 1;
        public float SfxVolume { get; private set; } = 1;

        public bool IsFullscreen { get; private set; }

        public Resolution GameResolution { get; private set; }
        public int QualityLevel { get; private set; }
        
        
        private void LoadAudioSettings()
        {
            MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
            SfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        }

        public void SaveAudioSettings(float master, float music, float sfx)
        {
            PlayerPrefs.SetFloat("MasterVolume", master);
            PlayerPrefs.SetFloat("MusicVolume", music);
            PlayerPrefs.SetFloat("SFXVolume", sfx);
            PlayerPrefs.Save();

            MasterVolume = master;
            MusicVolume = music;
            SfxVolume = sfx;
            ApplySettings();
        }

        private void LoadIsFullscreen()
        {
            IsFullscreen = PlayerPrefs.GetInt("IsFullscreen", 1) == 1;
        }

        public void SaveIsFullscreen(bool fullscreen)
        {
            PlayerPrefs.SetInt("IsFullscreen", fullscreen ? 1 : 0);
            PlayerPrefs.Save();
            
            IsFullscreen = fullscreen;
            ApplySettings();
        }

        private void LoadQualityLevel()
        {
            QualityLevel = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        }

        public void SaveQualityLevel(int quality)
        {
            PlayerPrefs.SetInt("QualityLevel", quality);
            PlayerPrefs.Save();
            
            QualityLevel = quality;
            ApplySettings();
        }

        private void LoadResolution()
        {
            var width = PlayerPrefs.GetInt("ResolutionWidth", Screen.currentResolution.width);
            var height = PlayerPrefs.GetInt("ResolutionHeight", Screen.currentResolution.height);

            GameResolution = new Resolution { width = width, height = height };
        }

        public void SaveResolution(int width, int height)
        {
            PlayerPrefs.SetInt("ResolutionWidth", width);
            PlayerPrefs.SetInt("ResolutionHeight", height);
            PlayerPrefs.Save();
            
            GameResolution = new Resolution { width = width, height = height };
            ApplySettings();
        }

        public void ApplySettings()
        {
            AudioListener.volume = MasterVolume;
            AudioManager.Instance.SetMusicVolume(MusicVolume);
            AudioManager.Instance.SetSfxVolume(SfxVolume);
            Screen.SetResolution(GameResolution.width, GameResolution.height, IsFullscreen);
            QualitySettings.SetQualityLevel(QualityLevel);
        }
    }
}
