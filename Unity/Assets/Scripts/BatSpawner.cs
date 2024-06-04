using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    [SerializeField] private GameObject batModel;


    [SerializeField] private float size = 0.05f;
    void Update()
    {

        if(Input.GetKey(KeyCode.Space))
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-10, 11), 5, Random.Range(-10, 11));
            GameObject instance =  Instantiate(batModel, randomSpawnPosition, Quaternion.identity);

            instance.gameObject.transform.localScale = new Vector3(size,size,size);
        }
    }
}
