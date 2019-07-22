using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTest : MonoBehaviour
{
    CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        cc.Move(new Vector3(x, 0, v)*Time.deltaTime*5f);
    }
    
}
