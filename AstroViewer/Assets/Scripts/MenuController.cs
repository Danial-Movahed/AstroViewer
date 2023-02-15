using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Toggle constToggle,eclipticToggle,defaultvr;
    public Button hamburger;
    public GameObject ui,vrBtn;
    public GameObject StarNames,ConstLines,StarColliders;
    public GameObject PS, eliptic;

    void Start()
    {
        if(PlayerPrefs.GetString("defaultMode", "true") == "true")
            defaultvr.isOn = true;
        else
            defaultvr.isOn = false;
        if(PlayerPrefs.GetString("constToggle", "true") == "true")
            constToggle.isOn = true;
        else
            constToggle.isOn = false;
        if(PlayerPrefs.GetString("ecplipticToggle", "true") == "true")
            eclipticToggle.isOn = true;
        else
            eclipticToggle.isOn = false;
        eclipticToggle.onValueChanged.AddListener(delegate {
            saveApply();
        });
        constToggle.onValueChanged.AddListener(delegate {
            saveApply();
        });
        defaultvr.onValueChanged.AddListener(delegate {
            saveApply();
        });
        hamburger.onClick.AddListener(delegate {
            toggleUiVis();
        });
    }
    void saveApply()
    {
        ParticleSystem.Particle[] points = new ParticleSystem.Particle[10109];
        PS.GetComponent<ParticleSystem>().SetParticles(points, points.Length);
        eliptic.SetActive(false);
        for (var i = ConstLines.transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(ConstLines.transform.GetChild(i).gameObject);
        }
        for (var i = StarNames.transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(StarNames.transform.GetChild(i).gameObject);
        }
        for (var i = StarColliders.transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(StarColliders.transform.GetChild(i).gameObject);
        }
        if(defaultvr)
            PlayerPrefs.SetString("defaultMode","true");
        else
            PlayerPrefs.SetString("defaultMode","false");
        
        if(eclipticToggle)
            PlayerPrefs.SetString("eclipticToggle","true");
        else
            PlayerPrefs.SetString("eclipticToggle","false");
        
        if(constToggle)
            PlayerPrefs.SetString("constToggle","true");
        else
            PlayerPrefs.SetString("constToggle","false");
        PlayerPrefs.Save();
        
        GameObject.Find("StarRotator").GetComponent<StarCalc>().StartCoroutine("plotStar");
        Debug.Log(PlayerPrefs.GetString("constToggle"));
    }
    void toggleUiVis()
    {
        ui.SetActive(!ui.activeSelf);
        vrBtn.SetActive(!vrBtn.activeSelf);
    }
}
