using UnityEngine;

public class EnemySpawnerProto : MonoBehaviour
{
	public GameObject Enemy;

	public float TimerInterval = 3;

	public float MaxSpawnTime = 150;
	public float DelayTillSpawn = 2;
	
	float Timer = 0;
	string Stage = "delay";
			DumbData dumbData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
	    dumbData = GameObject.Find("EasyData").GetComponent<DumbData>();

	    TimerInterval = dumbData.enemyTimerInterval;
	    MaxSpawnTime = dumbData.enemyMaxSpawnTime;
		    DelayTillSpawn = dumbData.enemyDelayTillSpawn;


    }

    // Update is called once per frame
    void Update()
    {
       Timer += Time.deltaTime; 
       if(Stage == "delay") {
	       if(Timer >= DelayTillSpawn) {
			Timer = 0;
			Stage = "spawn";
	       }
       }

       if(Stage == "spawn") {
		if(Timer >= TimerInterval) {
			Timer = 0;
		    var spawned = Instantiate(Enemy, transform.position, Quaternion.identity);
		    spawned.transform.localScale = Vector3.one * 5;
		}

       }
    }
}
