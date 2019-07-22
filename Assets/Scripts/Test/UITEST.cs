using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITEST : MonoBehaviour
{
    public Image btn;

    private void Awake()
    {
        var obj = Resources.Load("Textures/BaiShe") as GameObject;
        btn.GetComponent<Image>().sprite = obj.GetComponent<Image>().sprite;
    }
}
