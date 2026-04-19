using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip flip, match, mismatch, gameWin, gameLose;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayFlip() => source.PlayOneShot(flip);
    public void PlayMatch() => source.PlayOneShot(match);
    public void PlayMismatch() => source.PlayOneShot(mismatch);
    public void PlayGameWin() => source.PlayOneShot(gameWin);
    public void PlayGameLose() => source.PlayOneShot(gameLose);
}
