using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mg.Wy
{
    public class Rotate : MonoBehaviour
    {
        public Text currProgres;

        private int progress = 0;

        public float speed = 100f;

        public float currAngle = 0f;

        public float timeLeft = 7.2f;

        [Tooltip("After Player WIn the LuoPan")]
        public GameObject winUI;

   
        private void FixedUpdate()
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft <= 0)
            {
                /*if(UIManager.Instance.LuoCorrectCount == 1)
                {
                    progress += 10;
                    currProgres.text = progress.ToString() + "%";
                    if(progress == 100)
                    {
                        GameManager.Instance.WinTheGame();
                        winUI.SetActive(true);
                    }
                }*/
                timeLeft = 7.2f;
                UIManager.Instance.CallResumePanCount();
                transform.rotation = Quaternion.identity;
                currAngle = 0f;
                speed = 100f;
            }
            transform.Rotate(new Vector3(0, 0, -1) * speed * Time.deltaTime);
            currAngle += speed * Time.deltaTime;
            currAngle %= 360f;
            InputProcess();
        }

        void InputProcess()
        {
            if (Input.GetMouseButtonDown(0))
            {
                string name = IsOverGUI(Input.mousePosition);
                if (name.Equals("LuoPan") || name.Equals("Point"))
                {
                    speed = 0f;
                    if (checkIfCorrect())
                    {
                        UIManager.Instance.CallLuoCount();
                        progress += 10;
                        if (progress >= 100)
                            progress = 100;
                        currProgres.text = progress.ToString() + "%";
                        if (progress >= 100)
                        {
                            GameManager.Instance.WinTheGame();
                            winUI.SetActive(true);
                        }
                    }
                    else
                    {
                        //TODO NOTHING
                    }
                }
            }
        }

        private bool checkIfCorrect()
        {
            if(currAngle >= 112.5 && currAngle <= 157.5)
            {
                return true;
            }
            return false;
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
    }

}