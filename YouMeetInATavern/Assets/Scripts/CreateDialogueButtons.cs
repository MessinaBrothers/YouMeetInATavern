using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateDialogueButtons : MonoBehaviour {

    public GameObject buttonPrefab, linePrefab;

    public string dialogue;

    public float marginOfError;

    private GameObject currentButton;
    private Text currentButtonText;

    private string[] wordsToAdd;
    private int wordsToAddIndex;

    private float widthSum; // track button widths

    private int charIndex;
    private string wordsSoFar, currentWord;

    void Start() {
        print(dialogue);

        // if the dialogue lacks an index at the start, append one
        if (dialogue[0] == '>') {
            dialogue = dialogue.Substring(1);
        }
        if (dialogue[0] != '<') {
            dialogue = "<>" + dialogue;
        }
    }

    void Update() {
        if (charIndex < dialogue.Length) {
            char c = dialogue[charIndex];
            //Debug.LogFormat("Parsing char {0} at index {1}", c, charIndex);
            switch (c) {
                case '<':
                    // parse the key
                    int endIndex = dialogue.IndexOf('>', charIndex);
                    int key = endIndex - charIndex > 1 ? int.Parse(dialogue.Substring(charIndex + 1, endIndex - charIndex - 1)) : 0;
                    charIndex = endIndex;

                    // save the last button width
                    if (currentButton != null) {
                        widthSum += currentButton.GetComponent<RectTransform>().rect.width;
                    }

                    // clear the current words
                    wordsSoFar = "";
                    currentWord = "";

                    // start a new button
                    CreateButton(key);
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
                            // set the button text, ignoring the current word
                            currentButtonText.text = wordsSoFar;
                            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

                            CreateLine();
                        }
                    }
                    break;
            }
            charIndex += 1;
        }
    }

    private void CreateButton(int key) {
        currentButton = Instantiate(buttonPrefab);
        currentButtonText = currentButton.GetComponentInChildren<Text>();
        currentButtonText.text = "";
        currentButton.transform.SetParent(transform, false);
    }

    private void CreateLine() {
        string remainingDialogue = dialogue.Substring(charIndex - currentWord.Length).Trim();

        // create a new line
        GameObject newLine = Instantiate(linePrefab);
        newLine.GetComponent<CreateDialogueButtons>().dialogue = remainingDialogue;

        foreach (Transform child in newLine.transform) {
            Destroy(child.gameObject);
        }

        newLine.transform.SetParent(transform.parent);

        // nothing more to do here
        enabled = false;
    }
}
