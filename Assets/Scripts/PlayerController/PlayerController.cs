
//#define TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;
namespace Mg.Wy
{

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonTransformView))]
    [RequireComponent(typeof(PhotonAnimatorView))]
    public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
    {

        public float hitTime = 0f;

        #region Private Field

        private Animator animator = null;

#if ANDROID || IPHONE
        private Touch beginPos;
#else
        private Vector3 beginPos;
#endif
        private CharacterController cc;

        private bool isTouch = false;

        public Vector3 direction;
        
        
#endregion

#region MonoBehavior Callbacks

        private void Awake()
        {
            animator = GetComponent<Animator>();
            cc = GetComponent<CharacterController>();
            Input.multiTouchEnabled = false;
#if TEST
            direction = new Vector3(0, 0, GameManager.Instance.player.Speed);
#else
            direction = new Vector3(0, 0, 1);
#endif
        }
        

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {

            Debug.Log("Hit" + hit.gameObject.name);
            if (hit.gameObject.name[0] != '$') return;
            animator.SetBool("Hit", true);
            hitTime = 0.5f;
            direction.z = -direction.z;
        }

        private void Update()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
                return;
            if (hitTime > 0) hitTime -= Time.deltaTime;
            if (hitTime < 0) hitTime = 0;
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Base Layer.Jump"))
                animator.SetBool("Jump", false);
            if (info.IsName("Base Layer.Roll"))
                animator.SetBool("Roll", false);
            if(info.IsName("Base Layer.Hit"))
                animator.SetBool("Hit", false);
            if (GameManager.Instance.player == null) return;
            switch (GameManager.Instance.player.CurrTrack)
            {
                case 1:
                    if (Mathf.Abs(transform.position.x - WyConstants.Street1) <= 0.25f && !UIManager.Instance.inLeaf)
                    {
                        transform.position = new Vector3(WyConstants.Street1, transform.position.y, transform.position.z);
                        direction.x = 0;
                    }
                    break;
                case 2:
                    if (Mathf.Abs(transform.position.x - WyConstants.Street2) <= 0.25f && !UIManager.Instance.inLeaf)
                    {
                        transform.position = new Vector3(WyConstants.Street2, transform.position.y, transform.position.z);
                        direction.x = 0;
                    }
                    break;
                case 3:
                    if (Mathf.Abs(transform.position.x - WyConstants.Street3) <= 0.25f && !UIManager.Instance.inLeaf)
                    {
                        transform.position = new Vector3(WyConstants.Street3, transform.position.y, transform.position.z);
                        direction.x = 0;
                    }
                    break;
            }
            
            direction.y -= 9.8f * Time.deltaTime;
            if (GameManager.Instance.player != null && hitTime <= 0f)
                direction.z = GameManager.Instance.player.Speed;
            cc.Move(direction * Time.deltaTime);
#if ANDROID || IPHONE
            if (Input.touchCount <= 0)
            {
                isTouch = false;
                return;
            }
#endif

            ProcessInputs();
        }
#endregion

#region Private Functions

        private void ProcessInputs()
        {
            Debug.Log("Process Inputs");
#if IPHONE || ANDROID
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) 
#else
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Over UI");
#endif
                return;
            }
#if IPHONE || ANDROID
            Touch touch = Input.touches[0];
            if(touch.phase == TouchPhase.Began)
            {
                beginPos = touch;
                isTouch = true;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                if (isTouch)
                {
                    Vector2 direction = touch.position - beginPos.position;

                    if (UIManager .Instance.direction == 1)
                    {
                        if (Vector2.Dot(direction, new Vector2(0, 1)) > Mathf.Sqrt(2) / 2)
                        {
                            animator.SetBool("Jump", true);
#if TEST
                            direction.y = GameManager.Instance.player.JumpHeight;
#else
                        direction.y = GameManager.Instance.player.JumpHeight;
#endif
                        }
                        else if (Vector2.Dot(direction, new Vector2(0, 1)) < -Mathf.Sqrt(2) / 2)
                        {
                            animator.SetBool("Roll", true);
                        }
                        else if (Vector2.Dot(direction, new Vector2(1, 0)) > Mathf.Sqrt(2) / 2)
                        {

                            if (GameManager.Instance.player.CurrTrack != 3)
                            {
                                animator.SetBool("Right", true);
                                GameManager.Instance.player.CurrTrack++;
                                direction.x = GameManager.Instance.player.DodgeSpeed;
                            }
                        }
                        else if (Vector2.Dot(direction, new Vector2(1, 0)) < -Mathf.Sqrt(2) / 2)
                        {
                            if (GameManager.Instance.player.CurrTrack != 1)
                            {
                                animator.SetBool("Left", true);
                                GameManager.Instance.player.CurrTrack--;
                                direction.x = -GameManager.Instance.player.DodgeSpeed;
                            }
                        }
                    }
                    else
                    {
                        if (Vector2.Dot(direction, new Vector2(0, 1)) > Mathf.Sqrt(2) / 2)
                        {
                            animator.SetBool("Roll", true);
                            
                        }
                        else if (Vector2.Dot(direction, new Vector2(0, 1)) < Mathf.Sqrt(2) / 2)
                        {
                            animator.SetBool("Jump", true);
#if TEST
                            direction.y = GameManager.Instance.player.JumpHeight;
#else
                        direction.y = GameManager.Instance.player.JumpHeight;
#endif
                        }
                        else if (Vector2.Dot(direction, new Vector2(1, 0)) > Mathf.Sqrt(2) / 2)
                        {
                            if (GameManager.Instance.player.CurrTrack != 1)
                            {
                                animator.SetBool("Left", true);
                                GameManager.Instance.player.CurrTrack--;
                                direction.x = -GameManager.Instance.player.DodgeSpeed;
                            }
                        }
                        else
                        {
                            if (GameManager.Instance.player.CurrTrack != 3)
                            {
                                animator.SetBool("Right", true);
                                GameManager.Instance.player.CurrTrack++;
                                direction.x = GameManager.Instance.player.DodgeSpeed;
                            }
                        }
                    }
                }
                isTouch = false;
            }
            if(touch.phase == TouchPhase.Ended)
            {
                isTouch = false;
            }
