using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DescUILoader : MonoBehaviour
{
    public void Load(GameObject cam)
    {
        GameObject CamCanvas = GameObject.Find("CamCanvas");
        GameObject canv = GameObject.Find("DescCanvas");
        canv.GetComponent<Canvas>().GetComponent<RectTransform>().position = CamCanvas.GetComponent<Canvas>().GetComponent<RectTransform>().position;
        canv.GetComponent<Canvas>().GetComponent<RectTransform>().rotation = Quaternion.LookRotation( this.transform.position - cam.transform.position );
        canv.GetComponent<Canvas>().GetComponent<RectTransform>().localScale = CamCanvas.GetComponent<Canvas>().GetComponent<RectTransform>().localScale;
        canv.GetComponent<Canvas>().GetComponent<RectTransform>().sizeDelta = CamCanvas.GetComponent<Canvas>().GetComponent<RectTransform>().sizeDelta;
        canv.transform.GetChild(0).gameObject.SetActive(true);
        TextAsset File = Resources.Load<TextAsset>("StarDesc/"+gameObject.name+"/Desc");
        if(File != null)
        {
            canv.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameObject.name;
            canv.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = Resources.Load<TextAsset>("StarDesc/"+gameObject.name+"/Type").text;
            canv.transform.GetChild(0).GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>("StarDesc/"+gameObject.name+"/Type");;
            canv.transform.GetChild(0).GetChild(6).GetComponent<TextMeshProUGUI>().text = Resources.Load<TextAsset>("StarDesc/"+gameObject.name+"/Math").text;
            canv.transform.GetChild(0).GetChild(7).GetComponent<TextMeshProUGUI>().text = File.text;
            canv.transform.GetChild(0).GetChild(8).GetComponent<Image>().sprite = Resources.Load<Sprite>("StarDesc/"+gameObject.name+"/Image");
        }
        else
        {
            canv.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameObject.name;
            canv.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = "NI";
            canv.transform.GetChild(0).GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>("StarDesc/NI");
            canv.transform.GetChild(0).GetChild(6).GetComponent<TextMeshProUGUI>().text = "NI";
            canv.transform.GetChild(0).GetChild(7).GetComponent<TextMeshProUGUI>().text = "NI";
            canv.transform.GetChild(0).GetChild(8).GetComponent<Image>().sprite = Resources.Load<Sprite>("StarDesc/NI");
        }
    }
}
