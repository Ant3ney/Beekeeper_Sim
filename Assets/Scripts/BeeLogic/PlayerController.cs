using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D mRigidbody2D;
    public float speed = 5f;

    public GameObject BeeTemplate;
    public Vector2 lastDirection;
    [HideInInspector] public BeeManager myCloud;
    private Transform myCloudTransform;
    [HideInInspector] public BeeCreation beeCreation;

    public float myHealth = 100.0f;
    public float myMaxHealth = 100.0f;

    private float invincibleTimer = 0.0f;
    public float invincibleTime = 2.0f;
    private bool isInvincible = false;

    private bool inHitstun = false;
    private float hitstunTimer = 0.0f;
    private float hitStunTime = 0.35f;

    public int startBeeNumber = 40;
    public Transform beeParent;

    private Vector2 addedForce = Vector2.zero;

    public static PlayerController pcInstance;
    public RectTransform healthLeft;

    private float fullWidth;
    private SpriteRenderer myRenderer;
    private Color redTint = new Color(0.752f, 0.12f, 0.23f);
    public float flashRedTime = 0.25f;
    public float flashRedTimer = 0.0f;
    private bool isFlashingRed = false;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pcInstance = this;
        
        mRigidbody2D = GetComponent<Rigidbody2D>();
        myCloud = FindFirstObjectByType<BeeManager>();
        beeCreation = FindFirstObjectByType<BeeCreation>();
        
        myCloudTransform = myCloud.transform;

        for (int i = 0; i < startBeeNumber; i++)
        {
            GameObject bt = Instantiate(BeeTemplate);
            bt.transform.position = myCloudTransform.position;
            BeeObject beeComp = bt.GetComponent<BeeObject>();
            if(beeParent) bt.transform.SetParent(beeParent);
            myCloud.myBees.Add(beeComp);
        }
        
        lastDirection = new Vector2(1, 1);
        isInvincible = false;
        
        if(healthLeft) fullWidth = healthLeft.rect.width;
        
        myRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (healthLeft)
        {
            float pct = myHealth / myMaxHealth;

            Vector2 size = healthLeft.sizeDelta;
            size.x = fullWidth * pct;
            healthLeft.sizeDelta = size;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal"); // A (-1)  → D (+1)
        float moveZ = Input.GetAxis("Vertical"); // S (-1)  → W (+1)

        Vector3 move = new Vector3(moveX, moveZ, 0);
        mRigidbody2D.linearVelocity = speed * move;
        mRigidbody2D.linearVelocity += addedForce;

        if(move.magnitude >= 0.05f) lastDirection = new Vector2(Mathf.Sign(move.x), Mathf.Sign(move.y));
        //transform.position += 5f * Time.deltaTime * move;
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Hive"))
        {
            //well we will increase the bee count
            HiveObject ho = other.gameObject.GetComponent<HiveObject>();
            
            for (int i = 0; i < ho.addFromHive; i++)
            {
                GameObject newBee = Instantiate(BeeTemplate);
                BeeObject beeComp = newBee.GetComponent<BeeObject>();
                newBee.transform.position = myCloud.transform.position;
                myCloud.myBees.Add(beeComp);
            }
            
            ho.StartInactivity();
            //bee add arsenal is in its fixed update...
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        GameObject otherObject = collision.gameObject;

        if (otherObject.CompareTag("Enemy") && !isInvincible)
        {
            EnemyCharacter ec = collision.gameObject.GetComponent<EnemyCharacter>();
            float damageTake = ec.enemyDamage;

            Vector2 movePos = (transform.position - 
                               collision.transform.position).normalized * 5f;
            
            TakeDamage(damageTake, movePos);
        }
    }

    public IEnumerator InvincibleTime()
    {
        isInvincible = true;
        
        while (invincibleTimer < invincibleTime)
        {
            invincibleTimer += Time.fixedDeltaTime;
            isInvincible = true;
            yield return null;
        }

        isInvincible = false;
        invincibleTimer = 0.0f;
    }

    public IEnumerator HitstunTime(Vector2 hitForce)
    {
        inHitstun = true;
        addedForce = hitForce;

        while (hitstunTimer < hitStunTime)
        {
            hitstunTimer += Time.fixedDeltaTime;
            inHitstun = true;
            yield return null;
        }

        addedForce = Vector2.zero;
        inHitstun = false;
        hitstunTimer = 0.0f;
    }
    
    

    public void TakeDamage(float damage, Vector2 hitForce)
    {
        StartCoroutine(InvincibleTime());
        StartCoroutine(HitstunTime(hitForce));
        StartCoroutine(FlashRed());
        
        myHealth -= damage;
    }
    
    public IEnumerator FlashRed()
    {
        flashRedTimer = 0.0f;
        isFlashingRed = true;

        float halfTime = flashRedTime / 2f;
        while (flashRedTimer <= halfTime)
        {
            myRenderer.color = Color.Lerp(Color.white, redTint, flashRedTimer / halfTime);
            flashRedTimer += Time.fixedDeltaTime;
            yield return null;
        }

        while (flashRedTimer <= flashRedTime)
        {
            myRenderer.color = Color.Lerp(redTint, Color.white,
                (flashRedTimer - halfTime) / halfTime);
            flashRedTimer += Time.fixedDeltaTime;
            yield return null;
        }

        myRenderer.color = Color.white;
        flashRedTimer = 0.0f;
        isFlashingRed = false;
    }
}