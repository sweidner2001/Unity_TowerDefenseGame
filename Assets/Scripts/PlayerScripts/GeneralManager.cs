using UnityEngine;

public class GeneralManager : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public static GeneralManager Instance { get; private set; }

    [Header("HealthBar")]
    public Gradient HealthBarGradient;


    //########################### Geerbte Methoden #############################
    private void Awake()
    {
        if(GeneralManager.Instance == null)
            GeneralManager.Instance = this;
        else
            // es gibt schon eine Instance von der Klasse, wir zerstören diese hier
            Destroy(gameObject);
    }




}
