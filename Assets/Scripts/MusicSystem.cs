using UnityEngine;

public class MusicSystem : MonoBehaviour
{
	public AudioSource audioSource1;
	public AudioSource audioSource2;
	public AudioSource audioSource3;
	public AudioSource audioSource4;
	public AudioSource audioSource5;

	public AudioClip  gmLayer1;
	public AudioClip  gmLayer2;
	public AudioClip  gmLayer3;
	public AudioClip  gmLayer4;
	public AudioClip  gmLayer5;

	// Hard-coded template selector (Layer1..Layer5 | L1..L5 | 1..5)
	public string template = "Layer1";
	private string lastTemplate = "";

	// Target volumes (kept simple & explicit)
	private float targetVol1 = 1f;
	private float targetVol2 = 0f;
	private float targetVol3 = 0f;
	private float targetVol4 = 0f;
	private float targetVol5 = 0f;

	void Awake()
	{
		// Ensure 5 AudioSources exist
		AudioSource[] existingSources = GetComponents<AudioSource>();
		int countToAdd = 5 - existingSources.Length;
		for (int i = 0; i < countToAdd; i++)
		{
			gameObject.AddComponent<AudioSource>();
		}

		// Refresh list after adding missing sources
		existingSources = GetComponents<AudioSource>();

		// Assign to public variables
		if (existingSources.Length >= 5)
		{
			audioSource1 = existingSources[0];
			audioSource2 = existingSources[1];
			audioSource3 = existingSources[2];
			audioSource4 = existingSources[3];
			audioSource5 = existingSources[4];
		}
	}

	void Start()
	{
		// Hard-assign clips & loop flags
		if (audioSource1) { audioSource1.clip = gmLayer1; audioSource1.loop = true; audioSource1.playOnAwake = false; }
		if (audioSource2) { audioSource2.clip = gmLayer2; audioSource2.loop = true; audioSource2.playOnAwake = false; }
		if (audioSource3) { audioSource3.clip = gmLayer3; audioSource3.loop = true; audioSource3.playOnAwake = false; }
		if (audioSource4) { audioSource4.clip = gmLayer4; audioSource4.loop = true; audioSource4.playOnAwake = false; }
		if (audioSource5) { audioSource5.clip = gmLayer5; audioSource5.loop = true; audioSource5.playOnAwake = false; }

		// Start them all playing so we can crossfade by volume
		if (audioSource1 && audioSource1.clip) { audioSource1.volume = 1f; audioSource1.Play(); }
		if (audioSource2 && audioSource2.clip) { audioSource2.volume = 0f; audioSource2.Play(); }
		if (audioSource3 && audioSource3.clip) { audioSource3.volume = 0f; audioSource3.Play(); }
		if (audioSource4 && audioSource4.clip) { audioSource4.volume = 0f; audioSource4.Play(); }
		if (audioSource5 && audioSource5.clip) { audioSource5.volume = 0f; audioSource5.Play(); }

		// Default to Layer1
		TransitionToLayer1();
		lastTemplate = "Layer1";
	}

	void Update()
	{
		// React only when the template string changes
		if (template != lastTemplate)
		{
			switch (template)
			{
				case "Layer1":
				case "L1":
				case "1":
					TransitionToLayer1();
					break;
				case "Layer2":
				case "L2":
				case "2":
					TransitionToLayer2();
					break;
				case "Layer3":
				case "L3":
				case "3":
					TransitionToLayer3();
					break;
				case "Layer4":
				case "L4":
				case "4":
					TransitionToLayer4();
					break;
				case "Layer5":
				case "L5":
				case "5":
					TransitionToLayer5();
					break;
				default:
					// ignore unknown values
					break;
			}
			lastTemplate = template;
		}

		// Fade volumes every frame (simple hard-coded speed)
		float fadeUnitsPerSecond = 1.5f; // 0 -> 1 in ~0.66s
		float step = fadeUnitsPerSecond * Time.deltaTime;

		if (audioSource1) audioSource1.volume = Mathf.MoveTowards(audioSource1.volume, targetVol1, step);
		if (audioSource2) audioSource2.volume = Mathf.MoveTowards(audioSource2.volume, targetVol2, step);
		if (audioSource3) audioSource3.volume = Mathf.MoveTowards(audioSource3.volume, targetVol3, step);
		if (audioSource4) audioSource4.volume = Mathf.MoveTowards(audioSource4.volume, targetVol4, step);
		if (audioSource5) audioSource5.volume = Mathf.MoveTowards(audioSource5.volume, targetVol5, step);
	}

