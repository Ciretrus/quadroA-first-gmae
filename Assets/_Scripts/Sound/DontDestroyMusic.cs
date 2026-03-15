using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DontDestroyMusic : MonoBehaviour
{
    public static DontDestroyMusic instance { get; private set; }

    [SerializeField] private Button m_newGameButton;

    private AudioSource m_audioSource;
    public AudioSource audioSource => m_audioSource;
    private Coroutine m_musicFadeCoroutine;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (m_audioSource == null)
        {
            m_audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnValidate()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        m_newGameButton.onClick.AddListener(StartNewGame);
    }

    private void OnDisable()
    {
        m_newGameButton.onClick.RemoveListener(StartNewGame);
    }

    private void StartNewGame()
    {
        m_audioSource.loop = false;
    }

    public void FadeMusic(float targetVolume, float duration)
    {
        if (m_musicFadeCoroutine != null)
        {
            StopCoroutine(m_musicFadeCoroutine);
        }

        m_musicFadeCoroutine = StartCoroutine(FadeMusicCoroutine(targetVolume, duration));
    }

    private IEnumerator FadeMusicCoroutine(float targetVolume, float duration)
    {
        float startVolume = m_audioSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            m_audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }
        m_audioSource.volume = targetVolume;
        m_musicFadeCoroutine = null;
    }
}
