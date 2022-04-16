using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
[System.Serializable]
public class Ball : MonoBehaviour
{
    private int x;
    private int y;

    public int X { 
        get { return x; }
        set
        {
            if (isMovable())
            {
                x = value;
            }
        }
    }
    public int Y {
        get { return y; }
        set
        {
            if (isMovable())
            {
                y = value;
            }
        }
    }

    public GameObject controller;
    private GridGenerator.BallType type;

    public GridGenerator.BallType Type { 
        get { return type; }
    }

    private GridGenerator gridRef;
    public GridGenerator GridRef
    {
        get { return gridRef; }
    }

    private MovableBall moveableComponent;
    public MovableBall MoveableComponent
    {
        get { return moveableComponent; }
    }

    private BallColor colorComponent;
    public BallColor ColorComponent
    {
        get { return colorComponent; }
    }

    private GridGenerator.BallState ballState;
    public GridGenerator.BallState BallState
    {
        get { return ballState; }
        set
        {
            ballState = value;
        }
    }
    void Awake()
    {
        moveableComponent = GetComponent<MovableBall>();
        colorComponent = GetComponent<BallColor>();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gridRef.SelectedBall = null;
            this.ColorComponent.GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
        }

        if (ballState == GridGenerator.BallState.QUEUED)
            transform.localScale = new Vector3(0.1f, 0.1f, 1);

        if (ballState == GridGenerator.BallState.FINISHED)
            transform.localScale = new Vector3(0.2f, 0.2f, 1);
    }

    void OnMouseUp()
    {
        if (!controller.GetComponent<GridGenerator>().IsGameOver() &&
            ballState != GridGenerator.BallState.QUEUED && !PauseMenu.gameIsPaused)
        {
            this.ColorComponent.GetComponent<SpriteRenderer>().color = UnityEngine.Color.red;
            gridRef.SelectedBall = this;
            //Debug.Log("Hi ball");
        }

    }



    public void Init(int x, int y, GridGenerator grid, GridGenerator.BallType type
        , GridGenerator.BallState state)
    {
        this.x = x;
        this.y = y;
        gridRef = grid;
        this.type = type;
        ballState = state;
    }

    public static ArrayList havePath(Ball[,] a, int i1, int j1, int i2, int j2)
    {
        int[,] dadi = new int[9, 9];
        int[,] dadj = new int[9, 9];

        int[] queuei = new int[81];
        int[] queuej = new int[81];

        int first = 0, last = 0, x, y;

        for (x = 0; x < 9; x++)
            for (y = 0; y < 9; y++) dadi[x, y] = -1;

        queuei[0] = i2;
        queuej[0] = j2;
        dadi[i2, j2] = -2;

        while (first <= last)
        {
            x = queuei[first];
            y = queuej[first];
            first++;
            if (y > 0)
            {
                if (x == i1 & y - 1 == j1)
                {
                    dadi[i1, j1] = x;
                    dadj[i1, j1] = y;
                    return buildPath(dadi, dadj, i1, j1);
                }
                if (dadi[x, y - 1] == -1)
                    if (a[x, y - 1] == null ||
                        a[x, y - 1] != null && a[x, y - 1].BallState == GridGenerator.BallState.QUEUED)
                    {
                        last++;
                        queuei[last] = x;
                        queuej[last] = y - 1;
                        dadi[x, y - 1] = x;
                        dadj[x, y - 1] = y;
                    }
            }
            if (y < 8)
            {
                if (x == i1 & y + 1 == j1)
                {
                    dadi[i1, j1] = x;
                    dadj[i1, j1] = y;
                    return buildPath(dadi, dadj, i1, j1);
                }
                if (dadi[x, y + 1] == -1)
                    if (a[x, y + 1] == null ||
                        a[x, y + 1] != null && a[x, y + 1].BallState == GridGenerator.BallState.QUEUED)
                    {
                        last++;
                        queuei[last] = x;
                        queuej[last] = y + 1;
                        dadi[x, y + 1] = x;
                        dadj[x, y + 1] = y;
                    }
            }
            if (x > 0)
            {
                if (x - 1 == i1 & y == j1)
                {
                    dadi[i1, j1] = x;
                    dadj[i1, j1] = y;
                    return buildPath(dadi, dadj, i1, j1);
                }
                if (dadi[x - 1, y] == -1)
                    if (a[x - 1, y] == null ||
                        a[x - 1, y] != null && a[x - 1, y].BallState == GridGenerator.BallState.QUEUED)
                    {
                        last++;
                        queuei[last] = x - 1;
                        queuej[last] = y;
                        dadi[x - 1, y] = x;
                        dadj[x - 1, y] = y;
                    }
            }
            if (x < 8)
            {
                if (x + 1 == i1 & y == j1)
                {
                    dadi[i1, j1] = x;
                    dadj[i1, j1] = y;
                    return buildPath(dadi, dadj, i1, j1);
                }
                if (dadi[x + 1, y] == -1)
                    if (a[x + 1, y] == null ||
                        a[x + 1, y] != null && a[x + 1, y].BallState == GridGenerator.BallState.QUEUED)
                    {
                        last++;
                        queuei[last] = x + 1;
                        queuej[last] = y;
                        dadi[x + 1, y] = x;
                        dadj[x + 1, y] = y;
                    }
            }

        }
        return null;
    }

    public static ArrayList buildPath(int[,] dadi, int[,] dadj, int i1, int j1)
    {
        ArrayList arr = new ArrayList();
        int k;
        while (true)
        {
            arr.Add(new Point(i1, j1));
            
            k = i1;
            i1 = dadi[i1, j1];
            if (i1 == -2) break;
            j1 = dadj[k, j1];
        }
        return arr;
    }

    public bool isMovable()
    {
        return moveableComponent != null;
    }

    public bool isColored()
    {
        return colorComponent != null;
    }
}
