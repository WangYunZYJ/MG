#define SHOW

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Mg.Wy
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public GameObject _mainCamera;
        private VideoPlayer videoPlayer;


        #region Private Serializanle Fields
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        bool isCOnnecting;
        public GameObject BeginPanel;
        public Button benginBtn;

        public GameObject StagePanel;
        public Button connectBtn;

        public GameObject ConnectingSprite;
        public Button JumpBtn;
        #endregion

        #region Btn Events

        public void OnJumpBtnClicked()
        {
            videoPlayer.Stop();
            JumpBtn.gameObject.SetActive(false);
            StagePanel.SetActive(true);
        }

        #endregion

        #region Private Fields

        private string gameVersion = "1";

        #endregion

        #region MonoBehavior Callbacks

        private void Awake()
        {
            benginBtn.onClick.AddListener(BenginBtnEvent);
            connectBtn.onClick.AddListener(ConnectBtnEvent);
            ConnectingSprite.SetActive(false);

            Screen.SetResolution(450, 800, false);
            PhotonNetwork.AutomaticallySyncScene = true;
            JumpBtn.onClick.AddListener(OnJumpBtnClicked);
        }

        #endregion

        void HideUI()
        {

            JumpBtn.gameObject.SetActive(true);
            BeginPanel.SetActive(false);

            StagePanel.SetActive(false);

        }

        public void BenginBtnEvent()
        {
            videoPlayer = _mainCamera.GetComponent<VideoPlayer>();
            videoPlayer.playOnAwake = false;
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            videoPlayer.targetCameraAlpha = 1F;
            videoPlayer.frame = 100;

            videoPlayer.isLooping = false;
            videoPlayer.loopPointReached += EndReached;
            videoPlayer.Play();
            Invoke("HideUI", 1.0f);
        }
        
        void EndReached(UnityEngine.Video.VideoPlayer vp)
        {
            //vp.playbackSpeed = vp.playbackSpeed / 10.0F;
            StagePanel.SetActive(true);
        }

        public void ConnectBtnEvent()
        {
            Connect();
        }

        public void Connect()
        {
            ConnectingSprite.SetActive(true);
            isCOnnecting = true;
            if (PhotonNetwork.IsConnected)
            {
            }
            else
            {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #region MonoBehaviourPunCallbacks Callbacks


        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            if (isCOnnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
            }
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }


        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'MainSpace' ");

                PhotonNetwork.LoadLevel("MainSpace");
            }
        }

        #endregion
    }

}