using UnityEngine;

public class DialogDevEnvTestDriver : MonoBehaviour
{
	DialogSystem dialogSystem;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		DialogSettings dialogSettings = new DialogSettings();
		dialogSettings.onDialogEnded = onFinishedDialog;
		GetDialogSystem.RunDialog("Test Driver", dialogSettings);
	}

	// Update is called once per frame
	void Update()
	{

	}

	void onFinishedDialog() {
		Debug.Log("Finished dialog");
	}
}
