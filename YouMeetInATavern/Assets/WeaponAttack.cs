using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour {

    public GameObject weapon;

    private RandomClip weaponSound;
    private WeaponHitDetect hitDetect;

    private Animator weaponAnimator;

    void Start() {
        weaponSound = weapon.GetComponentInChildren<RandomClip>();
        hitDetect = weapon.GetComponentInChildren<WeaponHitDetect>();
        weaponAnimator = weapon.GetComponentInChildren<Animator>();

        hitDetect.enabled = false;
    }

    void Update() {
        if (Input.GetButtonDown("Fire3")) {
            hitDetect.Restart();
            weaponSound.PlaySound();
            weaponAnimator.SetTrigger("Swing");
        }
    }

    public void EndAttack() {
        hitDetect.enabled = false;
    }
}
