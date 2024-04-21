using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Audio;
using System.Linq;
public class Managers : MonoBehaviour           //디버깅 할 때 매개변수에 값이 할당됐는지에 대한 여부를 보려면 조사식1과 호출스택 창을 열어놓고 중단점에 왔을 때 그 코드를 할당 시켜야됨
{
    public bool invincibility = false;
    static Managers _instance;
    public static Managers instance { get { Init(); return _instance; } }
    private void Awake()
    {
        Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            if(count == totalCount)
            {
                Debug.Log(Resource._resources.Count);

                UI_jun.Init();
                GameManager.Init();
                Sound.mixer = Resource.Load<AudioMixer>("Sound_option.mixer");
                SceneManager.sceneLoaded += GameManager.Next_sceneLoaded;
                SceneManager.sceneLoaded += Sound.OnSceneLoaded;
                SceneManager.sceneLoaded += UI_jun.UI_on_scene_loaded;
                Sound.bgSound = gameObject.GetOrAddComponent<AudioSource>();
                GameManager.scene_name = SceneManager.GetActiveScene().name;
                GameManager.gameover += Sound.Game_over_sound;
                GameManager.gameover += UI_jun.Game_over_ui;
                if(invincibility)
                {
                    Managers.GameManager.stage_clear.Values.All((value) => true);
                }

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
        Grid._grid = gameObject.GetOrAddComponent<Grid>();
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
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.scene_name != "Lobby_screen")       //FIX : 나중에 여기 수정해야됨
        {
            if (UI_jun.UI_window_on["Option"].activeSelf)
            {
                GameManager.option_window_on = false;
                GameManager.game_stop = false;
                UI_jun.UI_window_off.Peek().SetActive(false);
                if(GameManager.scene_name != "Main_screen")
                {
                    Sound.bgSound.pitch = 1;
                }
                Time.timeScale = 1;
            }
            else
            {
                GameManager.option_window_on = true;
                GameManager.game_stop = true;
                UI_jun.UI_window_on["Option"].SetActive(true);
                Time.timeScale = 0;
                if (GameManager.scene_name != "Main_screen")
                {
                    Sound.bgSound.pitch = 0;
                }
                UI_jun.UI_window_off.Push(UI_jun.UI_window_on["Option"]);

            }
            //StartCoroutine(Option_window());
        }
    }
    /*IEnumerator Option_window()
    {

        yield return null;
    }*/
    public void Game_system_stop()
    {

    }

    
    public static SoundManager Sound { get { return instance?._sound; } }
    public static UIManager_jun UI_jun { get { return instance?._ui; } }
    //public static UIManager UI_base { get { return instance?._ui_base; } }
    public static GameManager GameManager { get { return instance?._game; } }
    public static ResourceManager Resource { get { return instance?._resources; } }
    public static PoolManager Pool { get { return instance?._pool; } }
    public static Camera_manager Main_camera { get { return instance?._camera; } }
    public static Grid_controller Grid { get { return instance?._grid; } }

    SoundManager _sound = new SoundManager();
    UIManager_jun _ui = new UIManager_jun();
    //UIManager _ui_base = new UIManager();
    ResourceManager _resources = new ResourceManager();
    Camera_manager _camera = new Camera_manager();
    GameManager _game = new GameManager();
    PoolManager _pool = new PoolManager();
    Grid_controller _grid = new Grid_controller();
}
