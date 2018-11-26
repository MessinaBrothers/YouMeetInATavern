using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour {

    public static event NPCStartInTaverneventHandler npcStartInTaverneventHandler;
    public delegate void NPCStartInTaverneventHandler(GameObject card);

    public static event NPCRandomlyLeavesEventHandler npcRandomlyLeavesEventHandler;
    public delegate void NPCRandomlyLeavesEventHandler(GameObject card);

    public GameObject cardPrefab;

    private GameData data;

    private List<GameObject> introducedNPCs;

    void Start() {
        data = FindObjectOfType<GameData>();

        introducedNPCs = new List<GameObject>();
    }

    void Update() {
        if (data.gameMode == GameData.GameMode.INTRODUCE) {

        }
    }

    void OnEnable() {
        InputController.gameflowEndInitialize += CreateCards;
        InputController.gameflowEndBeginTavern += LoadNPCs;
        InputController.gameflowBeginIntroduceNPCs += IntroduceNPCs;
        InputController.cardClickedEventHandler += HandleCardClick;
        InputController.endDialogueEventHandler += IntroduceNextNPC;
    }

    void OnDisable() {
        InputController.gameflowEndInitialize -= CreateCards;
        InputController.gameflowEndBeginTavern -= LoadNPCs;
        InputController.gameflowBeginIntroduceNPCs -= IntroduceNPCs;
        InputController.cardClickedEventHandler -= HandleCardClick;
        InputController.endDialogueEventHandler -= IntroduceNextNPC;
    }

    private void CreateCards() {
        // create parent for cards to go under
        Transform cardParent = new GameObject("NPCs").transform;

        // create cards for each NPC
        foreach (KeyValuePair<string, CardData> kvp in data.cardData) {
            if (kvp.Key.StartsWith("NPC_")) {
                GameObject card = CardFactory.CreateCard(kvp.Key);
                card.transform.parent = cardParent;

                data.npcs.Add(card);

                card.SetActive(false);  
            }
        }
    }

    private void HandleCardClick(GameObject card) {
        if (card.GetComponent<NPC>() == null) return;
        switch (data.gameMode) {
            case GameData.GameMode.INTRODUCE:
                if (card.GetComponent<NPC>().isBeingIntroduced == true) {
                    card.GetComponent<CardSFX>().PlayIntro();
                    InputController.NPCIntroEnd(card);
                    InputController.ChangeMode(GameData.GameMode.CONVERSE);
                    data.selectedCard = card;
                }
                break;
            case GameData.GameMode.CONVERSE:
                break;
            case GameData.GameMode.TAVERN:
                InputController.ChangeMode(GameData.GameMode.CONVERSE);
                data.selectedCard = card;
                break;
        }
    }

    private void IntroduceNextNPC(GameObject lastCard) {
        // check for NPCs to reintroduce, then new NPCs
        //print("Checking NPC introductions...");
        if (data.npcsToReintroduce.Count > 0) {
            GameObject card = data.npcsToReintroduce.Dequeue();
            IntroduceNPC(card);
        } else if (data.npcsToIntroduce.Count > 0) {
            string key = data.npcsToIntroduce.Dequeue();
            IntroduceNPC(key);
            //print("Introducing " + key);
        } else {
            InputController.ChangeMode(GameData.GameMode.TAVERN);
        }
    }

    private void IntroduceNPC(string npcKey) {
        // find the corresponding card GameObject
        foreach (GameObject go in data.npcs) {
            NPC npc = go.GetComponent<NPC>();
            if (npcKey == npc.key) {
                IntroduceNPC(go);
                break;
            }
        }
    }

    private void IntroduceNPC(GameObject card) {
        InputController.ChangeMode(GameData.GameMode.INTRODUCE);

        card.SetActive(true);

        introducedNPCs.Add(card);

        // move it anywhere offscreen so it doesn't appear at the beginning
        card.transform.position = new Vector3(0, -10, 0);

        NPC npc = card.GetComponent<NPC>();
        npc.isBeingIntroduced = true;
        npc.isUnintroduced = false;

        // add NPC to tavern list
        data.npcsInTavern.Add(card);
        
        InputController.NPCIntroStart(card);
    }

    private void LoadNPCs() {
        List<NPC> activeNPCs = new List<NPC>();
        
        // initialize each npc key in this scenario
        foreach (string key in data.scenario.npcs) {
            foreach (GameObject go in data.npcs) {
                NPC npc = go.GetComponent<NPC>();
                if (npc.key == key) {
                    Initialize(npc);

                    // set leaving hour
                    if (npc.key == "NPC_BARTENDER") {
                        npc.hourLeavesTavern = data.tavernCloseHour;
                    } else {
                        // add to shuffle leaving hours
                        activeNPCs.Add(npc);
                    }
                }
            }
        }

        // set the hour in which the NPCs leave
        UnityUtility.Shuffle(activeNPCs);
        int hourLeaving = data.tavernCloseHour;
        int npcCountLeavingAtOneTime = 2;
        for (int i = 0; i < activeNPCs.Count; i++) {
            // if this hour is at max npc capacity
            if (i % npcCountLeavingAtOneTime == 0) {
                // decrement the hour
                hourLeaving -= 1;
                // reset if at opening hour
                if (hourLeaving == data.tavernOpenHour) {
                    hourLeaving = data.tavernCloseHour - 1;
                }
            }
            //Debug.LogFormat("{0} is leaving at {1}", activeNPCs[i].name, hourLeaving);
            activeNPCs[i].hourLeavesTavern = hourLeaving;
        }
    }

    private void IntroduceNPCs() {
        // introduce the first NPC, if any
        IntroduceNextNPC(null);
    }

    private void Initialize(NPC npc) {
        // if the NPC has NOT been introduced
        if (npc.isUnintroduced == true) {
            // introduce the NPC
            data.npcsToIntroduce.Enqueue(npc.key);

            if (data.npcKey_introKey.ContainsKey(npc.key + data.scenario.id)) {
                //print(data.key_dialoguesNEW[data.npcKey_introKey[npc.key + data.scenario.order]].text);
                npc.nextDialogueID = data.npcKey_introKey[npc.key + data.scenario.id];
            } else {
                npc.nextDialogueID = data.npcKey_scenarioKey[npc.key + GameData.DIALOGUE_DEFAULT];
            }
        } else {
            // check if NPC has any result responses for this scenario
            string lastScenarioID = data.scenarios[data.nextScenarioIndex - 1].id;
            if (data.npcKey_resultsKey.ContainsKey(npc.key + lastScenarioID)) {
                string resultsKey = data.npcKey_resultsKey[npc.key + lastScenarioID];
                // check for an appropriate response based on previous answers
                Dialogue inquiry = GetSuccessfulInquiry(data.key_dialoguesNEW[resultsKey].inquiryKeys[0]);
                
                // if an appropriate response has been found
                if (inquiry != null) {
                    // set the NPC's next dialogue
                    npc.nextDialogueID = inquiry.nextDialogueKey;
                    // reintroduce the NPC
                    data.npcsToReintroduce.Enqueue(npc.gameObject);
                } else {
                    // activate the NPC in the tavern
                    npc.nextDialogueID = data.npcKey_scenarioKey[npc.key + GameData.DIALOGUE_DEFAULT];
                    ActivateNPC(npc.gameObject);
                }
            } else {
                // activate the NPC in the tavern
                npc.nextDialogueID = data.npcKey_scenarioKey[npc.key + GameData.DIALOGUE_DEFAULT];
                ActivateNPC(npc.gameObject);
            }
        }
    }

    private Dialogue GetSuccessfulInquiry(string inquiryKey) {
        string nextInquiryKey = data.key_dialoguesNEW[inquiryKey].inquiryKeys[0];

        if (GraphUtility.IsResultInquiryPass(data, inquiryKey)) {
            return data.key_dialoguesNEW[inquiryKey];
        } else if (nextInquiryKey != "") {
            return GetSuccessfulInquiry(nextInquiryKey);
        } else {
            return null;
        }
    }

    private void ActivateNPC(GameObject card) {
        // activate the GameObject
        card.SetActive(true);
        // add NPC to npc list
        data.npcsInTavern.Add(card);
        // broadcast that the NPC has entered the tavern
        npcStartInTaverneventHandler.Invoke(card);
    }
}
