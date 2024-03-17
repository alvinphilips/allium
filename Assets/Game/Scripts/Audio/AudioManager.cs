using System.Collections.Generic;
using Game.Scripts.Patterns;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Scripts.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        private AudioMixer _mixer;

        private AudioSource _musicSource;
        private AudioSource _sfxSource;

        [SerializeField]
        private List<AudioObject> sfxList = new();
        
        [SerializeField]
        private List<AudioObject> musicList = new();

        private readonly Dictionary<int, AudioObject> _audioLookup = new();

        private new void Awake()
        {
            base.Awake();

            _musicSource = gameObject.AddComponent<AudioSource>();
            _sfxSource = gameObject.AddComponent<AudioSource>();

            _mixer ??= UnityEngine.Resources.Load<AudioMixer>("Audio/AudioMixer");

            var music = _mixer.FindMatchingGroups("Music");
            if (music.Length != 0)
            {
                _musicSource.outputAudioMixerGroup = music[0];
            }
            
            var sfx = _mixer.FindMatchingGroups("SFX");
            if (sfx.Length != 0)
            {
                _sfxSource.outputAudioMixerGroup = sfx[0];
            }
            
            LoadAudio();
        }

        private void LoadAudio()
        {
            foreach (var sfx in sfxList)
            {
                _audioLookup.TryAdd(sfx.Id, sfx);
            }
            
            foreach (var music in musicList)
            {
                _audioLookup.TryAdd(music.Id, music);
            }
        }

        public bool PlaySound(string sfxName)
        {
            if (PlaySound(sfxName.GetHashCode()))
            {
                return true;
            }
            
            Debug.LogWarning($"Could not play sound effect with name {sfxName}.");
            return false;
        }

        public bool PlaySound(int sfxId)
        {
            if (!_audioLookup.TryGetValue(sfxId, out var sfx))
            {
                Debug.LogWarning($"Sound with ID {sfxId} not found!");
                return false;
            }
            
            _sfxSource.PlayOneShot(sfx!.clip, sfx.volume);
            _sfxSource.pitch = sfx.pitch;
            return true;
        }

        public bool PlayMusic(string musicName)
        { 
            if (PlayMusic(musicName.GetHashCode()))
            {
                return true;
            }
            
            Debug.LogWarning($"Could not play music with name {musicName}.");
            return false;
        }

        public bool PlayMusic(int musicId)
        {
            if (!_audioLookup.TryGetValue(musicId, out var music))
            {
                Debug.LogWarning($"Music with ID {musicId} not found!");
                return false;
            }

            _musicSource.clip = music.clip;
            _musicSource.volume = music.volume;
            _musicSource.pitch = music.pitch;
            _musicSource.loop = music.loop;
            _musicSource.Play();

            return true;
        }

        public void StopMusic()
        {
            _musicSource.Stop();
        }
        
        public void StopSfx()
        {
            _sfxSource.Stop();
        }

        #region Volume Control

        public void SetSfxVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            _sfxSource.volume = volume;
        }
        
        public void SetMusicVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            _musicSource.volume = volume;
        }

        public void MuteAll(bool mute)
        {
            _musicSource.mute = mute;
            _sfxSource.mute = mute;
        }
        
        #endregion
    }
}
