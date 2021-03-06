﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public DebugData debug;

    [Header("Player")]
    public bool canMove;
    public float playerStunTime;
    public float playerKnockbackDistance;

    [Header("Enemy")]
    public float enemyMoveSpeed;
    public float enemyRotSpeed;
    public float enemyReachDistance;
    public float enemyStunTime;
    public float enemyKnockbackDistance;
    public float enemyKnockbackSpeed;

    // once the boss dies, it's game over
    // alternative: once all enemies have died, you win
    // alternative: when the timer reaches zero, you win
    // alternative: once the boss dies, countdown to win
    // alternative: complete the mission, you win. mission varies
    // Mission: Destroy the leader
    // Mission: Save the barkeep
    // Mission: Find the mcguffin
    // Mission: Escort out the VIP
    // Mission: Choose a faction

    // Game plays out over multiple nights
    // First night, tutorial level. Get used to the controls. Fight an intro to the enemy (zombie, skeleton, rat, etc)
    // Second night, talk to patrons. Choose a faction / Gain an ally / Get some items. Fight a mini-boss
    // Third night, final battle. Your decisions give you resources (allies from faction, armored ally, better weapon). Fight the boss
    // Nights aren't timed. You leave and the night ends (or that's when the fight triggers?)

    // NPCs out of talk options:
    // "Leave me alone" "You're being very rude" "I gotta focus now" "I need to rest"
    // Can use multiple of these, getting angrier every time
    // Same NPC: "Leave me alone" > "You're being really rude!" > "STOP" > "GETEMOUTTAHERE" "..."
    // THEN "..."
    [Header("Game End")]
    public bool hasWon;

    [Header("Dialogue")]
    public Dictionary<uint, Dialogue> dialogues;
    public static uint INVALID_UID = 9999999;

    void Awake() {
        dialogues = new Dictionary<uint, Dialogue>();
    }
}
