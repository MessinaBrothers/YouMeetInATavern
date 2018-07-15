using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour {

    public GameObject[] questionButtons;

    private GameData data;

    void Start() {
        data = FindObjectOfType<GameData>();
    }
    
    public void LoadQuestions(GameObject card) {
        NPC npc = card.GetComponent<NPC>();

        // disable all question buttons
        foreach (GameObject button in questionButtons) {
            button.SetActive(false);
        }

        // ignore unlocked questions if NPC is being introduced
        if (npc.isBeingIntroduced == true) {
            return;
        }

        // get the list of questions
        Dictionary<uint, string> questions = data.npc_questions[npc.npcID];

        int buttonIndex = 0;

        // for each question
        foreach (KeyValuePair<uint, string> kvp in questions) {
            // if the question is unlocked
            if (data.isDialogueIndexUnlocked[kvp.Key]) {
                // activate the next button
                GameObject button = questionButtons[buttonIndex];
                button.SetActive(true);
                buttonIndex += 1;

                // set the button text to the question text
                button.GetComponentInChildren<Text>().text = kvp.Value;

                // what happens when you have more questions than buttons available? ESCAPE
                if (buttonIndex >= questionButtons.Length) {
                    return;
                }
            }
        }
    }
}
