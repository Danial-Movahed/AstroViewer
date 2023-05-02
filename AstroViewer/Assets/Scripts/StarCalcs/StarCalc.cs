using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;

public class StarCalc : MonoBehaviour
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
        public int hipID;
        public string color;
    }
    public static int maxStars = 10109;
    // public static int maxStars = 1;
    public star[] stardb = new star[118066];
    public string[] constellations = new string[86];
    private ParticleSystem.Particle[] points = new ParticleSystem.Particle[maxStars + 4];
    public GameObject cube, cubeController;
    public ParticleSystem PS;
    public GameObject cam;
    public GameObject circle;
    public GameObject StarNames, ConstLines, StarColliders;
    public void changeTimeDate(DateTime date)
    {
        time = date;
        StartCoroutine(plotStar());
    }
    private void setdb()
    {
        TextAsset theList = Resources.Load<TextAsset>("StarDatabase");
        string[] linesFromfile = theList.text.Split("\n");
        for (int i = 0; i < linesFromfile.Length - 1; i++)
        {
            string[] values = linesFromfile[i].Split(',');
            stardb[i].name = values[0];
            stardb[i].ra = float.Parse(values[1]) * 15 * Mathf.Deg2Rad;
            stardb[i].dec = float.Parse(values[2]) * Mathf.Deg2Rad;
            stardb[i].mag = float.Parse(values[3]);
            stardb[i].hipID = int.Parse(values[4]);
            stardb[i].color = values[5];
        }
        theList = Resources.Load<TextAsset>("Constellations");
        constellations = theList.text.Split("\n");
    }

    private star findRaDecByHipID(int id)
    {
        var star = stardb.First(s => s.hipID == id);
        return star;
    }

    public void plotConstellations()
    {
        for (int i = 0; i < 86; i++)
        {
            string[] tmp = constellations[i].Split(",");
            // First time manual calculation for group
            Vector3[] points = new Vector3[2];
            var star = findRaDecByHipID(int.Parse(tmp[2]));
            var angle = SetPosition(star.ra, star.dec);
            var startingPos = cube.transform.position;
            GameObject lineR = Instantiate(Resources.Load("ConstLine"), startingPos, Quaternion.identity) as GameObject;
            if (PlayerPrefs.GetString("constelAnimToggle","true") == "false")
                lineR.GetComponent<Animator>().GetComponent<Animator>().Play("LineAnimationOpen");
            points[0] = cube.transform.position - startingPos;
            star = findRaDecByHipID(int.Parse(tmp[3]));
            angle = SetPosition(star.ra, star.dec);
            points[1] = cube.transform.position - startingPos;
            lineR.GetComponent<LineRenderer>().SetPositions(points);
            GameObject constelGroup = Instantiate(Resources.Load("Empty"), points[1] + startingPos, Quaternion.LookRotation(cube.transform.position - cam.transform.position)) as GameObject;
            constelGroup.AddComponent<ConstellationAnimation>();
            constelGroup.name = tmp[0];
            constelGroup.tag = "Constellations";
            constelGroup.transform.SetParent(ConstLines.transform);
            lineR.transform.SetParent(constelGroup.transform);
            // ########################################
            for (int j = 4; j < tmp.Length; j += 2)
            {
                points = new Vector3[2];
                star = findRaDecByHipID(int.Parse(tmp[j]));
                angle = SetPosition(star.ra, star.dec);
                startingPos = cube.transform.position;
                lineR = Instantiate(Resources.Load("ConstLine"), startingPos, Quaternion.identity) as GameObject;
                if (PlayerPrefs.GetString("constelAnimToggle","true") == "false")
                    lineR.GetComponent<Animator>().GetComponent<Animator>().Play("LineAnimationOpen");
                lineR.transform.SetParent(constelGroup.transform);
                points[0] = cube.transform.position - startingPos;
                star = findRaDecByHipID(int.Parse(tmp[j + 1]));
                angle = SetPosition(star.ra, star.dec);
                points[1] = cube.transform.position - startingPos;
                lineR.GetComponent<LineRenderer>().SetPositions(points);
            }
            AddColliderAroundChildren(constelGroup);
            var ConstelName = Instantiate(Resources.Load("ConstelText"), constelGroup.GetComponent<BoxCollider>().center, Quaternion.identity) as GameObject;
            if (PlayerPrefs.GetString("constelAnimToggle","true") == "false")
                ConstelName.GetComponent<Animator>().GetComponent<Animator>().Play("ConstelTextShow");
            ConstelName.GetComponent<TextMeshPro>().text = tmp[0];
            ConstelName.transform.parent = constelGroup.transform;
            ConstelName.transform.localPosition = ConstelName.transform.position;
            ConstelName.transform.localRotation = Quaternion.LookRotation(cam.transform.position);
        }
    }

    private void AddColliderAroundChildren(GameObject assetModel)
    {
        var pos = assetModel.transform.localPosition;
        var rot = assetModel.transform.localRotation;
        var scale = assetModel.transform.localScale;

        // need to clear out transforms while encapsulating bounds
        assetModel.transform.localPosition = Vector3.zero;
        assetModel.transform.localRotation = Quaternion.identity;
        assetModel.transform.localScale = Vector3.one;

        // start with root object's bounds
        var bounds = new Bounds(Vector3.zero, Vector3.zero);
        if (assetModel.transform.TryGetComponent<LineRenderer>(out var mainRenderer))
        {
            // as mentioned here https://forum.unity.com/threads/what-are-bounds.480975/
            // new Bounds() will include 0,0,0 which you may not want to Encapsulate
            // because the vertices of the mesh may be way off the model's origin
            // so instead start with the first renderer bounds and Encapsulate from there
            bounds = mainRenderer.bounds;
        }

        var descendants = assetModel.GetComponentsInChildren<Transform>();
        foreach (Transform desc in descendants)
        {
            if (desc.TryGetComponent<LineRenderer>(out var childRenderer))
            {
                // use this trick to see if initialized to renderer bounds yet
                // https://answers.unity.com/questions/724635/how-does-boundsencapsulate-work.html
                if (bounds.extents == Vector3.zero)
                    bounds = childRenderer.bounds;
                bounds.Encapsulate(childRenderer.bounds);
            }
        }

        var boxCol = assetModel.AddComponent<BoxCollider>();
        boxCol.isTrigger = true;
        boxCol.center = bounds.center - assetModel.transform.position;
        boxCol.size = bounds.size + Vector3.one;

        // restore transforms
        assetModel.transform.localPosition = pos;
        assetModel.transform.localRotation = rot;
        assetModel.transform.localScale = scale;
    }

    private IEnumerator plotStar()
    {
        GameObject nameText;
        for (int i = 0; i < maxStars; i++)
        {
            var angle = SetPosition(stardb[i].ra, stardb[i].dec);
            points[i].position = cube.transform.position;
            points[i].startSize = 0.03f;
            switch (stardb[i].color)
            {
                case "red":
                    points[i].startColor = Color.red * (1.0f - Mathf.Pow(((stardb[i].mag + 0.1f) / 7), 3));
                    break;
                case "orange":
                    points[i].startColor = new Color(255, 147, 0, 255) * (1.0f - Mathf.Pow(((stardb[i].mag + 0.1f) / 7), 3));
                    break;
                case "blue":
                    points[i].startColor = Color.blue * (1.0f - Mathf.Pow(((stardb[i].mag + 0.1f) / 7), 3));
                    break;
                case "yellow":
                    points[i].startColor = Color.yellow * (1.0f - Mathf.Pow(((stardb[i].mag + 0.1f) / 7), 3));
                    break;
                default:
                    points[i].startColor = Color.white * (1.0f - Mathf.Pow(((stardb[i].mag + 0.1f) / 7), 3));
                    break;
            }
            if (stardb[i].name != "")
            {
                if(i < 9998)
                {
                    GameObject collider = Instantiate(Resources.Load("StarCollider"), cube.transform.position, Quaternion.identity) as GameObject;
                    collider.transform.SetParent(StarColliders.transform);
                    collider.name = stardb[i].name;
                }
                angle.x -= 1;
                cubeController.transform.rotation = Quaternion.Euler(angle);
                nameText = Instantiate(Resources.Load("StarText"), cube.transform.position, Quaternion.LookRotation(cube.transform.position - cam.transform.position)) as GameObject;
                nameText.transform.SetParent(StarNames.transform);
                nameText.GetComponent<TextMeshPro>().text = stardb[i].name;
                nameText.name = "Name " + stardb[i].name;
            }
        }
        // North
        cubeController.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
        points[maxStars].position = cube.transform.position;
        points[maxStars].startColor = Color.red;
        points[maxStars].startSize = 0.1f;

        cubeController.transform.rotation = Quaternion.Euler(new Vector3(-2, -90, 0));
        nameText = Instantiate(Resources.Load("StarText"), cube.transform.position, Quaternion.LookRotation(cube.transform.position - cam.transform.position)) as GameObject;
        nameText.transform.SetParent(StarNames.transform);
        nameText.GetComponent<TextMeshPro>().text = "N";
        nameText.GetComponent<TextMeshPro>().fontSize += 2;
        nameText.name = "Name North";
        // South
        cubeController.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        points[maxStars + 1].position = cube.transform.position;
        points[maxStars + 1].startColor = Color.blue;
        points[maxStars + 1].startSize = 0.1f;

        cubeController.transform.rotation = Quaternion.Euler(new Vector3(-2, 90, 0));
        nameText = Instantiate(Resources.Load("StarText"), cube.transform.position, Quaternion.LookRotation(cube.transform.position - cam.transform.position)) as GameObject;
        nameText.transform.SetParent(StarNames.transform);
        nameText.GetComponent<TextMeshPro>().text = "S";
        nameText.GetComponent<TextMeshPro>().fontSize += 2;
        nameText.name = "Name South";
        // East
        cubeController.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        points[maxStars + 2].position = cube.transform.position;
        points[maxStars + 2].startColor = Color.yellow;
        points[maxStars + 2].startSize = 0.1f;

        cubeController.transform.rotation = Quaternion.Euler(new Vector3(-2, 0, 0));
        nameText = Instantiate(Resources.Load("StarText"), cube.transform.position, Quaternion.LookRotation(cube.transform.position - cam.transform.position)) as GameObject;
        nameText.transform.SetParent(StarNames.transform);
        nameText.GetComponent<TextMeshPro>().text = "E";
        nameText.GetComponent<TextMeshPro>().fontSize += 2;
        nameText.name = "Name East";
        // West
        cubeController.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        points[maxStars + 3].position = cube.transform.position;
        points[maxStars + 3].startColor = Color.green;
        points[maxStars + 3].startSize = 0.1f;

        cubeController.transform.rotation = Quaternion.Euler(new Vector3(-2, 180, 0));
        nameText = Instantiate(Resources.Load("StarText"), cube.transform.position, Quaternion.LookRotation(cube.transform.position - cam.transform.position)) as GameObject;
        nameText.transform.SetParent(StarNames.transform);
        nameText.GetComponent<TextMeshPro>().text = "W";
        nameText.GetComponent<TextMeshPro>().fontSize += 2;
        nameText.name = "Name West";
        // #############################
        PS.SetParticles(points, points.Length);
        if (PlayerPrefs.GetString("eclipticToggle", "true") == "true")
        {
            var Cangle = SetPosition(270 * Mathf.Deg2Rad, 66.5 * Mathf.Deg2Rad);
            circle.SetActive(true);
            circle.transform.rotation = Quaternion.Euler(Cangle);
        }
        if (PlayerPrefs.GetString("constToggle", "true") == "true")
        {
            plotConstellations();
        }
        yield return 0;
    }

    public void SetTime(int hour, int minutes)
    {
        this.hour = hour;
        this.minutes = minutes;
        OnValidate();
    }

    public void SetLocation(float longitude, float latitude)
    {
        this.longitude = longitude;
        this.latitude = latitude;
    }

    public void SetDate(DateTime dateTime)
    {
        this.hour = dateTime.Hour;
        this.minutes = dateTime.Minute;
        this.date = dateTime.Date;
        OnValidate();
    }

    public void SetUpdateSteps(int i)
    {
        frameSteps = i;
    }

    public void SetTimeSpeed(float speed)
    {
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

public static class SunPosition
{
    public static void CalculateSunPosition(
        DateTime dateTime, double latitude, double longitude, double rightAscension, double declination, out double outAzimuth, out double outAltitude)
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