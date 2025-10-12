using UnityEngine;

public class AttackObject : MonoBehaviour
{
	public float destroyTimer = 0.25f;
	float timer = 0;

	public Vector3 attackerVelocity;
	public float attackObjectForce = 3f;
	public bool isRanged = false;
	public float rangedSpeed = 3f;

	public AudioClip audioClipImpactSoundEffect;
	
	public float damage = 20;
	private Rigidbody2D rb;
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		rb = rb.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if (isRanged)
		{
			rb.linearVelocity = attackerVelocity.normalized * rangedSpeed;
		}
		
		/*
		if(isRanged) {
			Vector3 currentPosition = this.transform.position;
			Vector3 movementAmount = attackerVelocity.normalized * rangedSpeed * Time.deltaTime;
			this.transform.position = currentPosition + movementAmount;
		}
		*/

		timer += Time.deltaTime; 
		if(timer >= destroyTimer) {
			Destroy(gameObject);
			timer = 0;
		}
	}


	/*
	void OldTrigger(Collider2D other)
	{
		Debug.Log("Ball overlapped with: " + other.gameObject.name);
		GameObject otherObj = other.gameObject;
		EnemyCharacter enemy = otherObj.GetComponent<EnemyCharacter>();
		bool isPlayer = otherObj.CompareTag("Player");

		Rigidbody2D otherRb = otherObj.GetComponent<Rigidbody2D>();

		if (otherRb != null)
		{
			// 4️⃣ Apply force in your chosen direction
			if (enemy) {

				otherRb.AddForce(attackerVelocity.normalized * attackObjectForce / 200, ForceMode2D.Impulse);
			} else if (isPlayer) {
				//Assuming it's a player
				otherRb.AddForce(attackerVelocity.normalized * attackObjectForce, ForceMode2D.Impulse);
				HealthSystem healthSystem = otherObj.GetComponent<HealthSystem>();
				healthSystem.reciveDamage(damage);
				Destroy(gameObject);
			}
			Debug.Log("Applied force to " + otherObj.name);

			AudioSource.PlayClipAtPoint(audioClipImpactSoundEffect, this.transform.position);
		}
	}
	*/

	void OnTriggerEnter2D(Collider2D other)
	{
		GameObject oObj = other.gameObject;

		if (oObj.CompareTag("Player"))
		{
			PlayerController pc = PlayerController.pcInstance;
			Vector2 moveForce = rb.linearVelocity.normalized * 1.5f;
			pc.TakeDamage(damage, moveForce);
			Destroy(gameObject);
		}
		
		
		if (oObj.CompareTag("Bee"))
		{
			BeeObject bo = oObj.GetComponent<BeeObject>();
			BeeObject.MoveState ms =  bo.GetMoveState();
			if (ms != BeeObject.MoveState.FreeCloud &&
			    ms != BeeObject.MoveState.Returning)
			{
				Destroy(gameObject);
			}
		}
		
	}
}
