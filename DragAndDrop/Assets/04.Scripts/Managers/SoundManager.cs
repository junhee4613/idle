using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SoundManager
{
    public AudioSource bgSound;
    public AudioMixer mixer;
    



    void Awake()
    {
        

    }
    public void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        AudioClip temp = Managers.Resource.Load<AudioClip>(arg0.name);
        if(temp != null)
        {
            if (arg0.name == temp.name)
            {
                BGMSound(temp);
            }
        }
    }

    public void BGMSound(AudioClip clip)
    {
        bgSound.clip = clip;
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM_sound_volume")[0];
        if (Managers.GameManager.scene_name.Contains("Stage"))
        {
            bgSound.loop = false;
            Managers.GameManager.bgm_length = clip.length;
            /*Managers.UI_jun.timer.maxValue = clip.length;
            Managers.UI_jun.timer = GameObject.Find("Timer").GetComponent<Slider>();
            Managers.UI_jun.timer.maxValue = clip.length;
            Managers.UI_jun.timer.value = clip.length;*/
        }
        else
        {
            bgSound.loop = true;
        }
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

        UnityEngine.MonoBehaviour.Destroy(go, clip.length);
    }

    public void Game_over_sound()
    {
        bgSound.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        if (volume > 0)
        {
            mixer.SetFloat("BGM_sound_volume", Mathf.Log10(volume) * 20);

        }
        else
        {
            mixer.SetFloat("BGM_sound_volume", Mathf.Log10(-80));
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (volume > 0)
        {
            mixer.SetFloat("SFX_sound_volume", Mathf.Log10(volume) * 20);
        }
        else
        {
            mixer.SetFloat("SFX_sound_volume", Mathf.Log10(-80));
        }
    }
}
