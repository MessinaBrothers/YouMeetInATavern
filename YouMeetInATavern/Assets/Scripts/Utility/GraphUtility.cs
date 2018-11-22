using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphUtility {

    public static bool DoesInquiryPass(GameData data, string inquiryKey) {

        Dialogue inquiry = data.key_dialoguesNEW[inquiryKey];
        string commands = inquiry.text;

        foreach (string command in commands.Split(' ')) {
            string[] commandSplit = command.Split(':');

            // add location to answers
            List<string> chosenAnswers = new List<string>(data.chosenAnswerKeys) {
                data.chosenLocation.ToString()
            };

            switch (commandSplit[0]) {
                case "MISSING":
                    if (chosenAnswers.Contains(commandSplit[1])) {
                        return false;
                    }
                    break;
                case "HAS":
                    if (chosenAnswers.Contains(commandSplit[1]) == false) {
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
}
