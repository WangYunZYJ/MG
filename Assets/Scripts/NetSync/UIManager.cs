using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

namespace Mg.Wy
{
    public class UIManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Camera Fields
        public int passby = 0;
        public bool inLeaf = false;
        public bool canTrigger = true;
        #endregion

        public int deadPer = 0;

        public int LuoCorrectCount = 0;
        
        [Tooltip("How Many Special Level Passed!")]
        public int specialCount = 0;

        #region Skill Fields

        public bool faHaiStop = false;

        public bool someGuyInSpecial = false;
        public bool isSpeedUpByXiaoQin = false;
        public bool isProtectedByXvXian = false;
        public bool isEffectByBaiShe = false;
        /// <summary>
        /// 1 baishe
        /// 2 xiaoqin
        /// 3 xvxian
        /// </summary>
        public string xvXianTarget = "";
        public int direction = 1;

        public Vector3 xvXianPos;
        public Vector3 baiShePos;
        public Vector3 faHaiPos;
        public Vector3 xiaoQinPos;
        #endregion


        public GameObject SkillBtns;
        public GameObject SkillTargetXvXian;
        public GameObject SkillTargetBaiShe;
        public GameObject SkillTargetXiaoQin;

        public Text timeText;

        public GameObject btns;
        public GameObject Image;
        public GameObject WaitingPanel;
 
        public int playerChecked = 0;
        
        private static UIManager instance = null;

        public static UIManager Instance { get => instance; set => instance = value; }

        private void Awake()
        {
            SkillBtns.SetActive(false);
            instance = this;
            playerChecked = 0;
            timeText.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (someGuyInSpecial)
            {
                RenderSettings.skybox = GameManager.Instance.run;
            }
            else
            {
                RenderSettings.skybox = GameManager.Instance.normal;
            }
            if (playerChecked == 4)
            {
                playerChecked = 0;
                btns.SetActive(false);
                Image.SetActive(false);
                WaitingPanel.SetActive(false);
                GameManager.Instance.InstantiatePlayer();
                GameManager.Instance.audioSource.clip = Resources.Load("Music/Background") as AudioClip;
                GameManager.Instance.audioSource.Play();
            }
            if (GameManager.Instance.xvXian && GameManager.Instance.xvXian.transform.position.x < 10)
                xvXianPos = GameManager.Instance.xvXian.transform.position;
            if (GameManager.Instance.faHai)
                faHaiPos = GameManager.Instance.faHai.transform.position;
            if (GameManager.Instance.baiShe && GameManager.Instance.baiShe.transform.position.x < 10)
                baiShePos = GameManager.Instance.baiShe.transform.position;
            if (GameManager.Instance.xiaoQin && GameManager.Instance.xiaoQin.transform.position.x < 10)
                xiaoQinPos = GameManager.Instance.xiaoQin.transform.position;

        }

        public void CallPassAddOne()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("PassAddOne", RpcTarget.All);
        }

        public void CallResumePass()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("ResumePass", RpcTarget.All);
        }

        public void CallSpeedUp()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("SpeedUp", RpcTarget.All);
        }

        public void CallProtected(string aim)
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("BeProtected", RpcTarget.All, aim);
        }

        public void CallEffectByBaiShe()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("EffectByBaiShe", RpcTarget.All);
        }

        public void DeCallSpeedUp()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("DeSpeedUp", RpcTarget.All);
        }

        public void DeCallProtected()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("DeBeProtected", RpcTarget.All);
        }

        public void DeCallEffectByBaiShe()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("DeEffectByBaiShe", RpcTarget.All);
        }

        public void CallSetBaiShePos()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("SetBaiShePos", RpcTarget.All);
        }

        public void CallSetXiaoQinPos()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("SetXiaoQinPos", RpcTarget.All);
        }

        public void CallSetXvXianPos()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("SetXvXianPos", RpcTarget.All);
        }

        public void CallSetFaHaiPos()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("SetFaHaiPos", RpcTarget.All);
        }

        public void CallChangeSpecial()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("ChangeSomeOneInSpecial", RpcTarget.All);
        }

        public void CallSpecialAdd()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("SpecialCountAdd", RpcTarget.All);
        }

        public void CallLuoCount()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("LuoPanAddOne", RpcTarget.All);
        }

        public void CallResumePanCount()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("ResetPanCount", RpcTarget.All);
        }

        public void CallResumeSomeOneSpecial()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("ResumeSomeOneSpecial", RpcTarget.All);
        }

        public void CallDeadOne()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("DeadOne", RpcTarget.All);
        }

        public void CallStopFaHai()
        {
            PhotonView pv = PhotonView.Get(this);
            pv.RPC("StopFaHai", RpcTarget.All);
        }

        #region PunRPC Calls

        [PunRPC]
        public void StopFaHai()
        {
            faHaiStop = true;
        }

        [PunRPC]
        public void DeadOne()
        {
            deadPer++;
        }

        [PunRPC]
        public void ResetPanCount()
        {
            LuoCorrectCount = 0;
        }

        [PunRPC]
        public void LuoPanAddOne()
        {
            LuoCorrectCount++;
        }

        [PunRPC]
        public void SpecialCountAdd()
        {
            specialCount++;
        }

        [PunRPC]
        public void ResumeSomeOneSpecial()
        {
            someGuyInSpecial = false;
        }

        [PunRPC]
        public void ChangeSomeOneInSpecial()
        {
            someGuyInSpecial = true;
        }

        [PunRPC]
        public void ChooseAddOne()
        {
            playerChecked++;
            //Debug.Log("Curr Players" + playerChecked);
        }


        [PunRPC]
        private void SpeedUp()
        {
            isSpeedUpByXiaoQin = true;
        }

        [PunRPC]
        public void BeProtected(string aim)
        {
            isProtectedByXvXian = true;
            xvXianTarget = aim;
        }

        [PunRPC]
        public void EffectByBaiShe()
        {
            isEffectByBaiShe = true;
        }

        [PunRPC]
        public void DeSpeedUp()
        {
            isSpeedUpByXiaoQin = false;
        }

        [PunRPC]
        public void PassAddOne()
        {
            passby++;
        }

        [PunRPC]
        public void ResumePass()
        {
            passby = 0;
        }

        [PunRPC]
        public void DeBeProtected()
        {
            isProtectedByXvXian = false;
            xvXianTarget = "";
        }

        [PunRPC]
        public void DeEffectByBaiShe()
        {
            isEffectByBaiShe = false;
        }

        [PunRPC]
        private void SetXiaoQinPos()
        {
            xiaoQinPos = GameManager.Instance.localPlayer.transform.position;
        }

        [PunRPC]
        private void SetBaiShePos()
        {
            baiShePos = GameManager.Instance.localPlayer.transform.position;
        }

        [PunRPC]
        private void SetXvXianPos()
        {
            xvXianPos = GameManager.Instance.localPlayer.transform.position;
        }

        [PunRPC]
        private void SetFaHaiPos()
        {
            faHaiPos = GameManager.Instance.localPlayer.transform.position;
        }
        #endregion

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }
    }
}
