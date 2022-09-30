using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ParticleSystemGen : MonoBehaviour
{
    public struct star
    {
        public float x;
        public float y;
        public float z;
    }
    public static int maxStars = 119614;
    public star[] stardb = new star[maxStars];
    private ParticleSystem.Particle[] points;
    private ParticleSystem particleSystem;
    private string line;
    private void setdb()
    {
        TextAsset theList = Resources.Load<TextAsset>("hygdata_v3");
        string[] linesFromfile = theList.text.Split("\n");
        for( int i=0; i<linesFromfile.Length-1;i++)
        {
            string[] values = linesFromfile[i].Split(',');
            stardb[i].x = float.Parse(values[0]);
            stardb[i].y = float.Parse(values[1]);
            stardb[i].z = float.Parse(values[2]);
        }
    }
    private void Create()
    {

        points = new ParticleSystem.Particle[maxStars];

        for (int i = 0; i < maxStars; i++)
        {
            points[i].position = new Vector3(stardb[i].x, stardb[i].z, stardb[i].y);
            points[i].startSize = 0.2f;
            points[i].startColor = new Color(1, 1, 1, 1);
        }

        particleSystem = gameObject.GetComponent<ParticleSystem>();

        particleSystem.SetParticles(points, points.Length);
    }

    void Start()
    {
        setdb();
        Create();
    }

    void Update()
    {
        //You can access the particleSystem here if you wish
    }
}