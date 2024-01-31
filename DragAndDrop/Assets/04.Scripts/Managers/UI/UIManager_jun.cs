using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_jun : MonoBehaviour
{
    public Stack<GameObject> UI_window_off = new Stack<GameObject>();
    public bool option_window_on;
    public Dictionary<string, GameObject> UI_window_on = new Dictionary<string, GameObject>();
    public GameObject canvas;
    public void Init()
    {
        if (canvas == null)
        {
            GameObject temp = GameObject.Find("Canvas");
            if (!temp)
            {
                temp = Managers.Resource.Instantiate("Canvas", null, true);
            }
            canvas = temp;
            DontDestroyOnLoad(canvas);

        }
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            Transform child = canvas.transform.GetChild(i);

            // 최상위 자식만 추가
            if (child.parent == canvas.transform)
            {
                UI_window_on.Add(child.name, child.gameObject);
            }
        }
        Debug.Log(UI_window_on.Count);

    }

}
