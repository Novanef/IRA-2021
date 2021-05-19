using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePiece : MonoBehaviour
{
    [SerializeField]
    private GameObject bloc;
    [SerializeField]
    private BoardState Manager;
    private int turn = 0;
    public GameObject CurrentPiece;
    public GameObject PreviousPiece;
    [SerializeField]
    private GameObject grid;
    private bool moving = false;
    [SerializeField]
    private PieceBehavior BKing;
    [SerializeField]
    private PieceBehavior WKing;
    [SerializeField]
    private GameObject Whites;
    [SerializeField]
    private GameObject Blacks;
    [SerializeField]
    private Text turncount;
    [SerializeField]
    private Text Wintext;
    [SerializeField]
    public GameObject Buttons;
    [SerializeField]
    public ButtonsBehavior Buttonscript;
    [SerializeField]
    private GameObject EndgameMenu;
    [SerializeField]
    private AudioSource Movesound;
    private bool pause = false;
    public bool end = false;
    // Start is called before the first frame update
    void Start()
    {
        CurrentPiece = null;
        PreviousPiece = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!end)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !pause && !Buttonscript.Activate)
            {
                EndgameMenu.SetActive(true);
                pause = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && pause && !Buttonscript.Activate)
            {
                EndgameMenu.SetActive(false);
                pause = false;
            }
            if (!Buttonscript.Activate || !pause)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100.0f))
                    {
                        if (!hit.transform.CompareTag("Chessboard"))
                        {
                            if (!hit.transform.CompareTag("case"))
                            {
                                Debug.Log("You selected the " + hit.transform.name);
                                if (!moving && CurrentPiece != hit.transform.gameObject && turn == hit.transform.gameObject.GetComponent<PieceBehavior>().player)
                                {
                                    PreviousPiece = CurrentPiece;
                                    CurrentPiece = hit.transform.gameObject;
                                    DestroyPrev();
                                    DisplayMove(hit.transform);
                                    moving = true;
                                }
                                else if (moving && CurrentPiece != hit.transform.gameObject && turn == hit.transform.gameObject.GetComponent<PieceBehavior>().player)
                                {
                                    PreviousPiece = CurrentPiece;
                                    CurrentPiece = hit.transform.gameObject;
                                    DestroyPrev();
                                    DisplayMove(hit.transform);
                                    moving = true;
                                }
                                if (moving && hit.transform.GetComponent<PieceBehavior>().attacked && hit.transform.GetComponent<PieceBehavior>().player != CurrentPiece.GetComponent<PieceBehavior>().player)
                                {
                                    hit.transform.gameObject.GetComponent<PieceBehavior>().Remove();
                                    Transform temp = hit.transform.GetComponent<PieceBehavior>().curcase.transform;
                                    Move(CurrentPiece, temp);
                                    DestroyPrev();
                                    PreviousPiece = CurrentPiece;
                                    CurrentPiece = null;
                                    Newturn();
                                }
                            }
                            else if (moving && hit.transform.GetComponent<CubeBehavior>().take)
                            {
                                hit.transform.GetComponent<CubeBehavior>().Remove();
                                Transform temp = hit.transform.gameObject.transform;
                                Move(CurrentPiece, temp);
                                DestroyPrev();
                                PreviousPiece = CurrentPiece;
                                CurrentPiece = null;
                                Newturn();
                            }
                            else if (moving && hit.transform.GetComponent<CubeBehavior>().targeted)
                            {
                                Transform temp = hit.transform.gameObject.transform;
                                Move(CurrentPiece, temp);
                                DestroyPrev();
                                PreviousPiece = CurrentPiece;
                                CurrentPiece = null;
                                Newturn();
                            }

                        }
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
        PieceBehavior pawn = CurrentPiece.GetComponent<PieceBehavior>();
        if (pawn.player == 0)
        {
            if (pawn.coordy == 2 && CheckCase(0, 2)) enableCase(0, 2);
            if (CheckCase(0, 1)) enableCase(0, 1);
            if (OccupCase(1, 1))
            {
                enableCase(1, 1);
            }
            if (OccupCase(-1, 1))
            {
                enableCase(-1, 1);
            }
        }
        else
        {
            if (pawn.coordy == 7 && CheckCase(0, -2)) enableCase(0, -2);
            if (CheckCase(0, -1)) enableCase(0, -1);
            if (OccupCase(1, -1))
            {
                enableCase(1, -1);
            }
            if (OccupCase(-1, -1))
            {
                enableCase(-1, -1);
            }
            
        }
    }
    void KingMove(Transform piece)
    {
        for(int i=-1;i<2; i++)
        {
            for(int j=-1; j < 2; j++)
            {
                if ((i != 0)|| (j !=0))
                {
                    enableCase(i, j);
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
        for (int i = 1; i <8 ; i++)
        {
            if (!CheckCase(i,0))
            {
                if (OccupCase(i, 0)) enableCase(i, 0);
                break;
            }
            enableCase(i,0);
        }
        for (int i = -1; i > -8; i--)
        {
            if (!CheckCase(i,0))
            {
                if (OccupCase(i, 0)) enableCase(i, 0);
                break;
            }
            enableCase(i,0);
        }
        for (int i = 1; i < 8; i++)
        {
            if (!CheckCase(0,i))
            {
                if (OccupCase(0, i)) enableCase(0, i);
                break;
            }
            enableCase(0, i);
        }
        for (int i = -1; i > -8; i--)
        {
            if (!CheckCase(0,i))
            {
                if (OccupCase(0, i)) enableCase(0, i);
                break;
            }
            enableCase(0, i);
        }
    }
    void BishopMove(Transform piece)
    {
        for (int i = 1; i < 8; i++)
        {
            if (!CheckCase(i, i))
            {
                if (OccupCase(i, i)) enableCase(i, i);
                break;
            }
            enableCase(i, i);
        }
        for (int i = -1; i > -8; i--)
        {
            if (!CheckCase(i, i))
            {
                if (OccupCase(i, i)) enableCase(i, i);
                break;
            }
            enableCase(i, i);
        }
        for (int i = 1; i < 8; i++)
        {
            if (!CheckCase(-i, i))
            {
                if (OccupCase(-i, i)) enableCase(-i, i);
                break;
            }
            enableCase(-i, i);
        }
        for (int i = -1; i > -8; i--)
        {
            if (!CheckCase(-i, i))
            {
                if (OccupCase(-i, i)) enableCase(-i, i);
                break;
            }
            enableCase(-i, i);
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
                    enableCase(i, j);
                }
            }
        }
    }

    void enableCase(int x, int y)
    {
        PieceBehavior curr = CurrentPiece.GetComponent<PieceBehavior>();
        if (curr.coordx + x > 0 && curr.coordx + x < 9 && curr.coordy + y < 9 && curr.coordy + y > 0)
        {
            CubeBehavior dest = FindCase(curr.coordx + x, curr.coordy + y).GetComponent<CubeBehavior>();
            if (CheckCase(x, y))
            {
                dest.targeted = true;
            }
            else if(OccupCase(x,y)){
                dest.take = true;
                dest.targeted = true;
            }
        }
    }
    void DestroyPrev()
    {
        foreach(Transform child in grid.transform)
        {
            CubeBehavior temp=child.GetComponent<CubeBehavior>();
            temp.targeted = false;
            temp.take = false;

        }
        moving = false;
        CheckWin();
    }
    GameObject FindCase(int x, int y)
    {
        return grid.transform.Find(string.Concat(x, y)).gameObject;
    }
    bool CheckCase(int x, int y)
    {
        PieceBehavior curr = CurrentPiece.GetComponent<PieceBehavior>();
        if (curr.coordx + x < 1 || curr.coordx + x > 8 || curr.coordy + y < 1 || curr.coordy + y > 8)
        {
            return false;
        }
        CubeBehavior curcase = FindCase(curr.coordx + x, curr.coordy + y).GetComponent<CubeBehavior>();
        return curcase.free;
    }
    bool OccupCase(int x, int y)
    {
        PieceBehavior curr = CurrentPiece.GetComponent<PieceBehavior>();
        if (curr.coordx + x < 1 || curr.coordx + x > 8 || curr.coordy + y < 1 || curr.coordy + y > 8)
        {
            return false;
        }
        CubeBehavior curcase = FindCase(curr.coordx + x, curr.coordy + y).GetComponent<CubeBehavior>();
        if(curcase.occupied!=-1) return CurrentPiece.GetComponent<PieceBehavior>().player != curcase.occupied;
        return false;
    }
    void Move(GameObject piece,Transform pos)
    {
        PieceBehavior coord = piece.GetComponent<PieceBehavior>();
        CubeBehavior lastcase = grid.transform.Find(string.Concat(coord.coordx, coord.coordy)).GetComponent<CubeBehavior>();
        lastcase.free = true;
        lastcase.occupied = -1;
        lastcase.danger = false;
        lastcase.kingcase = false;
        piece.transform.position = pos.transform.position;
        if (piece.CompareTag("pawn"))
        {
            piece.transform.position+= new Vector3(0,0.012f,0);
        }
        Movesound.Play();
        pos.GetComponent<CubeBehavior>().UpdatePos();
    }
    void CheckWin()
    {
        if (!BKing.gameObject.activeSelf)
        {
            Endgame(0);
        }
        if (!WKing.gameObject.activeSelf)
        {
            Endgame(1);
        }
    }
    void Endgame(int i)
    {
        EndgameMenu.SetActive(true);
        end = true;
        if (i == 0)
        {
            print("White win");
            Wintext.text = "WHITE WINS";
        }
        if (i == 1)
        {
            print("Black win");
            Wintext.text = "BLACK WINS";
        }
        if (i == 2)
        {
            print("Draw");
            Wintext.text = "DRAW";
        }
    }
    void Newturn()
    {
        turn = (turn + 1) % 2;
        if (turn % 2 == 0)
        {
            turncount.text = string.Concat("Current Turn : ", "White");
        }
        else
        {
            turncount.text = string.Concat("Current Turn : ", "Black");
        }
        Manager.CheckReset();
        Manager.IsCheck(Whites, BKing);
        Manager.IsCheck(Blacks, WKing);
    }
    
}
