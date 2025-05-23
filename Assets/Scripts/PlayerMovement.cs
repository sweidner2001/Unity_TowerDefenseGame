using System.Collections;
using UnityEngine;


// API: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
public class PlayerMovement : MonoBehaviour
{
    public float movingSpeed = 3;
    public int facingDirection = 1;

    public Rigidbody2D rb;
    public Animator animator;

    private bool isKnockedBAck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    // Update is called 50x frame
    void FixedUpdate()
    {

        if (isKnockedBAck == true)
        {
            return;
        }

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

        this.animator.SetFloat("horizontal", Mathf.Abs(horizontal));
        this.animator.SetFloat("vertical", Mathf.Abs(vertical));

        this.rb.linearVelocity = new Vector2(horizontal, vertical) * this.movingSpeed;
    }

    private void Flip()
    {
        facingDirection *= -1;
        this.transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);
    }

    public void Knockback(Transform enemyTransform, float force, float stunTime)
    {
        isKnockedBAck = true;
        Vector2 direction = (this.transform.position - enemyTransform.position).normalized;
        this.rb.linearVelocity = direction * force;
        StartCoroutine(KnockbackCounter(stunTime));
    }

    private IEnumerator KnockbackCounter(float stunTime)
    {
        // Wartezeit
        yield return new WaitForSeconds(stunTime);

        // anschließend Figur zum stehen bringen und die Bewegungs-Kontrolle zurückgeben
        this.rb.linearVelocity = Vector2.zero;
        this.isKnockedBAck = false;
    }
}
