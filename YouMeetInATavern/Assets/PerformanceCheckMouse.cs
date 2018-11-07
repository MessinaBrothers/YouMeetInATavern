using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceCheckMouse : MonoBehaviour {

    public int helperCount;

	void Start () {
		for (int i = 0; i < helperCount; i++) {
            GameObject go = new GameObject("Test" + i);
            go.transform.SetParent(transform);
            go.AddComponent<PerformanceCheckMouseHelper>();
        }
	}
	
	void Update () {
		
	}
}
