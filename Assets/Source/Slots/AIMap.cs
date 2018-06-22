using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMap {

    //  Update Variables
    public int controllerIndex = 0;

    private List<AIController> AI_Controllers;
    
    public AIMap () {
        AI_Controllers = new List<AIController>();
    }

    public void EnrollEnemyAI (AIController ai) {
        AI_Controllers.Add(ai);
        Debug.Log("AI Successfully Added.");
    }

    public void Update () {

        controllerIndex = (controllerIndex + 1) % AI_Controllers.Count;
    }
}
