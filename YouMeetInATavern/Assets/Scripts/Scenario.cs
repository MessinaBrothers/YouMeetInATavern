using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario {

    public uint id;

    public string name;

    public uint endsOnDay;

    public List<string> npcs;

    public Scenario() {
        npcs = new List<string>();
    }

}
