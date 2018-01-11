using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {

	public GameObject pausePanel;

	public void OnPressPause(){
		Time.timeScale = 0;
		if (pausePanel.activeSelf) {
			OnPressResume ();
		} else {
			pausePanel.SetActive (true);
		}
	}

	public void OnPressResume(){
		Time.timeScale = 1;
		pausePanel.SetActive (false);
	}

	public void OnPressQuit(){
		Time.timeScale = 1;
		SceneManager.LoadScene ("MainMenu");
	}
}
