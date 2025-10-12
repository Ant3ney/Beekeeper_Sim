using System;
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;

public class BeeShape
{
    public float currentTimer;
    public PredictedFormation formation;
    public Spline spline;
    public int numBeesAssigned;
    public int myIndex;
    public List<BeeObject> assignedBees;
    public List<float> beeDistancesTraveled;
    public List<Vector3> beeFinalPositions;
    public List<float> beeFinalDistances;
    public List<float> beeTimerAfterEnd;
    public BeeCreation scInstance;
    
    public float beeSpeed;
    public float beeReturnTimer;
    public float beeLength;
    

    private bool beginStall = false;
    private bool shouldDestroySelf = false;

    //TODO: if spline is empty or has no length, make sure it doesnt get created
    public BeeShape(Spline pSpline, List<BeeObject> pAssignedBees, 
        int index, PredictedFormation pFormation)
    {
        currentTimer = 0.0f;
        spline = pSpline;
        assignedBees = pAssignedBees;
        formation = pFormation;
        
        beeDistancesTraveled = new List<float>(pAssignedBees.Count);
        beeTimerAfterEnd = new List<float>(pAssignedBees.Count);
        beeFinalPositions = new List<Vector3>(pAssignedBees.Count);
        beeFinalDistances = new List<float>(pAssignedBees.Count);
        myIndex = index;
        shouldDestroySelf = false;
        for (int i = 0; i < pAssignedBees.Count; i++)
        {
            beeDistancesTraveled.Add(0);
            beeTimerAfterEnd.Add(0);

            Vector3 finalPositions = GetFinalBeePosition(i, out float outDist);
            beeFinalPositions.Add(finalPositions);
            beeFinalDistances.Add(outDist);
        }
        
        scInstance = BeeCreation.Instance;
        beeSpeed = scInstance.beeSpeed;
        beeReturnTimer = scInstance.beeReturnTimer;
        beeLength = scInstance.lengthPerBee;
    }

    //returns where bee should be in the final line up...
    //make sure we are using euler integrated things for 
    public Vector3 GetFinalBeePosition(int index, out float desiredDist)
    {
        float finalLength = spline.GetLength();
        float beeIntervals = finalLength / (float)assignedBees.Count;

        float atLength = beeIntervals * index + beeIntervals * 0.5f;
        desiredDist = atLength;
        float pct = atLength / finalLength;
        return spline.EvaluatePosition(pct);
    }

    //is it too much to let everything include bee movement be decided here?
    public void UpdateBeeShape()
    {
        currentTimer += Time.fixedDeltaTime;
        float len = spline.GetLength();
        
        //linear movement function for each bee
        for (int i = 0; i < assignedBees.Count; i++)
        {
            //each bee moves along the spline...
            float traveled = beeDistancesTraveled[i];
            traveled += Time.fixedDeltaTime * beeSpeed;
            
            float pct = traveled / len;
            Vector3 newPosition = spline.EvaluatePosition(Mathf.Clamp(pct, 0, 1));
            
            beeDistancesTraveled[i] = traveled;

            float finalDist = beeFinalDistances[i];
            Vector3 desiredEndpoint = beeFinalPositions[i];
            
            //TODO: CHECK TO SEE IF I NEED TO MOVE THIS LOGIC TO BEEOBJECT
            if (traveled < finalDist)
            {
                assignedBees[i].MoveBeePosition(newPosition);
            }
            else
            {
                beeTimerAfterEnd[i] += Time.fixedDeltaTime;
                if (beeTimerAfterEnd[i] > beeReturnTimer)
                {
                    assignedBees[i].SetMoveState(BeeObject.MoveState.Returning);
                    if (i == assignedBees.Count - 1) shouldDestroySelf = true;
                }
                else
                {
                    assignedBees[i].MoveBeePosition(desiredEndpoint);
                }
            }
            
        }

        if (shouldDestroySelf)
        {
            scInstance.allBeeShapes.Remove(this);
            formation.StartDestruction();
        }
        
    }
}

public class BeeCreation : MonoBehaviour
{
    public static BeeCreation Instance { get; private set; }

    public List<BeeShape> allBeeShapes;
    private Camera mCam;
    public Vector2 curMousePos;

    public PredictedFormation formationTemplate;
        
    public Spline activeSpline;
    public PredictedFormation activeFormation;

    public int splineAvailableBees = 0;
    public float splineLengthSoFar = 0.0f;
    public int splinePredictedBees = 0;
    
    public float testValue = 0;
    public GameObject testObject;
    public GameObject testPoint;

    public int totalBeeCount = 10;
    public Queue<BeeObject> beeReserve;
    public HashSet<BeeObject> beeSet;
    public List<BeeObject> beeList;
    
    
    //restrictions for how much your spline can go, its per bees
    [Header("Curve Restrictions")] 
    public int maxNumberKnots = 10;
    public float maxCurveLength = 5f;
    public float lengthPerBee = 0.1f;

    public float minimumDistance = 0.025f;
    public float beeReturnTimer = 3.0f;
    public float beeSpeed = 5f;
    //shape should die when bees finally get to lastpoint
    public float shapeFalloffTime = 0.3f;
    public float shapeFalloffTimer = 0.0f;

    //cooldown for creating new splines
    public float splineCreationCooldown = 0.5f;
    private float splineCreationTimer = 0.0f;
    public float betweenAddsTime = 0.1f;
    private float betweenAddsTimer = 0.0f;
    private float lastLength = 0.5f;

