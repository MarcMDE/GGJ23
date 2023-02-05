using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CARDINALS { FORWARD, BACKWARD, LEFT, RIGHT, UP, DOWN};
namespace GGJ23{
    public class GenerateChunkLoop : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] Transform rootParent;
        [SerializeField] GameObject prefab;

        [SerializeField] bool one = false;
        const float chunkSize = 64f;
        RootGenerator rootGenerator;
        GameObject[,,] meshes = new GameObject[2,2,2];
        GameObject last;

        int[] meshIndex = new int[] {0,0,0};
        //int[] meshIndex = new int[] {0,0,0};

         int[] order = {0};//{1,0,2};

        //private Dictionary<CUBE_ORIENTATION,List<Vector3>> _startingPoints = new Dictionary<CUBE_ORIENTATION,List<Vector3>>();

        void Start()
        {
            rootGenerator = GetComponent<RootGenerator>();
            ResetChunks( new Dictionary<CUBE_ORIENTATION,List<Vector3>>());
        }

        // Update is called once per frame
        void Update()
        {   
            if(Input.GetKeyDown(KeyCode.UpArrow)) MoveChunks(CARDINALS.FORWARD);
        }
        private List<Vector3> FlipListCoordinate(List<Vector3> vl, bool dimX){
            for(int j = 0; j < vl.Count; j++ )
            {
                vl[j] = dimX ? new Vector3(chunkSize - 1 - vl[j].x, vl[j].y, vl[j].z) : new Vector3(vl[j].x, vl[j].y, chunkSize - 1 - vl[j].z);
            }
            return vl;
        }
        void ResetChunks( Dictionary<CUBE_ORIENTATION,List<Vector3>> startingPoints ){
            
            bool first = startingPoints.Count <= 0;
            for (int z = 0; z<2; z++){
                for (int y = 0; y<2; y++){
                    for (int x = 0; x<2; x++){
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
                    meshes[0,0,0] = last;
                }else{
                    rootGenerator.SetChunk(startingPoints, meshes[0,i,0].GetComponent<MeshGenerator>());
                }

                //X
                Dictionary<CUBE_ORIENTATION,List<Vector3>> xdictR = new Dictionary<CUBE_ORIENTATION, List<Vector3>>();

                xdictR.Add(CUBE_ORIENTATION.XN, FlipListCoordinate(startingPoints[CUBE_ORIENTATION.XP],true));
                xdictR.Add(CUBE_ORIENTATION.XP, FlipListCoordinate(startingPoints[CUBE_ORIENTATION.XN],true));
                rootGenerator.SetChunk(xdictR, meshes[1,i,0].GetComponent<MeshGenerator>());
                
                //Z
                Dictionary<CUBE_ORIENTATION,List<Vector3>> ydictU = new Dictionary<CUBE_ORIENTATION, List<Vector3>>();
            
                ydictU.Add(CUBE_ORIENTATION.ZP, FlipListCoordinate(startingPoints[CUBE_ORIENTATION.ZN],false));
                ydictU.Add(CUBE_ORIENTATION.ZN, FlipListCoordinate(startingPoints[CUBE_ORIENTATION.ZP],false));
                rootGenerator.SetChunk(ydictU, meshes[0,i,1].GetComponent<MeshGenerator>());


                Dictionary<CUBE_ORIENTATION,List<Vector3>> dictRU = new Dictionary<CUBE_ORIENTATION, List<Vector3>>();
                dictRU.Add(CUBE_ORIENTATION.XN, FlipListCoordinate(ydictU[CUBE_ORIENTATION.XP],true));
                dictRU.Add(CUBE_ORIENTATION.XP, FlipListCoordinate(ydictU[CUBE_ORIENTATION.XN],true));
                dictRU.Add(CUBE_ORIENTATION.ZP, FlipListCoordinate(xdictR[CUBE_ORIENTATION.ZN],false));
                dictRU.Add(CUBE_ORIENTATION.ZN, FlipListCoordinate(xdictR[CUBE_ORIENTATION.ZP],false));
                
                rootGenerator.SetChunk(dictRU, meshes[1,i,1].GetComponent<MeshGenerator>());
                
                startingPoints = new Dictionary<CUBE_ORIENTATION,List<Vector3>>();
            }
            
                
                
        

        }
        async void MoveChunks(CARDINALS c){
            switch (c)
            {
                case CARDINALS.FORWARD: 
                        for(int i = 0; i<2; i++){
                            Transform currentTF = meshes[i,meshIndex[1],0].transform;
                            currentTF.position = new Vector3(currentTF.position.x,currentTF.position.y,currentTF.position.z + 2*(chunkSize - 1));

                            GameObject prev = meshes[i,meshIndex[1],0];
                            meshes[i,meshIndex[1],0] = meshes[i,meshIndex[1],1];
                            meshes[i,meshIndex[1],1] = prev;
                        }
                    break;
                case CARDINALS.BACKWARD: 
                        /*for(int i = 0; i<2; i++){
                            Transform currentTF = meshes[i,meshIndex[1],0].transform;
                            currentTF.position = new Vector3(currentTF.position.x,currentTF.position.y,currentTF.position.z + 2*(chunkSize - 1));

                            GameObject prev = meshes[i,meshIndex[1],0];
                            meshes[i,meshIndex[1],0] = meshes[i,meshIndex[1],1];
                            meshes[i,meshIndex[1],1] = prev;
                        }*/
                    break;
                case CARDINALS.LEFT: 
                    break;
                case CARDINALS.RIGHT: 
                    break;
                case CARDINALS.UP:
                    break;
                case CARDINALS.DOWN: 
                    break;
                default:
                    break;
            }
        }
    }
}
    