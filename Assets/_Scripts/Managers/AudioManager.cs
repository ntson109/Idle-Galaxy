using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance = null;

    public Sound[] sounds;

    public Sound[] musics;

    public Slider sliderSound;
    public Slider sliderMusic;

    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound s in musics)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        if (!PlayerPrefs.HasKey("Volume Sound"))
        {
            PlayerPrefs.SetInt("Volume Sound", 1);
        }
        else
        {
            if (PlayerPrefs.GetInt("Volume Sound") == 0)
            {
                sliderSound.value = 0;
                MuteAll(true);
            }
        }

        if (!PlayerPrefs.HasKey("Volume Music"))
        {
            PlayerPrefs.SetInt("Volume Music", 1);
        }
        else
        {
            if (PlayerPrefs.GetInt("Volume Music") == 0)
            {
                sliderMusic.value = 0;
                MuteAll(true, true);
            }
        }


    }

    public void Start()
    {
        Play("Menu",true);
    }

    public void Play(string name,bool isMusic = false)
    {
        if (!isMusic)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound :" + name + "not found!");
                return;
            }
            s.source.Play();
        }
        else
        {
            Sound s = Array.Find(musics, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound :" + name + "not found!");
                return;
            }
            s.source.Play();
        }
    }

    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound :" + name + "not found!");
            return;
        }
        s.source.PlayOneShot(s.clip);
    }

    public void Stop(string name,bool isMusic = false)
    {
        if (!isMusic)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound :" + name + "not found!");
                return;
            }
            s.source.Stop();
        }
        else
        {
            Sound s = Array.Find(musics, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound :" + name + "not found!");
                return;
            }
            s.source.Stop();
        }
    }

    public void Mute(string name, bool mute,bool isMusic = false)
    {
        if (!isMusic)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound :" + name + "not found!");
                return;
            }
            s.source.mute = mute;
        }
        else
        {
            Sound s = Array.Find(musics, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound :" + name + "not found!");
                return;
            }
            s.source.mute = mute;
        }
    }

    public void MuteAll(bool mute,bool isMusic = false)
    {
        if (!isMusic)
        {
            foreach (Sound s in sounds)
            {
                s.source.mute = mute;
            }
        }
        else
        {
            foreach (Sound s in musics)
            {
                s.source.mute = mute;
            }
        }
    }

    public void StopAll(bool isMusic = false)
    {
        if (!isMusic)
        {
            foreach (Sound s in sounds)
            {
                s.source.Stop();
            }
        }
        else
        {
            foreach (Sound s in musics)
            {
                s.source.Stop();
            }
        }
    }

    public void ControllSound(Slider _slider)
    {
        if(_slider.value == 0)
        {
            MuteAll(true);
        }
        else
        {
            MuteAll(false);
        }
        PlayerPrefs.SetInt("Volume Sound", (int)_slider.value);
    }

    public void ControllMusic(Slider _slider)
    {
        if (_slider.value == 0)
        {
            MuteAll(true,true);
        }
        else
        {
            MuteAll(false,true);
        }
        PlayerPrefs.SetInt("Volume Music", (int)_slider.value);
    }
}