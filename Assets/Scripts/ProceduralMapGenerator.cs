using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
public class ProceduralMapGenerator : MonoBehaviour
{
    private Mesh m_mesh;
    private Vector3[] m_vertices;
    private int[] m_triangles;
    
    [SerializeField] private GameObject m_terrain;
    private int m_terrainPosX;
    private int m_terrainPosZ;

    // Size of the terrain
    [SerializeField] private int m_xSize = 150;
    [SerializeField] private int m_zSize = 150;
    

    private void Start()
    {
        m_mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = m_mesh;

        m_terrainPosX = (m_xSize) / 2;
        m_terrainPosZ = (m_zSize) / 2;

        CreateMap();
        UpdateMap();
        
        Instantiate(m_terrain, new Vector3(-m_terrainPosX,100f,-m_terrainPosZ), Quaternion.identity);
    }

   
    // Create square matrice, each sides have vertices equal to xSize + 1 
    // Apply a noise on y vertices to make different heights
    void CreateMap()
    {
        m_vertices = new Vector3[(m_xSize + 1) * (m_zSize + 1)];

        for (int i = 0, z = 0; z <= m_zSize; z++)
        {
            for (int x = 0; x <= m_xSize; x++)
            {
                // Generate a noise on the plan to give "wave effect" 
                float y = Mathf.PerlinNoise(x * Random.Range(-1f,1f), z * Random.Range(-1f,1f)) * .5f;
                m_vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        // Declare new int var to draw 2 triangles in each square vertices to fill them
        // Loop in theses vertices and then add 1 vert to offset on right and also add +6 on triangle array to draw new in next square of vertices
        m_triangles = new int[m_xSize * m_zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < m_zSize; z++)
        {
            for (int x = 0; x < m_xSize; x++)
            {
                m_triangles[tris + 0] = vert + 0;
                m_triangles[tris + 1] = vert + m_xSize + 1;
                m_triangles[tris + 2] = vert + 1;
                m_triangles[tris + 3] = vert + 1;
                m_triangles[tris + 4] = vert + m_xSize + 1;
                m_triangles[tris + 5] = vert + m_xSize + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }
    }


    // Define Mesh.vertices and mesh.triangles and recalcul normal from all shared vertices
    void UpdateMap()
    {
        // Clear all vertices and triangles data
        m_mesh.Clear();
        
        m_mesh.vertices = m_vertices;
        m_mesh.triangles = m_triangles;
        
        m_mesh.RecalculateNormals();
        
        // Create mesh collider
        m_mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = m_mesh;
    }

}
