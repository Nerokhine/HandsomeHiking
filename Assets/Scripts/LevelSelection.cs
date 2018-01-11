using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class LevelSelection : MonoBehaviour {

	public GameObject levelPanel;
	public GameObject titlePanel;

	void Start(){
		#if UNITY_ADS
		Advertisement.Initialize ("1666836");
		#endif
	}

	public void ChangeToLevelSelect(){
		titlePanel.SetActive (false);
	}

	public void BackToTitle(){
		titlePanel.SetActive (true);
	}

	public void LoadLevel(int level){
		#if UNITY_ADS
		Advertisement.Show();
		#endif
		
		switch (level)
		{
		case 1:
			SceneManager.LoadScene ("Level1");
			break;
		case 2:
			SceneManager.LoadScene ("Level2");
			break;
		case 3:
			SceneManager.LoadScene ("Level3");
			break;
		case 4:
			SceneManager.LoadScene ("Level4");
			break;
		case 5:
			SceneManager.LoadScene ("Level5");
			break;
		case 6:
			SceneManager.LoadScene ("Level6");
			break;
		case 7:
			SceneManager.LoadScene ("Level7");
			break;
		case 8:
			SceneManager.LoadScene ("Level8");
			break;
		case 9:
			SceneManager.LoadScene ("Level9");
			break;
		default:
			SceneManager.LoadScene ("MainMenu");
			break;
		}
	}

}
