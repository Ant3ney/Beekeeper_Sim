using UnityEngine;
using Unity.Behavior;

public class EnemyCharacter : MonoBehaviour
{
	MainEnemyState mainEnemyState = MainEnemyState.MovingIntoTokenRange;
	private BehaviorGraphAgent behaviorAgent;

	public GameObject player;         // Reference to the player
	public float approachDistance = 6f;
	public float attackDistance = 1.5f;
	private UnityEngine.AI.NavMeshAgent agent;
	int tokens = 0;
	public int tokenTickets = 1;
	// Start is called once before the first execution of Update after the MonoBehaviour is created

	// SetVariableValue

	public MainEnemyState state = MainEnemyState.MovingIntoTokenRange;
	public AttackState attackState = AttackState.Lunging;

	Animator animator;

	public GameObject childObject;

	public GameObject attackObject;
	public float traceRadius = 1.5f;

	Vector3 positionLastFrame;
	Vector3 positionThisFrame;
	Vector3 attackVelocityDir;

	public float AttackObjectReach = 3f;

	Vector3 playerPosUpdated;
	void Awake() {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		// ✅ Stop the agent from rotating automatically
		agent.updateRotation = false;
		// Optional: if using 2D, stop movement on Y axis
		agent.updateUpAxis = false;
	}
	void Start()
	{
		animator = childObject.GetComponent<Animator>();
		if (player == null)
		{
			player = GameObject.FindWithTag("Player");
		}

		behaviorAgent = GetComponent<BehaviorGraphAgent>();

		
		GetTokenSystem.RegisterEnemy(this);

		positionThisFrame = this.transform.position;
		positionLastFrame = this.transform.position;

	}

	// Update is called once per frame
	void Update()
	{
		BlackboardVariable<MainEnemyState> newStateBlackboardVariable;
		behaviorAgent.BlackboardReference.GetVariable("Main Enemy State", out newStateBlackboardVariable);
		state = (MainEnemyState)newStateBlackboardVariable.ObjectValue;

		BlackboardVariable<AttackState> newAttackStateBlackboardVariable;
		behaviorAgent.BlackboardReference.GetVariable("Attack State", out newAttackStateBlackboardVariable);
		attackState = (AttackState)newAttackStateBlackboardVariable.ObjectValue;

		Debug.Log(newStateBlackboardVariable.ObjectValue);
		playerPosUpdated = player.transform.position;
	}

	float DistanceBetweenSelfAndPlayer() {
		if (player == null)
			return Mathf.Infinity;

		Vector2 selfPos = transform.position;
		Vector2 playerPos = player.transform.position;

		return Vector2.Distance(selfPos, playerPos);
	}

	public void Attacking(){
		positionLastFrame = positionThisFrame;
		positionThisFrame = this.transform.position;
		Vector3 delta = positionThisFrame - positionLastFrame;


		float distanceToPlayer = DistanceBetweenSelfAndPlayer();
		if (attackState == AttackState.Lunging && (attackDistance <= distanceToPlayer - 0.5f)) {
			Transform playerLocation = player.transform;
			agent.SetDestination(playerLocation.position);
			if (delta != Vector3.zero)
				attackVelocityDir = delta.normalized; // Unit vector (magnitude = 1)
			else
				attackVelocityDir = Vector3.zero;

		} else if (attackState == AttackState.Lunging && (attackDistance > distanceToPlayer - 0.75f)) {
			Engage();	
			behaviorAgent.BlackboardReference.SetVariableValue("Attack State", (object)AttackState.Engaging);
		}
	}

	public void SpawnAttackObject() {
		Vector3 playerPos = playerPosUpdated;
		
		Vector3 fromPlayerToEnemy = (playerPos - this.transform.position).normalized;
		Vector3 spawnPos = this.transform.position + (fromPlayerToEnemy * AttackObjectReach);

		// 2. Spawn the prefab
		GameObject spawnedAttackObject = Instantiate(attackObject, spawnPos, transform.rotation);
		AttackObject ao = spawnedAttackObject.GetComponent<AttackObject>();
		ao.attackerVelocity = fromPlayerToEnemy;
	}

	public void FinishedEngagement() {

		behaviorAgent.BlackboardReference.SetVariableValue("Attack State", (object)AttackState.Lunging);
		behaviorAgent.BlackboardReference.SetVariableValue("Main Enemy State", (object)MainEnemyState.MovingIntoTokenRange);
		//MoveNearPlayer();
		//

		// 🧩 Clear out any residual path data
		agent.ResetPath();

		// 🧩 Re-enable movement
		agent.isStopped = false;
		agent.updatePosition = true;
		agent.updateRotation = false; // since you disabled it in Awake
		agent.updateUpAxis = false;

		// 🧩 Force a re-evaluation of move behavior
		MoveNearPlayer();
	}

	public void Engage() {
		animator.SetTrigger("Attack");
		//Run Attack Anim
	}

	public void AwwardToken(int ammount) {
		tokens += ammount;
	}

	public int getTokenLotteryTicketNum() {
		int numTickets = tokenTickets - tokens;
		if(numTickets < 0) return 0;
		return numTickets;
	}

