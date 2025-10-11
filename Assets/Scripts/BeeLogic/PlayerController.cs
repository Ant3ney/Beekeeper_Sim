using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D mRigidbody2D;
    public float speed = 5f;

    public Vector2 lastDirection;
    [HideInInspector] public BeeManager myCloud;
    private Transform myCloudTransform;
    public 
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mRigidbody2D = GetComponent<Rigidbody2D>();
        myCloud = FindFirstObjectByType<BeeManager>();
        
        
        myCloudTransform = myCloud.transform;
        lastDirection = Vector2.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal"); // A (-1)  → D (+1)
        float moveZ = Input.GetAxis("Vertical"); // S (-1)  → W (+1)

        Vector3 move = new Vector3(moveX, moveZ, 0);
        mRigidbody2D.linearVelocity = speed * move;

        lastDirection = new Vector2(Mathf.Sign(move.x), Mathf.Sign(move.y));
        //transform.position += 5f * Time.deltaTime * move;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Hive"))
        {
            //well we will increase the bee count
            
        }
    }
}