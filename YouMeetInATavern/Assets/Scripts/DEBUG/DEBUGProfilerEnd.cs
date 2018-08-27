using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUGProfilerEnd : MonoBehaviour {
    
    void Update() {
        GetComponent<DEBUGProfiler>().EndLoadProfiler();
        enabled = false;
    }
}
