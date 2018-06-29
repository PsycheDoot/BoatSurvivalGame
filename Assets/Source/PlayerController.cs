using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    //  Input Variables;
    private Camera MainCamera;
    private Vector3 LastMouseCoordinate;
    private Vector3 DeltaMouse;

    const float FluidDensity = 997f;
    const float Area = 1;

    public float MaxSpeed = 10f;
    public float EngineThrust = 500f;
    public float Mass = 1000f;
    public float TurnRadius;
    public float DragCoefficient = .5f;
    private float CurrentVelocity = 0f;
    private Vector3 LastPos;
    Vector3 Velocity;
    Vector3 Acceleration;
    Vector3 NetForce;

    Rigidbody rigidbody;

    public PlayerController (ref Camera mainCam) {
        MainCamera = mainCam;
    }

	void Start () {
        LastPos = transform.position;
        rigidbody = gameObject.GetComponent<Rigidbody>();
	}

    void Update() {
        NetForce = Vector3.zero;


        rigidbody.mass = Mass;
        CurrentVelocity = rigidbody.velocity.magnitude;

        Vector3 DragForce = Velocity;
        DragForce.Scale(Velocity);
        DragForce *= (FluidDensity * DragCoefficient * Area) / 2;
        DragForce.Scale(-Velocity.normalized);
        NetForce += DragForce;
        // Debug.Log(Velocity + " " + DragForce);


        //Debug.Log(CurrentVelocity);
        if (Input.GetKey(KeyCode.W)) {
            if (CurrentVelocity < MaxSpeed) {                 //NetForce += transform.forward.normalized * EngineThrust;
                rigidbody.AddForceAtPosition(transform.forward.normalized * EngineThrust, transform.TransformPoint(Vector3.back));
                //Debug.DrawLine(transform.TransformPoint(Vector3.back), transform.forward.normalized * EngineThrust, Color.blue);
            }

        }
        if (Input.GetKeyDown(KeyCode.A)) {

        }
        if (Input.GetKey(KeyCode.A)) {
            //transform.localEulerAngles += new Vector3(0, -Mathf.Lerp(5, .5f, (CurrentVelocity == 0 ? 0 : CurrentVelocity/MaxSpeed)), 0);
            float centrip = (Mass * CurrentVelocity * CurrentVelocity) / TurnRadius;
            //Debug.Log((-transform.right * CurrentVelocity * CurrentVelocity) / TurnRadius);
            Vector3 CentrepetalForce = (transform.right.normalized * centrip + transform.position);
            rigidbody.AddForceAtPosition(transform.right, transform.TransformPoint(Vector3.back));
            //rigidbody.AddForce(CentrepetalForce, ForceMode.Force);
            Debug.DrawLine(transform.TransformPoint(Vector3.back), transform.right + transform.position, Color.blue);
            //Debug.DrawLine(transform.position, -transform.right.normalized * TurnRadius + transform.position, Color.red);
        }

        //PhysicsStep();
    }

    void PhysicsStep() {
        Vector3 displacement = Velocity * Time.deltaTime;
        Acceleration = NetForce / Mass;
        Vector3 deltaVelocity = Acceleration * Time.deltaTime;
        Velocity += deltaVelocity;
        transform.position += displacement;
    }

	void FixedUpdate () {
        //if (Input.GetMouseButton(0)) {
        //    MainCamera.transform.eulerAngles += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        //}
        //LastMouseCoordinate = Input.mousePosition;
	}
}
