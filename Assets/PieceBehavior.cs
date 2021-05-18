using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public int coordx;
    public int coordy;
    public int player = 0;
    public bool attacked = false;
    public CubeBehavior curcase;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Remove()
    {
        curcase.Remove();
    }
}
