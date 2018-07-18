using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour {

    public static event NPCCreatedEventHandler npcCreatedEventHandler;
    public delegate void NPCCreatedEventHandler(GameObject card);

    public static event NPCRemovedEventHandler npcRemovedEventHandler;
    public delegate void NPCRemovedEventHandler(GameObject card);

    public static event NPCIntroduceEventHandler npcIntroStartEventHandler;
    public delegate void NPCIntroduceEventHandler(GameObject card);

    public static event NPCIntroducedEventHandler npcIntroEndEventHandler;
    public delegate void NPCIntroducedEventHandler(GameObject card);

    public static event NPCStartInTaverneventHandler npcStartInTaverneventHandler;
    public delegate void NPCStartInTaverneventHandler(GameObject card);

    public GameObject cardPrefab;

    public bool startIntro;

    private GameData data;

    private Transform cardParent;

    private Queue<uint> npcsToIntroduce;

    void Start() {
        data = FindObjectOfType<GameData>();

        // create parent for cards to go under
        GameObject go = new GameObject("NPCs");
        cardParent = go.transform;
    }

    void Update() {
        if (data.gameMode == GameData.GameMode.INTRODUCE) {
            if (startIntro == true) {
                startIntro = false;

                //npcIntroStartEventHandler.Invoke(card);
            }
        }
    }

    void OnEnable() {
        CardClickable.cardClickedEventHandler += HandleCardClick;
        InputController.startTavernEventHandler += ContinueDay;
        InputController.stopConverseEventHandler += IntroduceNextNPC;
    }

    void OnDisable() {
        CardClickable.cardClickedEventHandler -= HandleCardClick;
        InputController.startTavernEventHandler -= ContinueDay;
        InputController.stopConverseEventHandler -= IntroduceNextNPC;
    }

    public void RemoveNPCFromTavern(GameObject card) {
        card.SetActive(false);
        data.npcs.Remove(card);
        npcRemovedEventHandler.Invoke(card);
    }

    private void HandleCardClick(GameObject card) {
        if (data.gameMode == GameData.GameMode.INTRODUCE) {
            if (card.GetComponent<NPC>().isBeingIntroduced == true) {
                card.GetComponent<CardSFX>().PlayIntro();
                npcIntroEndEventHandler.Invoke(card);
            }
        }
    }

    private void IntroduceNextNPC() {
        // if no more NPCs to introduce
        if (npcsToIntroduce.Count == 0) {
            // set game to TAVERN mode
            data.gameMode = GameData.GameMode.TAVERN;
        } else {
            // get the next NPC ID
            uint npcID = npcsToIntroduce.Dequeue();
            IntroduceNPC(npcID);
        }
    }

    private void IntroduceNPC(uint id) {
        print("Introducing NPC " + id);
        data.gameMode = GameData.GameMode.INTRODUCE;

        GameObject card = CreateCard(id);
        // move it anywhere offscreen so it doesn't appear at the beginning
        card.transform.position = new Vector3(0, 1000, 0);

        NPC npc = card.GetComponent<NPC>();
        npc.isBeingIntroduced = true;
        // set the next dialogue to be intro dialogue
        npc.nextDialogueID = GameData.DIALOGUE_INTRO;

        // add NPC to npc list
        data.npcs.Add(card);

        npcCreatedEventHandler.Invoke(card);

        npcIntroStartEventHandler.Invoke(card);
    }

    private GameObject CreateCard(uint npcID) {
        // get NPC data
        NPCData npcData = data.npcData[npcID];

        // create the card gameobject
        GameObject card = Instantiate(cardPrefab);
        card.name = npcData.name + " NPC";
        card.transform.parent = cardParent;

        // set the NPC values
        NPC npc = card.GetComponent<NPC>();
        npc.npcID = npcID;

        // set the card name
        card.GetComponentInChildren<Text>().text = npcData.name;

        // set the card image
        card.GetComponentInChildren<CardImage>().SetImage(Resources.Load<Sprite>("Card Art/" + npcData.imageFile));

        // set the NPC sfx

        return card;
    }

    private void ContinueDay() {
        startIntro = true;

        // place already-introduced NPCs in the tavern
        ActivateNPCs();

        // load list of NPCs to introduce
        if (data.scenario.day_introductions.ContainsKey(data.dayCount)) {
            npcsToIntroduce = new Queue<uint>(data.scenario.day_introductions[data.dayCount]);
        }

        // introduce the first NPC, if any
        IntroduceNextNPC();

        //GameObject card = Instantiate(cardPrefab);
        //card.transform.parent = cardParent;
        //// move it anywhere offscreen so it doesn't appear at the beginning
        //card.transform.position = new Vector3(0, 1000, 0);
        //NPC npc = card.GetComponent<NPC>();
        //card.name = npc.cardName + "NPC";
        //npc.isBeingIntroduced = true;
        //// set the next dialogue to be intro dialogue
        //npc.nextDialogueID = GameData.DIALOGUE_INTRO;

        //// add NPC to npc list
        //data.npcs.Add(card);

        //npcCreatedEventHandler.Invoke(card);

        //npcIntroStartEventHandler.Invoke(card);
    }

    private void ActivateNPCs() {
        foreach (Transform child in cardParent) {
            GameObject card = child.gameObject;
            // activate the GameObject
            card.SetActive(true);
            // add NPC to npc list
            data.npcs.Add(card);
            // broadcast that the NPC has entered the tavern
            npcStartInTaverneventHandler.Invoke(card);
        }
    }
}
