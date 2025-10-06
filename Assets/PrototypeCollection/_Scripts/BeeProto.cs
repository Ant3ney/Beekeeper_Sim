using UnityEngine;

public class BeeProto : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float Timer = 2;
    float current = 0;

    public float damagePerSecond = 50;
    	DumbData dumbData;
	    private SpriteRenderer spriteRenderer;
    void Start()
    {
	    	    dumbData = GameObject.Find("EasyData").GetComponent<DumbData>();

	    if (dumbData.beeSound != null)
	    {
		    // Play the sound at this GameObject's position
// Create a temporary GameObject manually (like PlayClipAtPoint does internally)
            GameObject tempAudio = new GameObject("TempBeeAudio");
            tempAudio.transform.position = transform.position;

            // Add an AudioSource and configure it
            AudioSource source = tempAudio.AddComponent<AudioSource>();
            source.clip = dumbData.beeSound;
            source.volume = dumbData.beeVolume;
            source.spatialBlend = 1f; // 3D sound
            source.pitch = Random.Range(0.8f, 1.2f); // 🎵 random pitch variation
            source.Play();

            // Destroy automatically after the sound finishes (pitch affects duration)
            Destroy(tempAudio, dumbData.beeSound.length / source.pitch);
	    }
	    else
	    {
		    Debug.LogWarning("Bee sound not assigned on " + gameObject.name);
	    }       

	    transform.localScale = dumbData.beeScale;

	    spriteRenderer = GetComponent<SpriteRenderer>();

	spriteRenderer.sprite = dumbData.beeSprite;

    }

    // Update is called once per frame
    void Update()
    {
	    current += 1 * Time.deltaTime;

       if(current > Timer)
       {
		Destroy(gameObject);
       }
    }
}
