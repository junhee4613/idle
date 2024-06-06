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
    /*public void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("사운드 매니저");
        
    }*/
    // Fix : 여기 수정(스테이지 별로 공통 명칭을 정해놓고 그 공통 명칭으로 처리)
    public void BGMSound(AudioClip clip, bool loop)
    {
        bgSound.clip = clip;
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM_sound_volume")[0];
        bgSound.loop = loop;
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
