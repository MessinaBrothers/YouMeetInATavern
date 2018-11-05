using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernController : MonoBehaviour {

    public GameObject tavern;

    void OnEnable() {
        InputController.gameflowEndBeginDay += ShowTavern;
        InputController.gameflowEndBeginNight += HideTavern;
    }

    void OnDisable() {
        InputController.gameflowEndBeginDay -= ShowTavern;
        InputController.gameflowEndBeginNight -= HideTavern;
    }

    private void HideTavern() {
        tavern.SetActive(false);
    }

    private void ShowTavern() {
        tavern.SetActive(true);
    }
}
