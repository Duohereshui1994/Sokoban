using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    // 静态变量，用于跟踪是否已经存在 BackgroundMusic 实例
    private static BackgroundMusic instance;

    private AudioSource audioSource;
    public AudioClip backgroundMusic;

    void Start()
    {
        // 如果已经存在实例，则销毁当前实例
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 将当前实例设置为静态变量
        instance = this;

        audioSource = GetComponent<AudioSource>();

        audioSource.clip = backgroundMusic;
        audioSource.loop = true;

        audioSource.Play();

        DontDestroyOnLoad(gameObject);
    }
}
