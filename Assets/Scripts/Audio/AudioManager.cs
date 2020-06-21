using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource music;

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

    [SerializeField]
    List<AudioClip> dogBarks = new List<AudioClip>();

    [SerializeField]
    AudioClip income;

    [SerializeField]
    AudioClip destroy;

    [SerializeField]
    AudioClip menuMusic;

    [SerializeField]
    AudioClip addedPoints;

    [SerializeField]
    AudioClip unlock;

   

    bool musicOff = false;
    bool soundEffectsOff = false;

    [SerializeField]
    Slider musicVolume;

    [SerializeField]
    Slider soundEffectVolume;

    // Start is called before the first frame update
    void Start()
    {
        musicVolume.value = 0.3f;
        soundEffectVolume.value = 0.3f;
        musicVolume.value = 0.3f;
        soundEffectVolume.value = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        music.volume = musicVolume.value;
        money.volume = soundEffectVolume.value;
        soundEffectsOff = soundEffectVolume;
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

    public void playDogBark()
    {
        source.Stop();

        source.clip = dogBarks[Random.Range(0, dogBarks.Count)];
        source.loop = false;

        source.pitch = Random.Range(1, 1.2f);
        source.Play();

        cashSpent();
    }

    public void playIncomeGained()
    {
        money.Stop();
        money.clip = income;
        money.loop = false;
        money.Play();
    }

    public void playDestroy()
    {
        source.Stop();
        source.clip = destroy;
        source.loop = false;

        source.pitch = Random.Range(1, 1.2f);

        source.Play();

        cashSpent();
    }

    public void playMusic()
    {
        music.Stop();
        music.clip = menuMusic;
        music.loop = true;
        music.Play();
    }

    public void stopSecondPlayBack()
    {
        money.Stop();
        
    }

    public void stopFirstPlayBack()
    {
        source.Stop();
    }

    public void playUnlock()
    {
        source.Stop();
        source.clip = unlock;
        source.loop = false;
        source.Play();
    }

    public void playPointsGained()
    {
        money.Stop();
        money.clip = addedPoints;
        money.loop = false;

        money.pitch = Random.Range(1, 1.2f);

        money.Play();
    }
}
