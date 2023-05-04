using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Entropedia;
public class MenuController : MonoBehaviour
{
    public Toggle constToggle, eclipticToggle, defaultvrToggle, realtimeToggle, arModeToggle, showTerrainToggle, constelPictureToggle, constelAnimToggle;
    public Button changeTimeBtn, timeChangeApplyBtn;
    public GameObject ui, vrBtn, StarNames, ConstLines, StarColliders, eliptic, changeTimeUi, terrain, hamburger;
    public TouchRotationControl trc;
    private bool isUiOpen = false, isTimeChangeUiOpen = false;
    public Sun sun;
    public StarCalc starCalc;
    public Text day,month,year,hour,minute,second;
    void Start()
    {
        if (PlayerPrefs.GetString("defaultMode", "true") == "true")
            defaultvrToggle.isOn = true;
        else
            defaultvrToggle.isOn = false;
        if (PlayerPrefs.GetString("constToggle", "true") == "true")
            constToggle.isOn = true;
        else
            constToggle.isOn = false;
        if (PlayerPrefs.GetString("eclipticToggle", "true") == "true")
            eclipticToggle.isOn = true;
        else
            eclipticToggle.isOn = false;
        if (PlayerPrefs.GetString("arEnabled", "true") == "true")
        {
            arModeToggle.isOn = true;
            trc.isAREnabled = true;
        }
        else
        {
            arModeToggle.isOn = false;
            trc.isAREnabled = false;
        }
        if (PlayerPrefs.GetString("realtimeToggle", "true") == "true")
        {
            realtimeToggle.isOn = true;
            sun.SetDate(starCalc.time);
            sun.SetPosition();
        }
        else
        {
            realtimeToggle.isOn = false;
            sun.SetDate(new DateTime(2023, 1, 1, 0, 0, 1));
        }
        if (PlayerPrefs.GetString("showTerrainToggle", "true") == "true")
            showTerrainToggle.isOn = true;
        else
            showTerrainToggle.isOn = false;
        if (PlayerPrefs.GetString("constelPictureToggle", "true") == "true")
            constelPictureToggle.isOn = true;
        else
            constelPictureToggle.isOn = false;
        if (PlayerPrefs.GetString("constelAnimToggle", "true") == "true")
            constelAnimToggle.isOn = true;
        else
            constelAnimToggle.isOn = false;
        eclipticToggle.onValueChanged.AddListener(delegate
        {
            toggleEclipticCircle();
        });
        constToggle.onValueChanged.AddListener(delegate
        {
            toggleConstellations();
        });
        defaultvrToggle.onValueChanged.AddListener(delegate
        {
            toggleDefaultMode();
        });
        realtimeToggle.onValueChanged.AddListener(delegate
        {
            toggleRealtimeSun();
        });
        arModeToggle.onValueChanged.AddListener(delegate
        {
            toggleArMode();
        });
        showTerrainToggle.onValueChanged.AddListener(delegate
        {
            toggleTerrainVis();
        });
        constelPictureToggle.onValueChanged.AddListener(delegate
        {
            toggleConstPicture();
        });
        constelAnimToggle.onValueChanged.AddListener(delegate
        {
            toggleConstelAnim();
        });
        hamburger.GetComponent<Button>().onClick.AddListener(delegate
        {
            toggleUiVis();
        });
        changeTimeBtn.onClick.AddListener(delegate
        {
            toggleChangeTimeUIVis();
        });
        timeChangeApplyBtn.onClick.AddListener(delegate{
            applyTimeChange();
        });
    }
    void applyTimeChange()
    {
        DateTime d = new DateTime(int.Parse(year.text),int.Parse(month.text),int.Parse(day.text),int.Parse(hour.text),int.Parse(minute.text),int.Parse(second.text));
        starCalc.SetDate(d);
        for(int i=0;i<StarColliders.transform.childCount;i++)
        {
            GameObject.Destroy(StarColliders.transform.GetChild(i).gameObject);
        }
        for(int i=0;i<StarNames.transform.childCount;i++)
        {
            GameObject.Destroy(StarNames.transform.GetChild(i).gameObject);
        }
        for(int i=0;i<ConstLines.transform.childCount;i++)
        {
            GameObject.Destroy(ConstLines.transform.GetChild(i).gameObject);
        }
        StartCoroutine(starCalc.plotStar());
        if(realtimeToggle.isOn)
        {
            sun.SetDate(d);
            sun.SetPosition();
        }
    }
    void toggleEclipticCircle()
    {
        eliptic.SetActive(eclipticToggle.isOn);
        if (eclipticToggle.isOn)
            PlayerPrefs.SetString("eclipticToggle", "true");
        else
            PlayerPrefs.SetString("eclipticToggle", "false");
        PlayerPrefs.Save();
    }
    void toggleConstellations()
    {
        if (constToggle.isOn)
        {
            PlayerPrefs.SetString("constToggle", "true");
            starCalc.plotConstellations();
        }
        else
        {
            for (var i = ConstLines.transform.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(ConstLines.transform.GetChild(i).gameObject);
            }
            PlayerPrefs.SetString("constToggle", "false");
        }
        PlayerPrefs.Save();
    }
    void toggleDefaultMode()
    {
        if (defaultvrToggle.isOn)
            PlayerPrefs.SetString("defaultMode", "true");
        else
            PlayerPrefs.SetString("defaultMode", "false");
        PlayerPrefs.Save();
    }
    void toggleRealtimeSun()
    {
        if (realtimeToggle.isOn)
        {
            PlayerPrefs.SetString("realtimeToggle", "true");
            sun.SetDate(starCalc.time);
            sun.SetPosition();
        }
        else
        {
            PlayerPrefs.SetString("realtimeToggle", "false");
            sun.SetDate(new DateTime(2023, 1, 1, 0, 0, 1));
            sun.SetPosition();
        }
        PlayerPrefs.Save();
    }
    void toggleArMode()
    {
        if (arModeToggle.isOn)
        {
            PlayerPrefs.SetString("arEnabled", "true");
            trc.isAREnabled = true;
        }
        else
        {
            PlayerPrefs.SetString("arEnabled", "false");
            trc.isAREnabled = false;
        }
        PlayerPrefs.Save();
    }
    void toggleTerrainVis()
    {
        terrain.SetActive(showTerrainToggle.isOn);
        if (showTerrainToggle.isOn)
            PlayerPrefs.SetString("showTerrainToggle", "true");
        else
            PlayerPrefs.SetString("showTerrainToggle", "false");
        PlayerPrefs.Save();
    }
    void toggleConstPicture()
    {

    }
    void toggleConstelAnim()
    {
        if (constelAnimToggle.isOn)
            PlayerPrefs.SetString("constelAnimToggle", "true");
        else
            PlayerPrefs.SetString("constelAnimToggle", "false");
        PlayerPrefs.Save();
        if (constelAnimToggle.isOn)
        {
            for (int i = 0; i < ConstLines.transform.childCount; i++)
            {
                for (int j = 0; j < ConstLines.transform.GetChild(i).transform.childCount; j++)
                {
                    if (ConstLines.transform.GetChild(i).transform.GetChild(j).tag == "ConstelLine")
                    {
                        ConstLines.transform.GetChild(i).transform.GetChild(j).GetComponent<Animator>().Play("LineAnimationClose");
                        continue;
                    }
                    if (ConstLines.transform.GetChild(i).transform.GetChild(j).tag == "ConstelName")
                    {
                        ConstLines.transform.GetChild(i).transform.GetChild(j).GetComponent<Animator>().Play("ConstelTextHide");
                        continue;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < ConstLines.transform.childCount; i++)
            {
                for (int j = 0; j < ConstLines.transform.GetChild(i).transform.childCount; j++)
                {
                    if (ConstLines.transform.GetChild(i).transform.GetChild(j).tag == "ConstelLine")
                    {
                        ConstLines.transform.GetChild(i).transform.GetChild(j).GetComponent<Animator>().Play("LineAnimationOpen");
                        continue;
                    }
                    if (ConstLines.transform.GetChild(i).transform.GetChild(j).tag == "ConstelName")
                    {
                        ConstLines.transform.GetChild(i).transform.GetChild(j).GetComponent<Animator>().Play("ConstelTextShow");
                        continue;
                    }
                }
            }
        }
    }
    void toggleUiVis()
    {
        if (!isUiOpen)
        {
            vrBtn.SetActive(false);
            hamburger.GetComponent<Animator>().Play("MenuBtnOpen");
            ui.GetComponent<Animator>().Play("MenuUIOpen");
            isUiOpen = true;
        }
        else
        {
            if (isTimeChangeUiOpen)
            {
                changeTimeUi.GetComponent<Animator>().Play("TimeUiClose");
                isTimeChangeUiOpen = false;
            }
            ui.GetComponent<Animator>().Play("MenuUIClose");
            hamburger.GetComponent<Animator>().Play("MenuBtnClose");
            isUiOpen = false;
            vrBtn.SetActive(true);
        }
    }
    void toggleChangeTimeUIVis()
    {
        if (!isTimeChangeUiOpen)
        {
            changeTimeUi.GetComponent<Animator>().Play("TimeUiOpen");
            isTimeChangeUiOpen = true;
        }
        else
        {
            changeTimeUi.GetComponent<Animator>().Play("TimeUiClose");
            isTimeChangeUiOpen = false;
        }

    }
}
