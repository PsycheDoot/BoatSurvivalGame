using UnityEngine;
using System.Collections.Generic;
using System;

//=================================================================================================
//  Slot
//=================================================================================================

public class Slot : MonoBehaviour{

    public bool Destroyed, Enabled;

    //  Private variables
    protected GameObject Parent;
    protected GameObject SlotObject;
    protected int X, Y, Floor;
    protected int Armor;

    void Start() {
        enabled = true;
        gameObject.transform.parent = Parent.transform;
        gameObject.name = name + " (" + X + ", " + Y + ", " + Floor + ")";
        gameObject.layer = Parent.layer;
        Armor = 100;
    }

    //  Setters
    public void setName(string n) { name = n; }
    public void setCoord(int x, int y, int f) {
        X = x;
        Y = y;
        Floor = f;
    }
    public void setGameObject (ref GameObject go) { SlotObject = go; }

    //  Getters
    public string getName () { return name; }
    public bool ApplyDamage(int d)
    {
        Armor -= d;
        if (Armor <= 0) {
            Destroyed = true;
            Enabled = false;
        }
        return Destroyed;
    }

    /*
    public virtual void Draw () {
        try { GameObject.Destroy(SlotObject); } catch (NullReferenceException nre) { }
        SlotObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        SlotObject.transform.parent = Parent.transform;
        SlotObject.layer = Parent.layer;
        SlotObject.transform.localPosition = new Vector3(X, Floor, Y);
        SlotObject.name = Type + SlotObject.transform.localPosition.ToString();

    } */

    public void setParent (ref GameObject parent) { Parent = parent; }

    public GameObject getParent () { return Parent; }
    public int getX () { return X; }
    public int getY () { return Y; }
    public int getFloor () { return Floor; }
}

//=================================================================================================
//  Boat
//=================================================================================================

public class Boat {

    //  GUI Variables
    private GameObject HealthBarPrefab;
    private GameObject healthBarGUI;

    //  State Variables
    private int Health;
    private int CurrentSpeed, MaxSpeed;
    private float Velocity;
    private int Acceleration;
    private List<Slot> BoatSlots;
    private List<EngineSlot> EngineSlots;
    private List<CargoSlot> CargoSlots;
    private List<GunSlot> GunSlots;
    private List<FishingSlot> FishingSlots;

    //  Engine Variables
    private Vector3 BoatPos;
    private List<GameObject> BoatPieces;
    private GameObject BoatParent;
    private Rigidbody BoatRigidbody;

    //  Constructors
    public Boat (int x, int y, string name, GameObject hb) {
        //  Initialize variables
        HealthBarPrefab = hb;
        healthBarGUI = GameObject.Instantiate<GameObject>(hb);
        BoatPieces = new List<GameObject>();
        BoatSlots = new List<Slot>();
        EngineSlots = new List<EngineSlot>();
        CargoSlots = new List<CargoSlot>();
        GunSlots = new List<GunSlot>();
        FishingSlots = new List<FishingSlot>();
        BoatParent = new GameObject(name);
        BoatParent.transform.position = new Vector3(x, 0, y);
        BoatRigidbody = BoatParent.AddComponent<Rigidbody>() as Rigidbody;
        BoatRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        BoatRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        BoatRigidbody.drag = 5f;
        BoatRigidbody.angularDrag = 5f;
        BoatRigidbody.mass = 10;
        BoatParent.layer = LayerMask.NameToLayer(name);
        //  Create default boat shape
        for (int i = -1; i < 2; ++i)
            if (!addNewSlot(0, i, 0, "Empty Slot")) Debug.Log("Can't add new slot.");
    }

    //  Update Functions
    public void Update () {
        BoatPos = BoatParent.transform.position;
    }

    public void Draw () {
        //BoatPieces.Clear();
        for (int i = 0; i < BoatSlots.Count; ++i) {
            //BoatSlots[i].Draw();
        }
    }

