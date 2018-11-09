using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernController : MonoBehaviour {

    public GameObject tavern;

    void OnEnable() {
        InputController.gameflowStartBeginTavern += ShowTavern;
        InputController.gameflowEndFinishTavern += HideTavern;
    }

    void OnDisable() {
        InputController.gameflowStartBeginTavern -= ShowTavern;
        InputController.gameflowEndFinishTavern -= HideTavern;
    }

    private void HideTavern() {
        tavern.SetActive(false);
    }

    private void ShowTavern(GameData data, uint dayCount) {
        tavern.SetActive(true);
    }
}
