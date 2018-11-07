using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceCheckMouseHelper : MonoBehaviour {

    private static int i;

	void Start () {
		
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
            i += 1;
            if (i == 10000) {
                print("We did it!");
                i = 0;
            }
        }
	}
}
