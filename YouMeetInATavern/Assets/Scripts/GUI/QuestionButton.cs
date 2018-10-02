using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionButton : MonoBehaviour {

    public Question question;

    public void BroadcastKey() {
        InputController.HandleQuestion(question);
    }
}
