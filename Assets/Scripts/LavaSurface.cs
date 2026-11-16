
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LavaSurface : MonoBehaviour
{
    [SerializeField] private float width = 12f;
    [SerializeField] private float depth = 4f;
    [SerializeField] private float waveHeight = 0.15f;
    [SerializeField] private float waveSpeed = 2f;
    [SerializeField] private float waveFrequency = 2f;
    [SerializeField] private int segments = 40;

    private Mesh mesh;
    private Vector3[] vertices;

    private void Awake()
    {
        segments = Mathf.Max(2, segments);

        mesh = new Mesh();
        mesh.name = "Lava Waves";

        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[(segments + 1) * 2];
        int[] triangles = new int[segments * 6];

        for (int i = 0; i <= segments; i++)
        {
            float x = -width / 2f + width * i / segments;

            vertices[i * 2] = new Vector3(x, -depth, 0f);
            vertices[i * 2 + 1] = new Vector3(x, 0f, 0f);
        }

        for (int i = 0; i < segments; i++)
        {
            int a = i * 2;
            int b = a + 1;
            int c = a + 2;
            int d = a + 3;
            int t = i * 6;

            triangles[t] = a;
            triangles[t + 1] = c;
            triangles[t + 2] = b;

            triangles[t + 3] = c;
            triangles[t + 4] = d;
            triangles[t + 5] = b;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    private void Update()
    {
        float time = Time.time * waveSpeed;

        for (int i = 0; i <= segments; i++)
        {
            float x = -width / 2f + width * i / segments;

            float wave = Mathf.Sin(
                x * waveFrequency + time
            ) * waveHeight;

            vertices[i * 2] = new Vector3(
                x, -depth, 0f
            );

            vertices[i * 2 + 1] = new Vector3(
                x, wave, 0f
            );
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    private void OnDestroy()
    {
        if (mesh != null)
        {
            Destroy(mesh);
        }
    }
}
