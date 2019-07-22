using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mg.Wy
{

    public class CameraMove : MonoBehaviour
    {
        public int pos = 1;
        private Vector3 cameraToPlayer;
        private GameObject Target = null;

        private void Start()
        {
            cameraToPlayer = new Vector3(pos, 1.1f, 0) - transform.position;
        }


        private void Update()
        {
            if (GameManager.Instance.localPlayer)
            {
                if (UIManager.Instance.inLeaf)
                {
                    Target = GameManager.Instance.copyPlayer;
                }
                else
                    Target = GameManager.Instance.localPlayer;
            }
        }

        private void LateUpdate()
        {
            if (GameManager.Instance.localPlayer && Target)
            {
                Vector3 nowPos = Target.transform.position - cameraToPlayer;
                transform.position = new Vector3(transform.position.x, transform.position.y, nowPos.z);
            }
        }

    }

}