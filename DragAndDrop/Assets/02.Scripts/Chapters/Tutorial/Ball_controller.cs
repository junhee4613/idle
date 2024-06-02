using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_controller : MonoBehaviour
{
    //Rigidbody2D rb;
    //Vector2 collider_info;
    //public float speed;
    private void Start()
    {
        //rb = gameObject.GetOrAddComponent<Rigidbody2D>();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {

        //collider_info = collision.contacts[0].normal;
        //rb.velocity = ball_reflect_normal(rb.velocity.normalized, collider_info).normalized * speed;
    }
    Vector2 ball_reflect_normal(Vector2 forward_dir, Vector2 current_dir)
    {
        return Vector2.Reflect(current_dir, forward_dir);
    }
}
