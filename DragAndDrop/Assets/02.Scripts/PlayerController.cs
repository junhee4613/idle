using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public int life = 3;
    Rigidbody2D rb;
    CircleCollider2D cc;
    public float speed;
    Vector2 mouse_pos;
    RaycastHit2D player_hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            player_hit = Physics2D.Raycast(mouse_pos, Vector2.zero, 0, 1 << 6);
            if (!player_hit)
            {
                //skwnddp wpwkr
            }
        }
    }
}
