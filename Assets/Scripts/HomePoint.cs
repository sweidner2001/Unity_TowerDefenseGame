using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HomePoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isAssigned = false;
    public float homePointRadius = 0.2f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //########################### Methoden #############################
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, this.homePointRadius);
    }
}
