using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
	Vector3 last;
	bool yes = false;
	Vector2 connectedAnchor;
	public GameObject hand;
	public GameObject blob;
	public GameObject confusedFace;
	float cringeAnimationTimer = 0;
	bool once = true;
	bool once2 = true;
	bool clockwise = true;

	void Update(){
		GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position = 
			new Vector3(blob.transform.position.x, 
				blob.transform.position.y,
				-10);
	}

	void Start(){
		blob.GetComponent<Rigidbody2D>().freezeRotation = true;
		last = Input.mousePosition;
		connectedAnchor = blob.GetComponent<HingeJoint2D> ().connectedAnchor;
	}
	void FixedUpdate () {
		float speed = 1000;

		float angle2 = hand.transform.localEulerAngles.z;
		angle2 = ((angle2 + 270f) % 360f);

		Vector3 screenPoint = Input.mousePosition;
		screenPoint.z = 5.0f; //distance of the plane from the camera
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(screenPoint);
		Vector3 dudePos = blob.transform.position;
		float angle = Mathf.Atan2(dudePos.y - mousePos.y, dudePos.x - mousePos.x); //Vector2.SignedAngle((Vector2)dudePos, (Vector2)mousePos);
		if (angle < 0) {
			angle = Mathf.PI + (Mathf.PI - Mathf.Abs (angle));
		}

		angle = ((Mathf.Rad2Deg * angle + 90f) % 360f);

		float distance = Mathf.Sqrt(Mathf.Pow(dudePos.y - mousePos.y, 2) +  Mathf.Pow(dudePos.x - mousePos.x, 2));
		Debug.Log ("Distance: " + distance.ToString());
		if (distance < 2f) {
			blob.GetComponent<HingeJoint2D> ().connectedAnchor = connectedAnchor - (connectedAnchor - connectedAnchor * (distance) / 2f);
			if (distance < .9f && distance > .7f) {
				hand.transform.localScale = new Vector3 (0.625f - (.625f - .625f * (distance) / .9f), 0.625f - (.625f - .625f * (distance) / .9f), 1);
				//blob.GetComponent<HingeJoint2D> ().connectedAnchor = connectedAnchor - (connectedAnchor - connectedAnchor * (distance - .7f) / 1.3f);
			} else if (distance < .7f) {
				speed = 16000;

				hand.transform.localScale = new Vector3 (.05f, .05f, 1f);
			} else {
				hand.transform.localScale = new Vector3 (0.625f, 0.625f, 1);
			}
			if (blob.GetComponent<HingeJoint2D> ().connectedAnchor.x < 0f && !yes) {
				yes = true;
			}
		} else {
			blob.GetComponent<HingeJoint2D> ().connectedAnchor = connectedAnchor;
			yes = false;
		}
			
		Debug.Log ("Angle: " + angle);

		Collider2D [] colliders = GameObject.FindObjectsOfType<Collider2D>();
		last = Input.mousePosition;
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
		float minAngle = Mathf.Min(Mathf.Abs(angle - angle2), Mathf.Abs(((360 - angle) + angle2)));


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
			if (angle > 180f && angle2 < 180) {
				if (angle - angle2 > ((360 - angle) + angle2)) {
					clockwise = true;
				} else {
					clockwise = false;
				}
			} else if (angle2 > 180 && angle < 180f) {
				if (angle2 - angle > ((360 - angle2) + angle)) {
					clockwise = false;
				} else {
					clockwise = true;
				}
			} else if (angle2 > angle) {
				clockwise = true;

			} else {
				clockwise = false;
			}

			if (clockwise) {
				motor.motorSpeed = -speed;
				if (!isTouching && cringeAnimationTimer > .12f && distance > 1.1f) {
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
				if (!isTouching && cringeAnimationTimer > .12f && distance > 1.1f) {
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

	}
}
