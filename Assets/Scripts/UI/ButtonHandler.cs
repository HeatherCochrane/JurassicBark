﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setActive(bool set)
    {
        this.gameObject.SetActive(set);
        CameraControl.instance.followTransform = null;
    }
}
