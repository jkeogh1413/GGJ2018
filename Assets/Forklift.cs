using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forklift : MonoBehaviour {
    public bool doYouEvenLift = false;
    public float liftHeight = 10f;
    public float speed = 1f;

    public Transform lift;
    public Transform fork;

    bool forkliftColliding = false;
    // Object positions
    Vector3 deltaPosition;
    Vector3 lastPosition;
    // Lift position
    Vector3 liftStartPos;
    Vector3 liftTarget;
    //Fork position
    public Vector3 forkStartPos;
    public Vector3 forkTarget;

    public void broILift() {
        doYouEvenLift = true;
    }

    public void broIDontLift() {
        doYouEvenLift = false;
    }

    // Use this for initialization
    void Start () {
        liftStartPos = lift.position;
        lastPosition = transform.position;
        liftTarget = new Vector3 (liftStartPos.x, liftStartPos.y + liftHeight, liftStartPos.z);

        forkStartPos = fork.position;
        forkStartPos = new Vector3 (forkStartPos.x, forkStartPos.y + liftHeight, forkStartPos.z);
    }
    
    // Update is called once per frame
    void Update () {
        float step = speed * Time.deltaTime;

        //Update position for collision
        if (!forkliftColliding) {
            lastPosition = transform.position;
        }

        // Move the arm
        if (doYouEvenLift) {
            lift.position = Vector3.MoveTowards (lift.position, liftTarget, step);
        }

        // Once the arm has reached its target height move the fork
        if (lift.position == liftTarget && doYouEvenLift) {
            forkTarget = new Vector3 (forkStartPos.x, forkStartPos.y + 1.7f, forkStartPos.z);
            fork.position = Vector3.MoveTowards (fork.position, forkTarget, step);
        }
    }

    void OnDrawGizmos() {
        liftStartPos = lift.position;
        Vector3 targetPos = new Vector3 (liftStartPos.x, liftStartPos.y + liftHeight, liftStartPos.z);
        Gizmos.color = Color.green;
        Gizmos.DrawLine (liftStartPos, targetPos);
    }

    // Collision logic
    void OnTriggerEnter (Collider col) {
        forkliftColliding = true;
    }

    void OnTriggerExit (Collider col) {
        forkliftColliding = false;
    }

    void OnTriggerStay (Collider col) {
        if (col.gameObject.tag == "Moveable") {
            lastPosition = transform.position;
            deltaPosition = transform.position - lastPosition;
            col.GetComponent<Rigidbody> ().AddForce (deltaPosition * 5000);
        }
    }
}