using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public int damage = 1;



    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    //########################### Methoden #############################
    /// <summary>
    /// Wird aufgerufen, wenn der Charakter mit einen anderen Collider kollidiert
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<PlayerHealth>()?.ChangeHealth(-damage);
    }

}
