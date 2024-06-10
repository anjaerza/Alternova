using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] AudioClip victory;
    [SerializeField] AudioClip pair;
    [SerializeField] AudioClip clickBlock;
    [SerializeField] AudioClip button;
    [SerializeField] AudioClip miss;

    AudioSource audioSource;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(button);
    }
    public void PlayVictory()
    {
        audioSource.PlayOneShot(victory);
    }
    public void PlayPair()
    {
        audioSource.PlayOneShot(pair);
    }
    public void PlayMiss()
    {
        audioSource.PlayOneShot(miss);
    }
    public void PlayClickBlock()
    {
        audioSource.PlayOneShot(clickBlock);
    }
}
