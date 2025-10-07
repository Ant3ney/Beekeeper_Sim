using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

[System.Serializable]
public struct DialogEntry
{
	public string ActiveSpeakerName;
	public string dialogText;
	public Sprite LeftSubjectSprite;
	public Sprite RightSubjectSprite;
	public bool CarryOverLastEntry;
	public AudioClip Voice;

	public bool overideBackground;
	public bool hideLeftSubject;
	public bool hideRightSubject;
	public bool hideSpeakerPanelAndText;
	public bool hideDialogPanel;
	public Sprite backgroundOveride;
}

[System.Serializable]
public struct DialogSequence
{
	public string Identifier;
	public DialogEntry[] DialogEntries;
}

public struct DialogSettings
{
	public Action  onDialogEnded;
}

[System.Serializable]
public class ObjectsAndComponents
{
	public ObjectsAndComponents(GameObject parent)
	{
		this.parent = parent;
		defineObjectsAndComponents();
	}

	public GameObject parent;
	public GameObject optionalBackgroundObj;
	public GameObject subjectObj;
	public GameObject dialogPanelObj;
	public GameObject speakerNameObj;
	public GameObject leftSpeakerObj;
	public GameObject rightSpeakerObj;
	public GameObject speakerPanelObj;

	public Image backgroundImage;
	public Image leftSpeakerImage;
	public Image rightSpeakerImage;
	public TextMeshProUGUI speakerName;
	public TextMeshProUGUI dialog;

	void defineObjectsAndComponents() {
		GameObject dialogTextObj = GameObject.FindGameObjectsWithTag("dialog_text_obj")[0];
		dialog = dialogTextObj.GetComponent<TextMeshProUGUI>();
		leftSpeakerObj = GameObject.FindGameObjectsWithTag("left_speaker_obj")[0];
		leftSpeakerImage = leftSpeakerObj.GetComponent<Image>();
		rightSpeakerObj = GameObject.FindGameObjectsWithTag("right_speaker_obj")[0];
		rightSpeakerImage = rightSpeakerObj.GetComponent<Image>();
		speakerName = GameObject.FindGameObjectsWithTag("speaker_text_obj")[0].GetComponent<TextMeshProUGUI>();
		optionalBackgroundObj = GameObject.FindGameObjectsWithTag("optional_background_obj")[0];
		backgroundImage = GameObject.FindGameObjectsWithTag("optional_background_obj")[0].GetComponent<Image>();
		dialogPanelObj = GameObject.FindGameObjectsWithTag("dialog_panel_obj")[0];
		speakerPanelObj = GameObject.FindGameObjectsWithTag("speaker_panel_obj")[0];
	}

}


public class DialogSystem : MonoBehaviour
{
	// Keep the serialized array private so Unity doesn’t eagerly deserialize on background thread
	[SerializeField] private DialogSequence[] dialogSequencesSerialized;

	// Cache to safely use after scene load
	private DialogSequence[] dialogSequences;

	// Flag to ensure initialization happens once
	private bool initialized;

	DialogSequence currentSequence = new DialogSequence();
	DialogEntry currentEntry = new DialogEntry();
	int cI = 0;

	ObjectsAndComponents objectsAndComponents;

	AudioSource audioSource;
	DialogSettings dialogSettings;

	void Awake()
	{
		// ✅ All Unity object access happens on the main thread here
		InitializeDialogSequences();
	}

	private void InitializeDialogSequences()
	{
		if (initialized) return;

		// Copy serialized data into runtime-safe storage
		dialogSequences = dialogSequencesSerialized;

		// At this point, Unity has already fully loaded the scene
		// so it’s safe to touch any UnityEngine.Object (Sprite, AudioClip, etc.)
		foreach (var sequence in dialogSequences)
		{
			foreach (var entry in sequence.DialogEntries)
			{
				// These lines are safe now
				string speakerName = entry.ActiveSpeakerName;
				Sprite leftSprite = entry.LeftSubjectSprite;
				AudioClip clip = entry.Voice;

				// Optional sanity check (safe now)
				if (clip != null)
					Debug.Log($"Loaded clip for {speakerName}: {clip.name}");
			}
		}

		initialized = true;

	}

