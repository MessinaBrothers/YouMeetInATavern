using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphUtility {

    public static bool IsTagInquiryPass(GameData data, string inquiryKey) {
        return IsInquiryPass(data, inquiryKey, data.unlockedDialogueKeys);
    }

    public static bool IsResultInquiryPass(GameData data, string inquiryKey) {
        // add location to answers
        List<string> chosenAnswers = new List<string>(data.chosenAnswerKeys) {
                data.chosenLocation.ToString()
            };
        return IsInquiryPass(data, inquiryKey, chosenAnswers);
    }

    private static bool IsInquiryPass(GameData data, string inquiryKey, ICollection<string> list) {

        Dialogue inquiry = data.key_dialoguesNEW[inquiryKey];
        string commands = inquiry.text;

        foreach (string command in commands.Split(' ')) {
            Debug.Log("Checking: " + command);
            string[] commandSplit = command.Split(':');

            switch (commandSplit[0]) {
                case "MISSING":
                    if (list.Contains(commandSplit[1])) {
                        return false;
                    }
                    break;
                case "HAS":
                    if (list.Contains(commandSplit[1]) == false) {
                        return false;
                    }
                    break;
                default:
                    Debug.Log("ERROR: cannot parse the command: " + command);
                    return false;
            }
        }
        
        return true;
    }

    public static bool IsType(GameData data, string dialogueKey, Dialogue.TYPE type) {
        if (dialogueKey == null || dialogueKey.Length == 0) {
            return false;
        } else if (data.key_dialoguesNEW.ContainsKey(dialogueKey)) {
            return data.key_dialoguesNEW[dialogueKey].type.Equals(type);
        } else {
            return false;
        }
    }
}
