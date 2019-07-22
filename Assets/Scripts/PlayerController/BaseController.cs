using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mg.Wy
{
    public class BaseController : MonoBehaviour
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
            direction = new Vector3(0, 0, 10);
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
            if (hitTime > 0) hitTime -= Time.deltaTime;
            if (hitTime < 0) hitTime = 0;
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Base Layer.Jump"))
                animator.SetBool("Jump", false);
            if (info.IsName("Base Layer.Roll"))
                animator.SetBool("Roll", false);
            if (info.IsName("Base Layer.Hit"))
                animator.SetBool("Hit", false);
            direction.y -= 9.8f * Time.deltaTime;
            cc.Move(direction * Time.deltaTime);
            if (hitTime <= 0f)
                direction.z = GameManager.Instance.player.Speed;
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
                    if (Vector2.Dot(direc, new Vector2(0, 1)) > Mathf.Sqrt(2) / 2)
                    {
                        animator.SetBool("Jump", true);
                        direction.y = WyConstants.JumpHeight;
                    }
                    else if (Vector2.Dot(direc, new Vector2(0, 1)) < -Mathf.Sqrt(2) / 2)
                    {
                        animator.SetBool("Roll", true);
                        cc.height = 0.2f;
                        cc.center = new Vector3(cc.center.x, 0.1f, cc.center.z);
                        Invoke("ResumeHeight", 1.5f);

                    }

                    isTouch = false;
                }

            }
            if (Input.GetMouseButtonUp(0))
                isTouch = false;
        }

        #endregion

        #region Common Functions

        private void ResumeHeight()
        {
            cc.height = 2f;
            cc.center = new Vector3(cc.center.x, 1, cc.center.z);
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

    }
}