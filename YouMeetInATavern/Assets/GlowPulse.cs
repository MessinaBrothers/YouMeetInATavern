using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowPulse : MonoBehaviour {

    public float lowAlpha, highAlpha;
    public float frequency;

    private Material mat;

    void Start() {
        mat = GetComponent<Renderer>().material;
    }

    void LateUpdate() {
        // https://steemit.com/gamedev/@superjustin5000/the-incredibly-useful-sine-waves-part-1-using-the-sin-function-trigonometry-game-dev-primer
        float amplitude = (highAlpha - lowAlpha) / 2;
        float waveFrequency = frequency;
        float shiftX = 0f;
        float shiftY = lowAlpha + amplitude;

        //amplitude = 1;
        //waveFrequency = 1;
        //shiftX = 0;
        //shiftY = 0;

        float a = amplitude * (Mathf.Sin(waveFrequency * (Time.time - shiftX))) + shiftY;
        SetAlpha(a);
    }

    private void SetAlpha(float a) {
        Color c = mat.color;
        c.a = a;
        mat.color = c;
    }
}
