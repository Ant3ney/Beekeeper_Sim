using UnityEngine;

public class StartSceneDialogDriver : MonoBehaviour
{
	public string dialogToPlay = "My Dialog";
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		DialogSettings dialogSettings = new DialogSettings();
		GetDialogSystem.RunDialog(dialogToPlay, dialogSettings);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
