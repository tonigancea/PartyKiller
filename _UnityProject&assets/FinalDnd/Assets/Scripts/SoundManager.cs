using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

    GeneralManager manager;

    private Text textforVoice;
    private Text textforEffect;
    private AudioSource voice;
    private AudioSource soundEffects;
    private AudioSource backgroundMusic;

    private float walkingTime = 3f;

    // Use this for initialization
    void Start () {
        manager = GameObject.Find("General").GetComponent<GeneralManager>();

        textforVoice = GameObject.Find("VoiceText").GetComponent<Text>();
        textforEffect = GameObject.Find("EffectText").GetComponent<Text>();

        Voice = GameObject.Find("Voice").GetComponent<AudioSource>();
        SoundEffects = GameObject.Find("SoundEffects").GetComponent<AudioSource>();

        backgroundMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        backgroundMusic.clip = Resources.Load<AudioClip>("backgroundMusic");
        backgroundMusic.Play();

        AddNewEffect("footsteps");
        AddNewEffect("cling");
        AddNewEffect("sorryPasscode");
    }

    void AddNewEffect(string effectName)
    {
        manager.Effects.Add(effectName, Resources.Load<AudioClip>(effectName));
    }

    string FormatString(string name, int index)
    {
        return name + ": Sound" + index;
    }
    string FormatEffects(string name)
    {
        return "Effect: " + name;
    }

    public void StopBackgroundMusic()
    {
        backgroundMusic.Pause();
    }

    public void ResumeBackgroundMusic()
    {
        backgroundMusic.UnPause();
    }

    public void PlaySpecificSound_Indexed(int index)
    {
        if (manager.Mechanics.CurrentItem.children.Count > 0 && index >= 0 && index < 3)
        {
            ShowString(FormatString(manager.Mechanics.GetItemAtIndex(manager.Mechanics.CurrentItem.children).name, index));

            AudioClip voice = Resources.Load<AudioClip>(manager.SoundDatabase[manager.Mechanics.GetItemAtIndex(manager.Mechanics.CurrentItem.children)][index]);
            PlayVoice(voice);
        }
    }

    public void PlaySpecificSound_Nearby(int index)
    {
        if (manager.Mechanics.CurrentItem.children.Count > 0 && index >= 0 && index < 3)
        {
            ShowString(FormatString(manager.Mechanics.CurrentItem.name, index));

            AudioClip voice = Resources.Load<AudioClip>(manager.SoundDatabase[manager.Mechanics.CurrentItem][index]);
            PlayVoice(voice);
        }
    }

    public void PlayRoomDescription()
    {
        ShowString("Description for " + manager.Mechanics.CurrentRoom.name);

        AudioClip voice = Resources.Load<AudioClip>(manager.SoundDatabase[manager.Mechanics.CurrentRoom][0]);
        PlayVoice(voice);
    }

    public void PlayControls()
    {
        ShowString("HELP: Controls");

        PlayVoice(Resources.Load<AudioClip>("controls"));
    }

    public void PlaySpecifiedEffect(string name)
    {
        textforEffect.text = FormatEffects(name);
        AudioClip effect = manager.Effects[name];
        PlayEffect(effect);
    }

    public void ShowString(string msg)
    {
        textforVoice.text = msg;
    }

    public void PlayVoice(AudioClip voice)
    {
        this.Voice.Stop();
        this.Voice.clip = voice;
        this.Voice.Play();
    }

    public void PlayEffect(AudioClip effect)
    {
        this.SoundEffects.Stop();
        this.SoundEffects.clip = effect;
        this.SoundEffects.Play();
    }

    public float WalkingTime
    {
        get
        {
            return walkingTime;
        }

        set
        {
            walkingTime = value;
        }
    }

    public AudioSource Voice
    {
        get
        {
            return voice;
        }

        set
        {
            voice = value;
        }
    }

    public AudioSource SoundEffects
    {
        get
        {
            return soundEffects;
        }

        set
        {
            soundEffects = value;
        }
    }
}
