using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mg.Wy
{
    public class Skills : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {

        #region FaHai Skill Field
        private string[] seq = { "132", "123", "312", "231" };
        private int currIndex = 0;
        #endregion
        public GameObject skillTargetXvXian;
        public GameObject skillTargetBaiShe;
        public GameObject skillTargetXiaoQin;
        public Text cd;

        private List<GameObject> obstacles = new List<GameObject>();

        private void Awake()
        {
            //if (GameManager.Instance.localPlayer.name.Equals(WyConstants.FaHai)) { 
            //GameObject tree = Resources.Load("$Tree") as GameObject;
            //GameObject tower = Resources.Load("$Tower") as GameObject;

            for(int i = 0; i <2; ++i)
            {
                var obj = PhotonNetwork.Instantiate("$Tree", new Vector3(WyConstants.Street1, -5, -10), Quaternion.identity, 0) as GameObject;
                //var obj1 = Instantiate(tree, new Vector3(WyConstants.Street1, -5, -10), Quaternion.identity) as GameObject;
                obstacles.Add(obj);
            }

            var tow = PhotonNetwork.Instantiate("$Tower", new Vector3(WyConstants.Street1, -5, -10), Quaternion.identity) as GameObject;
            obstacles.Add(tow);
        }


        public float currCd = 0;
        public bool canUseSkill = true;

        private void Update()
        {
            if (currCd <= 0)
            {
                currCd = 0;
                cd.text = "";
                canUseSkill = true;
            }
            else
            {
                cd.text = ((int)currCd).ToString();
            }
            currCd -= Time.deltaTime;       
        }

        private string IsOverGUI(Vector2 pos)
        {
            EventSystem es = EventSystem.current;
            PointerEventData ped = new PointerEventData(es);
            ped.position = pos;
            List<RaycastResult> rr = new List<RaycastResult>();
            rr.Clear();
            es.RaycastAll(ped, rr);
            if (rr.Count > 0)
                return rr[0].gameObject.name;
            else return "";
        }

        private void DeCallEffectByBaiShe()
        {
            UIManager.Instance.DeCallEffectByBaiShe();
        }

        private void DeSpeedUp()
        {
            UIManager.Instance.DeCallSpeedUp();
        }

        private void DeCallProtected()
        {
            UIManager.Instance.DeCallProtected();
        }

        public void OnPointerUp(PointerEventData eventData)
        {

            Debug.Log("Mouse Up");
            if (!canUseSkill) return;
            string objname = IsOverGUI(Input.mousePosition);
            if (!objname.Equals(WyConstants.XvXian) && !objname.Equals(WyConstants.BaiShe) && !objname.Equals(WyConstants.XiaoQin) && GameManager.Instance.localPlayer.name.Equals(WyConstants.XvXian))
            {
                skillTargetBaiShe.SetActive(false);
                skillTargetXvXian.SetActive(false);
                skillTargetXiaoQin.SetActive(false);
                return;
            }
            if (GameManager.Instance.localPlayer.name.Equals(WyConstants.XiaoQin) ||
               GameManager.Instance.localPlayer.name.Equals(WyConstants.BaiShe))
            {
                switch (GameManager.Instance.localPlayer.name)
                {
                    case WyConstants.XiaoQin:
                        UIManager.Instance.CallSpeedUp();
                        Invoke("DeSpeedUp", 3f);
                        currCd = 15f;
                        break;
                    case WyConstants.BaiShe:
                        UIManager.Instance.CallEffectByBaiShe();
                        Invoke("DeCallEffectByBaiShe", 5f);
                        currCd = 20f;
                        break;
                }
                return;
            }
            if (GameManager.Instance.localPlayer.name.Equals(WyConstants.XvXian)) {
                UIManager.Instance.CallProtected(objname);
                Invoke("DeCallProtected", 15f);
                skillTargetBaiShe.SetActive(false);
                skillTargetXvXian.SetActive(false);
                skillTargetXiaoQin.SetActive(false);
                currCd = 30f;
            }
            if (GameManager.Instance.localPlayer.name.Equals(WyConstants.FaHai))
            {
                StartCoroutine(FaHaiSkill());
                //TODO
                //Instantate some prefabs
                currCd = 10f;
            }
            canUseSkill = false;
            Debug.Log("skill aim" + objname);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Mouse Down");
            if (!canUseSkill) return;
            if (GameManager.Instance.localPlayer.name.Equals(WyConstants.XvXian))
            {
                skillTargetBaiShe.SetActive(true);
                skillTargetXvXian.SetActive(true);
                skillTargetXiaoQin.SetActive(true);
            }
        }

        IEnumerator FaHaiSkill()
        {
            for(int i = 0; i < seq[currIndex].Length; ++i)
            {
                switch (seq[currIndex][i])
                {
                    case '1':
                        var obj1 = obstacles[0];
                        var vec1 = UIManager.Instance.xvXianPos;
                        vec1.z += 25f;
                        int num = (int)vec1.z % 14;
                        if (num <= 3)
                            vec1.z += 5;
                        else if (num >= 11)
                            vec1.z -= 5;
                        vec1.y = 0;
                        obj1.transform.position = vec1;
                        obstacles.RemoveAt(0);
                        obstacles.Add(obj1);
                        break;
                    case '2':
                        var obj2 = obstacles[0];
                        var vec2 = UIManager.Instance.baiShePos;
                        vec2.z += 25f;

                        int num2 = (int)vec2.z % 14;
                        if (num2 <= 3)
                            vec2.z += 5;
                        else if (num2 >= 11)
                            vec2.z -= 5;

                        vec2.y = 0;
                        obj2.transform.position = vec2;
                        obstacles.RemoveAt(0);
                        obstacles.Add(obj2);
                        break;
                    case '3':
                        var obj3 = obstacles[0];
                        var vec3 = UIManager.Instance.xiaoQinPos;
                        vec3.z += 25f;

                        int num3 = (int)vec3.z % 14;
                        if (num3<= 3)
                            vec3.z += 5;
                        else if (num3 >= 11)
                            vec3.z -= 5;

                        vec3.y = 0;
                        obj3.transform.position = vec3;
                        obstacles.RemoveAt(0);
                        obstacles.Add(obj3);
                        break;
                }
                yield return new WaitForSeconds(1);
            }
            currIndex = (currIndex + 1) % seq.Length;
        }
    }
}