using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIDriver : MonoBehaviour
{

	public	GameObject Credits;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void LoadIntroAnimatic()
	{
		SceneManager.LoadScene("IntroAnimatic");
	}

	public void ShowCredits()
	{
		Credits.gameObject.SetActive(true);
	}

	public void HideCredits()
	{
		Credits.gameObject.SetActive(false);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

}
