using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyTest : MonoBehaviour {

    private static readonly string[] UNITY_METHODS = new string[] {
        "Awake",
        "Start",
        "IsUnityMethod",
        "StartTests",
        "SetUp",
        "CleanUp",
        "GetScriptClassName",
        "GetComponentFastPath",
        "Finalize",
        "MemberwiseClone",
        "obj_address"
    };

    public string StartTests() {
        string label = string.Format("{0}...", GetType().Name);
        string results = "\n";

        bool isPassed = true;

        var methods = GetType().GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        object[] parameters = new Object[0];
        foreach (var method in methods) {
            if (IsUnityMethod(method.Name) == false) {
                // set up initial conditions
                SetUp();
                // run the test
                bool isTestPassed = (bool)method.Invoke(this, parameters);
                // save the log
                results += string.Format("({1}) {0}\n", method.Name, isTestPassed == true ? "pass" : "fail");
                // determing if overall test passed
                if (isTestPassed == false) isPassed = false;
                // clean up for next test
                CleanUp();
            }
        }

        return string.Format("{0}{1}{2}", label, isPassed == true ? "passed" : "failed", results);
    }

    private bool IsUnityMethod(string name) {
        foreach (string s in UNITY_METHODS) {
            if (name.Equals(s)) return true;
        }
        return false;
    }

    private void SetUp() {
        GameObject go = new GameObject("go");
        go.transform.parent = transform;
        GameData data = go.AddComponent<GameData>();
        ScenarioParser scenarioParser = go.AddComponent<ScenarioParser>();
    }

    private void CleanUp() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }
}
