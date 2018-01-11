using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {
	public int numStars;
	public int numStarsCollected;
	// Use this for initialization
	void Start () {
		numStars = 1;
		numStarsCollected = 0;
	}

	public void CollectStar(){
		numStarsCollected++;
		GameObject.Find ("ScoreText").GetComponent<Text> ().text = numStarsCollected + "/" + numStars;
	}
}
