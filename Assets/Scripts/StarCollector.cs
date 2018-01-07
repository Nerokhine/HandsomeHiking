using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollector : MonoBehaviour {
	public bool collected = false;

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.name == "Blob" || collider.gameObject.name == "Hand") {
			collected = true;
			GetComponent<SpriteRenderer> ().enabled = false;
			GetComponent<PolygonCollider2D> ().enabled = false;
		}
	}
}
