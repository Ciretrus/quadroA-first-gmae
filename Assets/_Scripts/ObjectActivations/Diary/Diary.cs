using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Diary : MonoBehaviour
{
    public static Diary instance { get; private set; }

    [SerializeField] private GameObject m_diaryUI;
    [SerializeField] private GameObject m_spreadPrefab;
    [SerializeField] private Button m_buttonBack;
    [SerializeField] private Button m_buttonForward;
    private Dictionary<string, Sprite> m_pages = new Dictionary<string, Sprite>();
    private List<GameObject> m_spreads;
    private GameObject m_lastSpread;
    [Min(1)] private int m_currentSpread = 1;
    private int m_pageAmount;
    private bool m_isFull;

    private void Awake()
    {
        if (instance != null && instance != this) 
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);

        m_spreads = new List<GameObject>();
        AddSpread();

        m_buttonBack.onClick.AddListener(TurnPageBackward);
        m_buttonForward.onClick.AddListener(TurnPageForward);

        m_diaryUI.SetActive(false);
    }

    private void OnDestroy()
    {
        m_buttonBack.onClick.RemoveListener(TurnPageBackward);
        m_buttonForward.onClick.RemoveListener(TurnPageForward);
    }

    public void SetPage(Sprite sprite, string fileName)
    {
        if (!m_pages.ContainsKey(fileName))
        {
            m_pages.Add(fileName, sprite);
            AddSprite(sprite);
            if (m_isFull)
            {
                m_lastSpread.SetActive(false);
                m_spreads[m_currentSpread - 1].SetActive(false);
                AddSpread();
                AddSprite(sprite);
                m_currentSpread = m_spreads.Count;
            }
            m_pageAmount++;
        }
    }

    public void ChangeState(InputAction.CallbackContext context)
    {
        m_diaryUI.SetActive(!m_diaryUI.activeSelf);
    }

    private void AddSprite(Sprite sprite)
    {
        Image[] images = m_lastSpread.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            if (image.sprite != null)
            {
                m_isFull = true;
                continue;
            }
            m_isFull = false;
            image.sprite = sprite;
            break;
        }
    }

    private void TurnPageForward()
    {
        if (m_currentSpread >= (m_pageAmount % 2 != 0 ? (m_pageAmount + 1) / 2 : m_pageAmount / 2))
        {
            return;
        }

        m_spreads[m_currentSpread - 1].SetActive(false);
        m_currentSpread++;
        m_spreads[m_currentSpread - 1].SetActive(true);
    }

    private void TurnPageBackward()
    {
        if (m_currentSpread == 1)
        {
            return;
        }

        m_spreads[m_currentSpread - 1].SetActive(false);
        m_currentSpread--;
        m_spreads[m_currentSpread - 1].SetActive(true);
    }

    private void AddSpread()
    {
        m_lastSpread = Instantiate(m_spreadPrefab, m_diaryUI.transform);
        m_spreads.Add(m_lastSpread);
    }
}