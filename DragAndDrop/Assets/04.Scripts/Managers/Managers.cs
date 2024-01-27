using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Managers : MonoBehaviour
{
    public bool developer_mode = false;
    public bool invincibility = false;
    static Managers _instance;
    public string scene_name;
    public static Managers instance { get { Init(); return _instance; } }
    private void Awake()
    {
        Init();
        SceneManager.sceneLoaded += Next_sceneLoaded;
        scene_name = SceneManager.GetActiveScene().name;
    }
    public static void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);  //Scene 이 종료되도 파괴 되지 않게 
            _instance = go.GetComponent<Managers>();
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Option창이 켜지는 로직
        }
    }


    public void Next_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        scene_name = scene.name;
        switch (scene_name)
        {
            default:
                break;
        }
    }
    public static UIManager UI { get { return instance?._ui; } }
    public static GameManager GameManager { get { return instance?._game; } }
    public static ResourceManager Resource { get { return instance?._resources; } }
    public static PoolManager Pool { get { return instance?._pool; } }

    UIManager _ui = new UIManager();
    ResourceManager _resources = new ResourceManager();
    GameManager _game = new GameManager();
    PoolManager _pool = new PoolManager();
}
