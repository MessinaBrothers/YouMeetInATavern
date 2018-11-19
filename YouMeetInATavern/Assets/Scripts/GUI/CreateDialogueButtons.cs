using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateDialogueButtons : MonoBehaviour {

    public GameObject buttonPrefab, linePrefab;

    public string dialogue;

    public float marginOfError;

    private GameData data;
    private GameObject currentButton;
    private Text currentButtonText;

    private string[] wordsToAdd;
    private int wordsToAddIndex;

    private float widthSum; // track button widths

    private int charIndex;
    private string wordsSoFar, currentWord;
    private string currentKey;

    //private RectTransform rectTransform;

    void Start() {
        data = FindObjectOfType<GameData>();
        //// if the dialogue lacks an index at the start, append one
        //if (dialogue.Length > 0 && dialogue[0] != '<') {
        //    dialogue = "<" + data.selectedCard.GetComponent<NPC>().key + GameData.DIALOGUE_DEFAULT + ">" + dialogue;
        //}
        
        //rectTransform = GetComponent<RectTransform>();
    }

    void Update() {
        if (charIndex < dialogue.Length) {
            char c = dialogue[charIndex];
            //Debug.LogFormat("Parsing char {0} at index {1}", c, charIndex);
            switch (c) {
                case '<':
                    // parse the key
                    int endIndex = dialogue.IndexOf('>', charIndex);
                    currentKey = endIndex - charIndex > 1 ? dialogue.Substring(charIndex + 1, endIndex - charIndex - 1) : GameData.DIALOGUE_INVALID;
                    charIndex = endIndex;

                    // save the last button width
                    if (currentButton != null) {
                        widthSum += currentButton.GetComponent<RectTransform>().rect.width;
                    }

                    // clear the current words
                    wordsSoFar = "";
                    currentWord = "";

                    // start a new button
                    CreateButton(currentKey);
                    break;
                default:
                    // if we encounter a space, append the last word to words so far
                    if (c == ' ') {
                        wordsSoFar += " " + currentWord;
                        currentWord = "";
                    } else {
                        currentWord += c;
                        currentButtonText.text = wordsSoFar + " " + currentWord;
                        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

                        // determing if button is too big
                        float lastWidth = currentButton.GetComponent<RectTransform>().rect.width;
                        float width = GetComponent<RectTransform>().rect.width;
                        if (widthSum + lastWidth + marginOfError > width) {
                            // if the button is empty, destroy it
                            if (wordsSoFar == "") {
                                Destroy(currentButton);
                            } else {
                                // set the button text, ignoring the current word
                                currentButtonText.text = wordsSoFar;
                                LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
                            }

                            CreateLine();
                        }
                    }
                    break;
            }
            charIndex += 1;
        }
    }

    private void CreateButton(string key) {
        currentButton = Instantiate(buttonPrefab);
        currentButtonText = currentButton.GetComponentInChildren<Text>();
        currentButtonText.text = "";
        currentButton.transform.SetParent(transform, false);

        // set the key
        currentButton.GetComponent<DialogueButton>().SetKey(data, key);
    }

    private void CreateLine() {
        // create a new line
        GameObject newLine = Instantiate(linePrefab);

        // parse the remaining dialogue
        string remainingDialogue = dialogue.Substring(charIndex - currentWord.Length + 1).Trim();
        // append current key to the beginning

        remainingDialogue = "<" + currentKey + ">" + remainingDialogue;
        // save the dialogue
        newLine.GetComponent<CreateDialogueButtons>().dialogue = remainingDialogue;
        
        // destroy any children of the new line (bug?)
        foreach (Transform child in newLine.transform) {
            Destroy(child.gameObject);
        }

        // set the parent to this gameobject's parent
        newLine.transform.SetParent(transform.parent);

        // Disable ourselves; nothing more to do
        enabled = false;
    }
}
