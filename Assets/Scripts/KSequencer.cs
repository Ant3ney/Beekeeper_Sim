using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class KSequence
{
	public int numEnemiesDead = 0;

	public bool playDialog = false;
	public string dialogSequenceToPlay = "";

	public bool playMusic = false;
	public bool playmusicAfterDialog = false;
	public string musicTemplateToPlay = "";

	public bool changeLevel = false;
	public string levelToChangeTo = "";
}

public class KSequencer : MonoBehaviour
{
	int numEnemiesDead = 0;
	int currentSequenceIndex = 0;
	[SerializeField] public List<KSequence> sequences = new List<KSequence>();
	 public List<DialogSystem> dialogSystems = new List<DialogSystem>();

	void Start() { }
	void Update() { }

	public void EnemyDied(){
		numEnemiesDead++;
		checkAndRunSequences();
	}

	void checkAndRunSequences()
	{
		KSequence sequenceToCheck = sequences[currentSequenceIndex];
		if (sequenceToCheck.numEnemiesDead > numEnemiesDead) return;
		
		currentSequenceIndex++;

		if(
			sequenceToCheck.playMusic && !sequenceToCheck.playmusicAfterDialog ||
			(sequenceToCheck.playMusic && !sequenceToCheck.playDialog)
		) {
			GetMusicSystem.PlayTemplate(sequenceToCheck.musicTemplateToPlay);
		}  

		if (sequenceToCheck.playDialog) {
			DialogSettings dialogSettings = new DialogSettings();
			dialogSettings.useKSequence = true;
			dialogSettings.kSequence = sequenceToCheck;
			Time.timeScale = 0;

			DialogSystem dialogSystem = dialogSystems[currentSequenceIndex - 1];
			dialogSystem.gameObject.SetActive(true);
			dialogSystem.RunDialog(sequenceToCheck.dialogSequenceToPlay, dialogSettings);

		} else if (sequenceToCheck.changeLevel) {
			SceneManager.LoadScene(sequenceToCheck.levelToChangeTo);
		}
	}

	public void postDialog() {
		KSequence sequenceToCheck = sequences[currentSequenceIndex];
		if (sequenceToCheck.playMusic && sequenceToCheck.playmusicAfterDialog) {
			GetMusicSystem.PlayTemplate(sequenceToCheck.musicTemplateToPlay);
		}

		Debug.Log("Made it to post");
		if(sequenceToCheck.changeLevel) {
			SceneManager.LoadScene(sequenceToCheck.levelToChangeTo);
		}
	}


}

public static class GetKSequencer
{
	public static void EnemyDied() {
		  GameObject kSequencerObj = GameObject.FindWithTag("Sequencer");
		  KSequencer sequencer = kSequencerObj.GetComponent<KSequencer>();
		  sequencer.EnemyDied();
	}
}
