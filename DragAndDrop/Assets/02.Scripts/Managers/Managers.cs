using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance;
    public static Managers instance { get { Init(); return _instance; } }
    public static void Init()
    {

    }
    public static GameManager GameManager { get { return instance?._game; } }

    public GameManager _game = new GameManager();
}
