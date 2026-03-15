using UnityEngine;
using UnityEngine.UI;

public class DontDestroyMusic : MonoBehaviour
{
    [SerializeField] private Button m_newGameButton;
    private AudioSource m_audioSource;

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

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


}
