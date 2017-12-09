using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
	public static float cameraDistance = -10f;
	float speed;

	Vector2 connectedAnchor;
	public GameObject hand;
	public GameObject blob;
	public GameObject confusedFace;
	float cringeAnimationTimer = 0;
	bool once = true;
	bool once2 = true;
	bool clockwise = true;
	float lastDistance;

	void Start(){
		// Set camera zoom level
		Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x,
			Camera.main.transform.position.y,
			cameraDistance);
		
		blob.GetComponent<Rigidbody2D>().freezeRotation = true;
		connectedAnchor = blob.GetComponent<HingeJoint2D> ().connectedAnchor;
	}

	void Update(){
		// Camera follows blob
		Camera.main.GetComponent<Camera> ().transform.position = new Vector3(
			blob.transform.position.x, 
			blob.transform.position.y,
			cameraDistance);
	}

	void FixedUpdate () {
		speed = 1000;

		// Get the angle of the hand in degrees relative to the blob body
		float handAngle = hand.transform.localEulerAngles.z;
		handAngle = ((handAngle + 270f) % 360f);

		// Map mouse position to a world position
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = -1 * cameraDistance;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		// Get the angle of the mouse relative to the blob body
		Vector3 blobPos = blob.transform.position;
		float mouseAngle = Mathf.Atan2(blobPos.y - mousePos.y, blobPos.x - mousePos.x);
		if (mouseAngle < 0) mouseAngle = Mathf.PI + (Mathf.PI - Mathf.Abs (mouseAngle));
		mouseAngle = ((Mathf.Rad2Deg * mouseAngle + 90f) % 360f);

		// Get the distance between the mouse and the blob
		float distance = Mathf.Sqrt(Mathf.Pow(blobPos.y - mousePos.y, 2) +  Mathf.Pow(blobPos.x - mousePos.x, 2));

		Debug.Log ("Distance: " + distance.ToString());
		Debug.Log ("mouseAngle: " + mouseAngle);



		if (distance < 3f) {
			if (distance > 1.5f) {
				blob.GetComponent<HingeJoint2D> ().connectedAnchor = connectedAnchor - (connectedAnchor - connectedAnchor * (distance) / 3f)/1.2f;
			}
		} else {
			blob.GetComponent<HingeJoint2D> ().connectedAnchor = connectedAnchor;
		}

		Collider2D [] colliders = GameObject.FindObjectsOfType<Collider2D>();
		bool isTouching = false;
		PolygonCollider2D handCollider = null;
		foreach (PolygonCollider2D collider in hand.GetComponents<PolygonCollider2D>()) {
			if (collider.isActiveAndEnabled) {
				handCollider = collider;
				break;
			}
		}
		foreach(Collider2D collider in colliders){
			if (handCollider.IsTouching(collider)) {
				isTouching = true;
				break;
			}
		}
		
		blob.GetComponent<HingeJoint2D> ().useMotor = true;
		JointMotor2D motor = new JointMotor2D ();
		motor.maxMotorTorque = 1000000;
		float minAngle = Mathf.Min(Mathf.Abs(mouseAngle - handAngle), Mathf.Abs(((360 - mouseAngle) + handAngle)));
		minAngle = Mathf.Min (minAngle, 360 - minAngle);

		Debug.Log ("Angle Difference: " + minAngle);


		Color32 visible = new Color32 (255, 255, 255, 255);
		Color32 invisible = new Color32 (255, 255, 255, 0);
		if (isTouching) {
			cringeAnimationTimer = 0;
			blob.GetComponent<SpriteRenderer> ().color = invisible;
			confusedFace.GetComponent<SpriteRenderer> ().color = visible;
			speed = 400f;
			hand.GetComponent<Rigidbody2D> ().mass = 4000;
		} else {
			cringeAnimationTimer += Time.deltaTime;
			if (cringeAnimationTimer > 0.12f) {
				blob.GetComponent<SpriteRenderer> ().color = visible;
				confusedFace.GetComponent<SpriteRenderer> ().color = invisible;
				hand.GetComponent<Rigidbody2D> ().mass = 10;
				if (distance < .7f) {
					hand.GetComponent<Rigidbody2D> ().mass = 0;
				}
			}
		}

		if (minAngle < 40) {
			speed *= minAngle / 40f;
		}
			
		if (minAngle > 3) {
			if (mouseAngle > 180f && handAngle < 180) {
				if (mouseAngle - handAngle > ((360 - mouseAngle) + handAngle)) {
					clockwise = true;
				} else {
					clockwise = false;
				}
			} else if (handAngle > 180 && mouseAngle < 180f) {
				if (handAngle - mouseAngle > ((360 - handAngle) + mouseAngle)) {
					clockwise = false;
				} else {
					clockwise = true;
				}
			} else if (handAngle > mouseAngle) {
				clockwise = true;

			} else {
				clockwise = false;
			}

			if (clockwise) {
				motor.motorSpeed = -speed;
				if (minAngle > 10f) {
					if (once2) {
						foreach (PolygonCollider2D collider in hand.GetComponents<PolygonCollider2D>()) {
							if (collider.isActiveAndEnabled) {
								collider.enabled = false;
							} else {
								collider.enabled = true;
							}
						}
						foreach (PolygonCollider2D collider in blob.GetComponents<PolygonCollider2D>()) {
							if (collider.isActiveAndEnabled) {
								collider.enabled = false;
							} else {
								collider.enabled = true;
							}
						}
						hand.GetComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("YellowHand");
						blob.GetComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("face");
						confusedFace.GetComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("confused");
						once2 = false;
					}
					once = true;
				}
			} else {
				motor.motorSpeed = speed;
				if (minAngle > 10f) {
					if (once) {
						foreach (PolygonCollider2D collider in hand.GetComponents<PolygonCollider2D>()) {
							if (collider.isActiveAndEnabled) {
								collider.enabled = false;
							} else {
								collider.enabled = true;
							}
						}
						foreach (PolygonCollider2D collider in blob.GetComponents<PolygonCollider2D>()) {
							if (collider.isActiveAndEnabled) {
								collider.enabled = false;
							} else {
								collider.enabled = true;
							}
						}
						hand.GetComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("YellowHand2");
						blob.GetComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("face2");
						confusedFace.GetComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("confused2");
						once = false;
					}
					once2 = true;
				}
			}
		}

		blob.GetComponent<HingeJoint2D> ().motor = motor;

		lastDistance = distance;

	}
}
