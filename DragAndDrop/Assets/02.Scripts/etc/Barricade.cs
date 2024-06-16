using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Managers.GameManager.stage_clear[this.gameObject.name] == true)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
