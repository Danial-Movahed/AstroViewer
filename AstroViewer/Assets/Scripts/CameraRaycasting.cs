using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CameraRaycasting : MonoBehaviour
{
    public float lockTime = 3.0f; 
    private bool locking = false;
    private float timestamp = 0.1f;
    private RaycastHit hit;
    public GameObject CamCanvas, DescCanvas;
    public Image progress;
    private bool isUiOpen=false;
    public GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;
    void Update()
    {
        if(!isUiOpen)
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
                isUiOpen=true;
                locking = false;
                hit.collider.gameObject.GetComponent<DescUILoader>().Load(this.gameObject);
            }
            progress.fillAmount = Convert.ToInt32(locking) * (Time.time - timestamp + lockTime) / lockTime;
        }
        else
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = new Vector2(Screen.width/2,Screen.height/2);
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);
            string rayCol;
            try
            {
                rayCol = results[0].gameObject.name;
            }
            catch
            {
                rayCol = null;
            }
            if( rayCol == "CloseBG")
            {
                if (!locking)
                {
                    locking = true;
                    timestamp = Time.time + lockTime;
                }
            }
            else
                locking = false;
            if (locking && Time.time >= timestamp)
            {
                DescCanvas.transform.GetChild(0).gameObject.SetActive(false);
                isUiOpen = false;
                locking = false;
            }
            progress.fillAmount = Convert.ToInt32(locking) * (Time.time - timestamp + lockTime) / lockTime;
        }
    }
}
