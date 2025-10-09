using UnityEngine;
using Unity.Behavior;

public class EnemyCharacter : MonoBehaviour
{
	MainEnemyState mainEnemyState = MainEnemyState.MovingIntoTokenRange;
	private BehaviorGraphAgent behaviorAgent;

	public GameObject player;         // Reference to the player
	public float approachDistance = 6f;
	private UnityEngine.AI.NavMeshAgent agent;
	int tokens = 0;
	public int tokenTickets = 1;
	// Start is called once before the first execution of Update after the MonoBehaviour is created

	// SetVariableValue

	public MainEnemyState state = MainEnemyState.MovingIntoTokenRange;

	void Awake() {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		// ✅ Stop the agent from rotating automatically
		agent.updateRotation = false;
		// Optional: if using 2D, stop movement on Y axis
		agent.updateUpAxis = false;
	}
	void Start()
	{
		if (player == null)
		{
			player = GameObject.FindWithTag("Player");
		}

		behaviorAgent = GetComponent<BehaviorGraphAgent>();

		
		GetTokenSystem.RegisterEnemy(this);
	}

	// Update is called once per frame
	void Update()
	{
		BlackboardVariable<MainEnemyState> newStateBlackboardVariable;
		behaviorAgent.BlackboardReference.GetVariable("Main Enemy State", out newStateBlackboardVariable);
		state = (MainEnemyState)newStateBlackboardVariable.ObjectValue;
		Debug.Log(newStateBlackboardVariable.ObjectValue);


	}

	float DistanceBetweenSelfAndPlayer() {
		if (player == null)
			return Mathf.Infinity;

		Vector2 selfPos = transform.position;
		Vector2 playerPos = player.transform.position;

		return Vector2.Distance(selfPos, playerPos);
	}

	public void Attacking(){

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
			behaviorAgent.BlackboardReference.SetVariableValue("Main Enemy State", (object)MainEnemyState.WaitingForToken);
		} else if ((DistanceBetweenSelfAndPlayer() > (approachDistance + 5)) && (state == MainEnemyState.WaitingForToken)) {
			behaviorAgent.BlackboardReference.SetVariableValue("Main Enemy State", (object)MainEnemyState.MovingIntoTokenRange);
		} else if (tokens > 0) {
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
			//Debug.LogWarning("Could not find navigable point near player.");
		}
	}


	bool TryGetNavigablePointNear(Vector3 playerPos, float desiredRange, out Vector3 result)
	{
		const int maxAttempts = 20;
		float halfRange = desiredRange * 0.5f;
		float traceRadius = 1.5f; // ← thickness of the line trace (tweak as needed)
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
}
