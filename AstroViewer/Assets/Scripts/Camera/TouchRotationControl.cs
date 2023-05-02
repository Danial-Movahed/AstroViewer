using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotationControl : MonoBehaviour
{
    private Vector3 firstpoint;
    private Vector3 secondpoint;
    public bool isAREnabled;
    private float xAngle = 0.0f; //angle for axes x for rotation
    private float yAngle = 0.0f;
    private float xAngTemp = 0.0f; //temp variable for angle
    private float yAngTemp = 0.0f;
    public int speed = 4;
    private float touchDelta = 0.0F;
    private Vector2 prevDist = new Vector2(0,0);
    private Vector2 curDist = new Vector2(0,0);
    private int vertOrHorzOrientation = 0;
    private Vector2 midPoint = new Vector2(0,0);

    void Start()
    {
        firstpoint = new Vector3(0, 0, 0);
        secondpoint = new Vector3(0, 0, 0);

        xAngle = 0.0f;
        yAngle = 0.0f;

        transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
    }

    void Update()
    {
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(1).phase == TouchPhase.Began)
        {

        }

        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            midPoint = new Vector2(((Input.GetTouch(0).position.x + Input.GetTouch(1).position.x) / 2), ((Input.GetTouch(0).position.y - Input.GetTouch(1).position.y) / 2)); //store midpoint from first touches
            curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
            prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
            touchDelta = curDist.magnitude - prevDist.magnitude;

            if ((Input.GetTouch(0).position.x - Input.GetTouch(1).position.x) > (Input.GetTouch(0).position.y - Input.GetTouch(1).position.y))
            {
                vertOrHorzOrientation = -1;
            }
            if ((Input.GetTouch(0).position.x - Input.GetTouch(1).position.x) < (Input.GetTouch(0).position.y - Input.GetTouch(1).position.y))
            {
                vertOrHorzOrientation = 1;
            }

            if ((touchDelta < 0)) //
            {
                this.GetComponent<Camera>().fieldOfView = Mathf.Clamp(this.GetComponent<Camera>().fieldOfView + (1 * speed), 15, 90);
            }


            if ((touchDelta > 0))

            {
                this.GetComponent<Camera>().fieldOfView = Mathf.Clamp(this.GetComponent<Camera>().fieldOfView - (1 * speed), 15, 90);
            }

        }
        if (isAREnabled)
            return;
        if (Input.touchCount > 0)
        {
            //Touch began, save position
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstpoint = Input.GetTouch(0).position;
                xAngTemp = xAngle;
                yAngTemp = yAngle;
            }
            //Move finger by screen
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                secondpoint = Input.GetTouch(0).position;
                yAngle = yAngTemp - (secondpoint.y - firstpoint.y) * 90.0f / Screen.height;

                if (yAngle < 0)
                    yAngle += 360;
                if (yAngle > 360)
                    yAngle -= 360;

                if (yAngle > 90 && yAngle < 270)
                    xAngle = xAngTemp - (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
                else
                    xAngle = xAngTemp + (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;

                if (xAngle < 0)
                    xAngle += 360;

                if (xAngle > 360)
                    xAngle -= 360;

                transform.rotation = Quaternion.Euler(-yAngle, -xAngle, 0.0f);
            }
        }
    }
}