using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue {
    public uint id;
    public enum Type {
        PROMPT, CHOICE
    }
    public Type type;
    public string text;
    public List<uint> nextDialogues;
    public string reward;

    public Dialogue(uint id, Type type, string text, uint next0, uint next1, uint next2, string reward) {
        this.id = id;
        this.type = type;
        this.text = text;

        nextDialogues = new List<uint>();
        if (next0 != GameData.INVALID_UID) nextDialogues.Add(next0);
        if (next1 != GameData.INVALID_UID) nextDialogues.Add(next1);
        if (next2 != GameData.INVALID_UID) nextDialogues.Add(next2);

        this.reward = reward;
    }

    override public string ToString() {
        return string.Format("ID {0} ({1}): {2} > {3}", id, type, text, reward);
    }
}
