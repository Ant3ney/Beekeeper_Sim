using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{

	public float currentHealth = 100f;
	public float maxHealth = 100f;
	public bool isPlayer = false;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public RectTransform healthLeft;
	private float fullWidth;
	void Start()
	{
		 if(healthLeft) fullWidth = healthLeft.rect.width;
	}

	// Update is called once per frame
	void Update()
	{
		if(currentHealth <= 0) {
			if(isPlayer) {
				SceneManager.LoadScene("GameOver");
			} else {
				EnemyCharacter enemy = GetComponent<EnemyCharacter>();
				enemy.onDeath();
			}
		}

		if(healthLeft) {
			float percent = currentHealth / maxHealth;

			// Compute width relative to original full width
			Vector2 size = healthLeft.sizeDelta;
			size.x = fullWidth * percent;
			healthLeft.sizeDelta = size;
		}


	}

	public void reciveDamage(float dammage) {
		currentHealth -= dammage;
	}
}
