using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFind : MonoBehaviour
{
    private void Awake()
    {
        GameObject obj = transform.Find("Test").gameObject;
        Debug.Log(obj.name);
    }
}