#else
            if (Input.GetMouseButtonDown(0))
            {
                isTouch = true;
                beginPos = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                Vector2 direc = Input.mousePosition - beginPos;
                if (direc.magnitude < 1f)
                    return;
                direc = direc.normalized;
                //Debug.Log("IsTouch" + isTouch);
                //Debug.Log("MousePos" + Input.mousePosition);
                if (isTouch)
                {
                   
                    Debug.Log("Direction" + direc);
                    if (UIManager.Instance.direction == 1)
                    {
                        if (Vector2.Dot(direc, new Vector2(0, 1)) > Mathf.Sqrt(2) / 2)
                        {
                            animator.SetBool("Jump", true);
#if TEST
                            direction.y = GameManager.Instance.player.JumpHeight;
#else
                            direction.y = GameManager.Instance.player.JumpHeight;
#endif
                        }
                        else if (Vector2.Dot(direc, new Vector2(0, 1)) < -Mathf.Sqrt(2) / 2)
                        {
                            animator.SetBool("Roll", true);
                            cc.height = 0.02f;
                            cc.center = new Vector3(cc.center.x, 0.01f, cc.center.z);
                            Invoke("ResumeHeight", 1.5f);
                        }
                        else if (Vector2.Dot(direc, new Vector2(1, 0)) > Mathf.Sqrt(2) / 2)
                        {
                            if (UIManager.Instance.inLeaf)
                            {
                                isTouch = false;
                                return;
                            }
                            if (GameManager.Instance.player.CurrTrack != 3)
                            {
                                animator.SetBool("Right", true);
                                GameManager.Instance.player.CurrTrack++;
                                direction.x = GameManager.Instance.player.DodgeSpeed;
                            }
                        }
                        else if (Vector2.Dot(direc, new Vector2(1, 0)) < -Mathf.Sqrt(2) / 2)
                        {
                            if (UIManager.Instance.inLeaf)
                            {
                                isTouch = false;
                                return;
                            }
                            if (GameManager.Instance.player.CurrTrack != 1)
                            {
                                animator.SetBool("Left", true);
                                GameManager.Instance.player.CurrTrack--;
                                direction.x = -GameManager.Instance.player.DodgeSpeed;
                            }
                        }
                        
                        isTouch = false;
                    }
                    else
                    {
                        if (Vector2.Dot(direc, new Vector2(0, 1)) > Mathf.Sqrt(2) / 2)
                        {
                            animator.SetBool("Roll", true);
                            cc.height = 0.2f;
                            cc.center = new Vector3(cc.center.x, 0.1f, cc.center.z);
                            Invoke("ResumeHeight", 1.5f);

                        }
                        else if (Vector2.Dot(direc, new Vector2(0, 1)) < Mathf.Sqrt(2) / 2)
                        {
                            animator.SetBool("Jump", true);
#if TEST
                            direction.y = GameManager.Instance.player.JumpHeight;
#else
                            direction.y = GameManager.Instance.player.JumpHeight;
#endif
                        }
                        else if (Vector2.Dot(direc, new Vector2(1, 0)) > Mathf.Sqrt(2) / 2)
                        {
                            if (GameManager.Instance.player.CurrTrack != 1)
                            {
                                if (UIManager.Instance.inLeaf)
                                {
                                    isTouch = false;
                                    return;
                                }
                                animator.SetBool("Left", true);
                                GameManager.Instance.player.CurrTrack--;
                                direction.x = -GameManager.Instance.player.DodgeSpeed;
                            }
                        }
                        else
                        {
                            if (GameManager.Instance.player.CurrTrack != 3)
                            {
                                if (UIManager.Instance.inLeaf)
                                {
                                    isTouch = false;
                                    return;
                                }
                                animator.SetBool("Right", true);
                                GameManager.Instance.player.CurrTrack++;
                                direction.x = GameManager.Instance.player.DodgeSpeed;
                            }
                        }
                        isTouch = false;
                    }
                }

                isTouch = false;
            }
            if (Input.GetMouseButtonUp(0))
                isTouch = false;
#endif
        }

        #endregion

        #region Common Functions

        private void ResumeHeight()
        {
            cc.height = 0.08f;
            cc.center = new Vector3(cc.center.x, 0.04f, cc.center.z);
        }

        public void Run()
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Roll", false);
        }


        private void LockPos()
        {

        }
#endregion

#region IPunObservable

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
                /*if (stream.IsWriting)
                {
                    stream.SendNext(isFire);
                    stream.SendNext(Health);
                }
                else
                {
                    this.isFire = (bool)stream.ReceiveNext();
                    this.Health = (float)stream.ReceiveNext();
                }*/
                return;
        }

#endregion
    }
}