using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue {

    public string key;

    public string text;

    public List<string> playerResponseKeys;
    public List<string> clickableDialogueKeys;
    public string nextDialogueKey;

    public bool isEndOfConversation;

    public enum TYPE {
        INTRO, START, NPC_SAYS, CLICKABLE_TEXT, PLAYER_RESPONSE, STOP
    }
    public TYPE type;

    public Dialogue(string key, string text) {
        this.key = key;
        this.text = text;

        Initialize();
    }

    public Dialogue() {
        Initialize();
    }

    private void Initialize() {
        playerResponseKeys = new List<string>();
        clickableDialogueKeys = new List<string>();

        isEndOfConversation = false;
    }
}
