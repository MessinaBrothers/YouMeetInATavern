using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScenariosTest : MyTest {

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
