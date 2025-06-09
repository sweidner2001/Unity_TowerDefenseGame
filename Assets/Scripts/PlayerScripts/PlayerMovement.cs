using System.Collections;
using UnityEngine;


// API: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
public class PlayerMovement : MonoBehaviour
{
    //######################## Membervariablen ##############################
    //public float movingSpeed = 3;
    public int facingDirection = 1;

    private Rigidbody2D rb;
    public Animator animator;

    public bool isKnockedBAck { get; set; }

    public Player_Combat player_Combat;



    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.rb = GetComponentInParent<Rigidbody2D>();
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.K)) // Input.GetButtonDown("UserAttack")
        {
            player_Combat.Attack();
        }
    }


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

        this.rb.linearVelocity = new Vector2(horizontal, vertical) * PlayerStatsManager.Instance.movingSpeed;
    }




    //########################### Methoden #############################
    private void Flip()
    {
        facingDirection *= -1;
        this.transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);
    }

  
}
