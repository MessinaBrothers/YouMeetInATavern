using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConverseNPC : MonoBehaviour {

    public static event NPCStartConverseEventHandler npcStartConverseEventHandler;
    public delegate void NPCStartConverseEventHandler(GameObject card);

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }

    void Update() {

    }

    void OnEnable() {
        InputController.cardClickedEventHandler += HandleCardClick;
        InputController.npcIntroEndEventHandler += HandleIntroduction;
        InputController.dialogueEventHandler += HandleDialogue;
        InputController.continueDialogueEventHandler += ContinueDialogue;
        InputController.endDialogueEventHandler += EndDialogue;
    }

    void OnDisable() {
        InputController.cardClickedEventHandler -= HandleCardClick;
        InputController.npcIntroEndEventHandler -= HandleIntroduction;
        InputController.dialogueEventHandler -= HandleDialogue;
        InputController.continueDialogueEventHandler -= ContinueDialogue;
        InputController.endDialogueEventHandler -= EndDialogue;
    }

    private void HandleIntroduction(GameObject card) {
        // go straight from introduction mode into converse mode
        Converse(card);
    }

    private void HandleCardClick(GameObject card) {
        if (data.gameMode == GameData.GameMode.TAVERN) {
            // if in tavern, start any conversation with NPC
            npcStartConverseEventHandler.Invoke(card);
        }
    }

    private void HandleDialogue(string key) {

    }

    private void Converse(GameObject card) {
    }

    private void ContinueDialogue(GameObject card) {
        NPC npc = card.GetComponent<NPC>();
        npc.isBeingIntroduced = false;

        if (data.DEBUG_IS_PRINT && data.DEBUG_IS_PRINT_DIALOGUE) {
            Debug.LogFormat("Checking scenario-specific dialogue: NPC:{0}, scenario id:{1}", npc.key, data.scenario.id);
        }

        // set the next dialogue

        if (data.npc_dialogues[npc.key].ContainsKey(GameData.DIALOGUE_SCENARIO_PREFIX + data.scenario.id)) {
            // if the NPC has dialogue specific to THIS scenario, use it
            npc.nextDialogueID = GameData.DIALOGUE_SCENARIO_PREFIX + data.scenario.id;

            if (data.DEBUG_IS_PRINT && data.DEBUG_IS_PRINT_DIALOGUE) {
                Debug.LogFormat("An NPC has scenario-specific dialogue: NPC:{0}, scenario id:{1}", npc.key, data.scenario.id);
            }
        } else {
            npc.nextDialogueID = GameData.DIALOGUE_DEFAULT;
        }
    }

    private void EndDialogue(GameObject card) {
        NPC npc = card.GetComponent<NPC>();

        // play the goodbye clip
        card.GetComponent<CardSFX>().PlayGoodbye();
        
        if (npc.isBeingIntroduced) {
            npc.isBeingIntroduced = false;
        } else {
            // increment clock
            data.currentHour += 1;
            InputController.TickClock(data.currentHour);
        }
        InputController.ChangeMode(GameData.GameMode.TAVERN);


        if (data.DEBUG_IS_PRINT && data.DEBUG_IS_PRINT_DIALOGUE) {
            Debug.LogFormat("Checking scenario-specific dialogue: NPC:{0}, scenario id:{1}", npc.key, data.scenario.id);
        }

        // check for specific scenario dialogue
        string specificDialogueKey = npc.key + data.scenario.id;
        if (data.npcKey_scenarioKey.ContainsKey(specificDialogueKey)) {
            // set the specific scenario dialogue
            npc.nextDialogueID = data.npcKey_scenarioKey[specificDialogueKey];

            if (data.DEBUG_IS_PRINT && data.DEBUG_IS_PRINT_DIALOGUE) {
                Debug.LogFormat("An NPC has scenario-specific dialogue: NPC:{0}, scenario id:{1}", npc.key, data.scenario.id);
            }
        } else {
            // set generic dialogue
            npc.nextDialogueID = GameData.DIALOGUE_DEFAULT;
        }
    }
}
