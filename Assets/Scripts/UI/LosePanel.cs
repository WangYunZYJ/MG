using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mg.Wy {
    public class LosePanel : MonoBehaviour
    {
        public Button backToMain;
        public Button RePlay;

        private void Awake()
        {
            backToMain.onClick.AddListener(BtnActions);
            RePlay.onClick.AddListener(BtnActions);
        }

        private void BtnActions()
        {
            SceneManager.LoadScene(0);
            GameManager.Instance.LeaveRoom();
        }
    }
}
