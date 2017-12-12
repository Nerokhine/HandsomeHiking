using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollector : MonoBehaviour {
	public static bool collected = false;
	PolygonCollider2D blobCollider;
	// Use this for initialization
	void Start () {
		blobCollider = GameObject.Find ("Blob").GetComponent<PolygonCollider2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (GetComponent<PolygonCollider2D> ().IsTouching (blobCollider)) {
			collected = true;
			Destroy (gameObject);
		}
	}
}
