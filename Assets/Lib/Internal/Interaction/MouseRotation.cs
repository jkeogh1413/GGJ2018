using UnityEngine;


public class MouseRotation : MonoBehaviour {
	private Rigidbody rb;
	private Camera cam;

	public float sensitivity = 5f;
	public float verticalRotationRange = 90f;
	private float camRotation = 0f;

	void Start() {
		rb = GetComponent<Rigidbody> ();
		cam = GetComponentInChildren<Camera> ();
	}
		
	void rotate () {
		// Player rotation
		float yRot = Input.GetAxisRaw ("Mouse X");
		Vector3 rotationDirection = new Vector3 (0f, yRot, 0f) * sensitivity;
		rb.MoveRotation (rb.rotation * Quaternion.Euler (rotationDirection));

		// Camera rotation
		float xRot = Input.GetAxisRaw ("Mouse Y");
		rotationDirection = new Vector3 (xRot, 0f, 0f) * sensitivity;
		camRotation -= rotationDirection.x;
		camRotation = Mathf.Clamp (camRotation, -verticalRotationRange, verticalRotationRange);
		cam.transform.localEulerAngles = new Vector3 (camRotation, 0f, 0f);
	}

	void FixedUpdate() {
		rotate();
	}
}