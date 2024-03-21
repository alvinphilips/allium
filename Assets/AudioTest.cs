using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Audio;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic("BGM");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
