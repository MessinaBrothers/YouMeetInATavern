using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernController : MonoBehaviour {

    public GameObject tavern;

    void OnEnable() {
        InputController.gameflowEndBeginTavern += ShowTavern;
        InputController.gameflowEndFinishTavern += HideTavern;
    }

    void OnDisable() {
        InputController.gameflowEndBeginTavern -= ShowTavern;
        InputController.gameflowEndFinishTavern -= HideTavern;
    }

    private void HideTavern() {
        tavern.SetActive(false);
    }

    private void ShowTavern() {
        tavern.SetActive(true);
    }
}