	public void TransitionToLayer1() {
		// Only layer 1 up, others down
		targetVol1 = 1f;
		targetVol2 = 0f;
		targetVol3 = 0f;
		targetVol4 = 0f;
		targetVol5 = 0f;

				// Ensure they're playing if clips were assigned (hard-coded safety)
		if (audioSource1 && audioSource1.clip && !audioSource1.isPlaying) audioSource1.Play();
		if (audioSource2 && audioSource2.clip && !audioSource2.isPlaying) audioSource2.Play();
		if (audioSource3 && audioSource3.clip && !audioSource3.isPlaying) audioSource3.Play();
		if (audioSource4 && audioSource4.clip && !audioSource4.isPlaying) audioSource4.Play();
		if (audioSource5 && audioSource5.clip && !audioSource5.isPlaying) audioSource5.Play();

	}

	public void TransitionToLayer2() {
		targetVol1 = 1f;
		targetVol2 = 1f;
		targetVol3 = 0f;
		targetVol4 = 0f;
		targetVol5 = 0f;

				// Ensure they're playing if clips were assigned (hard-coded safety)
		if (audioSource1 && audioSource1.clip && !audioSource1.isPlaying) audioSource1.Play();
		if (audioSource2 && audioSource2.clip && !audioSource2.isPlaying) audioSource2.Play();
		if (audioSource3 && audioSource3.clip && !audioSource3.isPlaying) audioSource3.Play();
		if (audioSource4 && audioSource4.clip && !audioSource4.isPlaying) audioSource4.Play();
		if (audioSource5 && audioSource5.clip && !audioSource5.isPlaying) audioSource5.Play();

	}

	public void TransitionToLayer3() {
		targetVol1 = 1f;
		targetVol2 = 1f;
		targetVol3 = 1f;
		targetVol4 = 0f;
		targetVol5 = 0f;

				// Ensure they're playing if clips were assigned (hard-coded safety)
		if (audioSource1 && audioSource1.clip && !audioSource1.isPlaying) audioSource1.Play();
		if (audioSource2 && audioSource2.clip && !audioSource2.isPlaying) audioSource2.Play();
		if (audioSource3 && audioSource3.clip && !audioSource3.isPlaying) audioSource3.Play();
		if (audioSource4 && audioSource4.clip && !audioSource4.isPlaying) audioSource4.Play();
		if (audioSource5 && audioSource5.clip && !audioSource5.isPlaying) audioSource5.Play();

	}

	public void TransitionToLayer4() {
		// Only layer 4 up
		targetVol1 = 1f;
		targetVol2 = 1f;
		targetVol3 = 1f;
		targetVol4 = 1f;
		targetVol5 = 0f;

				// Ensure they're playing if clips were assigned (hard-coded safety)
		if (audioSource1 && audioSource1.clip && !audioSource1.isPlaying) audioSource1.Play();
		if (audioSource2 && audioSource2.clip && !audioSource2.isPlaying) audioSource2.Play();
		if (audioSource3 && audioSource3.clip && !audioSource3.isPlaying) audioSource3.Play();
		if (audioSource4 && audioSource4.clip && !audioSource4.isPlaying) audioSource4.Play();
		if (audioSource5 && audioSource5.clip && !audioSource5.isPlaying) audioSource5.Play();

	}

	public void TransitionToLayer5() {
		// Play ALL gameplay layers at full volume
		targetVol1 = 1f;
		targetVol2 = 1f;
		targetVol3 = 1f;
		targetVol4 = 1f;
		targetVol5 = 1f;

		// Ensure they're playing if clips were assigned (hard-coded safety)
		if (audioSource1 && audioSource1.clip && !audioSource1.isPlaying) audioSource1.Play();
		if (audioSource2 && audioSource2.clip && !audioSource2.isPlaying) audioSource2.Play();
		if (audioSource3 && audioSource3.clip && !audioSource3.isPlaying) audioSource3.Play();
		if (audioSource4 && audioSource4.clip && !audioSource4.isPlaying) audioSource4.Play();
		if (audioSource5 && audioSource5.clip && !audioSource5.isPlaying) audioSource5.Play();
	}

	public void RunMusicTemplate(string template)
	{
		// Hard set the field so Update() switch will handle it
		this.template = template;
	}
}

public static class GetMusicSystem
{
	public static void PlayTemplate(string template) {
		 GameObject musicSystemObj = GameObject.FindWithTag("MusicSystem");

		MusicSystem musicSystem = musicSystemObj.GetComponent<MusicSystem>();
		 musicSystem.RunMusicTemplate(template);
	}
}
