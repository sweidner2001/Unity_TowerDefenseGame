using UnityEngine;


// API: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
public class PlayerMovement : MonoBehaviour
{
    public float speed = 3;
    public int facingDirection = 1;

    public Rigidbody2D rb;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    // Update is called 50x frame
    void FixedUpdate()
    {
        // Tasten-Input, der in den Einstellungen konfiguriert wurde
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // horizontal > 0 --> nach rechts laufen, aber Bild links ausgerichtet
        // horizontal < 0 --> nach links laufen, aber Bild rechts ausgerichtet
        if (horizontal > 0 && transform.localScale.x < 0 ||
            horizontal < 0 && transform.localScale.x > 0)
        {
            Flip();   
        }

        animator.SetFloat("horizontal", Mathf.Abs(horizontal));
        animator.SetFloat("vertical", Mathf.Abs(vertical));

        rb.linearVelocity = new Vector2(horizontal, vertical) * speed;
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);
    }
}
