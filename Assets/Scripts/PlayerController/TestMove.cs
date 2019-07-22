using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    private Vector3 directions;
    private CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        directions = new Vector3(0, 0, 5);
    }

    private void Update()
    {
        cc.Move(directions*Time.deltaTime);
        if (!cc.isGrounded)
            directions.y -= 9.8f * Time.deltaTime;
        
    }
}