	public void MoveIntoTokenAttackRangeAction() {
		//Debug.Log("Could not find navigable point near player.");
		if (!agent.pathPending && agent.remainingDistance < 2f)
		{
			MoveNearPlayer();
		}

		if ((DistanceBetweenSelfAndPlayer() <= (approachDistance + 3)) && (state == MainEnemyState.MovingIntoTokenRange)) {
			//Debug.Log($"{gameObject.name} switching to MovingIntoTokenRange. Distance = {DistanceBetweenSelfAndPlayer():F2}, Current State = {state}");
			behaviorAgent.BlackboardReference.SetVariableValue("Main Enemy State", (object)MainEnemyState.WaitingForToken);
		} else if ((DistanceBetweenSelfAndPlayer() > (approachDistance + 5)) && (state == MainEnemyState.WaitingForToken)) {
			behaviorAgent.BlackboardReference.SetVariableValue("Main Enemy State", (object)MainEnemyState.MovingIntoTokenRange);
		} else if (tokens > 0) {
			tokens--;
			behaviorAgent.BlackboardReference.SetVariableValue("Main Enemy State", (object)MainEnemyState.Attacking);
		}

	}
	


	void MoveNearPlayer()
	{
		if (player == null || agent == null) return;

		Vector3 playerPos = player.transform.position;

		// ✅ Try to find a valid point near the player
		Vector3 targetPoint;
		if (TryGetNavigablePointNear(playerPos, approachDistance, out targetPoint))
		{
			agent.SetDestination(targetPoint);
			//Debug.Log($"{name} moving near player at {targetPoint}");
		}
		else
		{
			MoveNearPlayerFallback();
		}
	}


	bool TryGetNavigablePointNear(Vector3 playerPos, float desiredRange, out Vector3 result)
	{
		const int maxAttempts = 20;
		float halfRange = desiredRange * 0.5f;
		//traceRadius = 1.5f; // ← thickness of the line trace (tweak as needed)
		LayerMask playerMask = LayerMask.GetMask("Player"); // ensure your player is on the "Player" layer

		for (int i = 0; i < maxAttempts; i++)
		{
			// 1️⃣ Random point around the player on the XY plane
			Vector2 random2D = Random.insideUnitCircle.normalized * Random.Range(halfRange, desiredRange);
			Vector3 candidate = new Vector3(playerPos.x + random2D.x, playerPos.y + random2D.y, playerPos.z);

			// 2️⃣ Ensure candidate is on the NavMesh
			if (!UnityEngine.AI.NavMesh.SamplePosition(candidate, out UnityEngine.AI.NavMeshHit hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
				continue;

			Vector3 candidatePoint = hit.position;

			// 3️⃣ Must be within desired range of enemy
			float distance = Vector3.Distance(transform.position, candidatePoint);
			//if (distance > desiredRange)
			//   continue;

			// 4️⃣ Thick line trace (CircleCast) between enemy and candidate
			Vector2 start2D = transform.position;
			Vector2 direction = (candidatePoint - transform.position).normalized;
			float distanceToPoint = Vector2.Distance(start2D, candidatePoint);

			// Do a circle cast with a visible "thickness"
			RaycastHit2D hitInfo = Physics2D.CircleCast(start2D, traceRadius, direction, distanceToPoint, playerMask);
			if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player"))
			{
				// The player blocks the path — try another candidate
				continue;
			}

			if (hitInfo.collider != null)
			{
				//Debug.Log($"Hit: {hitInfo.collider.name} at {hitInfo.point}, layer: {LayerMask.LayerToName(hitInfo.collider.gameObject.layer)}");

				if (hitInfo.collider.CompareTag("Player"))
				{
					//Debug.Log("❌ Path blocked by player");
					continue; // skip this point
				}
			}

			// 5️⃣ Valid point found
			result = candidatePoint;

			// Optional: debug visualization
			Debug.DrawLine(transform.position, candidatePoint, Color.green, 1f);
			Debug.DrawRay(start2D, direction * distanceToPoint, Color.cyan, 1f);
			return true;
		}

		// 6️⃣ Fallback: no valid point found
		result = transform.position;
		return false;
	}


	void MoveNearPlayerFallback()
	{
		if (player == null || agent == null) return;

		Vector3 playerPos = player.transform.position;

		// ✅ Try to find a valid point near the player
		Vector3 targetPoint;
		if (TryGetNavigablePointNear(playerPos, approachDistance, out targetPoint))
		{
			agent.SetDestination(targetPoint);
			Debug.Log($"{name} moving near player at {targetPoint}");
			return;
		}

		// ⚠️ Could not find a nearby navigable point, so fallback
		Debug.LogWarning($"{name} could not find navigable point near player. Trying to move away...");

		Vector3 awayDir = (transform.position - playerPos).normalized;
		const int maxAttempts = 15;
		float searchRadius = approachDistance;
		bool found = false;
		Vector3 chosenPoint = transform.position;

		for (int i = 0; i < maxAttempts; i++)
		{
			// 🌀 Add some random spread while keeping overall "away from player" direction
			Vector2 randomOffset2D = Random.insideUnitCircle * (searchRadius * 0.5f);
			Vector3 randomOffset3D = new Vector3(randomOffset2D.x, 0f, randomOffset2D.y);

			// Base away direction, with slight randomness
			Vector3 candidate = transform.position + awayDir * searchRadius + randomOffset3D;

			UnityEngine.AI.NavMeshHit hit;
			if (UnityEngine.AI.NavMesh.SamplePosition(candidate, out hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
			{
				agent.SetDestination(hit.position);
				chosenPoint = hit.position;
				found = true;
				Debug.Log($"{name} fallback moving AWAY from player to random valid point {hit.position}");
				break;
			}
		}

		if (!found)
		{
			Debug.LogWarning($"{name} could not find *any* navigable point away from player after {maxAttempts} attempts.");
			agent.ResetPath();
		}
	}

}
