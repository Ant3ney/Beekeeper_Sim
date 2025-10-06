using UnityEngine;

public class DumbData : MonoBehaviour
{
	 [Header("Player Settings")]
	public float playerHealth = 100;
	public float playerMaxHealth = 100;
	public float playerMoveSpeed = 5f;
	public Vector3 playerScale = new Vector3(0.2f, 0.2f, 0.2f);
	public Sprite playerSprite;
	public Sprite playerAttackSprite;

	 [Header("Bee Settings")]
	public float spawnDelay = 0.001f;
	public int beeSpawnMultiplyer = 2;
	public Vector3 beeScale = new Vector3(0.5f, 0.51f, 0.51f);
	 public Sprite beeSprite;

	 [Header("Enemy Settings")]
	public Vector3 enemyScale = new Vector3(4f, 4f, 4f);
	public float enemyDamagePerSecond = 10;
	public float enemyTimerInterval = 3;
	public float enemyMaxSpawnTime = 150;
	public float enemyDelayTillSpawn = 2;
	 public Sprite enemySprite;


	 [Header("Music Settings")]
	 public AudioClip musicClip;
	 public bool loop = true;
	 public float MusicVolume = 0.5f;

	 private AudioSource audioSource;

	 [Header("Sound Settings")]
	 public AudioClip beeSound;
	 public float beeVolume = 0.25f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure and play the music
        audioSource.clip = musicClip;
        audioSource.loop = loop;
        audioSource.volume = MusicVolume;
        audioSource.spatialBlend = 0f; // 0 = 2D sound (not positional)

        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
