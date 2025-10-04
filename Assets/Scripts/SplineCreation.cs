using UnityEngine;
using UnityEngine.Splines;

public class SplineCreation : MonoBehaviour
{
    
    public Spline mPlayerSpline;
    private Camera mCam;

    public Vector2 curMousePos;
    public float testValue = 0;

    public GameObject testObject;
    public GameObject testPoint;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mCam = FindFirstObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        curMousePos = ScreenToWorldPosition(mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            mPlayerSpline.Add(new Vector3(curMousePos.x, curMousePos.y, 0));
            Instantiate(testPoint, transform);
        }

        if (mPlayerSpline.GetLength() > 0)
        {
            testObject.transform.position = mPlayerSpline.EvaluatePosition(testValue);
        }
    }

    Vector3 ScreenToWorldPosition(Vector2 mousePos)
    {
        return mCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
    }
}
