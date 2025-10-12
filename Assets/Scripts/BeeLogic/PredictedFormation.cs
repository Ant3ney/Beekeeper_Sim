using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class PredictedFormation : MonoBehaviour
{
    private Spline mSpline;
    private LineRenderer mLineRenderer;
    public float lengthPerLine = 0.05f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        mLineRenderer = GetComponent<LineRenderer>();
        if (mLineRenderer == null)
        {
            mLineRenderer = this.AddComponent<LineRenderer>();
        }

        mLineRenderer.loop = false;
    }

    public void Initialize(Spline pSpline)
    {
        mSpline = pSpline;
    }

    //can make this iterative based on length/curvature
    public void UpdateLinePositions()
    {
        Vector3[] emptyPos = new Vector3[0];
        mLineRenderer.SetPositions(emptyPos);
        List<Vector3> newPositions = new List<Vector3>();
        
        float splineLen = mSpline.GetLength();
        float traveled = 0.0f;

        //TODO: quick and dirty, may want to revisit later for optimization
        if (mSpline.Knots.Count() != 0 && splineLen != 0)
        {
            while (traveled < splineLen)
            {
                float pct = traveled / splineLen;
                float nextPoint = Mathf.Clamp(pct, 0, 1);
                Vector3 nextPos = mSpline.EvaluatePosition(nextPoint);
                newPositions.Add(nextPos);
                
                traveled += lengthPerLine;
            }
        }
        else
        {
            for (int i = 0; i < mSpline.Knots.Count(); i++)
            {
                BezierKnot bk = mSpline.Knots.ElementAt(i);
                newPositions.Add(bk.Position);
            }
        }

        if (newPositions.Count == 0) return;

        bool arePointsTooClose = CheckForPointCloseness(newPositions);

        if (arePointsTooClose)
        {
            //
            Vector2 origPos = newPositions[0];
            Vector2 newVec = new Vector2(1, 0);
            newVec.Normalize();

            float onePointWidth = mLineRenderer.startWidth + 0.05f;
            Vector3 firstVal = origPos - newVec * (onePointWidth / 2f);
            Vector3 secondVal = origPos + newVec * (onePointWidth / 2f);

            newPositions[0] = firstVal;
            newPositions.Add(secondVal);
        }

        mLineRenderer.positionCount = newPositions.Count;
        mLineRenderer.SetPositions(newPositions.ToArray());
    }

    
    //TODO: MAKE LOOK BETTER WITH ART
    public void StartDestruction()
    {
        Destroy(this.gameObject);
    }

    //returns true if they were all similar
    private bool CheckForPointCloseness(List<Vector3> positions)
    {
        if (positions.Count == 1) return true;

        float onePointWidth = mLineRenderer.startWidth + 0.05f;
        
        Vector3 startPos = positions[0];
        for (int i = 1; i < positions.Count; i++)
        {
            Vector3 curPos = positions[i];
            float distance = Vector3.Magnitude(curPos - startPos);
            if (distance > onePointWidth)
            {
                return false;
            }
        }

        return true;
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}
