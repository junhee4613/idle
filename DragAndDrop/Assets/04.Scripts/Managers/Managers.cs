using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Managers : MonoBehaviour
{
    public bool developer_mode = false;
    public bool invincibility = false;
    static Managers _instance;
    public string scene_name;
    public GameObject option;
    public static Managers instance { get { Init(); return _instance; } }
    private void Awake()
    {
        Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            if(count == totalCount)
            {
                Debug.Log(Resource._resources.Count);

                UI_jun.Init();
                SceneManager.sceneLoaded += Next_sceneLoaded;
                SceneManager.sceneLoaded += Sound.OnSceneLoaded;
                scene_name = SceneManager.GetActiveScene().name;
            }
            //여기부터 하면 됨 
            /*Debug.Log("key : " + key + " Count : " + count + " totalCount : " + totalCount);
            if (count == totalCount)
            {
                Managers.Data.Init();
                Managers.Game.Init();
                Init();
            }*/
        });
        
    }
    private void Start()
    {
        
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

            DontDestroyOnLoad(go);
            _instance = go.GetComponent<Managers>();
        }
    }
    public void Update()        
    {
        if (Input.GetKeyDown(KeyCode.Escape))       //FIX : 나중에 여기 수정해야됨
        {
            if (option.activeSelf)
            {
                option.SetActive(false);
            }
            else
            {
                option.SetActive(true);

            }
            //StartCoroutine(Option_window());
        }
    }
    /*IEnumerator Option_window()
    {

        yield return null;
    }*/


    public void Next_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        scene_name = scene.name;
        switch (scene_name)
        {
            default:
                break;
        }
        UI_jun.option_window_on = false;
    }
    public static SoundManager Sound { get { return instance?._sound; } }
    public static UIManager_jun UI_jun { get { return instance?._ui; } }
    //public static UIManager UI_base { get { return instance?._ui_base; } }
    public static GameManager GameManager { get { return instance?._game; } }
    public static ResourceManager Resource { get { return instance?._resources; } }
    public static PoolManager Pool { get { return instance?._pool; } }

    SoundManager _sound = new SoundManager();
    UIManager_jun _ui = new UIManager_jun();
    //UIManager _ui_base = new UIManager();
    ResourceManager _resources = new ResourceManager();
    GameManager _game = new GameManager();
    PoolManager _pool = new PoolManager();
}
