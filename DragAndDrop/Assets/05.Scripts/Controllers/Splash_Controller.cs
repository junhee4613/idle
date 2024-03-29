using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Splash_Controller : MonoBehaviour
{
    public GameObject splash_start_background;
    bool start;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            splash_start_background.SetActive(false);
            start = true;
        }
        if (start && time > 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            time += Time.deltaTime;
        }
        
    }
}
