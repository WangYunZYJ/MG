using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Mg.Wy
{
    [RequireComponent(typeof(Button))]
    public class PlayerSelectManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Private Fields

        [Tooltip("Player bg")]
        [SerializeField]
        private GameObject playerImg = null;


        private Button clickBtn;

        private Image img;
        #endregion



        #region Public Fields

        public Sprite fahai;
        public Sprite xvxian;
        public Sprite baishe;
        public Sprite xiaoqin;


        public Sprite fahaiSkillSprite;
        public Sprite xvxianSkillSprite;
        public Sprite xiaoqinSkillSprite;
        public Sprite baisheSkillSprite;
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            clickBtn = GetComponent<Button>();
            img = GetComponent<Image>();
        }

        private void Start()
        {
            clickBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.SkillBtns.SetActive(true);



                UIManager.Instance.SkillTargetBaiShe.SetActive(false);
                UIManager.Instance.SkillTargetXiaoQin.SetActive(false);
                UIManager.Instance.SkillTargetXvXian.SetActive(false);
                GameManager.Instance.playerPrefab = Resources.Load(this.gameObject.name) as GameObject;

                playerImg.SetActive(true);

                switch (this.gameObject.name)
                {
                    case WyConstants.FaHai:
                        UIManager.Instance.SkillBtns.GetComponent<Image>().sprite = fahaiSkillSprite;
                        playerImg.GetComponent<Image>().sprite = fahai;
                        break;
                    case WyConstants.XvXian:
                        UIManager.Instance.SkillBtns.GetComponent<Image>().sprite = xvxianSkillSprite;
                        playerImg.GetComponent<Image>().sprite = xvxian;
                        break;
                    case WyConstants.XiaoQin:
                        UIManager.Instance.SkillBtns.GetComponent<Image>().sprite = xiaoqinSkillSprite;
                        playerImg.GetComponent<Image>().sprite = xiaoqin;
                        break;
                    case WyConstants.BaiShe:
                        UIManager.Instance.SkillBtns.GetComponent<Image>().sprite = baisheSkillSprite;
                        playerImg.GetComponent<Image>().sprite = baishe;
                        break;
                }
                
                PhotonView pv = PhotonView.Get(this);
                pv.RPC("SetChoosed", RpcTarget.All);

                PhotonView pv2 = PhotonView.Get(UIManager.Instance);
                pv2.RPC("ChooseAddOne", RpcTarget.All);
            });
        }
        

        #endregion

        #region Custom Fuction
        

        #endregion

        #region IPunObservable Implements
        

        [PunRPC]
        public void SetChoosed()
        {
            Color color = Color.gray;
            color.a = 0.2f;
            img.color = color;
            clickBtn.onClick.RemoveAllListeners();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
           
        }
        #endregion
    }
}