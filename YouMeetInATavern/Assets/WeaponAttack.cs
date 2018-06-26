using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour {

    public int damage;
    public float range;

    private RandomClip weaponSound;
    private WeaponHitDetect hitDetect;

    private Animator weaponAnimator;

    private int id;

    public bool canAttack;

    void Start() {
        id = GetComponent<EntityID>().id;

        weaponSound = GetComponentInChildren<RandomClip>();
        hitDetect = GetComponentInChildren<WeaponHitDetect>();
        weaponAnimator = GetComponentInChildren<Animator>();

        hitDetect.enabled = false;

        canAttack = true;
    }

    void Update() {
        //if (canAttack == true && Input.GetButtonDown("Fire3")) {
        //    hitDetect.Restart();
        //    weaponSound.PlaySound();
        //    weaponAnimator.SetTrigger("Swing");

        //    canAttack = false;
        //}
    }

    public void Attack() {
        if (canAttack == true) {
            hitDetect.Restart();
            weaponSound.PlaySound();
            weaponAnimator.SetTrigger("Swing");

            canAttack = false;
        }
    }

    private void EndAttack(int attackWeaponID) {
        if (id == attackWeaponID && hitDetect != null) {
            hitDetect.enabled = false;
            canAttack = true;
        }
    }
    
    void OnEnable() {
        WeaponAnimationScripts.attackEndedEventHandler += EndAttack;
    }

    void OnDisable() {
        WeaponAnimationScripts.attackEndedEventHandler -= EndAttack;
    }
}
