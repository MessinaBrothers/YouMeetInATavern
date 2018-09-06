using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFactory : MonoBehaviour {

    public GameObject npcCardPrefab;

    private static GameData data;
    private static GameObject npcCardPrefabWrapper;

    void Start() {
        data = FindObjectOfType<GameData>();

        // set static variables
        npcCardPrefabWrapper = npcCardPrefab;
    }

    public static GameObject CreateCard(string key) {
        if (key.StartsWith("NPC_")) {
            return CreateNPCCard(key);
        } else if (key.StartsWith("ITEM_")) {
            return CreateItemCard(key);
        }
        throw new System.ArgumentException("ERROR: Key does not exist in card data: " + key);
    }

    public static GameObject CreateDeckCard(string key) {
        GameObject card = CreateCardHelper(key);

        // disable button so Player can't click on this card
        card.GetComponentInChildren<Button>().enabled = false;

        return card;
    }

    private static GameObject CreateNPCCard(string key) {
        GameObject card = CreateCardHelper(key);

        // get NPC data
        CardData npcData = data.cardData[key];

        // set the NPC values
        NPC npc = card.AddComponent<NPC>();
        npc.key = key;
        npc.isUnintroduced = true;
        
        // set the NPC sfx
        CardSFX sfx = card.GetComponent<CardSFX>();
        sfx.introductionClip = Resources.Load<AudioClip>("Card SFX/" + npcData.sfxIntro);
        sfx.greetingClips = Convert("Card SFX/", npcData.sfxOnClicks);
        sfx.startConversationClips = Convert("Card SFX/", npcData.sfxGreetings);
        sfx.goodbyeClips = Convert("Card SFX/", npcData.sfxGoodbyes);

        return card;
    }

    private static GameObject CreateItemCard(string key) {
        GameObject card = CreateCardHelper(key);

        // set the Item values
        Key keyComponent = card.AddComponent<Key>();
        keyComponent.key = key;

        // get item data
        CardData itemData = data.cardData[key];

        // set the NPC sfx
        CardSFX sfx = card.GetComponent<CardSFX>();
        sfx.greetingClips = Convert("Card SFX/", itemData.sfxOnClicks);

        return card;
    }

    private static GameObject CreateCardHelper(string key) {
        // get card data
        CardData cardData = data.cardData[key];

        // create the card gameobject
        GameObject card = Instantiate(npcCardPrefabWrapper);
        card.name = cardData.name;

        // set the card name text
        foreach (Text text in card.GetComponentsInChildren<Text>()) {
            text.text = cardData.name;
        }

        // set the card image
        card.GetComponentInChildren<CardImage>().SetImage(Resources.Load<Sprite>("Card Art/" + cardData.imageFile));

        return card;
    }

    // convert array of file locations to an array of audio clips
    private static AudioClip[] Convert(string fileLocation, string[] clipFileNames) {
        if (clipFileNames == null) {
            return new AudioClip[0];
        } else {
            AudioClip[] clips = new AudioClip[clipFileNames.Length];
            for (int i = 0; i < clipFileNames.Length; i++) {
                clips[i] = Resources.Load<AudioClip>(fileLocation + clipFileNames[i]);
            }
            return clips;
        }
    }
}
