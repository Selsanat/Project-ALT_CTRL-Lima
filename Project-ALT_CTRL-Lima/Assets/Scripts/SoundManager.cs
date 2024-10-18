using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using System.Collections;

/// <summary>
/// Script managing every audio in the game
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;


    [Header("Main Mixer")]
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField] private AudioMixer audioMixer;

    [Header("All the clips")]
    [SerializeField] private Sounds[] sounds;

    [Header("BgMusics")]
    [SerializeField] private BGMusic[] bgMusics;
    private BGMusic currentMusic;

    //Singleton initialization
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        InitializeAllClips();
    }
    private void Start()
    {
        foreach (Sounds s in sounds)
        {
            if (s.playeOnAwake)
            {
                PlayClip(s.name);
            }
        }
    }
    /// <summary>
    /// Create an audio source for each clip and set it with the right parameter
    /// </summary>
    void InitializeAllClips()
    {
        foreach (Sounds s in sounds)
        {
            if (s.clips.Length == 0 && s.clip == null)
            {
                Debug.LogWarning("No sounds in the sound manager !");
                return;
            }
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.outputAudioMixerGroup = audioMixerGroup;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playeOnAwake;
            s.source.clip.LoadAudioData();
        }

        foreach (BGMusic b in bgMusics)
        {

            b.sources = new AudioSource[4];
            b.sources[0] = gameObject.AddComponent<AudioSource>();
            b.sources[0].clip = b.Music;
            b.sources[1] = gameObject.AddComponent<AudioSource>();
            b.sources[1].clip = b.Vocals;
            b.sources[2] = gameObject.AddComponent<AudioSource>();
            b.sources[2].clip = b.Drums;
            b.sources[3] = gameObject.AddComponent<AudioSource>();
            b.sources[3].clip = b.Bass;

            for (int i = 0; i < 4; i++)
            {
                b.sources[i].clip.LoadAudioData();
                b.sources[i].pitch = b.pitch;
                b.sources[i].volume = 0;
                b.sources[i].outputAudioMixerGroup = audioMixerGroup;
                b.sources[i].loop = true;
                b.sources[i].playOnAwake = true;
            }
            b.sources[0].volume = b.volume;
        }

    }

    public void ResetValues()
    {
        foreach (BGMusic b in bgMusics)
        {
            for (int i = 0; i < 4; i++)
            {
                b.sources[i].pitch = b.pitch;
                b.sources[i].volume = 0;
                b.sources[i].outputAudioMixerGroup = audioMixerGroup;
                b.sources[i].loop = true;
                b.sources[i].playOnAwake = true;
            }
            b.sources[0].volume = b.volume;
        }
    }
    public Sequence StopbackgroundMusic()
    {
        Sequence mySequence = DOTween.Sequence();
        if (currentMusic != null)
        {
            foreach (AudioSource aS in currentMusic.sources)
            {
                mySequence.Join(aS.DOFade(0, 1.5f).OnComplete(() =>
                {
                    aS.Stop();
                }));
            }
            ResetValues();
        }
        return mySequence;
    }
    public void PlayARandomMusic()
    {
        StopbackgroundMusic().Play().OnComplete(() =>
        {
            UnityEngine.Random.seed = System.DateTime.Now.Millisecond;
            int random = UnityEngine.Random.Range(0, bgMusics.Length - 1);
            currentMusic = bgMusics[random];
            float randomTime = UnityEngine.Random.Range(0f, currentMusic.sources[0].clip.length);

            foreach (AudioSource aS in currentMusic.sources)
            {
                aS.time = randomTime;
                aS.Play();
                float vol = aS.volume;
                aS.volume = 0;
                aS.DOFade(vol, 1.5f);
            }
        });

    }

    public void AddPist(int numberOfTracksToAdd)
    {
        for (int i = 0; i < numberOfTracksToAdd; i++)
        {
            foreach (AudioSource aS in currentMusic.sources)
            {
                if (aS.volume < currentMusic.volume)
                {
                    aS.volume = currentMusic.volume;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Play a clip
    /// </summary>
    /// 
    /// <param name="name">The name of the clip</param>
    public void PlayClip(string name)
    {
        StartCoroutine(PlayClipCoroutine(name));
    }
    IEnumerator PlayClipCoroutine(string name)
    {
        if (name == "")
        {
            yield return null;
        }
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.clip == null)
        {
            Debug.LogWarning("The clip " + name + " doesn't exist !");
            yield return null;
        }
        if (s.delay > 0) yield return new WaitForSeconds(s.delay);
        if (s.Oneshot) s.source.PlayOneShot(s.clip);
        else s.source.Play();
    }
    public void PlayRandomClip(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s.playOneAfterAnother && s.source.isPlaying)
        {
            return;
        }
        if (name == "")
        {
            return;
        }

        if (s.clips.Length == 0)
        {
            if (s.clip == null)
            {
                Debug.LogWarning("The clip " + name + " doesn't exist or he only have one clip !");
                return;
            }
            PlayClip(name);
            return;
        }

        UnityEngine.Random.seed = System.DateTime.Now.Millisecond;
        int random = UnityEngine.Random.Range(0, s.clips.Length - 1);
        s.source.clip = s.clips[random];
        s.source.PlayDelayed(s.delay);
    }

    public void Pauseclip(string name)
    {
        if (name == "")
        {
            return;
        }
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.clip == null)
        {
            Debug.LogWarning("The clip " + name + " doesn't exist !");
            return;
        }
        s.source.Stop();
    }
}

[System.Serializable]
public class Sounds
{
    public string name;
    public AudioClip[] clips;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public bool loop;
    public bool Oneshot;
    public bool playeOnAwake;
    public bool playOneAfterAnother;
    public float delay = 0;

    [HideInInspector]
    public AudioSource source;
}

[System.Serializable]
public class BGMusic
{
    public string name;
    public AudioClip Music;
    public AudioClip Drums;
    public AudioClip Vocals;
    public AudioClip Bass;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource[] sources;
}
