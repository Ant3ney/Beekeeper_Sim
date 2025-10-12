using UnityEngine;

public class StartSceneDialogDriver : MonoBehaviour
{
	public string dialogToPlay = "My Dialog";
	public GameObject dialogGameObject;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		DialogSettings dialogSettings = new DialogSettings();
		dialogGameObject.SetActive(true);
		dialogGameObject.GetComponent<DialogSystem>().RunDialog(dialogToPlay, dialogSettings);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
