using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueButton : MonoBehaviour {

    private string key;

    void Start() {

    }

    void Update() {

    }

    public void BroadcastKey() {
        if (key == "") return;

        InputController.HandleDialogue(key);
    }

    public void SetKey(GameData data, string key) {
        this.key = key;
        
        if (key.Contains("ITEM_")) {
            SetTextColor(data.buttonItemColor);
        } else if (key.Contains("NPC_")) {
            SetTextColor(data.buttonNPCColor);
        } if (key.Contains("LOCATION_")) {
            SetTextColor(data.buttonLocationColor);
        }
    }

    public void SetTextColor(Color c) {
        Text text = GetComponentInChildren<Text>();
        text.color = c;
        text.gameObject.AddComponent<Shadow>();
    }
}