    private bool isCreatingShape = false;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        
        mCam = FindFirstObjectByType<Camera>();
        beeList = new List<BeeObject>();
        allBeeShapes = new List<BeeShape>();
        beeReserve = new Queue<BeeObject>();
        beeSet = new HashSet<BeeObject>();

        for (int i = 0; i < beeList.Count; i++)
        {
            AddBeeToArsenal(beeList[i]);
        }

        splineCreationTimer = splineCreationCooldown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 mousePosition = Input.mousePosition;
        curMousePos = ScreenToWorldPosition(mousePosition);
        
        HandleAllBeeShapes();
        HandleSplineCreation();
        
    }

    //do some kind of pool...
    public void AddBeeToArsenal(BeeObject bo)
    {
        if (!beeSet.Contains(bo))
        {
            beeReserve.Enqueue(bo);
        }
        beeSet.Add(bo);
    }

    private void HandleSplineCreation()
    {
        splineCreationTimer += Time.fixedDeltaTime;
        
        if (Input.GetMouseButtonDown(0) && beeReserve.Count > 0)
        {
            if (splineCreationTimer >= splineCreationCooldown)
            {
                splineAvailableBees = beeReserve.Count;
                splineCreationTimer = 0.0f;
                isCreatingShape = true;
                
                activeFormation = Instantiate(formationTemplate);
                activeFormation.Initialize(activeSpline);
            }
        }
        
        if (Input.GetMouseButton(0) && isCreatingShape)
        {
            ShapeAdd(new Vector3(curMousePos.x, curMousePos.y, 0));
            activeFormation.UpdateLinePositions();
        }

        if (Input.GetMouseButtonUp(0) && isCreatingShape)
        {
            float len = activeSpline.GetLength();
            
            splinePredictedBees = Mathf.FloorToInt(len / lengthPerBee);
            if (splinePredictedBees == 0 && splineAvailableBees > 0) splinePredictedBees = 1;
            
            List<BeeObject> bees = AssignBees();
            BeeShape newShape = new BeeShape(activeSpline, bees, 
                allBeeShapes.Count, activeFormation);
            activeSpline = new Spline();
            splineLengthSoFar = 0.0f;
            splinePredictedBees = 0;
            splineAvailableBees = 0;
            betweenAddsTimer = betweenAddsTime + 0.05f;
            isCreatingShape = false;
            allBeeShapes.Add(newShape);
        }
    }
    

    //here we will handle all bee shape logic in update
    private void HandleAllBeeShapes()
    {
        for (int i = 0; i < allBeeShapes.Count; i++)
        {
            allBeeShapes[i].UpdateBeeShape();
        }
    }

    Vector3 ClampSpline(Vector3 mousePosition, float maxLength)
    {
        float curLength = activeSpline.GetLength();
        Spline testSpline = new Spline();

        BezierKnot[] knots = activeSpline.ToArray();
        testSpline.Knots = knots;
        testSpline.Add(mousePosition);

        float newLength = testSpline.GetLength();
        newLength = Mathf.Max(newLength, 0.000001f);
        
        if (newLength > maxLength)
        {
            float pct = maxLength / newLength;
            return testSpline.EvaluatePosition(pct);
        }
        
        return mousePosition;
    }
    
    //LOOK INTO TANGENTS...
    void ShapeAdd(Vector3 mousePosition)
    {
        float totalLengthPossible = splineAvailableBees * lengthPerBee;

        bool distanceCheck = true;
        float dist = 0;
        BezierKnot bk;
        Vector3 lastPos = Vector3.zero;
        
        if (activeSpline.Knots.Count() > 0)
        {
            bk = activeSpline.Knots.Last();
            lastPos = bk.Position;
            dist = Vector2.Distance(lastPos, mousePosition);
            distanceCheck = dist >= minimumDistance;
        }
        
        if (betweenAddsTimer >= betweenAddsTime && 
            distanceCheck)
        {
            splineLengthSoFar = activeSpline.GetLength();
            if (splineLengthSoFar >= totalLengthPossible || totalLengthPossible == 0)
            {
                return;
            }

            Vector3 newPosition = mousePosition;
            
            if(activeSpline.Knots.Count() > 0)
            {
                newPosition = ClampSpline(newPosition, totalLengthPossible);

                float distance = (newPosition - lastPos).magnitude;
                activeSpline.Add(newPosition);
                Debug.Log("last dist was " + splineLengthSoFar + " added about " + distance);
            }
            else
            {
                activeSpline.Add(newPosition);
            }
            
            betweenAddsTimer = 0.0f;
        }
        else
        {
            betweenAddsTimer += Time.fixedDeltaTime;
        }
    }

    List<BeeObject> AssignBees()
    {
        List<BeeObject> listToReturn = new List<BeeObject>();
        for (int i = 0; i < splinePredictedBees; i++)
        {
            BeeObject bo = beeReserve.Dequeue();
            bo.SetMoveState(BeeObject.MoveState.Free);
            beeSet.Remove(bo);
            listToReturn.Add(bo);
        }

        return listToReturn;
    }

    Vector3 ScreenToWorldPosition(Vector2 mousePos)
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        mousePos.x = Mathf.Clamp(mousePos.x, 0, screenWidth);
        mousePos.y = Mathf.Clamp(mousePos.y, 0, screenHeight);
        return mCam.ScreenToWorldPoint(new Vector3(mousePos.x, 
            mousePos.y, 0));
    }
    
}
