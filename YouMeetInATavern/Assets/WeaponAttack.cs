using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour {

    public GameObject weapon;

    private RandomClip weaponSound;

    private Animator weaponAnimator;

    void Start() {
        weaponSound = weapon.GetComponentInChildren<RandomClip>();
        weaponAnimator = weapon.GetComponentInChildren<Animator>();
    }

    void Update() {
        if (Input.GetButtonDown("Fire3")) {
            weaponSound.PlaySound();
            weaponAnimator.SetTrigger("Swing");
        }
    }
}
