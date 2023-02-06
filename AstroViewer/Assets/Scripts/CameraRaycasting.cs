using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraRaycasting : MonoBehaviour
{
    public float lockTime = 3.0f; 
    private bool locking = false;
    private float timestamp = 0.1f;
    private bool isPollingForRaycast = true;
    private RaycastHit hit;
    public GameObject DescUI, CamCanvas, DescCanvas;
    public GameObject starName;
    public Image progress;
    
    void Start()
    {
        DescCanvas.GetComponent<Canvas>().transform.localScale = CamCanvas.GetComponent<Canvas>().transform.localScale;
    }

    void ShowStarDescUI(string name)
    {
        DescCanvas.GetComponent<Canvas>().transform.rotation = CamCanvas.GetComponent<Canvas>().transform.rotation;
        DescCanvas.GetComponent<Canvas>().transform.position = CamCanvas.GetComponent<Canvas>().transform.position;
        DescUI.SetActive(true);
        starName.GetComponent<TextMeshProUGUI>().text = name;
    }

    void Update()
    {
        if(isPollingForRaycast)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                if (!locking)
                {
                    locking = true;
                    timestamp = Time.time + lockTime;
                }
            }
            else
            {
                locking = false;
            }
            if (locking && Time.time >= timestamp)
            {
                isPollingForRaycast = false;
                locking = false;
                ShowStarDescUI(hit.collider.name);
            }
        }
        // Debug.Log(Convert.ToInt32(locking) * (Time.time - timestamp + lockTime) / lockTime);
        progress.fillAmount = Convert.ToInt32(locking) * (Time.time - timestamp + lockTime) / lockTime;
    }
}
