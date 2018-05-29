using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterController : MonoBehaviour {

    // "3.5M5L2B1" = after 3.5 seconds, spawn 5 minions, 2 lieutenants, and 1 boss
    public List<string> spawnTimes;

    void Start() {
        string s = "3.51M111L22B3";
        int start = 0;
        float spawnTime = float.Parse(s.Substring(start, s.IndexOf("M") - start));
        print(spawnTime);
        //int minionCount = int.Parse(
        start = s.IndexOf("M") + "M".Length;
        print(ParseMinionCount(s));
        start = s.IndexOf("L") + "L".Length;
        print(s.Substring(start, s.IndexOf("B") - start));
        start = s.IndexOf("B") + "B".Length;
        print(s.Substring(start, s.Length - start));
    }

    void Update() {

    }

    private int ParseMinionCount(string s) {
        int start = s.IndexOf("M") + "M".Length;
        return int.Parse(s.Substring(start, s.IndexOf("L") - start));
    }
}
