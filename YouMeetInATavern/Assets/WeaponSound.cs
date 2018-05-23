using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSound : MonoBehaviour {

    private static System.Random rng = new System.Random();

    public AudioClip[] clips;

    private AudioSource audioSource;

    private int clipIndex;

    void Start() {
        audioSource = GetComponent<AudioSource>();

        Shuffle(clips);
        clipIndex = 0;
    }

    public void PlaySound() {
        audioSource.PlayOneShot(clips[clipIndex]);
        clipIndex++;

        if (clipIndex == clips.Length) {
            Shuffle(clips);
            clipIndex = 0;
        }
    }

    private void Shuffle<T>(IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
