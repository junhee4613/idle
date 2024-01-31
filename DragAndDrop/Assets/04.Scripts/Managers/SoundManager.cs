using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] bgm;
    public AudioSource bgSound;
    public AudioMixer mixer;
    public Slider bgm_slider;
    public Slider sfx_slider;



    void Awake()
    {
        

    }
    public void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (arg0.name == bgm[i].name)
            {
                BGMSound(bgm[i]);
            }
        }
    }

    public void BGMSound(AudioClip clip)
    {
        bgSound.clip = clip;
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM_sound_volume")[0];
        bgSound.loop = true;
        bgSound.Play();
    }
    public void SFXSound(string name, AudioClip clip)           //나중에 오브젝트 풀링으로 관리하고 음악 클립은 어드레서블로 불러오기
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX_sound_volume")[0];
        audioSource.Play();

        Destroy(go, clip.length);
    }



    public void SetBGMVolume(float volume)
    {
        if (volume > 0)
        {
            mixer.SetFloat("BGMSoundVolume", Mathf.Log10(volume) * 20);

        }
        else
        {
            mixer.SetFloat("BGMSoundVolume", Mathf.Log10(-80));
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (volume > 0)
        {
            mixer.SetFloat("SFXSoundVolume", Mathf.Log10(volume) * 20);
        }
        else
        {
            mixer.SetFloat("SFXSoundVolume", Mathf.Log10(-80));
        }
    }
}
