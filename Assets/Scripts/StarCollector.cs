using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollector : MonoBehaviour {
	public bool collected = false;
	PolygonCollider2D blobCollider;
	PolygonCollider2D [] handColliders;
	// Use this for initialization
	void Start () {
		blobCollider = GameObject.Find ("Blob").GetComponent<PolygonCollider2D> ();
		handColliders = GameObject.Find ("Hand").GetComponents<PolygonCollider2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!collected && (GetComponent<PolygonCollider2D> ().IsTouching (blobCollider) 
			|| GetComponent<PolygonCollider2D> ().IsTouching (handColliders[0])
			|| GetComponent<PolygonCollider2D> ().IsTouching (handColliders[0]))) {
			collected = true;
			GetComponent<SpriteRenderer> ().enabled = false;
			GetComponent<PolygonCollider2D> ().enabled = false;

		}
	}
}
