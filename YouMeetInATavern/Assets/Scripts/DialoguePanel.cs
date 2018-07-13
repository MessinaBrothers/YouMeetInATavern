using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePanel : MonoBehaviour {

    public GameObject dialogueLinePrefab;

    void Start() {

    }

    void Update() {

    }

    public void SetDialogue(string text) {
        // clear all children
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        // add an empty object
        GameObject go = new GameObject("Empty", typeof(RectTransform));
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0, 0);
        go.transform.SetParent(transform);
        // add a dialogue line
        GameObject dialogueLine = Instantiate(dialogueLinePrefab);
        dialogueLine.transform.SetParent(transform);
        // set the dialogue text
        dialogueLine.GetComponent<CreateDialogueButtons>().dialogue = text;
    }
}
