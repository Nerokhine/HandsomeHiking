using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {
	int numStars;
	int numStarsCollected;
	public GameObject winPanel;
	public GameObject blurPanel;
	// Use this for initialization
	void Start () {
		numStars = 1;
		numStarsCollected = 0;
	}

	public void CollectStar(){
		numStarsCollected++;
		GameObject.Find ("ScoreText").GetComponent<Text> ().text = numStarsCollected + "/" + numStars;
		if (numStars == numStarsCollected) {
			ShowWinPanel ();
		}
	}

	void ShowWinPanel(){
		winPanel.SetActive (true);
		blurPanel.SetActive (true);
	}

	public void Continue(){
		SceneManager.LoadScene ("MainMenu");
	}
}
