using System;
using System.Collections.Generic;
using UnityEngine;

public class BeeManager : MonoBehaviour
{
    public float radius = 1;
    public List<BeeObject> myBees;
    public float distanceAway = 3.5f;
    public float speedTowardDrift = 1.5f;

    private PlayerController myPlayer;
    private Transform myPlayerTransform;
    private Transform myTransform;
    private Rigidbody2D myPlayerRigidbody;
    
    private Rigidbody2D mRigidbody;
    private Vector2 direction;
    private Vector2 lastPosition;

    private Vector2 lastDriftPosition;

    void Awake()
    {
        mRigidbody = GetComponent<Rigidbody2D>();

        myTransform = GetComponent<Transform>();
        myBees = new List<BeeObject>();
        myPlayer = FindFirstObjectByType<PlayerController>();
        myPlayerTransform = myPlayer.transform;
        myPlayerRigidbody = myPlayer.gameObject.GetComponent<Rigidbody2D>();
        
        BeeObject[] existingBees = FindObjectsByType<BeeObject>(FindObjectsSortMode.None);
        foreach (BeeObject bee in existingBees)
        {
            myBees.Add(bee);
        }

        lastDriftPosition = myPlayerTransform.position;
    }

    private Vector2 RotateBy(Vector2 vec, float angleRad)
    {
        return new Vector2(vec.x * Mathf.Cos(angleRad) - vec.y * Mathf.Sin(angleRad),
            vec.x* Mathf.Sin(angleRad) + vec.y * Mathf.Cos(angleRad));
    }
    
    public Vector2 FindPositionToDrift(Vector2 playerLastDirection)
    {
        Vector2 position = myPlayerTransform.position;

        float angle = Mathf.PI / 5;
        Vector2 newDir = RotateBy(playerLastDirection, angle).normalized;
        newDir *= distanceAway;
        position += newDir;
        return position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //continuing
        Vector2 speed = myPlayerRigidbody.linearVelocity;
        mRigidbody.linearVelocity = speed;

        Vector2 dirToDrift = lastDriftPosition - (Vector2) myTransform.position;
        dirToDrift = dirToDrift.normalized * speedTowardDrift;

        //mRigidbody.linearVelocity = dirToDrift + speed;
        
        //Vector2 lastVector = (Vector2) transform.position - lastPosition;
        
        foreach (BeeObject bee in myBees)
        {
            bee.MovingFunction();
            bee.AddToVelocity(speed);
        }

        Vector2 playerDirection = myPlayer.lastDirection;
        lastDriftPosition = FindPositionToDrift(playerDirection);
        
        lastPosition = transform.position;

    }
}