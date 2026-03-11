using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Diary : MonoBehaviour
{
    [SerializeField] private GameObject m_spreadPrefab;
    [SerializeField] private Button m_buttonBack;
    [SerializeField] private Button m_buttonForward;
    [SerializeField] private Sprite m_clearPage;
    [Min(1)] private int m_currentSpread = 1;

    private Dictionary<string, Sprite> m_pages = new Dictionary<string, Sprite>();
    private List<GameObject> m_spreads;
    private GameObject m_lastSpread;
    private int m_pageAmount;
    private bool m_isFull;

    private void Awake()
    {
        m_spreads = new List<GameObject>();
        AddSpread();

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        m_buttonBack.onClick.AddListener(TurnPageBackward);
        m_buttonForward.onClick.AddListener(TurnPageForward);
    }

    private void OnDisable()
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
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void AddSprite(Sprite sprite)
    {
        Image[] images = m_lastSpread.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            if (image.sprite != m_clearPage)
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
        m_lastSpread = Instantiate(m_spreadPrefab, transform);
        m_spreads.Add(m_lastSpread);
    }
}