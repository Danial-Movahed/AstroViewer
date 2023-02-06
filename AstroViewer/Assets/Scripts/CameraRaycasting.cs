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
    public GameObject StarName,StarType,TypeImage,StarMath,StarDesc,StarImage;
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
        // SHOW EVERYTHING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        TextAsset File = Resources.Load<TextAsset>("StarDesc/"+name);
        string[] linesFromfile = File.text.Split("\n");
        StarName.GetComponent<TextMeshProUGUI>().text = linesFromfile[0];
        StarType.GetComponent<TextMeshProUGUI>().text = linesFromfile[1];
        TypeImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("StarDesc/"+name+"Type");
        StarMath.GetComponent<TextMeshProUGUI>().text = linesFromfile[2];
        StarDesc.GetComponent<TextMeshProUGUI>().text = linesFromfile[3];
        StarImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("StarDesc/"+name);
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
