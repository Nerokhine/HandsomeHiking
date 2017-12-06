using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleController : MonoBehaviour {
	bool down = false;
	// Use this for initialization
	void Start () {
		//GetComponent<Rigidbody2D> ().gravityScale = 0;
		// GetComponent<Rigidbody2D> ().angularVelocity =
		//GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic;
		//GetComponent<Rigidbody2D> ().centerOfMass = new Vector2 (1f, 0f);
		GameObject.Find("LargeWheel").GetComponent<Rigidbody2D>().freezeRotation = true;
	}

	void Update(){
		GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position = 
			new Vector3(GameObject.Find ("LargeWheel").transform.position.x, 
				GameObject.Find ("LargeWheel").transform.position.y,
				-5);
		//GameObject.Find ("LargeWheel").GetComponent<HingeJoint2D> ().connectedAnchor -= new Vector2 (0.0001f, 0f);
	}

	/*void OnMouseDown(){
		last = Input.mousePosition;
		//GetComponent<Rigidbody2D> ().mass = 0;
		down = true;
	}

	void OnMouseUp(){
		down = false;
	}

	Vector3 last;

	void LateUpdate(){
		if (down) {
			gameObject.GetComponent<Rigidbody2D> ().AddForce (Input.mousePosition - last);
			//if(gameObject.di
			//gameObject.GetComponent<Rigidbody2D> ().velocity = Input.mousePosition - last;
			//gameObjec
			//gameObject.GetComponent<SliderJoint2D>().
			//gameObject.GetComponent<Rigidbody2D> ().
			last = Input.mousePosition;
		}
	}*/

	// Update is called once per frame
	//void Update () {
		//Vector3 v3 = Input.mousePosition;
		//v3.z = 10;
		//v3 = Camera.main.ScreenToWorldPoint(v3);
		//gameObject.GetComponent<Rigidbody2D> ().MovePosition (v3);
	//}
}
