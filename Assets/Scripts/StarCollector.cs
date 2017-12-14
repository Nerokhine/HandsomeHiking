using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollector : MonoBehaviour {
	public static bool collected = false;
	PolygonCollider2D blobCollider;
	PolygonCollider2D handCollider;
	// Use this for initialization
	void Start () {
		blobCollider = GameObject.Find ("Blob").GetComponent<PolygonCollider2D> ();
		handCollider = GameObject.Find ("Hand").GetComponent<PolygonCollider2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (GetComponent<PolygonCollider2D> ().IsTouching (blobCollider) || GetComponent<PolygonCollider2D> ().IsTouching (handCollider)) {
			collected = true;
			GetComponent<SpriteRenderer> ().sprite = null;
		}
	}
}
