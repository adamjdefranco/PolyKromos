using UnityEngine;
using System.Collections;

public class FireballMovement : MonoBehaviour {

	public float speed = -5f;
    private GameObject Player; 

	// Use this for initialization
	void Start () {
        Player = GameObject.FindWithTag("Player");
		Rigidbody rb = GetComponent<Rigidbody> ();
		Vector3 movement = new Vector3 (0.0f, 0.0f, speed);
		rb.velocity = movement;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.z < Player.transform.position.z - 10f) {
			Destroy (gameObject);
		}

		if (speed != -5f) {
			Rigidbody rb = GetComponent<Rigidbody> ();
			Vector3 movement = new Vector3 (0.0f, 0.0f, 0f);
			rb.velocity = movement;
		} else {
			Rigidbody rb = GetComponent<Rigidbody> ();
			Vector3 movement = new Vector3 (0.0f, 0.0f, speed);
			rb.velocity = movement;
		}
	}
		
}
