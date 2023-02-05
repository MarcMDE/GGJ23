using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum CARDINALS { FORWARD, BACKWARD, LEFT, RIGHT, UP, DOWN};
namespace GGJ23{
    public class GenerateChunkLoop : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] Transform rootParent;
        [SerializeField] GameObject prefab;

        [SerializeField] bool one = false;
        const float chunkSize = 32f;
        RootGenerator rootGenerator;
        GameObject[,,] meshes = new GameObject[2,2,2];

        bool _meshesReady;
        int _numMeshes;
        int _targetMeshes;

        public UnityAction OnMeshesReady;

        Dictionary<CUBE_ORIENTATION,List<Vector3>>[,,] startPointsDictArray = new Dictionary<CUBE_ORIENTATION,List<Vector3>>[2,2,2];
    
        int[] currentDicIndex = new int[3];

        
        //int[] meshIndex = new int[] {0,0,0};

         int[] order = {0,1};//{1,0,2};

        //private Dictionary<CUBE_ORIENTATION,List<Vector3>> _startingPoints = new Dictionary<CUBE_ORIENTATION,List<Vector3>>();

        public GameObject[,,] Meshes { get { return meshes; } }


        void Awake()
        {
            _targetMeshes = meshes.GetLength(0) * meshes.GetLength(1) * meshes.GetLength(2);
            rootGenerator = GetComponent<RootGenerator>();
        }


        void MeshLoaded()
        {
            _numMeshes++;
            if (_numMeshes >= _targetMeshes)
            {
                _meshesReady = true;
                OnMeshesReady.Invoke();
            }
        }

        public void Generate()
        {
            _meshesReady = false;
            _numMeshes = 0;
            ResetChunks( new Dictionary<CUBE_ORIENTATION,List<Vector3>>());
        }

        // Update is called once per frame
        void Update()
        {   
            if(Input.GetKeyDown(KeyCode.UpArrow)) MoveChunks(CARDINALS.FORWARD);
            if(Input.GetKeyDown(KeyCode.DownArrow)) MoveChunks(CARDINALS.BACKWARD);
            if(Input.GetKeyDown(KeyCode.LeftArrow)) MoveChunks(CARDINALS.LEFT);
            if(Input.GetKeyDown(KeyCode.RightArrow)) MoveChunks(CARDINALS.RIGHT);
            if(Input.GetKeyDown(KeyCode.W)) MoveChunks(CARDINALS.UP);
            if(Input.GetKeyDown(KeyCode.S)) MoveChunks(CARDINALS.DOWN);
            if(Input.GetKeyDown(KeyCode.Space)) ResetChunks( startPointsDictArray[ currentDicIndex[0] , currentDicIndex[1] , currentDicIndex[2] ]);
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
                        if(first){
                            
                            GameObject obj = Instantiate(prefab,rootParent);
                            obj.transform.position = rootParent.position + new Vector3(x*(chunkSize-1),y*(chunkSize-1),z*(chunkSize-1));
                            meshes[x,y,z] = obj;
                        }else{
                            if(x != 0 || y != 0 || z != 0){
                                Destroy(meshes[x,y,z]);
                                GameObject obj = Instantiate(prefab,rootParent);
                                obj.transform.position = meshes[0,0,0].transform.position + new Vector3(x*(chunkSize-1),y*(chunkSize-1),z*(chunkSize-1));
                                meshes[x,y,z] = obj;
                            }
                        }
                        meshes[x, y, z].GetComponent<MeshGenerator>().OnMeshGenerated += MeshLoaded;
                    }
                }
            }
                
            foreach (var i in order)
            {
                
                //Centers
                if(first || i > 0){
                    rootGenerator.SetChunk(startingPoints, meshes[0,i,0].GetComponent<MeshGenerator>());
                }else{
                    Debug.Log("SP count: "+startingPoints.Count); 
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
                
                Debug.Log("Count: "+ydictU.Count);

                startPointsDictArray[0,i,0] = startingPoints;
                startPointsDictArray[0,i,1] = ydictU;
                startPointsDictArray[1,i,0] = xdictR;
                startPointsDictArray[1,i,1] = dictRU;
                currentDicIndex = new int[]{0,0,0};

                startingPoints = new Dictionary<CUBE_ORIENTATION,List<Vector3>>();
            }
            
                
                
        

        }
        void MoveChunks(CARDINALS c){
            switch (c)
            {
                case CARDINALS.FORWARD: 
                    for(int j = 0 ; j<2; j++){
                        for(int i = 0; i<2; i++){
                            Transform currentTF = meshes[i,j,0].transform;
                            currentTF.position = new Vector3(currentTF.position.x,currentTF.position.y,currentTF.position.z + 2*(chunkSize - 1));

                            GameObject prev = meshes[i,j,0];
                            meshes[i,j,0] = meshes[i,j,1];
                            meshes[i,j,1] = prev;
                        }
                    }
                    currentDicIndex[2] = currentDicIndex[2] == 0 ? 1 : 0;
                    break;
                case CARDINALS.BACKWARD: 
                    for(int j = 0 ; j<2; j++){
                        for(int i = 0; i<2; i++){
                            Transform currentTF = meshes[i,j,1].transform;
                            currentTF.position = new Vector3(currentTF.position.x,currentTF.position.y,currentTF.position.z - 2*(chunkSize - 1));

                            GameObject prev = meshes[i,j,1];
                            meshes[i,j,1] = meshes[i,j,0];
                            meshes[i,j,0] = prev;
                        }
                    }
                    currentDicIndex[2] = currentDicIndex[2] == 0 ? 1 : 0;
                    break;
                case CARDINALS.LEFT: 
                    for(int j = 0 ; j<2; j++){
                        for(int i = 0; i<2; i++){
                            Transform currentTF = meshes[1,j,i].transform;
                            currentTF.position = new Vector3(currentTF.position.x - 2*(chunkSize - 1),currentTF.position.y,currentTF.position.z );

                            GameObject prev = meshes[1,j,i];
                            meshes[1,j,i] = meshes[0,j,i];
                            meshes[0,j,i] = prev;
                        }
                    }
                    currentDicIndex[0] = currentDicIndex[0] == 0 ? 1 : 0;
                    break;
                case CARDINALS.RIGHT:
                    for(int j = 0 ; j<2; j++){
                        for(int i = 0; i<2; i++){
                            Transform currentTF = meshes[0,j,i].transform;
                            currentTF.position = new Vector3(currentTF.position.x + 2*(chunkSize - 1),currentTF.position.y,currentTF.position.z );

                            GameObject prev = meshes[0,j,i];
                            meshes[0,j,i] = meshes[1,j,i];
                            meshes[1,j,i] = prev;
                        }
                    }
                    currentDicIndex[0] = currentDicIndex[0] == 0 ? 1 : 0;
                    break;
                case CARDINALS.UP:
                    for(int j = 0 ; j<2; j++){
                        for(int i = 0; i<2; i++){
                            Transform currentTF = meshes[j,0,i].transform;
                            currentTF.position = new Vector3(currentTF.position.x ,currentTF.position.y + 2*(chunkSize - 1) ,currentTF.position.z );

                            GameObject prev = meshes[j,0,i];
                            meshes[j,0,i] = meshes[j,1,i];
                            meshes[j,1,i] = prev;
                        }
                    }
                    currentDicIndex[1] = currentDicIndex[1] == 0 ? 1 : 0;
                    break;
                case CARDINALS.DOWN: 
                    for(int j = 0 ; j<2; j++){
                        for(int i = 0; i<2; i++){
                            Transform currentTF = meshes[j,1,i].transform;
                            currentTF.position = new Vector3(currentTF.position.x ,currentTF.position.y - 2*(chunkSize - 1) ,currentTF.position.z );

                            GameObject prev = meshes[j,1,i];
                            meshes[j,1,i] = meshes[j,0,i];
                            meshes[j,0,i] = prev;
                        }
                    }
                    currentDicIndex[1] = currentDicIndex[1] == 0 ? 1 : 0;
                    break;
                default:
                    break;

                
            }
            Debug.Log("Dick index: "+ currentDicIndex[0]+", "+currentDicIndex[1]+", "+currentDicIndex[2]);
        }
    }
}
    