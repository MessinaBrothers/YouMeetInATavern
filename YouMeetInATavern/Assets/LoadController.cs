using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadController : MonoBehaviour {

    public GameObject startingWeaponPrefab;
    
	void Start () {
        // load the player's weapon icon on game start
        // in the future: equip the weapon for the player?
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
	}
	
	void Update () {
		
	}
}
