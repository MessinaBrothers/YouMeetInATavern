using AuraAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCards : MonoBehaviour {

    public AudioClip unselectClip;

    private AudioSource audioSource;

    private GameObject selectedFirst, selectedSecond;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Select(GameObject card) {
        CardZoom selectedZoom = card.GetComponent<CardZoom>();

        // check if card is already selected
        if (selectedFirst == card || selectedSecond == card) {
            // unzoom selected card
            selectedZoom.Unzoom();
            // play unselect sound
            audioSource.PlayOneShot(unselectClip);
            // highlight the card
            card.GetComponentInChildren<AuraVolume>().enabled = false;

            // move selected choices (first and second)
            if (selectedFirst == card) {
                // second becomes first
                selectedFirst = selectedSecond;
            }
            // clear second
            selectedSecond = null;
        } else {
            // bring card forward
            selectedZoom.Zoom();
            // play sound effect
            card.GetComponent<CardSFX>().PlayGreeting();
            // highlight the card
            card.GetComponentInChildren<AuraVolume>().enabled = true;

            if (selectedFirst == null) {
                selectedFirst = card;
            } else if (selectedSecond == null) {
                selectedSecond = card;
            } else {
                // unzoom first card
                selectedFirst.GetComponent<CardZoom>().Unzoom();
                selectedFirst.GetComponentInChildren<AuraVolume>().enabled = false;
                // set second card as first
                selectedFirst = selectedSecond;
                // set selected card as second
                selectedSecond = card;
            }
        }
    }
}
