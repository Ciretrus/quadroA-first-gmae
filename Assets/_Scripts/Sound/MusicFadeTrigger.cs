using UnityEngine;

public class MusicFadeTrigger : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 2f; 
    [SerializeField] private float targetVolume = 0f; 
    private bool hasFaded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasFaded)
        {
            hasFaded = true;
            StartCoroutine(FadeOutMusic());
        }
    }

    private System.Collections.IEnumerator FadeOutMusic()
    {
        AudioSource musicSource = DontDestroyMusic.Instance.audioSource;
        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / fadeDuration);
            yield return null;
        }
        musicSource.volume = targetVolume;
    }
}