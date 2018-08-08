using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    
    public void OnPointerEnter(PointerEventData eventData) {
        InputController.DeckHover(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        InputController.DeckHover(false);
    }

}
