#define SHOW
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Mg.Wy
{
    public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public AudioSource audioSource;
        public GameObject LosePanel;

        public GameObject Luopan;
        public GameObject Pointer;
        public GameObject processText;
        public GameObject EffectedPaticle;
        public GameObject SpeedUpParticle;
        public GameObject ProtectedParticle;

        #region SHOW Field
#if SHOW
        public float playerSpeed = 0f;
        public int currHP = 1;
        public int direct = 1;

        public GameObject faHai = null;
        public GameObject xvXian = null;
        public GameObject xiaoQin = null;
        public GameObject baiShe = null;
#endif
        #endregion

        #region Public Fields
        [Tooltip("The prefab to use for representing the player")]
        [HideInInspector]
        public GameObject playerPrefab = null;
        [HideInInspector]
        public int playerCheckIndex = 0;
        public static GameManager Instance;
        public List<GameObject> charactorLists = new List<GameObject>();
        [HideInInspector]
        public GameObject localPlayer;

        public GameObject copyPlayer;

        [HideInInspector]
        public WyPlayer player;
        [HideInInspector]
        public int SelectCount { get => selectCount; set => selectCount = value; }

        public bool canIncreaseTime = false;
        public float currTime = 0f;
        public int doorShowTime = 0;
        public int currPlayer = 4;

        public Material normal;
        public Material special;
        public Material run;

       //public int specialTime = 0;

        #endregion


        #region Private Fields

        [Tooltip("Waiting other player to join panel")]
        [SerializeField]
        private GameObject waitingPanel;


        [Tooltip("All Ui component")]
        [SerializeField]
        private GameObject canvas;
        

        private int selectCount = 0;

        #endregion

        private bool slowDown = true;
        #region MonoBehavior Callbacks

        public int loseTime = 2;

        public void WinTheGame()
        {
            audioSource.clip = Resources.Load("Music/Victory") as AudioClip;
            audioSource.loop = false;
            audioSource.Play();
            //TODO WIN LOGIC
        }

        private void LoseGame()
        {
            LosePanel.SetActive(true);
            audioSource.clip = Resources.Load("Music/Defeated") as AudioClip;
            audioSource.loop = false;
            audioSource.Play();
            UIManager.Instance.CallDeadOne();
            //LeaveRoom();
            //TODO LOSE
            //SHOW LOSE UI
            //LEAVE ROOM
        }



        void UpdatePing()
        {
            int pingRate = PhotonNetwork.GetPing();
            Debug.Log("Ping: " + pingRate);
        }
        private void Start()
        {
            //InvokeRepeating("UpdatePing", 2, 2);
            if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
                waitingPanel.SetActive(false);
            Instance = this;
            
        }

        private void ResumeSpeed()
        {
            player.Speed *= 1.67f;
        }

        private void faHaiStop()
        {
            if (localPlayer.name.Equals(WyConstants.FaHai))
            {
                player.Speed *= 0.6f;
                Invoke("ResumeSpeed", 3f);
            }
        }

        private void Update()
        {

            if(UIManager.Instance.specialCount == 4 && !UIManager.Instance.inLeaf)
            {
                Luopan.SetActive(true);
                Pointer.SetActive(true);
                processText.SetActive(true);
            }
            if(selectCount == 4)
            {
                selectCount = 0;

                canvas.SetActive(false);
            }
            if (localPlayer == null || player == null) return;
            else
            {
                if (!EffectedPaticle)
                {
                    if(localPlayer.transform.Find("Effected"))
                    EffectedPaticle = localPlayer.transform.Find("Effected").gameObject;
                    if (EffectedPaticle)
                        EffectedPaticle.SetActive(false);
                }
                if (!ProtectedParticle)
                {
                    if(localPlayer.transform.Find("Protected"))
                    ProtectedParticle = localPlayer.transform.Find("Protected").gameObject;
                    if (ProtectedParticle)
                        ProtectedParticle.SetActive(false);
                }
                if (!SpeedUpParticle)
                {
                    if(localPlayer.transform.Find("SpeedUp"))
                    SpeedUpParticle = localPlayer.transform.Find("SpeedUp").gameObject;
                    if (SpeedUpParticle)
                        SpeedUpParticle.SetActive(false);
                }
                
            }
            if(UIManager.Instance.deadPer == 3 && localPlayer.name.Equals(WyConstants.FaHai))
            {
                WinTheGame();
            } 
            if (canIncreaseTime && !UIManager.Instance.inLeaf)
            {
                currTime += Time.deltaTime;
            }

            if(UIManager.Instance.faHaiPos.z > 5f && localPlayer.transform.position.z <= UIManager.Instance.faHaiPos.z && !localPlayer.name.Equals(WyConstants.FaHai))
            {
                loseTime--;
                UIManager.Instance.CallStopFaHai();
                if(loseTime == 0)
                    LoseGame();
            }

            if (UIManager.Instance.faHaiStop)
            {
                faHaiStop();
                UIManager.Instance.faHaiStop = false;
            }

                #region TRANSFORM SYNC
                if (xvXian == null && !localPlayer.name.Equals(WyConstants.XvXian))
            {
                xvXian = GameObject.Find("XvXian(Clone)");
            }
            else if(xvXian == null && localPlayer.name.Equals(WyConstants.XvXian))
            {
                xvXian = GameObject.Find("XvXian");
            }

            if (baiShe == null && !localPlayer.name.Equals(WyConstants.BaiShe))
            {
                baiShe = GameObject.Find("BaiShe(Clone)");
            }
            else if (baiShe == null && localPlayer.name.Equals(WyConstants.BaiShe))
            {
                baiShe = GameObject.Find("BaiShe");
            }

            if (xiaoQin == null && !localPlayer.name.Equals(WyConstants.XiaoQin))
            {
                xiaoQin = GameObject.Find("XiaoQin(Clone)");
            }
            else if (xiaoQin == null && localPlayer.name.Equals(WyConstants.XiaoQin))
            {
                xiaoQin = GameObject.Find("XiaoQin");
            }

            if (faHai == null && !localPlayer.name.Equals(WyConstants.FaHai))
            {
                faHai = GameObject.Find("FaHai(Clone)");
            }
            else if (faHai == null && localPlayer.name.Equals(WyConstants.FaHai))
            {
                faHai = GameObject.Find("FaHai");
            }

            #endregion
            if(UIManager.Instance.isSpeedUpByXiaoQin == true && localPlayer.name != WyConstants.FaHai&&!slowDown)
            {
                SpeedUpParticle.SetActive(true);
                player.Speed *= 1.1f;
                slowDown = true;
#if SHOW
                playerSpeed *= 1.1f;
#endif
            }
            else if(UIManager.Instance.isSpeedUpByXiaoQin == false && localPlayer.name != WyConstants.FaHai && slowDown)
            {
                SpeedUpParticle.SetActive(false);
                player.Speed /= 1.1f;

                slowDown = false;
#if SHOW
                playerSpeed /= 1.1f;
#endif
            }

            if(UIManager.Instance.isProtectedByXvXian && UIManager.Instance.xvXianTarget.Equals(localPlayer.name) && localPlayer.name != WyConstants.FaHai)
            {
                ProtectedParticle.SetActive(true);
                player.CurrHp = 2;
                UIManager.Instance.xvXianTarget = "";

#if SHOW
                currHP = 2;
#endif
            }
            else if (!UIManager.Instance.isProtectedByXvXian && !localPlayer.Equals(WyConstants.FaHai))
            {
                player.CurrHp = 1;
                ProtectedParticle.SetActive(false);
                //localPlayer.transform.Find("Protected").gameObject.SetActive(false);
#if SHOW
                currHP = 1;
#endif
            }

            if (UIManager.Instance.isEffectByBaiShe && localPlayer.name.Equals(WyConstants.FaHai))
            {
                EffectedPaticle.SetActive(true);
                UIManager.Instance.direction = -1;
#if SHOW
                direct = -1;
#endif
            }
            else if(!UIManager.Instance.isEffectByBaiShe && localPlayer.name.Equals(WyConstants.FaHai))
            {
                EffectedPaticle.SetActive(false);
#if SHOW
                direct = 1;
#endif
                UIManager.Instance.direction = 1;
            }
        }
        #endregion

        #region Private Methods


        #endregion

        #region Photon Callbacks


        public override void OnPlayerEnteredRoom(Player other)
        {
            //Debug.Log("Curr Players" + PhotonNetwork.CurrentRoom.PlayerCount);
            if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
                waitingPanel.SetActive(false);
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); 
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
            }
        }

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        #endregion

        #region Common Functions


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        
        public void InstantiatePlayer()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate

                Vector3 pos = new Vector3();
                switch (playerPrefab.name)
                {
                    case WyConstants.BaiShe:
                        pos = new Vector3(WyConstants.Street1, WyConstants.BeginHeight, 10f);
                        player = new BaiShe();
                        break;
                    case WyConstants.FaHai:
                        pos = new Vector3(WyConstants.Street2, WyConstants.BeginHeight, 0f);
                        player = new FaHai();
                        break;
                    case WyConstants.XiaoQin:
                        pos = new Vector3(WyConstants.Street3, WyConstants.BeginHeight, 10f);
                        player = new XiaoQin();
                        break;
                    case WyConstants.XvXian:
                        pos = new Vector3(WyConstants.Street2, WyConstants.BeginHeight, 10f);
                        player = new XvXian();
                        break;
                    default:
                        Debug.Log("None Such Player Prefabs");
                        break;
                }

                localPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, pos, Quaternion.identity, 0);
                canIncreaseTime = true;
                localPlayer.name = GameManager.Instance.localPlayer.name.Substring(0, GameManager.Instance.localPlayer.name.Length - 7);
            }

        }


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //TODO
        }

        #endregion
    }
}