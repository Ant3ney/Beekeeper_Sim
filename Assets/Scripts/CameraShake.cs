using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool isShaking = false;

    public static CameraShake Instance;
    private float shakeTimer = 0.0f;
    public float shakeTime = 0.3f;
    public float shakeStrength = 0.2f;
    public Vector3 startPosition;

    private float moveTimer = 0.0f;
    public int numberMoves = 3;
    
    public IEnumerator StartShake()
    {
        isShaking = true;
        startPosition = transform.localPosition;

        float changeTime = shakeTime / (float)numberMoves;

        while (shakeTimer < shakeTime)
        {
            shakeTimer += Time.deltaTime;

            moveTimer += Time.deltaTime;
            if (moveTimer > changeTime)
            {
                Vector2 newOffset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * shakeStrength;
                transform.localPosition = startPosition + (Vector3) newOffset;
                moveTimer = 0.0f;
            }
            
            yield return null;
        }

        shakeTimer = 0.0f;
        moveTimer = 0.0f;
        transform.localPosition = startPosition;
        isShaking = false;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Instance == null) Instance = this;
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
