using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float jumpForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb =
            collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Debug.Log("hit");
            Vector2 newVelocity = rb.velocity;
            newVelocity.y = jumpForce;
            rb.velocity = newVelocity;
        }
    }
}
