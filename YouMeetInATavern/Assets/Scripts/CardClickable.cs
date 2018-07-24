using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClickable : MonoBehaviour {
    
    public void Broadcast() {
        InputController.HandleCardClick(gameObject);
    }
}
