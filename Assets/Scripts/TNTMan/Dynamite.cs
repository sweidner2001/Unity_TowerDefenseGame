using UnityEngine;

public class Dynamite : MonoBehaviour
{
    //######################## Membervariablen ##############################
    private Animator animator;



    //########################### Geerbte Methoden #############################
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //############################### Methoden ################################
    protected void DynamiteExplode()
    {
        animator.SetTrigger("startExplosion");
    }
    public void DynamiteDestroy()
    {
        Destroy(gameObject);
    }
}
