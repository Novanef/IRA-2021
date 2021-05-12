using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePiece : MonoBehaviour
{
    [SerializeField]
    private GameObject bloc;
    private GameObject CurrentPiece;
    private GameObject PreviousPiece;
    // Start is called before the first frame update
    void Start()
    {
        CurrentPiece = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (!hit.transform.CompareTag("Chessboard"))
                {
                    Debug.Log("You selected the " + hit.transform.name);
                    if (CurrentPiece != hit.transform.gameObject)
                    {
                        PreviousPiece = CurrentPiece;
                        CurrentPiece = hit.transform.gameObject;
                        DisplayMove(hit.transform);
                    }
                    if (PreviousPiece != null)
                    {
                        DestroyPrev(PreviousPiece.transform);
                    }
                }
            }
        }
    }

    void DisplayMove(Transform piece)
    {
        switch (piece.tag)
        {
            case "pawn":PawnMove(piece) ; break;
            case "king": KingMove(piece); break;
            case "queen": QueenMove(piece); break;
            case "knight": KnightMove(piece) ; break;
            case "rook": RookMove(piece); break;
            case "bishop": BishopMove(piece) ; break;
        }
    }
    void PawnMove(Transform piece)
    {
        AdjustPos(GetBloc(piece), 1, 1);
        AdjustPos(GetBloc(piece), 1, -1);
    }
    void KingMove(Transform piece)
    {
        for(int i=-1;i<2; i++)
        {
            for(int j=-1; j < 2; j++)
            {
                if ((i != 0)|| (j !=0))
                {
                    AdjustPos(GetBloc(piece), i, j);
                }
            }
        }
    }
    void QueenMove(Transform piece)
    {
        RookMove(piece);
        BishopMove(piece);
    }
    void RookMove(Transform piece)
    {
        for(int i = -7; i < 8; i++)
        {
            if(i!=0) AdjustPos(GetBloc(piece), i, 0);
        }
        for (int j = -7; j < 8; j++)
        {
            if (j != 0) AdjustPos(GetBloc(piece), 0, j);
        }
    }
    void BishopMove(Transform piece)
    {
        for (int i = -7; i < 8; i++)
        {
            if (i != 0)
            {
                AdjustPos(GetBloc(piece), i, i);
                AdjustPos(GetBloc(piece), -i,-i);
                AdjustPos(GetBloc(piece), -i, i);
                AdjustPos(GetBloc(piece), i, -i);
            }
        }
    }
    void KnightMove( Transform piece)
    {
        for(int i = -2; i < 3; i++)
        {
            for(int j = -2; j < 3; j++)
            {
                if(Mathf.Abs(i)+ Mathf.Abs(j)== 3)
                {
                    AdjustPos(GetBloc(piece), i, j);
                }
            }
        }
    }
    GameObject GetBloc(Transform piece)
    {
        return Instantiate(bloc, piece.transform.position, piece.transform.rotation, piece);
    }

    void AdjustPos(GameObject cube, int x, int y)
    {
        cube.transform.localRotation = Quaternion.Euler(-89.98f, 0, 0);
        cube.transform.localPosition += new Vector3(0.06f * x, 0.06f * y, 0);
    }
    void DestroyPrev(Transform Prev)
    {
        for(int i = 0; i < Prev.childCount; i++)
        {
            Destroy(Prev.GetChild(i).gameObject);
        }
    }
}
