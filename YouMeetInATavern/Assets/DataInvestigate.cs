using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInvestigate : MonoBehaviour {

    [Header("Dialogue")]
    public Dictionary<uint, Dialogue> dialogues;
    public static uint INVALID_UID = 9999999;

    void Awake() {
        dialogues = new Dictionary<uint, Dialogue>();
    }
}
