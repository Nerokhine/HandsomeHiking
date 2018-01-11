using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {

	public GameObject pausePanel;
	public GameObject blurPanel;

	public void OnPressPause(){
		Time.timeScale = 0;
		if (pausePanel.activeSelf) {
			OnPressResume ();
		} else {
			pausePanel.SetActive (true);
			blurPanel.SetActive (true);
		}
	}

	public void OnPressResume(){
		Time.timeScale = 1;
		pausePanel.SetActive (false);
		blurPanel.SetActive (false);
	}

	public void OnPressQuit(){
		Time.timeScale = 1;
		SceneManager.LoadScene ("MainMenu");
	}
}
