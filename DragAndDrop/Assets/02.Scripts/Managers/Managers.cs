using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using AYellowpaper.SerializedCollections;
using System;

using Object = UnityEngine.Object;

public enum SceneName
{
    None,
    Tutorial,
    Lobby,
    Main,
    Chapter1Boss,
    Chapter2General,
    Chapter2Boss,
    Max
}
public class Managers : MonoBehaviour           //디버깅 할 때 매개변수에 값이 할당됐는지에 대한 여부를 보려면 조사식1과 호출스택 창을 열어놓고 중단점에 왔을 때 그 코드를 할당 시켜야됨
{   //todo : 버튼을 누르면 맨처음 튜토리얼 진행했던 시점으로 돌아가는 기능도 필요
    

    [SerializedDictionary("SceneName", "SceneAsset")]
    public SerializedDictionary<SceneName, UnityEditor.SceneAsset> SceneAssetDic = new SerializedDictionary<SceneName, UnityEditor.SceneAsset>();

    public bool invincibility = false;
    public bool tutorial_skip = false;
    public bool oprator_key = false;
    public bool non_damage = false;


    static Managers _instance;
    public static Managers instance { get { Init(); return _instance; } }
    private void Awake()
    {
        Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            if (count == totalCount)
            {
                UI_jun.Init();
                Sound.mixer = Resource.Load<AudioMixer>("Sound_option.mixer");
                SceneManager.sceneLoaded += GameManager.Next_sceneLoaded;
                Sound.bgSound = gameObject.GetOrAddComponent<AudioSource>();
                GameManager.gameover += Sound.Game_over_sound;
                GameManager.gameover += UI_jun.Game_over_ui;
                if(invincibility)
                {
                    GameManager.stage_clear_init();
                }
                else if (tutorial_skip)
                {
                    Managers.GameManager.stage_clear["Tutorial_stage"] = true;
                }
                GameManager.load_end = true;
            }
        });
        //Grid._grid = gameObject.GetOrAddComponent<Grid>(); //이거 아직 사용 안하는중
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
        if (GameManager.load_end)
        {
            if (!GameManager.splash && Input.GetKeyDown(KeyCode.Tab))
            {
                GameManager.splash = true;
                if (invincibility || tutorial_skip)
                {
                    UI_jun.Fade_out_next_in("Black", 0, 1f, "Main_screen", 1f);
                }
                else
                {
                    UI_jun.Fade_out_next_in("Black", 0, 1f, "Tutorial_stage", 1f);
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape) && GameManager.splash && !UI_jun.fade_start)       //FIX : 나중에 여기 수정해야됨
            {
                OptionUIController();
            }
            if (GameManager.scene_name == "Main_screen")
            {

            }
        }
    }

    public void OptionUIController()
    {
        if (UI_jun.UI_window_on["Option"].activeSelf)
        {
            GameManager.option_window_on = false;
            GameManager.game_stop = false;
            UI_jun.UI_window_off.Peek().SetActive(false);
            if (GameManager.scene_name != "Main_screen")
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
    }
    
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
    public static Barrage_manager Barrage { get { return instance?._barrage; } }

    SoundManager _sound = new SoundManager();
    UIManager_jun _ui = new UIManager_jun();
    //UIManager _ui_base = new UIManager();
    ResourceManager _resources = new ResourceManager();
    Camera_manager _camera = new Camera_manager();
    GameManager _game = new GameManager();
    PoolManager _pool = new PoolManager();
    Grid_controller _grid = new Grid_controller();
    Barrage_manager _barrage = new Barrage_manager();
}
