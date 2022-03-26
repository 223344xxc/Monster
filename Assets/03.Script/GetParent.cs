using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetParent : MonoBehaviour
{
    public GameObject Parent;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetParentObject();
    }

    void GetParentObject()
    {
        Parent = transform.parent.gameObject;
    }
}
