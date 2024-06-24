﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Gift : MonoBehaviour
{
    private Audio audioManager;
    private AudioSource audioSource;
    public AudioClip soundWin;
    public ParticleSystem fireworksEffect;

    private void Start()
    {
        audioManager = FindObjectOfType<Audio>();
        audioSource = GetComponent<AudioSource>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }

        if (fireworksEffect == null)
        {
            fireworksEffect = GetComponent<ParticleSystem>();
        }

        if (fireworksEffect != null)
        {
            fireworksEffect.Stop();
        }
    }

    private void OnMouseDown()
    {
        StartCoroutine(ZoomAndPlayEffect());
    }

    private IEnumerator ZoomAndPlayEffect()
    {
        // Dừng nhạc từ AudioManager
        if (audioManager != null)
        {
            audioManager.StopMusic();
        }
        else
        {
            Debug.LogError("AudioManager reference is null!");
        }

        // Phát âm thanh
        if (audioSource != null && soundWin != null)
        {
            audioSource.PlayOneShot(soundWin);
        }

        // Phát hiệu ứng pháo hoa
        if (fireworksEffect != null)
        {
            var mainModule = fireworksEffect.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.white);
            fireworksEffect.Play();
        }

        // Hiệu ứng zoom
        yield return StartCoroutine(ZoomToCenter());

        // Chuyển đến màn hình chính sau khi hoàn tất
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator ZoomToCenter()
    {
        Vector3 targetPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 targetWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(targetPosition.x, targetPosition.y, Camera.main.nearClipPlane));
        targetWorldPosition.z = transform.position.z;

        Vector3 startPosition = transform.position;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = new Vector3(3f, 3f, 3f); // tỉ lệ trophy zoom vào

        float duration = 5f; // Thời gian thực hiện hiệu ứng zoom
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetWorldPosition, elapsed / duration);
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetWorldPosition;
        transform.localScale = targetScale;
    }
}
