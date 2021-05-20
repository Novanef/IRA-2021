using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsBehavior : MonoBehaviour
{
    public bool Activate = false;
    public PieceBehavior pawn;
    [SerializeField]
    public MovePiece main;
    [SerializeField]
    private GameObject WReserve;
    [SerializeField]
    private GameObject BReserve;
    [SerializeField]
    private GameObject Whites;
    [SerializeField]
    private GameObject Blacks;
    [SerializeField]
    private GameObject Chessboard;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        Activate = true;
        pawn = main.PreviousPiece.GetComponent<PieceBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Activate)
        {
           gameObject.SetActive(false);
        }
    }

    public void PromotionQueen()
    {
        GameObject Reserve;
        if (pawn.player == 0) {
            Reserve = WReserve;
        }
        else {
            Reserve = BReserve;
        }
        pawn.gameObject.SetActive(false);
        GameObject Queen=Instantiate(FindPiece("queen",Reserve),pawn.curcase.gameObject.transform.position, Quaternion.Euler(-89.98f, 0, 0), Chessboard.transform);
        if (pawn.player == 0)
        {
           Queen.transform.SetParent(Whites.transform);
        }
        else
        {
            Queen.transform.SetParent(Blacks.transform);
        }
        pawn.curcase.UpdatePos();
        Activate = false;
    }
    public void PromotionKnight()
    {
        GameObject Reserve;
        if (pawn.player == 0)
        {
            Reserve = WReserve;
        }
        else
        {
            Reserve = BReserve;
        }
        pawn.gameObject.SetActive(false);
        GameObject Knight=Instantiate(FindPiece("knight", Reserve), pawn.curcase.gameObject.transform.position, Quaternion.Euler(-89.98f, 0, 0), Chessboard.transform);
        pawn.curcase.UpdatePos();
        Activate = false;
    }
    public void PromotionBishop()
    {
        GameObject Reserve;
        if (pawn.player == 0)
        {
            Reserve = WReserve;
        }
        else
        {
            Reserve = BReserve;
        }
        pawn.gameObject.SetActive(false);
        Instantiate(FindPiece("bishop", Reserve), pawn.curcase.gameObject.transform.position, Quaternion.Euler(-89.98f, 0, 0), Chessboard.transform);
        pawn.curcase.UpdatePos();
        Activate = false;
    }
    public void PromotionRook()
    {
        GameObject Reserve;
        if (pawn.player == 0)
        {
            Reserve = WReserve;
        }
        else
        {
            Reserve = BReserve;
        }
        pawn.gameObject.SetActive(false);
        Instantiate(FindPiece("rook", Reserve), pawn.curcase.gameObject.transform.position, Quaternion.Euler(-89.98f, 0, 0), Chessboard.transform);
        pawn.curcase.UpdatePos();
        Activate = false;
    }
    GameObject FindPiece(string tag,GameObject Reserve)
    {
        foreach (Transform child in Reserve.transform)
        {
            if (child.tag == tag)
            {
                child.gameObject.SetActive(true);
                return child.gameObject;
            }
        }
        return null;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
