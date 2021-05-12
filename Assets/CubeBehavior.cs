using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehavior : MonoBehaviour
{
    public GameObject hitting;
    // Start is called before the first frame update
    void Start()
    {
        GameObject Chessboard = GameObject.FindGameObjectWithTag("Chessboard");
        RaycastHit hitboard;
        RaycastHit hitpiece;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hitboard, 100))
        {
            if (!hitboard.transform.CompareTag("Chessboard"))
            {
                hitting = hitboard.transform.gameObject;
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitpiece, 100))
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}
