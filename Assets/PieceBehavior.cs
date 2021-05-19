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
    [SerializeField]
    private GameObject Buttons;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (this.CompareTag("pawn"))
        {
            if (coordy == 1 || coordy == 8)
            {
                Promotion();
            }
        }
    }
    public void Remove()
    {
        curcase.Remove();
    }
    void Promotion()
    {
        if(!Buttons.GetComponent<ButtonsBehavior>().main.end) Buttons.SetActive(true);
    }
}
