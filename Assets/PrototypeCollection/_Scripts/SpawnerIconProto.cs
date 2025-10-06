using UnityEngine;

public class SpawnerIconProto : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     void OnDrawGizmos()
    {
        // Set color
        Gizmos.color = Color.cyan;

        // Example shapes:
        Gizmos.DrawSphere(transform.position, 0.5f);
        // Gizmos.DrawCube(transform.position, Vector3.one * 1f);
        // Gizmos.DrawWireSphere(transform.position, 1f);
        // Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }
}
