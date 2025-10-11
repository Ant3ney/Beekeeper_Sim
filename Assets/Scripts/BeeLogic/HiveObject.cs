using UnityEngine;

public class HiveObject : MonoBehaviour
{
    private bool isInactive = false;
    public float hiveRespawnCooldown = 25.0f;
    private float hiveRespawnTimer = 0.0f;
    public int addFromHive = 3;

    private Collider2D myCollider;
    private SpriteRenderer mySpriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isInactive)
        {
            hiveRespawnTimer += Time.fixedDeltaTime;
            if (hiveRespawnTimer > hiveRespawnCooldown)
            {
                StartActivity();
            }
        }
    }

    public void StartInactivity()
    {
        isInactive = true;

        myCollider.enabled = false;
        mySpriteRenderer.enabled = false;
        hiveRespawnTimer = 0.0f;

    }

    public void StartActivity()
    {
        isInactive = false;

        myCollider.enabled = true;
        mySpriteRenderer.enabled = true;
    }
}
