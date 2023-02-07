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
        TextAsset File = Resources.Load<TextAsset>("StarDesc/"+gameObject.name);
        string[] linesFromfile = File.text.Split("\n");
        canv.transform.GetChild(0).gameObject.SetActive(true);
        canv.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = linesFromfile[0];
        canv.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = linesFromfile[1];
        canv.transform.GetChild(0).GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>("StarDesc/"+name+"Type");
        canv.transform.GetChild(0).GetChild(6).GetComponent<TextMeshProUGUI>().text = linesFromfile[2];
        canv.transform.GetChild(0).GetChild(7).GetComponent<TextMeshProUGUI>().text = linesFromfile[3];
        canv.transform.GetChild(0).GetChild(8).GetComponent<Image>().sprite = Resources.Load<Sprite>("StarDesc/"+name);
    }
}
