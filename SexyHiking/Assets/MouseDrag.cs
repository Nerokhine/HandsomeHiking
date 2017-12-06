using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour {
	bool down = false;
	// Use this for initialization
	void Start () {
		
	}

	void OnMouseDown(){
		last = Input.mousePosition;
		gameObject.GetComponent<Rigidbody2D> ().gravityScale = 0;
		//FindObjectsOfType<HingeJoint2D>().is
		//gameObject.GetComponent<Rigidbody2D> ().
		//GameObject.Find("Cool").GetComponent<DistanceJoint2D>().
		down = true;
	}

	void OnMouseUp(){
		down = false;
		gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		gameObject.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
		gameObject.GetComponent<Rigidbody2D> ().isKinematic = false;
	}

	Vector3 last;

	void LateUpdate(){
		if (down) {
			gameObject.GetComponent<Rigidbody2D> ().velocity = Input.mousePosition - last;
			//gameObject.GetComponent<Rigidbody2D> ().
			last = Input.mousePosition;
		}
	}
}
