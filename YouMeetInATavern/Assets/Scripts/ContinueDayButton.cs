using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueDayButton : MonoBehaviour {
    
    public void BroadcastClick() {
        InputController.ContinueDay();
    }
}
