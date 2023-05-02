using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
public class Gyroscope : MonoBehaviour
{
    private Quaternion sensorAngleInHeadset = Quaternion.Euler(0,0,180);
    public Camera c;
    private Quaternion _neutralizer = Quaternion.Euler(90,0,0);
    public GameObject compassImage;
    Quaternion GetUprightAttitude()
    {
        return Input.gyro.attitude * sensorAngleInHeadset;
    }
    void Start()
    {
        Input.gyro.enabled = true;
    }
    void Update()
    {
        var corrected = _neutralizer * GetUprightAttitude();
        c.transform.rotation = corrected;
        if(Application.isEditor)
            return;
        if(XRGeneralSettings.Instance.Manager.isInitializationComplete)
            return;
        if(PlayerPrefs.GetString("arEnabled", "true") != "true")
            return;
        compassImage.transform.localRotation = Quaternion.Euler(0,0,corrected.eulerAngles.y);
    }
}
