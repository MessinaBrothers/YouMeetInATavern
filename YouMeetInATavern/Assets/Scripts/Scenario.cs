using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario {

    public uint id;

    public string name;

    public uint endsOnDay;

    public Dictionary<uint, List<string>> day_introductions;

    public Scenario() {
        day_introductions = new Dictionary<uint, List<string>>();
    }

}
