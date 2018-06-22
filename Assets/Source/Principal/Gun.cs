using UnityEngine;
using System.Collections;

public class Gun {
    //  State keeping
    private string Name;
    private string AmmoType;
    private int Ammo, AmmoCapacity;
    private int Condition;
    private int MaxDamage, MinDamage;
    private int Range;

    public Gun () {
        Name = "Glock 17";
        AmmoType = "9mm";
        Condition = (int)Random.Range(1,100);
        Range = (int)Random.Range(5, 50);
        MaxDamage = (int)Random.Range(10, 20);
        MinDamage = (int)Random.Range(1, 10);
    }

    public Gun (string name, string ammoType, int cond, int range, int maxDam, int minDam, int ammoCap) {
        Name = name;
        AmmoType = ammoType;
        Condition = cond;
        Range = range;
        MaxDamage = maxDam;
        MinDamage = minDam;
        AmmoCapacity = ammoCap;
        Ammo = ammoCap;
    }

    //  Getter
    public int getCondition () { return Condition; }
    public int getRandomDamage () { return (int)Random.Range(MinDamage, MaxDamage); }
    public int getRange () { return Range; }

    public RaycastHit fireGun (Ray targetVector, Color color) {
        Debug.DrawRay(targetVector.origin, targetVector.direction*Range, Color.blue);
        RaycastHit hit;
        Physics.Raycast(targetVector, out hit, Range);
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy Boat")) {
            Debug.Log("Hit enemy boat");
            if (!hit.collider.gameObject.GetComponent<Slot>().ApplyDamage(Random.Range(MinDamage, MaxDamage)))
                Debug.DrawLine(targetVector.origin, hit.point, Color.red);
        }
        return hit;
    }
}
