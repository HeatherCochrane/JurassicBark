using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource money;

    [SerializeField]
    List<AudioClip> popClips = new List<AudioClip>();

    [SerializeField]
    AudioSource source;

    [SerializeField]
    AudioClip paddockCreation;

    [SerializeField]
    AudioClip pathCreation;

    [SerializeField]
    AudioClip moneyOut;

    
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

    public void playWood()
    {
        source.Stop();
        source.clip = paddockCreation;
        source.loop = false;

        source.pitch = Random.Range(1, 1.5f);

        source.Play();

        cashSpent();
    }

    public void playStone()
    {
        source.Stop();
        
        source.clip = pathCreation;
        source.loop = false;

        source.pitch = Random.Range(1, 1.2f);
        source.Play();

        cashSpent();
    }

    public void cashSpent()
    {
        money.Stop();
        money.clip = moneyOut;
        money.loop = false;
        money.Play();
    }
}
