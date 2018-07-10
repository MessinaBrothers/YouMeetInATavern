using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroduceNPC : MonoBehaviour {

    public static event NPCEnteredTavernEventHandler npcEnteredTavernEventHandler;
    public delegate void NPCEnteredTavernEventHandler(GameObject card);

    public static event NPCIntroducedEventHandler npcIntroducedEventHandler;
    public delegate void NPCIntroducedEventHandler(GameObject card);

    public GameObject cardPrefab;
    public Transform offscreenPos, introPos, enterPos;

    public bool startIntro;
    public float moveIntroTime;
    private float moveIntroTimer;

    private GameData data;

    private GameObject card;
    private Transform startTransform, endTransform;

    void Start() {
        data = FindObjectOfType<GameData>();

        card = Instantiate(cardPrefab);

        Introduce(card);
    }

    void Update() {
        if (data.gameMode == GameData.GameMode.INTRODUCE) {
            if (startIntro == true) {
                startIntro = false;

                moveIntroTimer = moveIntroTime;
            }

            if (moveIntroTimer > 0) {
                moveIntroTimer -= Time.deltaTime;
                card.transform.position = Vector3.Slerp(endTransform.position, startTransform.position, moveIntroTimer / moveIntroTime);
                card.transform.rotation = Quaternion.Slerp(endTransform.rotation, startTransform.rotation, moveIntroTimer / moveIntroTime);
            }
        
            if (card.transform.position == enterPos.position) {
                npcEnteredTavernEventHandler.Invoke(card);
                enabled = false;
            }
        }
    }

    void OnEnable() {
        DialogueButton.dialogueEventHandler += HandleDialogue;
        CardClickable.cardClickedEventHandler += HandleCardClick;
    }

    void OnDisable() {
        DialogueButton.dialogueEventHandler -= HandleDialogue;
        CardClickable.cardClickedEventHandler -= HandleCardClick;
    }

    public void Introduce(GameObject card) {
        this.card = card;

        card.transform.position = offscreenPos.position;
        card.transform.rotation = offscreenPos.rotation;

        startTransform = offscreenPos;
        endTransform = introPos;
    }

    private void HandleDialogue(int key) {
        //if (key == 0) {
        //    moveIntroTimer = moveIntroTime;

        //    startTransform = introPos;
        //    endTransform = enterPos;

        //    data.gameMode = GameData.GameMode.TAVERN;
        //}
    }

    private void HandleCardClick(GameObject card) {
        if (data.gameMode == GameData.GameMode.INTRODUCE && card == this.card) {
            card.GetComponent<CardSFX>().PlayIntro();
            npcIntroducedEventHandler.Invoke(card);
        }
    }
}