    //  Getters
    public Vector3 getPos () { return BoatPos; }
    public GameObject getParent () { return BoatParent; }
    public void getSlot (int x, int y, int f, out Slot s) {
        int tmp = BoatSlots.FindIndex(
            delegate (Slot sl) {
                if (x == sl.getX()
                    && y == sl.getY()
                    && f == sl.getFloor()) return true;
                return false;
            }
        );
        if (tmp >= 0) s = BoatSlots[tmp];
        else s = null;

    }

    //  Utility Functions
    public void Fire (float x, float y, float f) {
        if (GunSlots.Count == 0) {
            Debug.Log("There are no gun slots on this ship. Can't fire!");
            return;
        }
        GunSlot gunslot;
        int shotsFired = 0;
        for (int i = 0; i < GunSlots.Count; ++i) {
            if (true) {
                Debug.Log("Found GunSlot");
                gunslot = GunSlots[i];
                gunslot.fireOnTarget(BoatParent.transform.position + new Vector3(gunslot.getX(), gunslot.getFloor(), gunslot.getY()), new Vector3(x, f, y));
                shotsFired++;
            }
        }
        if (shotsFired == 0) Debug.Log("There are no active gun slots on this ship.");
    }
    
    //  Returns true when it boat is at destination, false otherwise
    public bool gotoPosition (float x, float y) {
        //  Boat is greater than units away
        if (Mathf.Abs(x - BoatPos.x) > 5 || Mathf.Abs(y - BoatPos.z) > 5) {
            BoatRigidbody.AddRelativeForce(new Vector3(0, 0, 10f), ForceMode.Impulse);
            if (Velocity > 1) {
                Vector3 targetVector = new Vector3(x - BoatPos.x, 0, y - BoatPos.z);
                Vector3 newVector = Vector3.RotateTowards(BoatParent.transform.forward, targetVector, .05f, 0f);
                BoatParent.transform.rotation = Quaternion.LookRotation(newVector);
            }
            Velocity += .05f;
            return false;
        }
        Velocity = 0;
        return true;
    }

    //  Add a slot to the ship at the coordinate
    public bool addNewSlot (int x, int y, int f, string type) {
        //make sure the coordinate doesn't exist
        for (int i = 0; i < BoatSlots.Count; ++i) {
            if (BoatSlots[i].getX() == x
                && BoatSlots[i].getY() == y
                && BoatSlots[i].getFloor() == f) {
                Debug.Log("Can't add this piece.");
                return false;
            }
        }
        GameObject tmpgo = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tmpgo.transform.parent = BoatParent.transform;
        tmpgo.transform.localPosition = new Vector3(x, f, y);
        Slot tmps;
        if (type == "Gun Slot") { tmps = tmpgo.AddComponent<GunSlot>(); }
        else if (type == "Engine Slot") { tmps = tmpgo.AddComponent<EngineSlot>(); }
        else if (type == "Cargo Slot") { tmps = tmpgo.AddComponent<CargoSlot>(); }
        else if (type == "Fishing Slot") { tmps = tmpgo.AddComponent<FishingSlot>(); }
        else { tmps = tmpgo.AddComponent<Slot>(); }
        tmps.setName(type);
        tmps.setParent(ref BoatParent);
        tmps.setCoord(x, y, f);
        tmps.setGameObject(ref tmpgo);
        BoatPieces.Add(tmpgo);
        BoatSlots.Add(tmps);
        if (type == "Gun Slot") { GunSlots.Add(tmps as GunSlot); }
        else if (type == "Engine Slot") { EngineSlots.Add(tmps as EngineSlot); }
        else if (type == "Cargo Slot") { CargoSlots.Add(tmps as CargoSlot); }
        else if (type == "Fishing Slot") { FishingSlots.Add(tmps as FishingSlot); }
        return true;
    }

    public int changeSlot (Slot slotPiece) {
        int tmp = BoatSlots.FindIndex(
            delegate(Slot sl) {
                if (slotPiece.getFloor() == sl.getFloor()
                    && slotPiece.getX() == sl.getX()
                    && slotPiece.getY() == sl.getY()) return true;
                return false;
            } 
        );
        if (tmp >= 0) BoatSlots[tmp] = slotPiece;
        return tmp;
    }
}