using UnityEngine;
public class Gyroscope : MonoBehaviour
{
    private void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
    }
    void Update()
    {
        if (SystemInfo.supportsGyroscope)
            transform.rotation = GyroToUnity(Input.gyro.attitude);
    }
    private Quaternion GyroToUnity (Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
