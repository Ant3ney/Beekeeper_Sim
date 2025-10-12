using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class TokenSystem : MonoBehaviour
{
	public float ticketRewardDelay = 0.5f;
	float timmer = 0;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public List<EnemyCharacter> enemyRegistry = new List<EnemyCharacter>();
	public static TokenSystem tsInstance;
	public Dictionary<GameObject, HealthSystem> enemyHealths = new Dictionary<GameObject, HealthSystem>();

	void Awake()
	{
		if(tsInstance == null) tsInstance = this;
	}

	// Update is called once per frame
	void Update()
	{
		timmer += Time.deltaTime;
		if(timmer >= ticketRewardDelay) {
			timmer = 0;
			//Debug.Log("Token Update");
			TokenTic();
		}
	}

	void TokenTic() {
		List<EnemyCharacter> enemyLotteryTickets = MakeEnemyLotteryTickets();
		if (enemyLotteryTickets.Count > 0)
		{
			int randomIndex = Random.Range(0, enemyLotteryTickets.Count); // upper bound is exclusive
			EnemyCharacter randomEnemy = enemyLotteryTickets[randomIndex];
			randomEnemy.AwwardToken(1);
			//Debug.Log("Given Token Token side");
		}
		else
		{
			//Debug.LogWarning("enemyLotteryTickets is empty!");
		}

	}

	List<EnemyCharacter> MakeEnemyLotteryTickets(){
		List<EnemyCharacter> enemyLotteryTickets = new List<EnemyCharacter>();
		if(enemyRegistry.Count <= 0) {
			//Debug.LogWarning("enemy Token registry empty!");
		}
		for (int i = 0; i < enemyRegistry.Count; i++)
		{
			EnemyCharacter enemy = enemyRegistry[i];
			int lotteryTickets = enemy.getTokenLotteryTicketNum();
			if (enemy.state == MainEnemyState.WaitingForToken) {
				//Debug.LogWarning("enemy is waiting for token!");
				for (int j = 0; j < lotteryTickets; j++) {
					enemyLotteryTickets.Add(enemy);
				}
			}
			else {
				
				//Debug.LogWarning("registerd enemy is not waiting for token!");
			}
		}
		return enemyLotteryTickets;
	}

	public void RegisterEnemy(EnemyCharacter enemy) {
		if (!enemyRegistry.Contains(enemy)) {
			//Debug.LogWarning("Truly added enemy  registry !");
			enemyRegistry.Add(enemy);
		} else {
			//Debug.LogWarning("Faild to add to registry!");
		}

		HealthSystem enemyHealth = enemy.GetComponent<HealthSystem>();
		enemyHealths.Add(enemy.gameObject, enemyHealth);
	}
	public void UnregisterEnemy(EnemyCharacter enemy)
	{
		enemyRegistry.Remove(enemy);
		enemyHealths.Remove(enemy.gameObject);
		
	}
}


public static class GetTokenSystem {

	public static void RegisterEnemy(EnemyCharacter enemy) {
		TokenSystem tokenSystem = GameObject.Find("TokenSystem").GetComponent<TokenSystem>();
		//Debug.Log("enemy registered");
		tokenSystem.RegisterEnemy(enemy);
	}
	public static void UnregisterEnemy(EnemyCharacter enemy) {
		TokenSystem tokenSystem = GameObject.Find("TokenSystem").GetComponent<TokenSystem>();
		tokenSystem.UnregisterEnemy(enemy);
	}
}
