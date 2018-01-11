using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLogic : MonoBehaviour {

	public static bool hasLoadedBefore = false;
	public GameObject titlePanel;

	public void Start(){
		if (hasLoadedBefore) {
			titlePanel.SetActive (false);
		} else {
			hasLoadedBefore = true;
		}
	}
}
