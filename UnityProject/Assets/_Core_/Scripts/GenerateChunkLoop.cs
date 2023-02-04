using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GGJ23{
    /*public class GenerateChunkLoop : MonoBehaviour
    {
        // Start is called before the first frame update
        RootGenerator rootGenerator;
        [SerializeField] MeshGenerator[] meshGenerators;
        [SerializeField] Vector3[,] _startingPoints = new Vector3[,];//[(int)CUBE_ORIENTATION.XP,i];

        void Start()
        {
            rootGenerator = GetComponent<RootGenerator>();
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        void ResetChunks(bool first = false){
            
            if(first){
                for(int i = 0; i<3; i++){
                    //Centers
                    _startingPoints = rootGenerator.SetChunk(new Vector3[0,0], meshGenerators[10+i*3]);
                    rootGenerator.SetChunk(new Vector3[0,0]);

                    //Cross
                    Vector3[] newPoints0 = new Vector3[,];
                    Vector3[] newPoints1 = new Vector3[,];
                    Vector3[] newPoints2 = new Vector3[,];
                    Vector3[] newPoints3 = new Vector3[,];



                    _startingPoints = rootGenerator.SetChunk(new Vector3[0,0], meshGenerators[4+i*3]);
                    _startingPoints = rootGenerator.SetChunk(new Vector3[0,0], meshGenerators[9+i*3]);
                    _startingPoints = rootGenerator.SetChunk(new Vector3[0,0], meshGenerators[11+i*3]);
                    _startingPoints = rootGenerator.SetChunk(new Vector3[0,0], meshGenerators[19+i*3]);
                }
            }else{
                //rootGenerator.SetOirigin();
            }

        }
    }*/
}
    