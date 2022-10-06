using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ParticleSystemGen : MonoBehaviour
{
    public struct star
    {
        public float mag;
        public float x;
        public float y;
        public float z;
    }
    public static int maxStars = 119613;
    public star[] stardb = new star[maxStars];
    private ParticleSystem.Particle[] points;
    private ParticleSystem particleSystem;
    private string line;
    private void setdb()
    {
        TextAsset theList = Resources.Load<TextAsset>("dbnew");
        string[] linesFromfile = theList.text.Split("\n");
        for(int i=0; i<linesFromfile.Length-1; i++)
        {
            string[] values = linesFromfile[i].Split(',');
            stardb[i].mag = float.Parse(values[0]);
            stardb[i].x = float.Parse(values[1]);
            stardb[i].z = float.Parse(values[2]);
            stardb[i].y = float.Parse(values[3]);
        }
    }
    private void Create()
    {

        points = new ParticleSystem.Particle[maxStars];

        for (int i = 0; i < maxStars; i++)
        {
            points[i].position = new Vector3(stardb[i].x, stardb[i].y, stardb[i].z);
            points[i].startSize = 0.2f;
            points[i].startColor = Color.white * (1.0f - ((stardb[i].mag + 1.44f) / 50));
        }

        particleSystem = gameObject.GetComponent<ParticleSystem>();

        particleSystem.SetParticles(points, points.Length);
        //particleSystem.SetParticles(points, 1);
    }

    void Start()
    {
        setdb();
        Create();
    }









    // private ParticleSystem particleSystem;
    // private ParticleSystem.Particle[] points;
    // public int maxParticles = 100;
    // public TextAsset starCSV;
    // void Awake()
    // {
    //     particleSystem = GetComponent<ParticleSystem>();
    //     particleSystem.maxParticles = maxParticles;
    //     ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[1];
    //     bursts[0].minCount = (short)maxParticles;
    //     bursts[0].maxCount = (short)maxParticles;
    //     bursts[0].time = 0.0f;
    //     particleSystem.emission.SetBursts(bursts, 1);
    // }
    // void Start()
    // {
    //     string[] lines = starCSV.text.Split('\n');
    //     ParticleSystem.Particle[] particleStars = new ParticleSystem.Particle[maxParticles];
    //     particleSystem.GetParticles(particleStars);
    //     for (int i = 0; i < maxParticles; i++)
    //     {
    //         string[] components = lines[i].Split(',');
    //         particleStars[i].position = new Vector3(float.Parse(components[1]),
    //                                                 float.Parse(components[3]),
    //                                                 float.Parse(components[2]));
    //         //particleStars[i].remainingLifetime = Mathf.Infinity;
    //         //particleStars[i].Color.white * (1.0f - ((float.Parse(components[0]) + 1.44f) / 8));
    //     }
    //     particleSystem.SetParticles(particleStars, maxParticles);
    // }
}