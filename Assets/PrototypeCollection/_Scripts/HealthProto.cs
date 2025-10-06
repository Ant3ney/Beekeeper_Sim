using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthProto : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public float health = 100;
	public float maxHealth = 100;
	// float healthRegen = 30;
	//public float healthRegenDelay = 2;
	string stage = "healed";
	public RectTransform healthLeft;
	private float fullWidth;

	DumbData dumbData;

    void Start()
    {
         if(healthLeft) fullWidth = healthLeft.rect.width;
	    dumbData = GameObject.Find("EasyData").GetComponent<DumbData>();

	PlayerCharacterProto player = GetComponent<PlayerCharacterProto>();
	if (player) {

		health = dumbData.playerHealth;
		maxHealth = dumbData.playerMaxHealth;
	}

    }

    // Update is called once per frame
    void Update()
    {
	    if(healthLeft) {
		    float percent = health / maxHealth;

		    // Compute width relative to original full width
		    Vector2 size = healthLeft.sizeDelta;
		    size.x = fullWidth * percent;
		    healthLeft.sizeDelta = size;
	    }
    }

    public void ReciveDamage(float damage) {
	    Debug.Log("Damage: " + damage);
	health -= damage;
	if(health > 0) stage = "recentDamage";
	else { 
		stage = "dead";

			health = 0;
		PlayerCharacterProto player = GetComponent<PlayerCharacterProto>();
		if (player) {
			SceneManager.LoadScene("GameOver");
			return;
		}

		EnemyProto enemy = GetComponent<EnemyProto>();
		if(enemy) {
			    Destroy(gameObject);
			    return;
		}

	}

    }
}
