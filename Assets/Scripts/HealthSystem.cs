using System.Collections;
using System.Collections.Generic;
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
	
	//for enemies
	private SpriteRenderer myRenderer;
	private Color redTint = new Color(0.752f, 0.12f, 0.23f);
	public float flashRedTime = 0.25f;
	public float flashRedTimer = 0.0f;
	private bool isFlashingRed = false;
	void Start()
	{
		 if(healthLeft) fullWidth = healthLeft.rect.width;
		 myRenderer = GetComponent<SpriteRenderer>();
		 if (!myRenderer) myRenderer = GetComponentInChildren<SpriteRenderer>();
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
		Debug.Log("got hit");
		if(!isFlashingRed && myRenderer) StartCoroutine(FlashRed());
	}

	public IEnumerator FlashRed()
	{
		flashRedTimer = 0.0f;
		isFlashingRed = true;

		float halfTime = flashRedTime / 2f;
		while (flashRedTimer <= halfTime)
		{
			myRenderer.color = Color.Lerp(Color.white, redTint, flashRedTimer / halfTime);
			flashRedTimer += Time.fixedDeltaTime;
			yield return null;
		}

		while (flashRedTimer <= flashRedTime)
		{
			myRenderer.color = Color.Lerp(redTint, Color.white,
				(flashRedTimer - halfTime) / halfTime);
			flashRedTimer += Time.fixedDeltaTime;
			yield return null;
		}

		myRenderer.color = Color.white;
		flashRedTimer = 0.0f;
		isFlashingRed = false;
	}
}
