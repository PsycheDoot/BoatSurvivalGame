using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

    //  State Variables
    private Boat AI_Boat;
    private Transform NavigationTarget;
    private bool Alerted, Pursuant, Attacking, Fleeing;
    int AlertRadius;
    int AttackRadius;
    int AlertToAttackTime;
    int AlertTimer;
    int TimeToForget;

    void Start () {
        Alerted = false;
        Pursuant = false;
        Attacking = false;
        Fleeing = false;
        AlertRadius = 15;
        AttackRadius = 10;
        AlertToAttackTime = 7;
        AlertTimer = 0;
        TimeToForget = 10;
    } 

    void Update () {
        if (Alerted) {
            if (AlertTimer > AlertToAttackTime) {
                Pursuant = true;
                Attacking = true;
            }
        }
        if (Pursuant) {
            Pursuant = AI_Boat.gotoPosition(NavigationTarget.position.x, NavigationTarget.transform.position.z);
        }
        if (Attacking) {

        }
        if (Fleeing) {

        }
    }

    public void FollowBoat (int x, int y) {
        Pursuant = true;
        
    }

    public void Fire (int x, int y, int f) {
        AI_Boat.Fire(x, y, f);
    }
}
