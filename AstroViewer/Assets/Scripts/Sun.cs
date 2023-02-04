using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Entropedia
{
    public class Sun : MonoBehaviour
    {
        [SerializeField]
        float longitude;

        [SerializeField]
        float latitude;

        [SerializeField]
        [Range(0, 24)]
        public int hour;

        [SerializeField]
        [Range(0, 60)]
        public int minutes;

        DateTime time;

        [SerializeField]
        float timeSpeed = 1;

        [SerializeField]
        int frameSteps = 1;
        int frameStep;

        [SerializeField]
        DateTime date;

        public struct star
        {
            public string name;
            public float ra;
            public float dec;
            public float mag;
        }
        public static int maxStars = 9998;
        public star[] stardb = new star[maxStars];
        private ParticleSystem.Particle[] points = new ParticleSystem.Particle[maxStars];
        public GameObject cube,cubeController;
        public ParticleSystem PS;
        public GameObject cam;
        private void setdb()
        {
            TextAsset theList = Resources.Load<TextAsset>("dbnew");
            string[] linesFromfile = theList.text.Split("\n");
            for(int i=0; i<maxStars; i++)
            {
                string[] values = linesFromfile[i].Split(',');
                stardb[i].name = values[0];
                stardb[i].ra = float.Parse(values[1]) * 15 * Mathf.Deg2Rad;
                stardb[i].dec = float.Parse(values[2]) * Mathf.Deg2Rad;
                stardb[i].mag = float.Parse(values[3]);
            }
        }


        private IEnumerator plotStar()
        {
            for(int i=0; i<maxStars; i++)
            {
                var angle=SetPosition(stardb[i].ra,stardb[i].dec);
                points[i].position = cube.transform.position;
                points[i].startSize=0.03f;
                points[i].startColor = Color.white * (1.0f - Mathf.Pow(((stardb[i].mag + 0.1f) / 7),3));
                if(stardb[i].name != "")
                {
                    cubeController.transform.rotation=Quaternion.Euler(angle);
                    GameObject collider = Instantiate(Resources.Load("StarCollider"),cube.transform.position,Quaternion.identity) as GameObject;
                    collider.name = "Collider "+stardb[i].name;
                    angle.x-=1;
                    cubeController.transform.rotation=Quaternion.Euler(angle);
                    GameObject nameText = Instantiate(Resources.Load("StarText"),cube.transform.position,Quaternion.LookRotation( cube.transform.position - cam.transform.position )) as GameObject;
                    nameText.GetComponent<TextMeshPro>().text=stardb[i].name;
                    nameText.name = stardb[i].name;
                }
            }
            PS.SetParticles(points, points.Length);
            yield return 0; 
        }   

        public void SetTime(int hour, int minutes) {
            this.hour = hour;
            this.minutes = minutes;            
            OnValidate();
        }

        public void SetLocation(float longitude, float latitude){
          this.longitude = longitude;
          this.latitude = latitude;
        }

        public void SetDate(DateTime dateTime){
         this.hour = dateTime.Hour;
         this.minutes = dateTime.Minute;
         this.date = dateTime.Date;
         OnValidate();
        }

        public void SetUpdateSteps(int i) {
            frameSteps = i;
        }

        public void SetTimeSpeed(float speed) {
            timeSpeed = speed;
        }

        private void Start()
        {
            setdb();
            time = DateTime.Now;
            hour = time.Hour;
            minutes = time.Minute;
            date = time.Date;
            StartCoroutine("plotStar");
        }

        private void OnValidate()
        {
            time = date + new TimeSpan(hour, minutes, 0);

            Debug.Log(time);
        }

        // private void Update()
        // {
        //     time = time.AddSeconds(timeSpeed * Time.deltaTime);
        //     if (frameStep==0) {
        //         SetPosition();
        //     }
        //     frameStep = (frameStep + 1) % frameSteps;
        // }

        Vector3 SetPosition(double rightAscension, double declination)
        {
            Vector3 angles = new Vector3();
            double alt;
            double azi;
            SunPosition.CalculateSunPosition(time, (double)latitude, (double)longitude, rightAscension, declination, out azi, out alt);
            angles.x = (float)alt * Mathf.Rad2Deg;
            angles.y = (float)azi * Mathf.Rad2Deg;
            cubeController.transform.rotation = Quaternion.Euler(angles);
            return angles;
        }

        
    }

    /*
     * The following source came from this blog:
     * http://guideving.blogspot.co.uk/2010/08/sun-position-in-c.html
     */
    public static class SunPosition
    {
        public static void CalculateSunPosition(
            DateTime dateTime, double latitude, double longitude, double rightAscension, double declination ,out double outAzimuth, out double outAltitude)
        {
            // Convert to UTC  
            dateTime = dateTime.ToUniversalTime();            

            // Number of days from J2000.0.  
            double julianDate = 367 * dateTime.Year -
                (int)((7.0 / 4.0) * (dateTime.Year +
                (int)((dateTime.Month + 9.0) / 12.0))) +
                (int)((275.0 * dateTime.Month) / 9.0) +
                dateTime.Day - 730531.5;

            double julianCenturies = julianDate / 36525.0;

        //     // Sidereal Time  
            double siderealTimeHours = 6.6974 + 2400.0513 * julianCenturies;

            double siderealTimeUT = siderealTimeHours +
                (366.2422 / 365.2422) * (double)dateTime.TimeOfDay.TotalHours;

            double siderealTime = siderealTimeUT * 15 + longitude;

            // Refine to number of days (fractional) to specific time.  
            julianDate += (double)dateTime.TimeOfDay.TotalHours / 24.0;
            julianCenturies = julianDate / 36525.0;
            // Horizontal Coordinates  
            double hourAngle = CorrectAngle(siderealTime * Mathf.Deg2Rad) - rightAscension;

            if (hourAngle > Math.PI)
            {
                hourAngle -= 2 * Math.PI;
            }

            double altitude = Math.Asin(Math.Sin(latitude * Mathf.Deg2Rad) *
                Math.Sin(declination) + Math.Cos(latitude * Mathf.Deg2Rad) *
                Math.Cos(declination) * Math.Cos(hourAngle));

            // Nominator and denominator for calculating Azimuth  
            // angle. Needed to test which quadrant the angle is in.  
            double aziNom = -Math.Sin(hourAngle);
            double aziDenom =
                Math.Tan(declination) * Math.Cos(latitude * Mathf.Deg2Rad) -
                Math.Sin(latitude * Mathf.Deg2Rad) * Math.Cos(hourAngle);

            double azimuth = Math.Atan(aziNom / aziDenom);

            if (aziDenom < 0) // In 2nd or 3rd quadrant  
            {
                azimuth += Math.PI;
            }
            else if (aziNom < 0) // In 4th quadrant  
            {
                azimuth += 2 * Math.PI;
            }

            outAltitude = altitude;
            outAzimuth = azimuth;
        }

        /*! 
        * \brief Corrects an angle. 
        * 
        * \param angleInRadians An angle expressed in radians. 
        * \return An angle in the range 0 to 2*PI. 
        */
        private static double CorrectAngle(double angleInRadians)
        {
            if (angleInRadians < 0)
            {
                return 2 * Math.PI - (Math.Abs(angleInRadians) % (2 * Math.PI));
            }
            else if (angleInRadians > 2 * Math.PI)
            {
                return angleInRadians % (2 * Math.PI);
            }
            else
            {
                return angleInRadians;
            }
        }
    }

}