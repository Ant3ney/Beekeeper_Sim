using UnityEngine;

public class PlayerCharacterProto : MonoBehaviour
{

		DumbData dumbData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
	    dumbData = GameObject.Find("EasyData").GetComponent<DumbData>();

	 transform.localScale =	dumbData.playerScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        void OnCollisionStay2D(Collision2D collision)
    {
	    GameObject otherObject = collision.gameObject;
	    EnemyProto Enemy = otherObject.GetComponent<EnemyProto>();

	    if (Enemy) {
		    float damagePerSecond = Enemy.damagePerSecond;
		    float DamageThisFrame =  damagePerSecond * Time.fixedDeltaTime;
		    HealthProto healthProto = GetComponent<HealthProto>();

		    healthProto.ReciveDamage(DamageThisFrame);
	    }


	    //Debug.Log("Collided with: " + otherObject.name);
    }

}
