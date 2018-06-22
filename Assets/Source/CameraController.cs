using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {

    //  State Keeping Variables
    private Boat BoatToFollow;
    private Vector3 LastMouseCoordinate;
    private Vector3 DeltaMouse;

    //  Camera rig
    private int MouseSensitivity, ScrollSensitivity;
    private Camera MainCamera;
    private GameObject CameraPivotH;
    private GameObject CameraPivotV;
    private GameObject MainCameraObject;

    void Start () {
        //  Initialize the camera rig
        MouseSensitivity = 5;
        ScrollSensitivity = 5;
        CameraPivotH = new GameObject("Camera Pivot Horizontal");
        CameraPivotV = new GameObject("Camera Pivot Vertical");
        MainCameraObject = new GameObject("Main Camera");
        MainCameraObject.tag = "MainCamera";
        MainCameraObject.transform.localPosition += new Vector3(0,0,-25);
        MainCamera = MainCameraObject.AddComponent<Camera>() as Camera;
        MainCameraObject.transform.parent = CameraPivotV.transform;
        CameraPivotV.transform.parent = CameraPivotH.transform;
    }

    void Update () {
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt)) {
            CameraPivotH.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X")*MouseSensitivity, 0));
            CameraPivotV.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y")*MouseSensitivity, 0, 0));
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0) MainCameraObject.transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel")*ScrollSensitivity);
    }

    public bool FollowBoat (ref Boat b) {
        BoatToFollow = b;
        return true;
    }

    public void FreeCamera () {

    }
}
