using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroduceNPC : MonoBehaviour {

    public GameObject cardPrefab;

    public Transform offscreenPos, introPos;

    public bool startIntro;

    public float moveIntroTime;
    private float moveIntroTimer;

    private GameObject card;

    void Start() {
        card = Instantiate(cardPrefab);

        card.transform.position = offscreenPos.position;
        card.transform.rotation = offscreenPos.rotation;
    }

    void Update() {
        if (startIntro == true) {
            startIntro = false;

            moveIntroTimer = moveIntroTime;
        }

        if (moveIntroTimer > 0) {
            moveIntroTimer -= Time.deltaTime;
            card.transform.position = Vector3.Slerp(introPos.position, offscreenPos.position, moveIntroTimer / moveIntroTime);
        }
    }
}
