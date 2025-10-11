using System;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BeeObject : MonoBehaviour
{
    public enum MoveState
    {
        Free,
        FreeCloud,
        Returning,
        GoingToShape
    }

    private MoveState curMoveState;
    private BeeManager myCloud;
    private Transform myCloudTransform;
    private Transform myTransform;
    private BeeCreation scInstance;
    private Rigidbody2D mRigidbody;
    private bool initialized = false;
    private Vector2 headedVector;
    private bool hasDirection = false;
    private bool hasChosenSince = false;

    [Header("Game Feel Parameters")] 
    public float beeRoamSpeed = 2f;
    public float beeReturnSpeed = 8f;
    public float snapCloseness = 0.02f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //TODO:change/optimize later
        myCloud = FindFirstObjectByType<BeeManager>();
        
        scInstance = BeeCreation.Instance;
        initialized = false;

        mRigidbody = GetComponent<Rigidbody2D>();
        headedVector = Vector3.zero;
        
        myTransform = transform;
        myCloudTransform = myCloud.transform;
        
        SetMoveState(MoveState.FreeCloud);
    }

    public void FixedUpdate()
    {
        if (!scInstance) scInstance = BeeCreation.Instance;
            
        if (scInstance && !initialized)
        {
            scInstance.beeList.Add(this);
            scInstance.AddBeeToArsenal(this);
            initialized = true;
        }
        //MovingFunction();
    }
    
    
    public void AddToVelocity(Vector2 velocity)
    {
        if(curMoveState == MoveState.FreeCloud)
            mRigidbody.linearVelocity += velocity;
    }

    //call movement function by manager... bee cloud should be bee manager
    public void MovingFunction()
    {
        if (curMoveState == MoveState.Returning)
        {
            Vector3 cloudPosition = myCloudTransform.position;

            Vector3 myPos = myTransform.position;
            Vector3 toCloud = cloudPosition - myPos;
            Vector3 normToCloud = Vector3.Normalize(toCloud);
            float distToPlayer = toCloud.magnitude;

            Vector3 newVec = beeReturnSpeed * normToCloud;
            float actualSpeed = newVec.magnitude * Time.fixedDeltaTime;

            if (actualSpeed > distToPlayer)
            {
                newVec = Vector3.Normalize(newVec);
                newVec *= distToPlayer;
                CollideWithPlayer();
            }
            
            mRigidbody.linearVelocity = newVec;
            //Vector3 newPos = myPos + newVec;
            //transform.position = newPos;
            
        }
        else if (curMoveState == MoveState.FreeCloud)
        {
            mRigidbody.linearVelocity = headedVector;
            Vector3 myPos = myTransform.position;
            Vector3 cloudPos = myCloudTransform.position;
            float radius = myCloud.radius;
            Vector2 meToCloud = cloudPos - myPos;
            float distance = meToCloud.magnitude;

            Vector2 curDir = mRigidbody.linearVelocity;
            float dotval = Vector2.Dot(curDir.normalized, meToCloud.normalized);   
            //Debug.Log(dotval);
            
            if (distance > radius && dotval < 0)
            {
                hasDirection = false;
            }
            
            if (!hasDirection)
            {
                //find new position within circle...
                //get random edge of circle
                
                Vector2 direction = GetCorrectDirectionForCloud();
                headedVector = direction;
                mRigidbody.linearVelocity = headedVector * beeRoamSpeed;
                hasDirection = true;
            }

            //just random movement in the cloud...

        }
    }
    

    private Vector2 GetCorrectDirectionForCloud()
    {
        Vector3 cloudPos = myCloudTransform.position;
        Vector3 myPos = myTransform.position;
        
        Vector3 meToCloud = Vector3.Normalize(cloudPos - myPos);
        //90 degree randomization, means 0.5 dot product
        //first end
        float angle = Mathf.PI / 4;
        angle = Random.Range(-angle, angle);
        Vector2 direction = RotateBy(meToCloud, angle);
        return direction.normalized;
    }

    private Vector2 RotateBy(Vector2 vec, float angleRad)
    {
        return new Vector2(vec.x * Mathf.Cos(angleRad) - vec.y * Mathf.Sin(angleRad),
            vec.x* Mathf.Sin(angleRad) + vec.y * Mathf.Cos(angleRad));
    }

    private void CollideWithPlayer()
    {
        if (curMoveState == MoveState.Returning)
        {
            scInstance.AddBeeToArsenal(this);
            mRigidbody.linearVelocity = Vector2.zero;
            SetMoveState(MoveState.FreeCloud);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BeeCloud"))
        {   
            CollideWithPlayer();
        }
    }

    public void SetMoveState(MoveState state)
    {
        MoveState oldState = curMoveState;
        curMoveState = state;

        if (curMoveState != oldState && state == MoveState.FreeCloud)
        {
            hasDirection = false;
        }
    }

    public void MoveBeePosition(Vector3 newPosition)
    {
        //might have to change this to rigidbody/physics movement
        if (curMoveState == MoveState.Free)
        {
            //Vector2 newVec = newPosition - transform.position;
            //mRigidbody.linearVelocity = newVec;
            mRigidbody.MovePosition(newPosition);
        }
        else if (curMoveState == MoveState.FreeCloud)
        {
            mRigidbody.MovePosition(newPosition);
        }
    }
    
    
    void LerpToPlayer()
    {
        
    }
    
    //collision stuff...
}
