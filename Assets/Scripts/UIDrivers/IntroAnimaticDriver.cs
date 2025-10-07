using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroAnimaticDriver : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		//Intro Animatic        

		DialogSettings dialogSettings = new DialogSettings();
		dialogSettings.onDialogEnded = onFinishedDialog;
		GetDialogSystem.RunDialog("Intro Animatic", dialogSettings);
	}

	// Update is called once per frame
	void Update()
	{

	}

	void onFinishedDialog() {
		//Change level to prototype

		SceneManager.LoadScene("Tutorial");
	}

}
