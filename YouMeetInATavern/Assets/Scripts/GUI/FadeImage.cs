﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour {

    public bool fadeIn, fadeOut;

    private RawImage image;

    private float fadeTime, timer;

    void Start() {
        image = GetComponent<RawImage>();
    }

    void Update() {
        if (timer < fadeTime) {
            timer += Time.deltaTime;
            if (fadeIn == true) {
                SetAlpha(Mathf.Lerp(1, 0, timer / fadeTime));
                if (timer >= fadeTime) {
                    fadeIn = false;
                }
            } else if (fadeOut == true) {
                SetAlpha(Mathf.Lerp(0, 1, timer / fadeTime));
                if (timer >= fadeTime) {
                    fadeOut = false;
                }
            }
        }
    }

    public void FadeIn(float time) {
        fadeIn = true;
        fadeOut = false;
        BeginFade(1, time);
    }

    public void FadeOut(float time) {
        fadeOut = true;
        fadeIn = false;
        BeginFade(0, time);
    }

    private void BeginFade(float a, float time) {
        SetAlpha(a);
        fadeTime = time;
        timer = 0;
    }

    private void SetAlpha(float a) {
        Color c = image.color;
        c.a = a;
        image.color = c;
    }
}
