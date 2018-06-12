using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour {

    // "Want something?" >Yes >No
    // "Quiet night out, wouldn't ya say?" >Any rumors? >Let's hope it stays that way
    // "This scar? I used to be a soldier." >Are you any good in a fight? >Got any old equipment laying around?

    void Start() {

    }

    void Update() {

    }

    void OnEnable() {
        PlayerInput.playerInteractEventHandler += Chat;
    }

    void OnDisable() {
        PlayerInput.playerInteractEventHandler -= Chat;
    }

    private void Chat(GameObject interactable) {
        if (interactable == gameObject) {
            print("a;lsdkfads;lfkjadsf");
        }
    }
}
