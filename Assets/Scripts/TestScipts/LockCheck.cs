using System.Collections;
using UnityEngine;
namespace Puzzles {
    public class DoorActivation : MonoBehaviour
    {

        [SerializeField] private Vector3 m_doorAngel = new Vector3(90f, 0, 0);
        [SerializeField] private float m_openingTime = 1f;
        [SerializeField] private BasePuzzle m_puzzle;

        private void OnEnable()
        {
            if (m_puzzle != null)
            {
                print("sub");
                m_puzzle.m_onSolved += OpenDoor;
            }
        }
        private void OnDisable()
        {
            if (m_puzzle != null)
            {
                print("unsub");
                m_puzzle.m_onSolved -= OpenDoor;
            }
        }
        private void OpenDoor()
        {
            StartCoroutine(OpenDoorCoroutine());
        }
        IEnumerator OpenDoorCoroutine()
        {
            print("open");
            for (float i = 0; i < m_openingTime; i += Time.deltaTime)
            {
                Vector3 angle = m_doorAngel * Time.deltaTime;
                transform.Rotate(angle);
                yield return null;
            }
            print("opened");

        }
    }
}