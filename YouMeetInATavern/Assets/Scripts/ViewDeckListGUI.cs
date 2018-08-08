using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ViewDeckListGUI : MonoBehaviour {

    public GameObject ListItemprefab;

    private GameData data;
    
    // ordered list for traversal
    private List<GameObject> listItems;
    // dictionary for activating by key
    private Dictionary<string, GameObject> key_listItem;
    
    public void Load(GameData data, List<string> unlockKeys) {
        this.data = data;
        listItems = new List<GameObject>();
        key_listItem = new Dictionary<string, GameObject>();

        for (int i = 0; i < unlockKeys.Count; i++) {
            // create ListItem prefab for each card
            GameObject listItem = Instantiate(ListItemprefab);
            // add ListItem to ourselves
            listItem.transform.SetParent(transform);
            // set ListItem values
            listItem.GetComponent<DeckListItem>().SetText(data.cardData[unlockKeys[i]].name);
            // disable ListItem
            listItem.SetActive(false);
            // add to list
            listItems.Add(listItem);
            // add to dictionary
            key_listItem.Add(unlockKeys[i], listItem);
        }
    }
    
    public void Display() {
        foreach (KeyValuePair<string, GameObject> kvp in key_listItem) {
            // activate ListItem if key is unlocked
            kvp.Value.SetActive(data.unlockedDialogueKeys.Contains(kvp.Key));
        }
    }
}
