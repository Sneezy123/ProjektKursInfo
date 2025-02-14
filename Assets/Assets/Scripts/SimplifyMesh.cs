using UnityEngine;

public class SimplifyMesh : MonoBehaviour
{
    public float quality = 0.5f;

    void Start()
    {
        var originalMesh = GetComponent<MeshFilter>().sharedMesh;
        var meshSimpifier = new UnityMeshSimplifier.MeshSimplifier();

        meshSimpifier.Initialize(originalMesh);
        meshSimpifier.SimplifyMesh(quality);

        var destMesh = meshSimpifier.ToMesh();
        GetComponent<MeshFilter>().sharedMesh = destMesh;
    }
}
