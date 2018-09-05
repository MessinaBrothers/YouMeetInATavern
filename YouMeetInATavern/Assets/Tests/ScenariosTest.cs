using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScenariosTest : MonoBehaviour {

    private static string[] unityMethods = new string[] {
        "Awake",
        "Start",
        "IsUnityMethod",
        "SetUp",
        "CleanUp",
        "GetScriptClassName",
        "GetComponentFastPath",
        "Finalize",
        "MemberwiseClone",
        "obj_address"
    };

    void Awake() {
        string label = string.Format("Testing {0}...", name);
        string results = "\n";

        bool isPassed = true;

        var methods = typeof(ScenariosTest).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
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

        Debug.LogFormat("{0}{1}{2}", label, isPassed == true ? "passed" : "failed", results);

        DestroyImmediate(gameObject);
    }

    private bool IsUnityMethod(string name) {
        foreach (string s in unityMethods) {
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

    private bool Should_ScenariosExist_When_Parse() {
        GameData data = GetComponentInChildren<GameData>();

        GetComponentInChildren<ScenarioParser>().Parse(data);

        return data.scenarios != null && data.scenarios.Length > 0 && data.scenarios[0] != null;
    }

    private bool Should_ScenariosExistWithoutGaps_When_Parse() {
        GameData data = GetComponentInChildren<GameData>();

        GetComponentInChildren<ScenarioParser>().Parse(data);
        
        bool isEndScenarios = false;
        for (int i = 0; i < data.scenarios.Length; i++) {
            if (isEndScenarios == true) {
                if (data.scenarios[i] != null) {
                    return false;
                }
            } else if (data.scenarios[i] == null) {
                isEndScenarios = true;
            }
        }

        return true;
    }
}
