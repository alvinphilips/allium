using UnityEngine;

namespace Game.Scripts.Audio
{
    [CreateAssetMenu(menuName = "Audio/Audio Object", fileName = "New Audio Object")]
    public class AudioObject : ScriptableObject
    {
        public string clipName;
        public AudioClip clip;
        [Range(0,1)]
        public float volume = 1.0f;

        public bool loop;
        public float pitch;
        
        public int Id => clipName.GetHashCode();
    }
}
