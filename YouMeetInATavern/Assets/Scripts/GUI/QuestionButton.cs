using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionButton : MonoBehaviour {
    
    public Dialogue dialogue;

    public void BroadcastKey() {
        InputController.HandleQuestion(dialogue);
    }
}
