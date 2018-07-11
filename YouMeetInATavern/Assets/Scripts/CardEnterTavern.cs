using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEnterTavern : MonoBehaviour {

    public static event NPCEnteredTavernEventHandler npcEnteredTavernEventHandler;
    public delegate void NPCEnteredTavernEventHandler(GameObject card);

    public Transform enterPos;
    private Transform startTransform, endTransform;

    public float moveIntroTime;
    private float moveIntroTimer;

    private GameData data;

    private GameObject card;

    void Start() {

    }

    void Update() {
        if (moveIntroTimer > 0) {
            moveIntroTimer -= Time.deltaTime;
            card.transform.position = Vector3.Slerp(endTransform.position, startTransform.position, moveIntroTimer / moveIntroTime);
            card.transform.rotation = Quaternion.Slerp(endTransform.rotation, startTransform.rotation, moveIntroTimer / moveIntroTime);

            if (card.transform.position == enterPos.position) {
                npcEnteredTavernEventHandler.Invoke(card);
            }
        }
    }
}
