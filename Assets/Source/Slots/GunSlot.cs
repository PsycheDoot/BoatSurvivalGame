using UnityEngine;
using System.Collections.Generic;

//  Gun Slot Alpha
//  Level 1: 1 gun per slot
//  Level 2: 2 guns per slot
//  Level 3: 3 guns per slot
public class GunSlot : Slot {
    //  State keeping
    private List<Ammo> AmmoSupply; 
    private List<Gun> Guns;
    private List<GameObject> GunObjects;
    private int Level;

    public GunSlot () {
        AmmoSupply = new List<Ammo>();
        Guns = new List<Gun>();
        GunObjects = new List<GameObject>();
        Level = 1;
        Enabled = false;
    }

    public bool addGun (Gun g) {
        if (Guns.Count == Level || Destroyed) return false;
        Guns.Add(g);
        Draw();
        Enabled = true;
        return true;
    }

    public bool upgradeSlot () {
        if (Level >= 3) return false;
        Level++;
        return true;
    }

    
    public void Draw () {
        for (int i = 0; i < GunObjects.Count; ++i)
            Destroy(GunObjects[i]);
        GunObjects.Clear();
        for (int i = 0; i < Guns.Count; ++i) {
            GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tmp.transform.localScale = new Vector3(.3f, .3f, .3f);
            tmp.transform.parent = SlotObject.transform;
            tmp.layer = Parent.layer;
            tmp.transform.localPosition = new Vector3((float)i/3 - (float)1/3, .5f, 0);
            tmp.name = "Gun " + tmp.transform.localPosition.ToString() + i;
            GunObjects.Add(tmp);
        }
    }

    //  Calculate the trajectories of 3 different projectiles
    public List<RaycastHit> fireOnTarget (Vector3 position, Vector3 direction) {
        List<RaycastHit> Hits = new List<RaycastHit>();
        for (int i = 0; i < Guns.Count; ++i) {
            Vector3 adj = new Vector3(0, .5f, 0);
            Ray targetVector = new Ray(position + adj, direction - position - adj);
            RaycastHit hit = Guns[i].fireGun (targetVector, Color.red);
            Hits.Add(hit);
        }
        return Hits;
    }
}
