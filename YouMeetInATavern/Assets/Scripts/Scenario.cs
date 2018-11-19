using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario {

    public string id;

    public string name;
    
    /// <summary>
    /// Order this scenario appears in game.
    /// Starts at index 0.
    /// Index of -1 means random order
    /// </summary>
    public uint order;

    /// <summary>
    /// How long the tavern remains open each day
    /// </summary>
    public int openHours;

    public List<string> npcs;

    /// <summary>
    /// How long it takes to fade into the scenario
    /// </summary>
    public float fadeInTime;
    /// <summary>
    /// How long it pauses (after fading) to introduce NPCs
    /// </summary>
    public float introPauseTime;

    public Scenario() {
        npcs = new List<string>();
    }

}
