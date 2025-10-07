using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseDriver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool isPaused = false;

    public GameObject PausedUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape)) // 0 = left mouse button
		{
			TogglePause();
		}
    }

    public void ExitToMainMenu() {
    		SceneManager.LoadScene("MainMenu");
    }

    void TogglePause() {
	    isPaused = !isPaused;

	    if (isPaused) {
			Time.timeScale = 0;
			PausedUI.gameObject.SetActive(true);
	    } else {
			Time.timeScale = 1;
			PausedUI.gameObject.SetActive(false);
	    }

    }


}
