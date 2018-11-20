using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }

    void Update() {

    }

    void OnEnable() {
        InputController.endDialogueEventHandler += EndDialogue;
        InputController.dialogueEventHandler += HandleDialogue;
        InputController.cardClickedEventHandler += HandleCardClick;
        InputController.questionEventHandler += HandleQuestion;
        InputController.createCardEventHandler += CreateCard;
        InputController.dialogueSettingEventHandler += HandleSetting;
    }

    void OnDisable() {
        InputController.endDialogueEventHandler -= EndDialogue;
        InputController.dialogueEventHandler -= HandleDialogue;
        InputController.cardClickedEventHandler -= HandleCardClick;
        InputController.questionEventHandler -= HandleQuestion;
        InputController.createCardEventHandler -= CreateCard;
        InputController.dialogueSettingEventHandler -= HandleSetting;
    }

    private void HandleCardClick(GameObject card) {
        if (data.gameMode == GameData.GameMode.TAVERN) {
            // if in tavern, start any conversation with NPC
            InputController.NPCConverseStart(card);
        }
    }

    private void HandleDialogue(string key) {
        // get the Dialogue
        Dialogue dialogue = data.key_dialoguesNEW[key];

        foreach (string cardID in dialogue.unlockCardKeys) {
            print("unlocking: " + cardID);
            InputController.CreateCard(cardID);
        }
    }

    private void HandleSetting(string arg) {
        //print("Handling setting: " + arg);
        if (arg == "GOODBYE=FALSE") {
            data.isGoodbyeEnabled = false;
        } else if (arg == "GOODBYE=TRUE") {
            data.isGoodbyeEnabled = true;
        }
    }

    public void HandleQuestion(Dialogue question) {
        //int endIDIndex = question.text.IndexOf('>');
        //string unlockKey = question.text.Substring("<".Length, endIDIndex - "<".Length);

        //// unlock the dialogue
        //DeckController.Add(unlockKey);
    }

    public void CreateCard(string unlockKey) {
        // unlock the dialogue
        DeckController.Add(unlockKey);
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
            npc.nextDialogueID = npc.key + GameData.DIALOGUE_DEFAULT;
        }
    }
}
