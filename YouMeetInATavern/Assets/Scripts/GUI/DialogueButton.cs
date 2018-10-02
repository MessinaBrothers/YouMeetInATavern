using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueButton : MonoBehaviour {

    private string key;
    private List<string> keys;

    void Awake() {
        keys = new List<string>();
    }

    void Update() {

    }

    public void BroadcastKey() {
        //if (key == "") return;
        
        //InputController.HandleDialogue(key);

        foreach (string key in keys) {
            if (key.Contains(GameData.DIALOGUE_SETTING)) {
                InputController.HandleDialogueSetting(key);
            } else {
                InputController.HandleDialogue(key);
            }
        }
    }

    public void SetKey(GameData data, string key) {
        //this.key = key;

        foreach (string k in key.Split(GameData.PARSER_DELIMITER)) {
            keys.Add(k);
        }
        
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
