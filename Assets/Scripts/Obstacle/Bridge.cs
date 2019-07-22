using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mg.Wy
{
    public class Bridge : MonoBehaviour
    {

        private float Stree1Pos = 1f;
        public bool isSpecial = false;
        [Tooltip("Temp And JumpFish")]
        public string Obstacle1 = "";
        [Tooltip("Flower and Wine")]
        public string Obstacle2 = "";
        private string[] seq = { "1232132", "1321233", "3212132", "2132123" };

        //private float currTime;
        //Obs Seq
        /// <summary>
        /// 1232132
        /// 1321233
        /// 3212132
        /// 2132123
        /// 
        /// 
        /// 1112112
        /// </summary>
        private float distance = 0f;
        private int count = 0;
        private int surfaceCount = 0;
        public List<GameObject> bridges = new List<GameObject>();
        public List<GameObject> tempObstacles = new List<GameObject>();
        public List<GameObject> wineObstacles = new List<GameObject>();
        public List<GameObject> surfaces = new List<GameObject>();
        public List<GameObject> fonts = new List<GameObject>();
        public GameObject door;

        public string font =  "风雨西湖寻前缘纸伞为媒喜成婚端午现形惊元魂勇闯瑶池救夫君水漫金山铸大错雷锋宝塔镇妖蛇归去同修赎孽债绝情出家上金山";

        private int doorShowTime = 0;

        private int currSeqIndex = 0;
        private int currPos = 0;
        private void Awake()
        {
            //door = PhotonNetwork.Instantiate("%Door", new Vector3(5, 3, -10), Quaternion.identity, 0) as GameObject;
            if (isSpecial)
            {
                for (int i = 0; i < seq.Length; ++i)
                {
                    seq[i] = "1111111";
                    Stree1Pos = 500;
                }
                for(int i  =0; i < font.Length; ++i)
                {
                    GameObject ob = Resources.Load("Font/" + font[i]) as GameObject;
                    Instantiate(ob, new Vector3(500, -1, 0), Quaternion.identity);
                    fonts.Add(ob);
                }
            }
            GameObject temp = Resources.Load(Obstacle1) as GameObject;
            GameObject wine = Resources.Load(Obstacle2) as GameObject;
            
            for (int i = 0; i < 2; ++i)
            {
                var obj1 = Instantiate(temp, new Vector3(Stree1Pos, -5, -10), Quaternion.identity) as GameObject;
                obj1.transform.parent = gameObject.transform;
                tempObstacles.Add(obj1);
                if (isSpecial)
                    obj1.transform.position = new Vector3(500f, 0f, 0f);
            }
            for(int i = 0; i < 5; ++i)
            {
                var obj1 = Instantiate(wine, new Vector3(Stree1Pos, -5, -10), Quaternion.identity) as GameObject;
                obj1.transform.parent = gameObject.transform;
                wineObstacles.Add(obj1);
            }

            #region Instance Origin Obs
            for (int i = 0; i < seq[currSeqIndex].Length; ++i)
            {
                switch (seq[currSeqIndex][i])
                {
                    case '1':
                        if (i != 3 && i != 6)
                        {
                            var obj = wineObstacles[0];
                            obj.transform.position = new Vector3(Stree1Pos, 0, (i - 1) * 28f);
                            wineObstacles.RemoveAt(0);
                            wineObstacles.Add(obj);
                        }
                        else
                        {
                            var obj = tempObstacles[0];
                            obj.transform.position = new Vector3(Stree1Pos, 0, (i - 1) * 28f);
                            tempObstacles.RemoveAt(0);
                            tempObstacles.Add(obj);
                        }
                        break;
                    case '2':
                        if (i != 3 && i != 6)
                        {
                            var obj = wineObstacles[0];
                            obj.transform.position = new Vector3(3.25f, 0, (i - 1) * 28f + 14);
                            wineObstacles.RemoveAt(0);
                            wineObstacles.Add(obj);
                        }
                        else
                        {
                            var obj = tempObstacles[0];
                            obj.transform.position = new Vector3(3.25f, 0, (i - 1) * 28f + 14);
                            tempObstacles.RemoveAt(0);
                            tempObstacles.Add(obj);
                        }
                        break;
                    case '3':
                        if (i != 3 && i != 6)
                        {
                            var obj = wineObstacles[0];
                            obj.transform.position = new Vector3(5.5f, 0, (i ) * 28f);
                            wineObstacles.RemoveAt(0);
                            wineObstacles.Add(obj);
                        }
                        else
                        {
                            var obj = tempObstacles[0];
                            obj.transform.position = new Vector3(5.5f, 0, (i) * 28f);
                            tempObstacles.RemoveAt(0);
                            tempObstacles.Add(obj);
                        }
                        break;
                }
            }
            #endregion

            currSeqIndex++;
        }

        private void Update()
        {
            if (isSpecial) return;
            if (!GameManager.Instance.localPlayer)
                return;
            if(GameManager.Instance.currTime >= 50)
            {
                GameManager.Instance.doorShowTime++;
                GameManager.Instance.currTime = 0;
                var vec = door.transform.position;
                vec.z = Mathf.Max(GameManager.Instance.xiaoQin.transform.position.z, Mathf.Max(GameManager.Instance.xvXian.transform.position.z, GameManager.Instance.baiShe.transform.position.z)) + 50;
                door.transform.position = vec;
            }
            distance = GameManager.Instance.localPlayer.transform.position.z;
            int res = (int)(distance / 28f);
            int resSurface = (int)(distance / 300f);
            while(res > count && (distance/28f - res * 1.0 >= 0.5f))
            {
                count++;
                //count = res;
                var bridge = bridges[0];
                bridges.RemoveAt(0);
                bridge.transform.position = new Vector3(bridge.transform.position.x, bridge.transform.position.y, 28f * (count + 6));
                bridges.Add(bridge);

                #region Instance Obs
                switch (seq[currSeqIndex][currPos])
                {
                    case '1':
                        if (currPos != 3 && currPos != 6)
                        {
                            var obj = wineObstacles[0];
                            obj.transform.position = new Vector3(Stree1Pos, 0, (res + 6) * 28f);
                            wineObstacles.RemoveAt(0);
                            wineObstacles.Add(obj);
                        }
                        else
                        {
                            var obj = tempObstacles[0];
                            obj.transform.position = new Vector3(Stree1Pos, 0, (res + 6) * 28f);
                            tempObstacles.RemoveAt(0);
                            tempObstacles.Add(obj);
                        }
                        break;
                    case '2':
                        if (currPos != 3 && currPos != 6)
                        {
                            var obj = wineObstacles[0];
                            obj.transform.position = new Vector3(3.25f, 0, (res + 6) * 28f + 14);
                            wineObstacles.RemoveAt(0);
                            wineObstacles.Add(obj);
                        }
                        else
                        {
                            var obj = tempObstacles[0];
                            obj.transform.position = new Vector3(3.25f, 0, (res + 6) * 28f + 14);
                            tempObstacles.RemoveAt(0);
                            tempObstacles.Add(obj);
                        }
                        break;
                    case '3':
                        if (currPos != 3 && currPos != 6)
                        {
                            var obj = wineObstacles[0];
                            obj.transform.position = new Vector3(5.5f, 0, (res + 7) * 28f);
                            wineObstacles.RemoveAt(0);
                            wineObstacles.Add(obj);
                        }
                        else
                        {
                            var obj = tempObstacles[0];
                            obj.transform.position = new Vector3(5.5f, 0, (res + 7) * 28f);
                            tempObstacles.RemoveAt(0);
                            tempObstacles.Add(obj);
                        }
                        break;
                }
                #endregion
                currPos++;
                if(currPos == 7)
                {
                    currPos = 0;
                    currSeqIndex = (currSeqIndex + 1) % seq.Length;
                }
            }
            while(resSurface > surfaceCount)
            {
                surfaceCount++;
                var surfacs = surfaces[0];
                surfacs.transform.position = new Vector3(surfacs.transform.position.x, surfacs.transform.position.y, surfacs.transform.position.z + 600f);
                surfaces.RemoveAt(0);
                surfaces.Add(surfacs);
            }
        }
    }
}
