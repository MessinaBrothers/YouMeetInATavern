using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConverseNPC : MonoBehaviour {

    public static event NPCStartedTalkingEventHandler npcStartedTalkingEventHandler;
    public delegate void NPCStartedTalkingEventHandler(GameObject card);

    public Transform conversePos, enterPos;
    private Transform startTransform, endTransform;

    public float moveIntroTime;
    private float moveIntroTimer;

    private GameData data;

    private GameObject card;

    void Start() {
        data = FindObjectOfType<GameData>();

        // we need a new transform to copy the card's initial transform into our start transform
        GameObject go = new GameObject("ConverseNPC_startTransform");
        go.transform.parent = transform;
        startTransform = go.transform;
    }

    void Update() {
        if (data.gameMode == GameData.GameMode.CONVERSE) {
            if (moveIntroTimer > 0) {
                moveIntroTimer -= Time.deltaTime;
                card.transform.position = Vector3.Slerp(endTransform.position, startTransform.position, moveIntroTimer / moveIntroTime);
                card.transform.rotation = Quaternion.Slerp(endTransform.rotation, startTransform.rotation, moveIntroTimer / moveIntroTime);

                if (card.transform.position == conversePos.position) {
                    npcStartedTalkingEventHandler.Invoke(card);
                }
            }
        }

    }

    void OnEnable() {
        CardClickable.cardClickedEventHandler += HandleCardClick;
        IntroduceNPC.npcIntroducedEventHandler += HandleIntroduction;
    }

    void OnDisable() {
        CardClickable.cardClickedEventHandler -= HandleCardClick;
        IntroduceNPC.npcIntroducedEventHandler -= HandleIntroduction;
    }

    private void HandleIntroduction(GameObject card) {
        // go straight from introduction mode into converse mode
        print("ASDFADSF");
        Converse(card);
    }

    private void HandleCardClick(GameObject card) {
        // start any conversation with NPC if in tavern mode
        if (data.gameMode == GameData.GameMode.TAVERN) {
            Converse(card);
        // if already conversing, play a sound
        } else if (data.gameMode == GameData.GameMode.CONVERSE) {
            card.GetComponent<CardSFX>().PlayGreeting();
        }
    }

    private void Converse(GameObject card) {
        // save the card for future use
        this.card = card;

        // copy the card's position and rotation into our startTransform
        startTransform.position = card.transform.position;
        startTransform.rotation = card.transform.rotation;

        // set the end transform to the conversation transform
        endTransform = conversePos;

        // reset the move timer
        moveIntroTimer = moveIntroTime;

        // set the game mode to converse
        data.gameMode = GameData.GameMode.CONVERSE;
    }
}
