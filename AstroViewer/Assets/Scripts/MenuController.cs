using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Toggle constToggle,eclipticToggle,defaultvr;
    public Button hamburger;
    public GameObject ui;

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
        GameObject.Find("Particle System").GetComponent<ParticleSystem>().SetParticles(points, points.Length);
        GameObject tmp = GameObject.Find("Const");
        while(tmp != null)
        {
            GameObject.Destroy(tmp);
            tmp = GameObject.Find("Const");
        }
        GameObject.Destroy(GameObject.Find("DayeratolBorooj"));
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
        
        GameObject.Find("StarRotator").GetComponent<StarCalc>().StartCoroutine("plotStar");
    }
    void toggleUiVis()
    {
        ui.SetActive(!ui.activeSelf);
    }
}
