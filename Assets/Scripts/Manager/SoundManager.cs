using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    public List<AudioClip> voiceLines = new List<AudioClip>();
    public AudioSource soundSource;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    
    public void SwitchClip()
    {
        AudioClip currentLine = soundSource.clip;
        
        int index=voiceLines.IndexOf(currentLine);
        AudioClip nextLine = voiceLines[0];
        if (index + 1 < voiceLines.Count-1)
        {
            nextLine = voiceLines[index+1];
        }
        if(nextLine != null)
            soundSource.clip = nextLine;
    }

    public void PlayClip()
    {
        soundSource.Play();
    }
}
