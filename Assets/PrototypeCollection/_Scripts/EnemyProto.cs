using UnityEngine;
using UnityEngine.AI;

public class EnemyProto : MonoBehaviour
{
	GameObject Player = null;
	Transform PlayerLocation;
	UnityEngine.AI.NavMeshAgent agent;
	public float damagePerSecond = 10;

	DumbData dumbData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //
        public float Timer = 2;
    float current = 0;

    public bool startDeath = false;
    void Start()
    {
	    Player = GameObject.Find("Player");
	    dumbData = GameObject.Find("EasyData").GetComponent<DumbData>();

	    //float moveX = Input.GetAxis("Horizontal"); // A (-1)  → D (+1)
	    //float moveZ = Input.GetAxis("Vertical");   // S (-1)  → W (+1)

	    //Vector3 move = new Vector3(moveX, moveZ, 0);
	    //transform.position += move * 5f * Time.deltaTime;
	    //
	    //

	    agent = 	GetComponent<UnityEngine.AI.NavMeshAgent>();
	    agent.updateRotation = false;
	    agent.updateUpAxis = false;

	    damagePerSecond = dumbData.enemyDamagePerSecond;

	    transform.localScale = dumbData.enemyScale;
	
	    //	 public Sprite enemySprite;
	    Transform child = transform.GetChild(0); // gets the first child
        SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
	sr.sprite = dumbData.enemySprite;
    }

    // Update is called once per frame
    void Update()
    {

	    PlayerLocation = Player.transform;	
	    bool ShouldMoveDown = transform.position.y > PlayerLocation.transform.position.y;
	    bool ShouldMoveLeft = transform.position.x > PlayerLocation.transform.position.x;
	    float MoveX = ShouldMoveLeft ? -1 : 1;
	    float MoveY = ShouldMoveDown ? -1 : 1;

	    Vector3 Move = new Vector3(MoveX, MoveY, 0);
	    //transform.position += Move * 3f * Time.deltaTime;

	    agent.SetDestination(PlayerLocation.position);

	    if(startDeath)
	    {

		    current += 1 * Time.deltaTime;

		    if(current > Timer)
		    {
			    Destroy(gameObject);
		    }
	    }
    }

        void OnCollisionStay2D(Collision2D collision)
    {
	    GameObject otherObject = collision.gameObject;
	    BeeProto bee = otherObject.GetComponent<BeeProto>();

	    if (bee) {
		    float damagePerSecond = bee.damagePerSecond;
		    float DamageThisFrame =  damagePerSecond * Time.fixedDeltaTime;
		    HealthProto healthProto = GetComponent<HealthProto>();

		    healthProto.ReciveDamage(DamageThisFrame);
	    }


	    //Debug.Log("Collided with: " + otherObject.name);
    }
}
