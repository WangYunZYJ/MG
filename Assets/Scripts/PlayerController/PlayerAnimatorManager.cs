using UnityEngine;
using System.Collections;
using Photon.Pun;

namespace Mg.Wy
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        #region MonoBehaviour Callbacks
        private Animator animator;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }


        void Update()
        {

            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            if (!animator)
            {
                return;
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (v < 0)
            {
                v = 0;
            }
            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h);
        }


        #endregion
    }
}