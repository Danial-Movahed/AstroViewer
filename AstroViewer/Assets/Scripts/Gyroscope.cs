using UnityEngine;
public class Gyroscope : MonoBehaviour
{
    public Vector3 sensorAngleInHeadset;
    public Camera c;
    Quaternion _neutralizer = Quaternion.identity;
    Quaternion GetUprightAttitude() {
        return Input.gyro.attitude * Quaternion.Euler(sensorAngleInHeadset);
    }
    void Start()
    {
        Input.gyro.enabled = true;
    }

    // Each frame, rotate our video sphere according to the corrected orientation.
    void Update() {
        var corrected = _neutralizer * GetUprightAttitude();
        c.transform.rotation = corrected;
    }
}
