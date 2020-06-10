using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> popClips = new List<AudioClip>();

    [SerializeField]
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playPop()
    {
        source.Stop();
        source.clip = popClips[0];
        source.loop = false;
        source.Play();
    }

    public void playOpen()
    {

        source.Stop();
        source.clip = popClips[1];
        source.loop = false;
        source.Play();
    }

    public void playClose()
    {

        source.Stop();
        source.clip = popClips[2];
        source.loop = false;
        source.Play();
    }
}
