using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mg.Wy
{
    public class WinPanel : MonoBehaviour
    {
        public Button checkBtn;
        public GameObject storyPanel;

        public List<GameObject> storys = new List<GameObject>();

        private void Awake()
        {
            checkBtn.onClick.AddListener(checkBtnActions);
        }

        private void checkBtnActions()
        {
            storyPanel.SetActive(true);
            StartCoroutine(ShowStory());
        }

        IEnumerator ShowStory()
        {
            for (int i = 0; i < storys.Count; ++i)
            {
                for (int j = 0; j < 100; ++j)
                {
                    yield return new WaitForSeconds(0.02f);
                    var color = storys[i].GetComponent<Image>().color;
                    color.a -= 0.01f;
                    storys[i].GetComponent<Image>().color = color;
                }
            }

            SceneManager.LoadScene(0);
            GameManager.Instance.LeaveRoom();
        }
    }
}