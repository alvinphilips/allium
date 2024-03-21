using UnityEngine;

namespace Game.Scripts.Utils
{
    public static class UtilsMath
    {
        public static float LinearToDecibel(float linear)
        {
            return linear > 0 ? 20f * Mathf.Log10(linear) : -80f;
        }
    }
}