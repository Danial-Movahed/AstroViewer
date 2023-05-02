using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Entropedia;
public class MenuController : MonoBehaviour
{
    // public Toggle constToggle,eclipticToggle,defaultvrToggle,realTimeSunToggle;
    public Toggle constToggle,eclipticToggle,defaultvrToggle, alwaysShowMenuToggle, arModeToggle, showTerrainToggle, constelPictureToggle, constelAnimToggle;
    public Button hamburger, changeTimeBtn;
    public GameObject ui,vrBtn,StarNames,ConstLines,StarColliders,PS, eliptic, changeTimeUi;
    public TouchRotationControl trc;
    public Sun sun;
    // public TextMeshProUGUI sunTimesText;
    public MenuBtnAnimationController menuBtnAnimationController, changeTimeUiAnimationController;

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
        if(PlayerPrefs.GetString("eclipticToggle", "true") == "true")
            eclipticToggle.isOn = true;
        else
            eclipticToggle.isOn = false;
        if(PlayerPrefs.GetString("arEnabled", "true") == "true")
            trc.isAREnabled = true;
        else
            trc.isAREnabled = false;
        if(PlayerPrefs.GetString("alwaysShowMenuToggle", "true") == "true")
            alwaysShowMenuToggle.isOn = true;
        else
            alwaysShowMenuToggle.isOn = false;
        if(PlayerPrefs.GetString("showTerrainToggle", "true") == "true")
            showTerrainToggle.isOn = true;
        else
            showTerrainToggle.isOn = false;
        if(PlayerPrefs.GetString("constelPictureToggle", "true") == "true")
            constelPictureToggle.isOn = true;
        else
            constelPictureToggle.isOn = false;
        if(PlayerPrefs.GetString("constelAnimToggle", "true") == "true")
            constelAnimToggle.isOn = true;
        else
            constelAnimToggle.isOn = false;

        // if(PlayerPrefs.GetString("realTimeSunToggle","true") == "true")
        // {
        //     realTimeSunToggle.isOn = true;
        //     sun.isRealtimeEnabled = true;
        //     SunTimeSlider.SetActive(false);
        // }
        // else
        // {
        //     realTimeSunToggle.isOn = false;
        //     sun.isRealtimeEnabled = false;
        //     SunTimeSlider.SetActive(true);
        //     setupTimeSliderText();
        // }
        // SunTimeSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate {
        //     sunChangeTime();
        // });
        eclipticToggle.onValueChanged.AddListener(delegate {
            toggleEclipticCircle();
        });
        constToggle.onValueChanged.AddListener(delegate {
            toggleConstellations();
        });
        defaultvrToggle.onValueChanged.AddListener(delegate {
            toggleDefaultMode();
        });
        alwaysShowMenuToggle.onValueChanged.AddListener(delegate {
            toggleShowMenu();
        });
        arModeToggle.onValueChanged.AddListener(delegate {
            toggleArMode();
        });
        showTerrainToggle.onValueChanged.AddListener(delegate {
            toggleTerrainVis();
        });
        constelPictureToggle.onValueChanged.AddListener(delegate {
            toggleConstPicture();
        });
        constelAnimToggle.onValueChanged.AddListener(delegate {
            toggleConstelAnim();
        });
        // realTimeSunToggle.onValueChanged.AddListener(delegate {
        //     setupTimeSliderText();
        //     saveApply();
        // });
        hamburger.onClick.AddListener(delegate {
            toggleUiVis();
        });
        changeTimeBtn.onClick.AddListener(delegate {
            toggleChangeTimeUIVis();
        });
    }
    // void setupTimeSliderText()
    // {
    //     sun.minutes = 0;
    //     sun.hour = 1;
    //     sun.SetTime(1,0);
    //     sun.SetPosition();
    //     sunTimeText.text = "Sun Time: 1:00";
    // }
    // void sunChangeTime()
    // {
    //     sun.minutes = 0;
    //     sun.hour = (int)SunTimeSlider.GetComponent<Slider>().value;
    //     sun.SetTime((int)SunTimeSlider.GetComponent<Slider>().value,0);
    //     sun.SetPosition();
    //     sunTimeText.text = "Sun Time: "+((int)SunTimeSlider.GetComponent<Slider>().value).ToString()+":00";
    // }
    void toggleEclipticCircle()
    {

    }
    void toggleConstellations()
    {

    }
    void toggleDefaultMode()
    {

    }
    void toggleShowMenu()
    {

    }
    void toggleArMode()
    {

    }
    void toggleTerrainVis()
    {

    }
    void toggleConstPicture()
    {

    }
    void toggleConstelAnim()
    {

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
        
        // if(realTimeSunToggle.isOn)
        // {
        //     PlayerPrefs.SetString("realTimeSunToggle","true");
        //     SunTimeSlider.SetActive(false);
        //     sun.isRealtimeEnabled = true;
        //     sun.SetDate(new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,DateTime.Now.Hour,DateTime.Now.Minute,0));
        //     sun.SetPosition();
        // }
        // else
        // {
        //     PlayerPrefs.SetString("realTimeSunToggle","false");
        //     SunTimeSlider.SetActive(true);
        //     sun.isRealtimeEnabled = false;
        // }
        PlayerPrefs.Save();
        
        GameObject.Find("StarRotator").GetComponent<StarCalc>().StartCoroutine("plotStar");
    }
    void toggleUiVis()
    {
        if(!ui.activeSelf)
            menuBtnAnimationController.PlayAnimation("MenuBtnOpen");
        else
            menuBtnAnimationController.PlayAnimation("MenuBtnClose");
        ui.SetActive(!ui.activeSelf);
        vrBtn.SetActive(!vrBtn.activeSelf);
    }
    void toggleChangeTimeUIVis()
    {
        if(!changeTimeUi.activeSelf)
            changeTimeUiAnimationController.PlayAnimation("ChangeTimeUiOpen");
        else
            changeTimeUiAnimationController.PlayAnimation("ChangeTimeUiClose");
        changeTimeUi.SetActive(!changeTimeUi.activeSelf);
    }
}
