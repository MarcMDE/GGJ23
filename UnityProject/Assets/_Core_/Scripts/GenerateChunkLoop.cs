using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GGJ23{
    public class GenerateChunkLoop : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] Transform rootParent;
        [SerializeField] GameObject prefab;

        [SerializeField] bool one = false;
        const float chunkSize = 64f;
        RootGenerator rootGenerator;
        GameObject[,,] meshes = new GameObject[3,3,3];
        GameObject last;

         int[] order = {1};//{1,0,2};

        //private Dictionary<CUBE_ORIENTATION,List<Vector3>> _startingPoints = new Dictionary<CUBE_ORIENTATION,List<Vector3>>();

        void Start()
        {
            rootGenerator = GetComponent<RootGenerator>();
            ResetChunks( new Dictionary<CUBE_ORIENTATION,List<Vector3>>());
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        async void ResetChunks( Dictionary<CUBE_ORIENTATION,List<Vector3>> startingPoints ){
            
            bool first = startingPoints.Count <= 0;
            for (int z = 0; z<3; z++){
                for (int y = 0; y<3; y++){
                    for (int x = 0; x<3; x++){
                        GameObject obj = Instantiate(prefab,rootParent);
                        obj.transform.position = obj.transform.parent.position + new Vector3(x*(chunkSize-1),y*(chunkSize-1),z*(chunkSize-1));
                        meshes[x,y,z] = obj;
                    }
                }
            }
            if(one){
                rootGenerator.SetChunk(startingPoints, meshes[1,1,1].GetComponent<MeshGenerator>());
                return;
            }
        
            foreach (var i in order)
            {
                
                //Centers
                if(!first && i == 1){
                    meshes[1,1,1] = last;
                }else{
                    rootGenerator.SetChunk(startingPoints, meshes[1,i,1].GetComponent<MeshGenerator>());
                }

                //X
                Dictionary<CUBE_ORIENTATION,List<Vector3>> xdictL = new Dictionary<CUBE_ORIENTATION, List<Vector3>>();
                Dictionary<CUBE_ORIENTATION,List<Vector3>> xdictR = new Dictionary<CUBE_ORIENTATION, List<Vector3>>();
                
                List<Vector3> vl = startingPoints[CUBE_ORIENTATION.XN];

                for(int j = 0; j < vl.Count; j++ )
                {
                    vl[j] = new Vector3(chunkSize - 1 - vl[j].x, vl[j].y, vl[j].z);
                }

                xdictL.Add(CUBE_ORIENTATION.XP, vl );

                Debug.Log("Startpoint: "+startingPoints[CUBE_ORIENTATION.XN][0]);
                Debug.Log("xdict: "+xdictL[CUBE_ORIENTATION.XP][0]);
                rootGenerator.SetChunk(xdictL, meshes[0,i,1].GetComponent<MeshGenerator>());
                Debug.Log("xdict: "+xdictL[CUBE_ORIENTATION.XP][0]);
                //xdictR.Add(CUBE_ORIENTATION.XN, startingPoints[CUBE_ORIENTATION.XP]);
                //xdictR.Add(CUBE_ORIENTATION.XP, xdictL[CUBE_ORIENTATION.XN]);

                //rootGenerator.SetChunk(xdictR, meshes[2,i,1].GetComponent<MeshGenerator>());
                
                

                /*//Cross
                Vector3[] newPoints0 = new Vector3[,];
                Vector3[] newPoints1 = new Vector3[,];
                Vector3[] newPoints2 = new Vector3[,];
                Vector3[] newPoints3 = new Vector3[,];



                _startingPoints = rootGenerator.SetChunk(new Vector3[0,0], meshes[4+i*3]);
                _startingPoints = rootGenerator.SetChunk(new Vector3[0,0], meshes[9+i*3]);
                _startingPoints = rootGenerator.SetChunk(new Vector3[0,0], meshes[11+i*3]);
                _startingPoints = rootGenerator.SetChunk(new Vector3[0,0], meshes[19+i*3]);*/





                startingPoints = new Dictionary<CUBE_ORIENTATION,List<Vector3>>();
            }
            
                
                
        

        }
    }
}
    