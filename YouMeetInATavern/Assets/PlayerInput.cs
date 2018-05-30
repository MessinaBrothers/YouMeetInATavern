using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    private WeaponAttack weapon;

    void Start() {
        weapon = GetComponentInChildren<WeaponAttack>();
    }

    void Update() {
        if (Input.GetButtonDown("Fire3")) {
            weapon.Attack();
        }
    }
}
