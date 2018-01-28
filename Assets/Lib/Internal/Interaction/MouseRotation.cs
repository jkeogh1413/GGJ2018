using UnityEngine;


public class MouseRotation : MonoBehaviour {

	private float speed = 2.0f;

	private float yaw = 0f;
	private float pitch = 0f;

	void Update() {
		yaw += speed * Input.GetAxis("Mouse X");
		pitch -= speed * Input.GetAxis("Mouse Y");

		transform.eulerAngles = new Vector3 (pitch, yaw, 0f);
	}
}