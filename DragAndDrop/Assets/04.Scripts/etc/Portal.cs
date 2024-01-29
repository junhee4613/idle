using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IInteraction_obj
{
    public void Awake()
    {
        gameObject.TryGetComponent<BoxCollider2D>(out BoxCollider2D bc);
        if (bc == null)
        {
            gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
        }
        if (gameObject.layer == 0)
        {
            gameObject.layer = 8;
        }
    }
    private void Start()
    {
        
    }

    public void practice()
    {
        SceneManager.LoadScene(gameObject.name);
    }
}
