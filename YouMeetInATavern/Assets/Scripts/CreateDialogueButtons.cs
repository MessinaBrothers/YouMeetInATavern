using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateDialogueButtons : MonoBehaviour {

    public GameObject buttonPrefab;

    public string dialogue = "<>Sit anywhere you like.<>The tavern's dead right now.";

    public float marginOfError;

    private GameObject lastButton;
    private Text lastButtonText;

    private string[] wordsToAdd;
    private int wordsToAddIndex;

    private string wordsSoFar;

    private float widthSum; // track button widths

    void Start() {
        CreateButtons(dialogue);
    }

    void Update() {
        //// add words, one at a time
        //if (wordsToAddIndex < wordsToAdd.Length) {
        //    lastButtonText.text += " " + wordsToAdd[wordsToAddIndex];
        //    wordsToAddIndex += 1;
        //    LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

        //    // determine button width
        //    Rect lastRect = lastButton.GetComponent<RectTransform>().rect;
        //    float width = GetComponent<RectTransform>().rect.width;
        //    print(width);
        //    if (widthSum + lastRect.width + marginOfError > width) {
        //        print("Button is too big!");
        //        wordsToAddIndex = int.MaxValue;
        //        lastButtonText.text = wordsSoFar;
        //        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        //    } else {
        //        print("Button fits");
        //        wordsSoFar = lastButtonText.text;
        //    }
        //}
    }

    void OnGUI() {
        // add words, one at a time
        if (wordsToAddIndex < wordsToAdd.Length) {
            lastButtonText.text += " " + wordsToAdd[wordsToAddIndex];
            wordsToAddIndex += 1;
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

            // determine button width
            Rect lastRect = lastButton.GetComponent<RectTransform>().rect;
            float width = GetComponent<RectTransform>().rect.width;
            print(width);
            if (widthSum + lastRect.width + marginOfError > width) {
                print("Button is too big!");
                wordsToAddIndex = int.MaxValue;
                lastButtonText.text = wordsSoFar;
                LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            } else {
                print("Button fits");
                wordsSoFar = lastButtonText.text;
            }
        }
    }

    private void CreateButtons(string dialogue) {
        CreateButton("Sit anywhere you like. Words will be added to make the line too long.");
        //CreateButton("Tavern's dead right now.");
    }

    private void CreateButton(string snippet) {
        wordsToAdd = snippet.Split(' ');
        wordsToAddIndex = 0;

        wordsSoFar = "";

        lastButton = Instantiate(buttonPrefab);
        lastButtonText = lastButton.GetComponentInChildren<Text>();
        lastButtonText.text = "";
        lastButton.transform.SetParent(transform, false);

        // TODO determine if button is too big
        // If too big, cut a word and retry
        // Keep doing until no words left
        // Create a new Panel with the remaining dialogue
    }
}