	void Start()
	{
		// You can now safely use dialogSequences here

		//objectsAndComponents.dialog.text = "This is the test dialog";
		audioSource = GetComponent<AudioSource>();
	}

	void Update() {
		if (Input.GetMouseButtonUp(0)) // 0 = left mouse button
		{
			NextDialog();
		}
	}


	public void RunDialog(string identifier, DialogSettings inDialogSettings = default) {
		objectsAndComponents = new ObjectsAndComponents(this.gameObject);
		audioSource = GetComponent<AudioSource>();
		dialogSettings = inDialogSettings;
		Time.timeScale = 0;
		//stop time
		currentSequence = new DialogSequence
		{
			DialogEntries = new DialogEntry[0],
			Identifier = "Default"
		};

		for(int i = 0; i < dialogSequences.Length; i++) {
			if(dialogSequences[i].Identifier ==  identifier) {
				currentSequence = dialogSequences[i];
				currentEntry = currentSequence.DialogEntries[0];
				cI = 0;
				RunEntry(currentEntry);
				return;
			}
		}
	}

	public void RunEntry(DialogEntry dialogEntry) {
		objectsAndComponents.dialog.text = dialogEntry.dialogText;
		objectsAndComponents.speakerName.text = dialogEntry.ActiveSpeakerName;
		objectsAndComponents.leftSpeakerImage.sprite = dialogEntry.LeftSubjectSprite;
		objectsAndComponents.rightSpeakerImage.sprite = dialogEntry.RightSubjectSprite;

		if(dialogEntry.overideBackground) {
			objectsAndComponents.optionalBackgroundObj.gameObject.SetActive(true);
			objectsAndComponents.backgroundImage.sprite = dialogEntry.backgroundOveride;
		} else {
			objectsAndComponents.optionalBackgroundObj.gameObject.SetActive(false);
		}

		if(dialogEntry.hideLeftSubject) {
			objectsAndComponents.leftSpeakerObj.gameObject.SetActive(false);
		} else {
			objectsAndComponents.leftSpeakerObj.gameObject.SetActive(true);
		}

		if(dialogEntry.hideRightSubject) {
			objectsAndComponents.rightSpeakerObj.gameObject.SetActive(false);
		} else {
			objectsAndComponents.rightSpeakerObj.gameObject.SetActive(true);
		}

		if (dialogEntry.hideDialogPanel)
		{
			Image img = objectsAndComponents.dialogPanelObj.GetComponent<Image>();
			Color c = img.color;
			c.a = 0f;          // make transparent
			img.color = c;     // reassign modified color
		}
		else
		{
			Image img = objectsAndComponents.dialogPanelObj.GetComponent<Image>();
			Color c = img.color;
			c.a = 1f;          // make opaque
			img.color = c;
		}


		if(dialogEntry.hideSpeakerPanelAndText) {
			objectsAndComponents.speakerPanelObj.gameObject.SetActive(false);
		} else {
			objectsAndComponents.speakerPanelObj.gameObject.SetActive(true);
		}

		audioSource.Stop();
		if(dialogEntry.Voice) {

			audioSource.clip = dialogEntry.Voice;
			audioSource.Play();
		}
	}

	public void NextDialog() {
		cI++;
		if (cI >= currentSequence.DialogEntries.Length) {
			Time.timeScale = 1;
			HideDialog();
			dialogSettings.onDialogEnded();
			return;
		}
		currentEntry = currentSequence.DialogEntries[cI];
		RunEntry(currentEntry);
	}

	public void HideDialog() {
		gameObject.SetActive(false);
	}

}
