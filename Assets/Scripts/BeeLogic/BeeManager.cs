using System.Collections.Generic;
using UnityEngine;

public class BeeManager : MonoBehaviour
{
    public float radius = 1;
    public List<BeeObject> myBees;

    private PlayerController myPlayer;
    private Transform myPlayerTransform;
    private Rigidbody2D myPlayerRigidbody;
    
    private Rigidbody2D mRigidbody;
    private Vector2 direction;
    private Vector2 lastPosition;

    void Awake()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
        
        myBees = new List<BeeObject>();
        myPlayer = FindFirstObjectByType<PlayerController>();
        myPlayerTransform = myPlayer.transform;
        myPlayerRigidbody = myPlayer.gameObject.GetComponent<Rigidbody2D>();
        
        BeeObject[] existingBees = FindObjectsByType<BeeObject>(FindObjectsSortMode.None);
        foreach (BeeObject bee in existingBees)
        {
            myBees.Add(bee);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //continuing
        Vector2 speed = myPlayerRigidbody.linearVelocity;
        mRigidbody.linearVelocity = speed;

        //Vector2 lastVector = (Vector2) transform.position - lastPosition;
        
        foreach (BeeObject bee in myBees)
        {
            bee.MovingFunction();
            bee.AddToVelocity(speed);
        }
        
        lastPosition = transform.position;

    }
}