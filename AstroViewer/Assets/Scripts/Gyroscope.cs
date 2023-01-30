using UnityEngine;
public class Gyroscope : MonoBehaviour
{
    public Vector3 sensorAngleInHeadset;
    public Camera c;
    // Quaternion _neutralizer = Quaternion.identity;
    Quaternion _neutralizer = Quaternion.Euler(90,180,-180);
    Quaternion GetUprightAttitude() {
        return Input.gyro.attitude * Quaternion.Euler(sensorAngleInHeadset);
    }
    void Start()
    {
        Input.gyro.enabled = true;
    }
    void Update() {
        var corrected = _neutralizer * GetUprightAttitude();
        c.transform.rotation = corrected;
    }
}
