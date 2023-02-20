using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Entropedia;
public class MenuController : MonoBehaviour
{
    public Toggle constToggle,eclipticToggle,defaultvrToggle,realTimeSunToggle;
    public Button hamburger;
    public GameObject ui,vrBtn, SunTimeSlider;
    public GameObject StarNames,ConstLines,StarColliders;
    public GameObject PS, eliptic;
    public Sun sun;
    public TextMeshProUGUI sunTimeText;

    void Start()
    {
        if(PlayerPrefs.GetString("defaultMode", "true") == "true")
            defaultvrToggle.isOn = true;
        else
            defaultvrToggle.isOn = false;
        if(PlayerPrefs.GetString("constToggle", "true") == "true")
            constToggle.isOn = true;
        else
            constToggle.isOn = false;
        if(PlayerPrefs.GetString("ecplipticToggle", "true") == "true")
            eclipticToggle.isOn = true;
        else
            eclipticToggle.isOn = false;
        if(PlayerPrefs.GetString("realTimeSunToggle","true") == "true")
        {
            realTimeSunToggle.isOn = true;
            sun.isRealtimeEnabled = true;
            SunTimeSlider.SetActive(false);
        }
        else
        {
            realTimeSunToggle.isOn = false;
            sun.isRealtimeEnabled = false;
            SunTimeSlider.SetActive(true);
            setupTimeSliderText();
        }
        SunTimeSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate {
            sunChangeTime();
        });
        eclipticToggle.onValueChanged.AddListener(delegate {
            saveApply();
        });
        constToggle.onValueChanged.AddListener(delegate {
            saveApply();
        });
        defaultvrToggle.onValueChanged.AddListener(delegate {
            saveApply();
        });
        realTimeSunToggle.onValueChanged.AddListener(delegate {
            setupTimeSliderText();
            saveApply();
        });
        hamburger.onClick.AddListener(delegate {
            toggleUiVis();
        });
    }
    void setupTimeSliderText()
    {
        sun.minutes = 0;
        sun.hour = 1;
        sun.SetTime(1,0);
        sun.SetPosition();
        sunTimeText.text = "Sun Time: 1:00";
    }
    void sunChangeTime()
    {
        sun.minutes = 0;
        sun.hour = (int)SunTimeSlider.GetComponent<Slider>().value;
        sun.SetTime((int)SunTimeSlider.GetComponent<Slider>().value,0);
        sun.SetPosition();
        sunTimeText.text = "Sun Time: "+((int)SunTimeSlider.GetComponent<Slider>().value).ToString()+":00";
    }
    void saveApply()
    {
        ParticleSystem.Particle[] points = new ParticleSystem.Particle[10109];
        PS.GetComponent<ParticleSystem>().SetParticles(points, points.Length);
        eliptic.SetActive(false);
        for (var i = ConstLines.transform.childCount - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(ConstLines.transform.GetChild(i).gameObject);
        }
        for (var i = StarNames.transform.childCount - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(StarNames.transform.GetChild(i).gameObject);
        }
        for (var i = StarColliders.transform.childCount - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(StarColliders.transform.GetChild(i).gameObject);
        }
        if(defaultvrToggle.isOn)
            PlayerPrefs.SetString("defaultMode","true");
        else
            PlayerPrefs.SetString("defaultMode","false");
        
        if(eclipticToggle.isOn)
            PlayerPrefs.SetString("eclipticToggle","true");
        else
            PlayerPrefs.SetString("eclipticToggle","false");
        
        if(constToggle.isOn)
            PlayerPrefs.SetString("constToggle","true");
        else
            PlayerPrefs.SetString("constToggle","false");
        
        if(realTimeSunToggle.isOn)
        {
            PlayerPrefs.SetString("realTimeSunToggle","true");
            SunTimeSlider.SetActive(false);
            sun.isRealtimeEnabled = true;
            sun.SetDate(new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,DateTime.Now.Hour,DateTime.Now.Minute,0));
            sun.SetPosition();
        }
        else
        {
            PlayerPrefs.SetString("realTimeSunToggle","false");
            SunTimeSlider.SetActive(true);
            sun.isRealtimeEnabled = false;
        }
        PlayerPrefs.Save();
        
        GameObject.Find("StarRotator").GetComponent<StarCalc>().StartCoroutine("plotStar");
    }
    void toggleUiVis()
    {
        ui.SetActive(!ui.activeSelf);
        vrBtn.SetActive(!vrBtn.activeSelf);
    }
}
