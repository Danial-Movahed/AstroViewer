using UnityEngine;
using System.Collections;

public class DrawCircle: MonoBehaviour {
    public float ThetaScale = 0.01f;
    public float radius = 5f;
    private int Size;
    private LineRenderer LineDrawer;
    private float Theta = 0f;

    void Start() {
        LineDrawer = GetComponent<LineRenderer>();
    }

    void Update() {
        Theta = 0f;
        Size = (int)((1f / ThetaScale) + 1f);
        LineDrawer.SetVertexCount(Size);
        for (int i = 0; i < Size; i++) {
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float x = radius * Mathf.Cos(Theta);
            float y = radius * Mathf.Sin(Theta);
            LineDrawer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}