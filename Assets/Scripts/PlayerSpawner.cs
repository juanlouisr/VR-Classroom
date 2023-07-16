using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    [SerializeField]
    public GameObject prefabToInstantiate;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 originalPosition = prefabToInstantiate.transform.position;
        Quaternion originalRotation = prefabToInstantiate.transform.rotation;

        // Instantiate the prefab at its original position and rotation
        GameObject instantiatedPrefab = Instantiate(prefabToInstantiate, originalPosition, originalRotation);

    }
}
