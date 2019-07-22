using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFish : MonoBehaviour
{
    public GameObject center;
    public float speed = 5f;

    
    private void Update()
    {
        center.transform.Rotate(new Vector3(0, 0, -speed), Space.World);
    }
}
