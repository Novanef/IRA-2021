using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardState : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject Grid;
    private bool WCheck = false;
    private bool BCheck = false;
    [SerializeField]
    private Text CheckAlert;
    public Queue<PieceBehavior> WThreatList = new Queue<PieceBehavior>();
    public Queue<PieceBehavior> BThreatList = new Queue<PieceBehavior>();
    public Queue<PieceBehavior> DangerList = new Queue<PieceBehavior>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckReset()
    {
        WCheck = false;
        BCheck = false;
        for(int i = 0; i<DangerList.Count; i++)
        {
            DangerList.Dequeue().GetComponent<PieceBehavior>().curcase.danger = false;
        }
    }
    
    public void IsCheck(GameObject Pieces,PieceBehavior King)
    {
        CubeBehavior TargetCase = King.curcase.GetComponent<CubeBehavior>();
        foreach(Transform child in Pieces.transform)
        {
            if(child.gameObject.activeSelf) Getpiece(child,TargetCase);
        }
        if (WCheck)
        {
            CheckAlert.text = "[!] Black King Check [!]";
        }
        else if (BCheck)
        {
            CheckAlert.text = "[!] White King Check [!]";
        }
        else
        {
            CheckAlert.text = "";
        }
        if (BThreatList.Count > 0&&WCheck)
        {
            for(int i = 0; i < BThreatList.Count; i++)
            {
                BThreatList.Dequeue().GetComponent<PieceBehavior>().curcase.danger = true;
            }
        }
        if (WThreatList.Count > 0&&BCheck)
        {
            for (int i = 0; i < WThreatList.Count; i++)
            {
                WThreatList.Dequeue().GetComponent<PieceBehavior>().curcase.danger = true;
            }
        }
    }
    void Getpiece(Transform piece,CubeBehavior target)
    {
        switch (piece.tag)
        {
            case "pawn": PawnMove(piece); break;
            case "king": KingMove(piece); break;
            case "queen": QueenMove(piece); break;
            case "knight": KnightMove(piece); break;
            case "rook": RookMove(piece); break;
            case "bishop": BishopMove(piece); break;
        }
    }
    void PawnMove(Transform piece)
    {
        if (piece.GetComponent<PieceBehavior>().player == 0)
        {
            if (OccupCase(piece,1, 1))
            {
                enableCase(piece, 1, 1);
            }
            if (OccupCase(piece, -1, 1))
            {
                enableCase(piece, -1, 1);
            }
        }
        else
        {
            if (OccupCase(piece, 1, -1))
            {
                enableCase(piece, 1, -1);
            }
            if (OccupCase(piece, -1, -1))
            {
                enableCase(piece, -1, -1);
            }

        }
    }
    void KingMove(Transform piece)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if ((i != 0) || (j != 0))
                {
                    enableCase(piece, i, j);
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
        for (int i = 1; i < 8; i++)
        {
            if (!CheckCase(piece, i, 0))
            {
                if (OccupCase(piece, i, 0)) enableCase(piece, i, 0);
                break;
            }
            enableCase(piece, i, 0);
        }
        for (int i = -1; i > -8; i--)
        {
            if (!CheckCase(piece, i, 0))
            {
                if (OccupCase(piece, i, 0)) enableCase(piece, i, 0);
                break;
            }
            enableCase(piece, i, 0);
        }
        for (int i = 1; i < 8; i++)
        {
            if (!CheckCase(piece, 0, i))
            {
                if (OccupCase(piece, 0, i)) enableCase(piece, 0, i);
                break;
            }
            enableCase(piece, 0, i);
        }
        for (int i = -1; i > -8; i--)
        {
            if (!CheckCase(piece, 0, i))
            {
                if (OccupCase(piece, 0, i)) enableCase(piece, 0, i);
                break;
            }
            enableCase(piece, 0, i);
        }
    }
    void BishopMove(Transform piece)
    {
        for (int i = 1; i < 8; i++)
        {
            if (!CheckCase(piece, i, i))
            {
                if (OccupCase(piece, i, i)) enableCase(piece, i, i);
                break;
            }
            enableCase(piece, i, i);
        }
        for (int i = -1; i > -8; i--)
        {
            if (!CheckCase(piece, i, i))
            {
                if (OccupCase(piece, i, i)) enableCase(piece, i, i);
                break;
            }
            enableCase(piece, i, i);
        }
        for (int i = 1; i < 8; i++)
        {
            if (!CheckCase(piece, -i, i))
            {
                if (OccupCase(piece,-i, i)) enableCase(piece,-i, i);
                break;
            }
            enableCase(piece,-i, i);
        }
        for (int i = -1; i > -8; i--)
        {
            if (!CheckCase(piece,-i, i))
            {
                if (OccupCase(piece,-i, i)) enableCase(piece,-i, i);
                break;
            }
            enableCase(piece,-i, i);
        }
    }
    void KnightMove(Transform piece)
    {
        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 3; j++)
            {
                if (Mathf.Abs(i) + Mathf.Abs(j) == 3)
                {
                    enableCase(piece,i, j);
                }
            }
        }
    }

    void enableCase(Transform piece,int x, int y)
    {
        PieceBehavior curr = piece.GetComponent<PieceBehavior>();
        if (curr.coordx + x > 0 && curr.coordx + x < 9 && curr.coordy + y < 9 && curr.coordy + y > 0)
        {
            CubeBehavior dest = FindCase(curr.coordx + x, curr.coordy + y).GetComponent<CubeBehavior>();
            if (dest.kingcase)
            {
                if (dest.occupied == 0&&curr.player==1)
                {
                    BCheck = true;
                    BThreatList.Enqueue(piece.GetComponent<PieceBehavior>());
                    DangerList.Enqueue(piece.GetComponent<PieceBehavior>());
                }
                else if(dest.occupied == 1 && curr.player == 0)
                {
                    WCheck = true;
                    BThreatList.Enqueue(piece.GetComponent<PieceBehavior>());
                    DangerList.Enqueue(piece.GetComponent<PieceBehavior>());
                }
            }
        }
    }
    bool CheckCase(Transform piece,int x, int y)
    {
        PieceBehavior curr = piece.GetComponent<PieceBehavior>();
        if (curr.coordx + x < 1 || curr.coordx + x > 8 || curr.coordy + y < 1 || curr.coordy + y > 8)
        {
            return false;
        }
        CubeBehavior curcase = FindCase(curr.coordx + x, curr.coordy + y).GetComponent<CubeBehavior>();
        return curcase.free;
    }
    bool OccupCase(Transform piece,int x, int y)
    {
        PieceBehavior curr = piece.GetComponent<PieceBehavior>();
        if (curr.coordx + x < 1 || curr.coordx + x > 8 || curr.coordy + y < 1 || curr.coordy + y > 8)
        {
            return false;
        }
        CubeBehavior curcase = FindCase(curr.coordx + x, curr.coordy + y).GetComponent<CubeBehavior>();
        if (curcase.occupied != -1) return curr.player != curcase.occupied;
        return false;
    }
    GameObject FindCase(int x, int y)
    {
        return Grid.transform.Find(string.Concat(x, y)).gameObject;
    }
}
