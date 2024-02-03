using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject frogger;
    private float yCoord;
    [SerializeField]
    private float yMin, yMax;

    // Start is called before the first frame update
    void Start()
    {
        yCoord = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        yCoord = Mathf.Max(yMin, Mathf.Min(yMax, frogger.transform.position.y));
        transform.position = new Vector3 (transform.position.x, yCoord, transform.position.z);        
    }
}
