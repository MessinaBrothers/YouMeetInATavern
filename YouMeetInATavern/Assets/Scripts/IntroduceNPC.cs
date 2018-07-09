using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroduceNPC : MonoBehaviour {

    public static event NPCIntrocedEventHandler npcIntroducedEventHandler;
    public delegate void NPCIntrocedEventHandler(GameObject card);

    public GameObject cardPrefab;

    public Transform offscreenPos, introPos, enterPos;

    public bool startIntro;

    public float moveIntroTime;
    private float moveIntroTimer;

    private GameObject card;

    private Transform startTransform, endTransform;

    void Start() {
        card = Instantiate(cardPrefab);

        card.transform.position = offscreenPos.position;
        card.transform.rotation = offscreenPos.rotation;
        
        startTransform = offscreenPos;
        endTransform = introPos;
    }

    void Update() {
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
            print("Card is in enter position");
            enabled = false;
        }
    }

    void OnEnable() {
        DialogueButton.dialogueEventHandler += HandleDialogue;
    }

    void OnDisable() {
        DialogueButton.dialogueEventHandler -= HandleDialogue;
    }

    private void HandleDialogue(int key) {
        if (key == 0) {
            moveIntroTimer = moveIntroTime;

            startTransform = introPos;
            endTransform = enterPos;
        }
    }
}
