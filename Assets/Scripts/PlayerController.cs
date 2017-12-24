using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public static float cameraDistance = -10f;
	public GameObject hand;
	public GameObject blob;
	public GameObject confusedFace;

	Sprite yellowHandSprite, yellowHandSprite2;
	Sprite faceSprite, faceSprite2;
	Sprite confusedSprite, confusedSprite2;

	Color32 visible = new Color32 (255, 255, 255, 255);
	Color32 invisible = new Color32 (255, 255, 255, 0);

	const float maxMagnitudePush = 7500000;
	float maxSoFarPush;
	float speed;
	float lastDistance;
	const float defaultSpeed = 1000f;
	const float strength = 200f;
	Vector2 connectedAnchor;
	float cringeAnimationTimer = 0;
	bool once = true;
	bool once2 = true;
	bool once3 = true;
	bool clockwise = true;
	bool oncePush;
	float pushCounter;
	float pushRunningTotal;

	void Start(){
		maxSoFarPush = 0;
		pushCounter = 0;
		pushRunningTotal = 0;
		oncePush = true;
		yellowHandSprite = Resources.Load <Sprite> ("YellowHand");
		yellowHandSprite2 = Resources.Load <Sprite> ("YellowHand2");
		faceSprite = Resources.Load <Sprite> ("face");
		faceSprite2 = Resources.Load <Sprite> ("face2");
		confusedSprite = Resources.Load <Sprite> ("confused");
		confusedSprite2 = Resources.Load <Sprite> ("confused2");

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
		speed = defaultSpeed;

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

		//Debug.Log ("Distance: " + distance.ToString());
		//Debug.Log ("mouseAngle: " + mouseAngle);
		//Debug.Log("Hand Angle: " + (360 - handAngle));
		Collider2D [] colliders = GameObject.FindObjectsOfType<Collider2D>();
		bool isTouching = false;
		PolygonCollider2D handCollider = null;

		// Get the active collider for the hand
		foreach (PolygonCollider2D collider in hand.GetComponents<PolygonCollider2D>()) {
			if (collider.isActiveAndEnabled) {
				handCollider = collider;
				break;
			}
		}

		// Check if the hand is touching a collider
		foreach(Collider2D collider in colliders){
			if (handCollider.IsTouching(collider)) {
				isTouching = true;
				break;
			}
		}

		// Adjust the anchor of the hand based on the distance of the mouse/touch to the blob
		if (distance < 3f) {
			if (lastDistance < distance && isTouching) {
				oncePush = true;
				float magnitude = (distance - lastDistance) * 10000000;
				if (magnitude > maxMagnitudePush) {
					magnitude = maxMagnitudePush;
				}
				if (magnitude > maxSoFarPush) {
					maxSoFarPush = magnitude;
				}
				//pushCounter += 1f;
				//pushRunningTotal += magnitude;

				//Debug.Log ("X: " + Mathf.Sin ((360 - handAngle) * Mathf.Deg2Rad * -1));
				//Debug.Log ("Y: " + Mathf.Cos ((360 - handAngle) * Mathf.Deg2Rad * -1));
				//blob.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Mathf.Sin ((360 - handAngle) * Mathf.Deg2Rad) * magnitude * -1,
				//	Mathf.Cos ((360 - handAngle) * Mathf.Deg2Rad) * magnitude * -1));
			} else {
				//pushRunningTotal = 0;
				//pushCounter = 0;
				maxSoFarPush = 0;
				/*if (oncePush) {
					float average = pushRunningTotal / pushCounter;
					blob.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Mathf.Sin ((360 - handAngle) * Mathf.Deg2Rad) * average * -1,
						Mathf.Cos ((360 - handAngle) * Mathf.Deg2Rad) * average * -1));
					oncePush = false;
					pushRunningTotal = 0;
					average = 0;
				}*/
			}
			if (distance > 1.7f) {
				blob.GetComponent<HingeJoint2D> ().connectedAnchor = connectedAnchor - (connectedAnchor - connectedAnchor * (distance) / 3f);
			} else {
				blob.GetComponent<HingeJoint2D> ().connectedAnchor = connectedAnchor - (connectedAnchor - connectedAnchor * (1.7f) / 3f);
			}
		} else {
			if (oncePush) {
				//float average = pushRunningTotal / pushCounter;
				blob.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Mathf.Sin ((360 - handAngle) * Mathf.Deg2Rad) * maxSoFarPush * -1,
					Mathf.Cos ((360 - handAngle) * Mathf.Deg2Rad) * maxSoFarPush * -1));
				oncePush = false;
				//pushRunningTotal = 0;
				//pushCounter = 0;
				//average = 0;
			}
			blob.GetComponent<HingeJoint2D> ().connectedAnchor = connectedAnchor;
		}

		lastDistance = distance;


		// Hand Touching Logic (face animation and mass and motor speed adjustments)
		if (isTouching) {
			cringeAnimationTimer = 0;
			blob.GetComponent<SpriteRenderer> ().color = invisible;
			confusedFace.GetComponent<SpriteRenderer> ().color = visible;
			speed = strength;
			hand.GetComponent<Rigidbody2D> ().mass = 4000;
			if (once3) {
				hand.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
				once3 = false;
			}
		} else {
			cringeAnimationTimer += Time.deltaTime;
			if (cringeAnimationTimer > 0.12f) {
				blob.GetComponent<SpriteRenderer> ().color = visible;
				confusedFace.GetComponent<SpriteRenderer> ().color = invisible;
				hand.GetComponent<Rigidbody2D> ().mass = 10;
			}
			once3 = true;
		}

		blob.GetComponent<HingeJoint2D> ().useMotor = true;
		JointMotor2D motor = new JointMotor2D ();
		motor.maxMotorTorque = 1000000;

		// Get the angle between the mouse/touch and the hand
		float minAngle = Mathf.Min(Mathf.Abs(mouseAngle - handAngle), Mathf.Abs(((360 - mouseAngle) + handAngle)));
		minAngle = Mathf.Min (minAngle, 360 - minAngle);

		//Debug.Log ("Angle Difference: " + minAngle);

		// Slow down hand rotation when the angle between the hand and the mouse/touch is small
		if (minAngle < 40) {
			speed *= minAngle / 40f;
		}
			
		if (minAngle > 3) {
			// Figure out which direction for the hand to rotate in
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
						/*foreach (PolygonCollider2D collider in blob.GetComponents<PolygonCollider2D>()) {
							if (collider.isActiveAndEnabled) {
								collider.enabled = false;
							} else {
								collider.enabled = true;
							}
						}*/
						hand.GetComponent<SpriteRenderer> ().sprite = yellowHandSprite;
						blob.GetComponent<SpriteRenderer> ().sprite = faceSprite;
						confusedFace.GetComponent<SpriteRenderer> ().sprite = confusedSprite;
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
						/*foreach (PolygonCollider2D collider in blob.GetComponents<PolygonCollider2D>()) {
							if (collider.isActiveAndEnabled) {
								collider.enabled = false;
							} else {
								collider.enabled = true;
							}
						}*/
						hand.GetComponent<SpriteRenderer> ().sprite = yellowHandSprite2;
						blob.GetComponent<SpriteRenderer> ().sprite = faceSprite2;
						confusedFace.GetComponent<SpriteRenderer> ().sprite = confusedSprite2;
						once = false;
					}
					once2 = true;
				}
			}
		}

		blob.GetComponent<HingeJoint2D> ().motor = motor;

	}
}
