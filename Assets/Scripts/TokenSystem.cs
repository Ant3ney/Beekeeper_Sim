using System.Collections.Generic;
using UnityEngine;

public class TokenSystem : MonoBehaviour
{
	public float ticketRewardDelay = 0.5f;
	float timmer = 0;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	List<EnemyCharacter> enemyRegistry = new List<EnemyCharacter>();
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		timmer += Time.deltaTime;
		if(timmer >= ticketRewardDelay) {
			timmer = 0;
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
		}
		else
		{
			//Debug.LogWarning("enemyLotteryTickets is empty!");
		}

	}

	List<EnemyCharacter> MakeEnemyLotteryTickets(){
		List<EnemyCharacter> enemyLotteryTickets = new List<EnemyCharacter>();
		for (int i = 0; i < enemyRegistry.Count; i++)
		{
			EnemyCharacter enemy = enemyRegistry[i];
			int lotteryTickets = enemy.getTokenLotteryTicketNum();
			if (enemy.state == MainEnemyState.WaitingForToken) {
				for (int j = 0; j < lotteryTickets; j++) {
					enemyLotteryTickets.Add(enemy);
				}
			}
		}
		return enemyLotteryTickets;
	}

	public void RegisterEnemy(EnemyCharacter enemy) {
		if (!enemyRegistry.Contains(enemy))
			enemyRegistry.Add(enemy);
	}
	public void UnregisterEnemy(EnemyCharacter enemy) {
		enemyRegistry.Remove(enemy);
	}
}


public static class GetTokenSystem {

	public static void RegisterEnemy(EnemyCharacter enemy) {
		TokenSystem tokenSystem = GameObject.Find("TokenSystem").GetComponent<TokenSystem>();
		tokenSystem.RegisterEnemy(enemy);
	}
	public static void UnregisterEnemy(EnemyCharacter enemy) {
		TokenSystem tokenSystem = GameObject.Find("TokenSystem").GetComponent<TokenSystem>();
		tokenSystem.UnregisterEnemy(enemy);
	}
}
