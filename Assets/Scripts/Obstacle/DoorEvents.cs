using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mg.Wy
{
    public class DoorEvents : MonoBehaviourPunCallbacks
    {
        

        public GameObject mainCamera;
        public GameObject _camera;

        private void Awake()
        {
            mainCamera = GameObject.Find("Main Camera") as GameObject;
            _camera = GameObject.Find("Camera") as GameObject;
            if (_camera)
                _camera.SetActive(false);
        }

        private void CallResumePass()
        {
            UIManager.Instance.CallResumePass();
            UIManager.Instance.canTrigger = true;
        }

        private void BackToMain()
        {
            UIManager.Instance.CallResumeSomeOneSpecial();
            Destroy(GameManager.Instance.copyPlayer);
            _camera.SetActive(false);
            mainCamera.SetActive(true);
            UIManager.Instance.inLeaf = false;
            RenderSettings.skybox = GameManager.Instance.normal;

            GameManager.Instance.localPlayer.SetActive(true);
            /*Vector3 pos = GameManager.Instance.localPlayer.transform.position;

            pos.z = Mathf.Max(UIManager.Instance.xvXianPos.z, Mathf.Max(UIManager.Instance.xiaoQinPos.z, UIManager.Instance.baiShePos.z));

            if (GameManager.Instance.localPlayer.name.Equals(WyConstants.BaiShe))
            pos.z = Mathf.Max(UIManager.Instance.xvXianPos.z,UIManager.Instance.xiaoQinPos.z);
            else if (GameManager.Instance.localPlayer.name.Equals(WyConstants.XiaoQin))
            {
                pos.z = Mathf.Max(UIManager.Instance.xvXianPos.z, UIManager.Instance.baiShePos.z);
            }else if (GameManager.Instance.localPlayer.name.Equals(WyConstants.XvXian))
            {
                pos.z = Mathf.Max(UIManager.Instance.xiaoQinPos.z, UIManager.Instance.baiShePos.z);
            }
            GameManager.Instance.localPlayer.transform.position = pos;
            */
            
        }

        void ResumeActive()
        {
            gameObject.SetActive(true);
        }
        IEnumerator enterDoor()
        {
            yield return new WaitForSeconds(1.5f);
            GameObject ob = Resources.Load(GameManager.Instance.localPlayer.name + "1") as GameObject;
            GameManager.Instance.copyPlayer = Instantiate(ob, new Vector3(499f, 1.1f, 10), Quaternion.identity);

            UIManager.Instance.inLeaf = true;
            _camera.SetActive(true);
            mainCamera.SetActive(false);
            RenderSettings.skybox = GameManager.Instance.special;
            // gameObject.SetActive(false);
            Invoke("ResumeActive", 25f);
            GameManager.Instance.localPlayer.SetActive(false);
            Invoke("BackToMain", 20f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!GameManager.Instance.localPlayer) return;

            if (other.name.Equals(GameManager.Instance.localPlayer.name))
            {
                UIManager.Instance.CallChangeSpecial();
                UIManager.Instance.CallSpecialAdd();
                if (!UIManager.Instance.canTrigger) return;
                UIManager.Instance.canTrigger = false;
                Invoke("CallResumePass", 40f);
                UIManager.Instance.CallPassAddOne();
                if(/*UIManager.Instance.passby == 1 && !*/!GameManager.Instance.localPlayer.name.Equals(WyConstants.FaHai))
                {
                    StartCoroutine(enterDoor());
                }
            }
        }
    }
}
