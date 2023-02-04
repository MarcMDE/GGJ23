using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using System.Threading;
using System.Linq;
using UnityEngine.Serialization;
using UnityEngine.Events;

//Todo Shared Vertices
//Todo Borders
//Todo test smaller data?

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour {

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    public UnityAction OnMeshGenerated;

    /*private List<Vector3> vertices;
    private List<Vector2> uvs;
    private List<int> tris;*/

    private Vector4[] pointVector;
    private int[] dims = new int[3];
    private int[] voxelDims = new int[3];
    private int[] threadDims = new int[3];
    //private float[,] Matrix2D;

    private float[,,] data;

    private bool dataLoaded = false;
    
    private int numPoints;
    private int numVoxels;

    private int[] meshTriangles;
    private Vector3[] meshVertices;

    private bool meshListsUpdated = false;
    private bool meshUpdating = false;

    const int threadGroupSize = 8;
    
    [Header("References")]
    [SerializeField] private ComputeShader shader;
    [SerializeField] private Material mat;
    
    [Header("Mesh Parameters")]
    [SerializeField] private float thmin = 0.3f;
    [SerializeField] private bool interpolate = false;
    [SerializeField] private bool shadeSmooth;
    [SerializeField] private bool closeBorders;
    
    [Header("Clustering Parameters")]
    [SerializeField] private int clustersLeft = 1;

    private System.DateTime startTime;
    private System.DateTime maskingStarts;
    private System.DateTime maskingEnds;
    private System.DateTime smoothingStarts;
    private System.DateTime smoothingEnds;
    private System.DateTime computeStarts;
    private System.DateTime computeEnds;
    private System.DateTime reassignStarts;
    private System.DateTime reassignEnds;
    private System.DateTime clusteringStarts;
    private System.DateTime clusteringEnds;
    /*[Range (2, 100)]
    public int numPointsPerAxis = 30;*/


    // Buffers
    ComputeBuffer triangleBuffer;
    ComputeBuffer pointsBuffer;
    ComputeBuffer triCountBuffer;

    bool settingsUpdated = true;

    void Start()
    {
        var mesh = new Mesh {
			name = "Eco Mesh"
		};
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        meshRenderer = GetComponent<MeshRenderer>();
    
    }

    void Update () {
        if (dataLoaded && settingsUpdated && !meshUpdating) {

            RequestMeshUpdate ();
            meshUpdating = true;
            meshListsUpdated = false;
            settingsUpdated = false;
        }

        if (meshListsUpdated)
        {
            ApplyMeshLists();
            PrintTimes();
            meshListsUpdated = false;
            meshUpdating = false;
            
        }
    }

    public void LoadData(float[,,] indata){
        

        data = indata;

        for(int i = 0; i<3; i++){
            dims[i] = data.GetLength(i);
            voxelDims[i] = dims[i] - 1;
            threadDims[i] = Mathf.CeilToInt (voxelDims[i] / (float) threadGroupSize);
        }

        numPoints = dims[0] * dims[1] * dims[2];
        numVoxels = voxelDims[0] * voxelDims[1] * voxelDims[2];
        
        

        dataLoaded = true;
    }
    private void GenerateMockMatrix(){
        for(int i = 0; i<3; i++) dims[i] = 32;
        for(int z = 0; z<dims[2];z++){
            for(int y = 0; y<dims[1];y++){
                for(int x = 0; x<dims[0];x++){
                    
                }
            }
        }
    }
    private void SetMatrix()
    {
        pointVector = new Vector4[numPoints];
        
        for (int i = 0; i < numPoints; i++)
        {
            int cz = i / (dims[0]*dims[1]);
            int r = i % (dims[0]*dims[1]);
            int cy = r / dims[0];
            int cx = r % dims[0];

            pointVector[i] = new Vector4(cx, cy, cz, data[cx,cy,cz]);
            
        }
    }

    

    private void RequestMeshUpdate () {
        //Time
            startTime = System.DateTime.UtcNow;
      
            
        SetMatrix();
        
        CreateBuffers();

        //Time
            computeStarts = System.DateTime.UtcNow;
        
        Triangle[] triangles = UpdateMesh();

        //Time
            computeEnds = System.DateTime.UtcNow;
        
        
        Thread GenerateMeshThreadTHD = new Thread(() => GenerateMeshThread(triangles));
        GenerateMeshThreadTHD.Start();
    }
    private void GenerateMeshThread(Triangle[] triangles)
    {
        //Time
            reassignStarts = System.DateTime.UtcNow;
        
        CalculateGeometry(triangles);

        //Time
            reassignEnds = System.DateTime.UtcNow;
        
        
        //RemoveSmallTriangleClusters();

        
        meshListsUpdated = true;
    }

    private Triangle[] UpdateMesh ()
    {
        pointsBuffer.SetData(pointVector);

        triangleBuffer.SetCounterValue (0);
        shader.SetBuffer (0, "points", pointsBuffer);
        shader.SetBuffer (0, "triangles", triangleBuffer);
        shader.SetInts ("dims", dims);
        shader.SetFloat ("isoLevel", thmin);
        shader.SetBool("interpolate", interpolate);

        shader.Dispatch (0, threadDims[0], threadDims[1], threadDims[2]);
        
        // Get number of triangles in the triangle buffer
        ComputeBuffer.CopyCount (triangleBuffer, triCountBuffer, 0);
        int[] triCountArray = { 0 };
        triCountBuffer.GetData (triCountArray);
        int numTris = triCountArray[0];

        // Get triangle data from shader
        Triangle[] tris = new Triangle[numTris];
        triangleBuffer.GetData (tris, 0, 0, numTris);

        return tris;
        
    }
    
    private void CalculateGeometry(Triangle[] tris)
    {
        int numTris = tris.Length;
        
        
        meshTriangles = new int[numTris * 3];
        
        if (shadeSmooth)
        {
            List<Vector3> vertices = new List<Vector3>();

            for (int i = 0; i < numTris; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Vector3 current = tris[i][j];
                    int index = vertices.LastIndexOf(current);
 
                    if (index >= 0 )
                    {
                        meshTriangles[i * 3 + j] = index;
                    }
                    else
                    {
                        vertices.Add(current);
                        meshTriangles[i * 3 + j] = vertices.Count - 1;
                    }
                }
            }
            meshVertices = vertices.ToArray();
        }
        else
        {
            meshVertices = new Vector3[numTris * 3];
            for (int i = 0; i < numTris; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    meshTriangles[i * 3 + j] = i * 3 + j;
                    meshVertices[i * 3 + j] = tris[i][j];
                }
            }

        }

        
    }

    private void RemoveSmallTriangleClusters()
    {
        if (clustersLeft > 0)
        {
            int[] labels = GetClusterLabels();

            Dictionary<int, int> sortedLabelDict = GetStortedDictionary(labels);
            
            
            if(clustersLeft < sortedLabelDict.Count){

                List<Triangle> triList = new List<Triangle>();
                var labelsToKeep = sortedLabelDict.Take(clustersLeft).ToDictionary(pair => pair.Key, pair => pair.Value);
                 
                for (int i = 0; i < meshTriangles.Length; i+=3)
                {
                    if( labelsToKeep.ContainsKey(labels[meshTriangles[i]])){
                        Triangle tri = new Triangle();
                        tri.a = meshVertices[meshTriangles[i]];
                        tri.b = meshVertices[meshTriangles[i+1]];
                        tri.c = meshVertices[meshTriangles[i+2]];
                        triList.Add(tri);
                    }
                }
                CalculateGeometry(triList.ToArray());
            }
            
        }
        
    }

    private int[] GetClusterLabels()
    {
        int[] labels = new int[meshVertices.Length];
        bool done = false;
            
        //init cluster labels
        for (int i = 0; i < labels.Length; i++)
        {
            labels[i] = i;
        }

        while (!done)
        {
            done = true;
            for (int i = 0; i < meshTriangles.Length; i+=3)
            {
                    
                int label1 = labels[meshTriangles[i]];
                int label2 = labels[meshTriangles[i+1]];
                int label3 = labels[meshTriangles[i+2]];
    
                    
                if ((label1 != label2) || (label1 != label3))
                {
                    int triLabel = Mathf.Min(label1, Mathf.Min(label2, label3));

                    for (int j = 0; j < 3; j++)
                    {
                        labels[meshTriangles[i+j]] = triLabel;
                    }

                    done = false;
                }
            }
        }

        return labels;
    }

    private Dictionary<int, int> GetStortedDictionary(int[] labels)
    {
        Dictionary<int, int> labelDict = new Dictionary<int, int>();

        for (int i = 0; i < labels.Length; i++)
        {
            if (labelDict.ContainsKey(labels[i]))
            {
                labelDict[labels[i]] += 1;
            }
            else
            {
                labelDict.Add(labels[i], 1);
            }
        }
        return (from entry in labelDict orderby entry.Value descending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    private void PrintClusterList(Dictionary<int, int> sortedLabelDict)
    {
        foreach(var item in sortedLabelDict)
        {
            Debug.Log("Cluster " + item.Key +": "+item.Value);
        }
    }

    private void ApplyMeshLists()
    {
        Mesh mesh = meshFilter.mesh;
        mesh.Clear ();

        mesh.vertices = meshVertices;
        mesh.triangles = meshTriangles;
        mesh.Optimize ();
        mesh.RecalculateNormals ();
        
        meshRenderer.material = mat;
        transform.localPosition = Vector3.zero;

        /*
        BoxCollider bc = gameObject.GetComponent<BoxCollider>();

        if (bc is not null)
        {
            Destroy(bc);
        }
        bc = gameObject.AddComponent<BoxCollider>();
        bc.isTrigger = true;
        */

        OnMeshGenerated.Invoke();

        //transform.localScale = new Vector3( 1f/dims[2] , 1f/dims[1] , 1f/dims[0] );
        //transform.localPosition = - transform.parent.InverseTransformPoint(transform.TransformPoint(bc.center));
        
    }
    private void PrintTimes(){
        System.TimeSpan ts0 = System.DateTime.UtcNow - startTime;
        System.TimeSpan ts5 = maskingEnds - maskingStarts;
        System.TimeSpan ts1 = smoothingEnds - smoothingStarts;
        System.TimeSpan ts2 = computeEnds - computeStarts;
        System.TimeSpan ts3 = reassignEnds - reassignStarts;
        System.TimeSpan ts4 = clusteringEnds - clusteringStarts;
        
        Debug.Log ("Elapsed Time: "+ts0.ToString ());
        Debug.Log ("Masking Time: "+ts5.ToString ());
        Debug.Log ("Smoothing Time: "+ts1.ToString ());
        Debug.Log ("Compute Time: "+ts2.ToString ());
        Debug.Log ("Reassing Time: "+ts3.ToString ());
        Debug.Log ("Clustering Time: "+ts4.ToString ());
    }
    void OnDestroy () {
        if (Application.isPlaying) {
            ReleaseBuffers ();
        }
    }

    void CreateBuffers ()
    {
        int maxTriangleCount = numVoxels * 5;

        // Always create buffers in editor (since buffers are released immediately to prevent memory leak)
        // Otherwise, only create if null or if size has changed
        if (pointsBuffer == null || numPoints != pointsBuffer.count) {
            ReleaseBuffers ();
            
            triangleBuffer = new ComputeBuffer (maxTriangleCount, sizeof (float) * 3 * 3, ComputeBufferType.Append);
            pointsBuffer = new ComputeBuffer (numPoints, sizeof (float) * 4);
            triCountBuffer = new ComputeBuffer (1, sizeof (int), ComputeBufferType.Raw);

        }
    }

    void ReleaseBuffers () {
        if (triangleBuffer != null) {
            triangleBuffer.Release ();
            pointsBuffer.Release ();
            triCountBuffer.Release ();
        }
    }

    void OnValidate() {
        settingsUpdated = true;
    }

    struct Triangle {
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public Vector3 this [int i] {
            get {
                switch (i) {
                    case 0:
                        return a;
                    case 1:
                        return b;
                    default:
                        return c;
                }
            }
        }
    }

}