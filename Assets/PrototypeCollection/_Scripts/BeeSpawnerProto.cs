using UnityEngine;

public class BeeSpawnerProto : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public GameObject prefab;
	public int SpawnMultiplyer = 1;
	DumbData dumbData;
	public float spawnTimer = 0.02f;
	float timer = 0; 

	void Start()
	{
		dumbData = GameObject.Find("EasyData").GetComponent<DumbData>();
		SpawnMultiplyer = dumbData.beeSpawnMultiplyer;
		spawnTimer = dumbData.spawnDelay;
	}


	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;
		if (Input.GetMouseButton(0))
		{
			Vector3 mInput = Input.mousePosition;
			mInput.z = Mathf.Abs(Camera.main.transform.position.z);

			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mInput);
			mouseWorldPos.z = 0;

			if (prefab != null)
			{
				if(timer >= spawnTimer) {
					timer = 0;
					for(int i = 0; i < SpawnMultiplyer; i++) {
						Instantiate(prefab, mouseWorldPos, Quaternion.identity);
					}
				}
			}
		}
	}
}
