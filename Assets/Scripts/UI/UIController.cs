using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject m_objectActivationText;
    [SerializeField] private GameObject m_newDiaryRecordText;
    [SerializeField] private TMP_Text m_figureText;

    public void ShowObjectActivationText(bool shouldBeActivated)
    {
        m_objectActivationText.SetActive(shouldBeActivated);
    }

    public void ShowFigureText(bool shouldBeActivated, string name)
    {
        m_figureText.text = name;
        m_figureText.gameObject.SetActive(shouldBeActivated);
    }

    public void ShowDiaryNotification(bool shouldBeActivated)
    {
        m_newDiaryRecordText.SetActive(shouldBeActivated);
    }
}
