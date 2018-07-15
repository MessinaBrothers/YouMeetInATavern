using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayController : MonoBehaviour {

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }

    void OnEnable() {
        NPCController.npcCreatedEventHandler += AddNPC;
    }

    void OnDisable() {
        NPCController.npcCreatedEventHandler -= AddNPC;
    }

    private void AddNPC(GameObject card) {
        // increment npc count
        data.tavernNPCCount += 1;
    }

}
