using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehavior : MonoBehaviour
{
    public GameObject hitting;
    public int coordx;
    public int coordy;
    public int occupied = -1; /* -1 si non occuppée, 0 si blanc, 1 si noir*/
    public bool free = true;
    public bool targeted = false;
    public bool take = false;
    public MeshRenderer rend;
    public Material green;
    public Material red;
    private PieceBehavior piece;
    // Start is called before the first frame update
    void Start()
    {
        name = string.Concat(coordx, coordy);
        rend = GetComponent<MeshRenderer>();
        RaycastHit hitpiece;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hitpiece, 100))
        {
            free = false;
            piece = hitpiece.transform.GetComponent<PieceBehavior>();
            occupied = piece.player;
            piece.coordx = coordx;
            piece.coordy = coordy;
            piece.attacked = false;
            piece.curcase = this;
        }
    }
    private void OnEnable()
    {
        UpdatePos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (!targeted)
        {
            rend.enabled = false;
        }
        else rend.enabled = true;
        if (take)
        {
            rend.material = red;
            piece.attacked = true;
        }
        else
        {
            rend.material = green;
        }
    }

    public void UpdatePos()
    {
        RaycastHit hitpiece;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hitpiece, 100))
        {
            free = false;
            piece = hitpiece.transform.GetComponent<PieceBehavior>();
            occupied = piece.player;
            piece.coordx = coordx;
            piece.coordy = coordy;
            piece.attacked = false;
            piece.curcase = this;
        }
        else{
            piece = null;
            }
    }
    public void Remove()
    {
        piece.attacked = false;
        piece.gameObject.SetActive(false);
        piece = null;
    }
}
