using System.Collections.Generic;
using UnityEngine;

public class MovingPath : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public static MovingPath Instance { get; private set; }

    public List<Transform> Checkpoints { get; set; }



    //########################### Geerbte Methoden #############################
    void Awake()
    {
        // Singleton-Pattern: Nur eine Instanz erlauben
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        // Waypoints nur laden, wenn sie noch nicht geladen wurden
        if (Checkpoints == null || Checkpoints.Count == 0)
        {
            LoadCheckpoints();
        }
    }



    void Start()
    {
        // Waypoints nur laden, wenn sie noch nicht geladen wurden
        if (Checkpoints == null || Checkpoints.Count == 0)
        {
            LoadCheckpoints();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    //########################## Methoden ###########################
    protected void LoadCheckpoints()
    {
        Checkpoints = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Checkpoint"))
            {
                Checkpoints.Add(child);
            }
        }
    }



    public Vector3 GetWaypointPosition(int index)
    {
        if (index < 0 || index >= Checkpoints.Count)
        {
            Debug.LogError("Index out of bounds: " + index);
            return Vector3.zero;
        }
        return Checkpoints[index].position;
    }



    //********************** Gizmos **********************
    private void OnDrawGizmos()
    {
        for (int i = 1; i < Checkpoints.Count; i++)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(Checkpoints[i - 1].transform.position, Checkpoints[i].transform.position);
        }
    }
}
