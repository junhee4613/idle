using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_cs : MonoBehaviour
{
    private AudioSource audioSource;
    private float[] lastData;
    public float beatThreshold_num;
    public bool temp_bool = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastData = new float[audioSource.clip.samples * audioSource.clip.channels];
        audioSource.Play();
        Debug.Log(Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {
        if (temp_bool)
        {
            Debug.Log(Time.deltaTime);
            temp_bool = false;
        }
        Debug.Log(audioSource.time);

    }
    void OnAudioFilterRead(float[] data, int channels)
    {
        // 비트 분석을 위한 간격 임계값 (조절 가능)
        float beatThreshold = beatThreshold_num;

        // 이전 프레임의 데이터와 현재 프레임의 데이터를 비교하여 비트 간격을 계산
        for (int i = 0; i < data.Length; i++)
        {
            float amplitude = Mathf.Abs(data[i]);
            float lastAmplitude = Mathf.Abs(lastData[i]);
            // 간격이 임계값을 초과하면 비트로 판정
            if (amplitude - lastAmplitude > beatThreshold)
            {
                Debug.Log("Beat detected!");
                // 여기에서 원하는 이벤트를 실행할 수 있습니다.
            }
        }

        // 현재 프레임의 데이터를 저장
        System.Array.Copy(data, lastData, data.Length);
    }
}
