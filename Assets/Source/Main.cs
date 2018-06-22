using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public class Main : MonoBehaviour {
    //  GUI Elements
    public GameObject HealthBarPrefab;
    private GameObject MainMenuObject;
    private GameObject InGameMenuObject;
    private GameObject OptionsMenuObject;
    private GameObject QuitMenuObject;
    //  GUI Menu States Variables
    private bool isMainMenu;
    private bool isInGameMenu;
    private bool isOptionsMenu;
    private bool isQuitMenu;
    //  Engine State Variables
    private CameraController CameraControl;
    private GameObject WaterScenery;
    private List<Boat> BoatsInWorld;
    //  Input and Player Variables
    private GameObject RallyPoint;
    private Vector3 mouseToWorld;
    private RaycastHit mouseHit, mouseHitNav;
    public Texture2D Pointer, EnemyCursor, FriendlyCursor, NavigationCursor;
    private bool NavigationMode;
    private bool PlayerNavigate;
    private Boat PlayerBoat;

    void Start () {
        
        //  Import Cursors
        Pointer = new Texture2D(32, 32);
        EnemyCursor = new Texture2D(32, 32);
        FriendlyCursor = new Texture2D(32, 32);
        NavigationCursor = new Texture2D(32, 32);
        Pointer.LoadImage(File.ReadAllBytes("Assets/Assets/GUI/Pointer.PNG"));
        EnemyCursor.LoadImage(File.ReadAllBytes("Assets/Assets/GUI/EnemyCursor.PNG"));
        FriendlyCursor.LoadImage(File.ReadAllBytes("Assets/Assets/GUI/FriendlyCursor.PNG"));
        NavigationCursor.LoadImage(File.ReadAllBytes("Assets/Assets/GUI/NavigationCursor.PNG"));

        //  Initialize Player Variables
        PlayerNavigate = false;

        //  Find GUI canvas elements in scene
        MainMenuObject = GameObject.Find("Main Menu Canvas");
        InGameMenuObject = GameObject.Find("In Game Canvas");
        //OptionsMenuObject = GameObject.Find("Options Menu Canvas");
        //QuitMenuObject = GameObject.Find("Quit Menu Canvas")
        //  Explicitly set GUI menu states
        isMainMenu = true;
        isInGameMenu = false;
        isOptionsMenu = false;
        isQuitMenu = false;
        //  Initialize state keeping variables
        BoatsInWorld = new List<Boat>();
        //  Find and set appropriate game object states
        CameraControl = gameObject.GetComponent<CameraController>();
        WaterScenery = GameObject.Find("Water");
        WaterScenery.SetActive(false);
    }

        
    Ray mouseVector;
          
	void FixedUpdate () {
        //  GUI menu states
        MainMenuObject.SetActive(isMainMenu);
        InGameMenuObject.SetActive(isInGameMenu);
        //OptionsMenuObject.SetActive(isOptionsMenu);
        //QuitMenuObject.SetActive(isQuitMenu);

        // Input
        MouseInput();
        KeyboardInput();

        for (int i = 0; i < BoatsInWorld.Count; ++i) {
            BoatsInWorld[i].Update();
        }
        if (PlayerNavigate == true)
            try { PlayerNavigate = !PlayerBoat.gotoPosition(mouseHitNav.point.x, mouseHitNav.point.z); } catch (NullReferenceException nre) { }
    }

    private void MouseInput() {
        try {
            mouseToWorld = Input.mousePosition;
            mouseToWorld.z = 25;
            mouseVector = Camera.main.ScreenPointToRay(mouseToWorld);
            Physics.Raycast(mouseVector, out mouseHit, 100);
            if (NavigationMode) {
                Cursor.SetCursor(NavigationCursor, new Vector2(15, 15), CursorMode.Auto);
            } else {
                if (mouseHit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy Boat")) {
                    Cursor.SetCursor(EnemyCursor, new Vector2(15, 15), CursorMode.Auto);
                } else if (mouseHit.collider.gameObject.layer == LayerMask.NameToLayer("Player Boat")) {
                    Cursor.SetCursor(FriendlyCursor, new Vector2(15, 15), CursorMode.Auto);
                } else {
                    Cursor.SetCursor(Pointer, new Vector2(0, 0), CursorMode.Auto);
                }
            }
        } catch (NullReferenceException nre) { }
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftAlt)) {
            try {
                mouseToWorld = Input.mousePosition;
                mouseToWorld.z = 25;
                mouseVector = Camera.main.ScreenPointToRay(mouseToWorld);
                if (NavigationMode) {
                    Physics.Raycast(mouseVector, out mouseHitNav, 100);
                    Destroy(RallyPoint);
                    RallyPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    RallyPoint.GetComponent<SphereCollider>().enabled = false; ;
                    RallyPoint.transform.position = mouseHitNav.point;
                    PlayerNavigate = true;

                } else if (mouseHit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy Boat")) {
                    Physics.Raycast(mouseVector, out mouseHit, 100);
                    PlayerBoat.Fire(mouseHit.point.x, mouseHit.point.z, mouseHit.point.y);
                }
            } catch (NullReferenceException nre) { }
        }
    }

    private void KeyboardInput() {
        NavigationMode = Input.GetKey(KeyCode.LeftShift);
    }

    private void StartNewGame () {
        isMainMenu = false;
        //  Explicitly create player 
        Gun glock17 = new Gun("Glock 17", "9mm", 100, 20, 15, 7, 17);
        Gun m4a1 = new Gun("M4A1", "5.56mm", 100, 50, 25, 20, 30);
        PlayerBoat = new Boat(0, 0, "Player Boat", HealthBarPrefab);
        PlayerBoat.addNewSlot(0, 0, 1, "Gun Slot");
        Slot slotRef;
        PlayerBoat.getSlot(0, 0, 1, out slotRef);
        GunSlot gunSlotRef = slotRef as GunSlot;
        gunSlotRef.upgradeSlot();
        gunSlotRef.addGun(m4a1);
        gunSlotRef.addGun(m4a1);
        //  Explicitly create enemy
        BoatsInWorld.Add(PlayerBoat);
        PlayerBoat.Draw();
        BoatsInWorld.Add(new Boat(20, 10, "Enemy Boat", HealthBarPrefab));
        //  Prepare the scene
        WaterScenery.SetActive(true);
        if (!CameraControl.FollowBoat(ref PlayerBoat)) Debug.Log("Camera couldn't follow the player.");
    }

    private void ContinueGame () {

    }

    private void Options () {

    }

    private void Quit () {

    }

    private void ClearScene () {

    }
}
