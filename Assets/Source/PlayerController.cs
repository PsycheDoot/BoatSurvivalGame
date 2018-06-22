using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    //  Input Variables;
    private Camera MainCamera;
    private Vector3 LastMouseCoordinate;
    private Vector3 DeltaMouse;

    public PlayerController (ref Camera mainCam) {
        MainCamera = mainCam;
    }

	void Start () {
		
	}

	void FixedUpdate () {
        if (Input.GetMouseButton(0)) {
            MainCamera.transform.eulerAngles += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        }
        LastMouseCoordinate = Input.mousePosition;
	}
}
