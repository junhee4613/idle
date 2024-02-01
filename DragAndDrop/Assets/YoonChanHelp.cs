using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class YoonChanHelp : MonoBehaviour
{
    [SerializeField]private TextAsset TestText;
    [SerializeField] List<TestObject> targetOBj;
    // Start is called before the first frame update
    void Start()
    {
        targetOBj = JsonConvert.DeserializeObject<List<TestObject>>(TestText.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public class TestObject
{
    [SerializeField]public float simple_time;
    [SerializeField]public byte simple_pattern_type;
    [SerializeField]public byte hard_pattern_type;
}
