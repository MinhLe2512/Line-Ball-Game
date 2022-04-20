using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class GridGenerator : MonoBehaviour
{
    //Type of balls
    public enum BallType
    {
        NORMAL,
        GHOST,
        COUNT
    };

    public enum BallState
    {
        QUEUED,
        FINISHED
    }
    [System.Serializable]public struct BallPrefab
    {
        public BallType type;
        public GameObject prefab;
    }
    //Basic stats
    [SerializeField] private BallPrefab[] ballPrefabs;
    [SerializeField] private Tile backgroundPrefab;
    [SerializeField] private float fillTime;

    [SerializeField] private int numberOfQueueBalls;
    [SerializeField] private int xDim;
    [SerializeField] private int yDim;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;

    public TextMeshProUGUI scoreTextGO;
    public TextMeshProUGUI highscoreTextGO;

    private int score;
    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    private Dictionary<Vector2, Tile> tiles;
    private Ball selectedBall;
    public int X
    {
        set { xDim = value; }
        get { return xDim; }
    }
    public int Y
    {
        set { yDim = value; }
        get { return yDim; }
    }
    public int QueueBall
    {
        set { numberOfQueueBalls = value; }
        get { return numberOfQueueBalls; }
    }
    public float FillTime
    {
        set { fillTime = value; }
        get { return fillTime; }
    }
    public Tile BackgroundPrefab
    {
        set { backgroundPrefab = value; }
        get { return backgroundPrefab; }
    }
    public BallPrefab[] BallPrefabs
    {
        set { ballPrefabs = value; }
        get { return ballPrefabs; }
    }
    public Ball SelectedBall
    {
        set { selectedBall = value; }
        get
        {
            return selectedBall;
        }
    }
    public Dictionary<Vector2, Tile> Tiles
    {
        set { tiles = value; }
        get { return tiles; }
    }
    private static bool endTurn = false;
    public bool EndTurn
    {
        get { return endTurn; }
        set
        {
            endTurn = value;
        }
    }

    private Dictionary<BallType, GameObject> ballPrefabDict;
    public Dictionary<BallType, GameObject> BallPrefabDict
    {
        set { ballPrefabDict = value; }
        get { return ballPrefabDict; }
    }
    private Ball[,] listBalls;
    private int ballCounter;
    public int BallCounter
    {
        set { ballCounter = value; }
        get { return ballCounter; }
    }

    private int maxBall;
    public int MaxBall
    {
        set { maxBall = value; }
        get { return maxBall; }
    }
    public Ball[,] ListBalls
    {
        set { listBalls = value; }
        get { return listBalls; }
    }

    private Queue<Ball> ballQueue;
    public Queue<Ball> BallQueue
    {
        get { return ballQueue; }
        set { ballQueue = value; }
    }
    //Game Ending
    private bool gameOver = false;
    public bool GameOver
    {
        get { return gameOver; }
        set { gameOver = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        
        scoreText.SetText("Score: " + score.ToString());
        highscoreText.SetText("Highscore: " + PlayerPrefs.GetInt("highScore").ToString());
        maxBall = xDim * yDim;
        ballCounter = 0;

        if (listBalls != null)
            Array.Clear(listBalls, 0, listBalls.Length);
        ballQueue = new Queue<Ball>();

        ballPrefabDict = new Dictionary<BallType, GameObject>();
        for (int i = 0; i < ballPrefabs.Length; i++)
        {
            if (!ballPrefabDict.ContainsKey(ballPrefabs[i].type)) {
                ballPrefabDict.Add(ballPrefabs[i].type, ballPrefabs[i].prefab);
            }
        }

        GenerateGrid();

        listBalls = new Ball[xDim, yDim];

        int tmpX, tmpY;

        for (int i = 0; i < 5; i++)
        {
            
            tmpX = Random.Range(0, xDim);
            tmpY = Random.Range(0, yDim);

            if (listBalls[tmpX, tmpY] == null)
            {
                SpawnNewBall(tmpX, tmpY, BallType.NORMAL, BallState.FINISHED);

                listBalls[tmpX, tmpY].ColorComponent.SetColor((BallColor.ColorType)Random.Range(0,
                            listBalls[tmpX, tmpY].ColorComponent.NumColors));
            }
            //listBalls[tmpX, tmpY].MoveableComponent.Move(tmpX, tmpY, fillTime);
            
        }
        //StartCoroutine(Fill());
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(ballCounter);
        if (endTurn)
        {
            while (ballQueue.Count > 0)
            {
                Ball tmpBall = ballQueue.Dequeue();
                if (listBalls[tmpBall.X, tmpBall.Y] != null && listBalls[tmpBall.X, tmpBall.Y].BallState == BallState.QUEUED)
                {
                    listBalls[tmpBall.X, tmpBall.Y].BallState = BallState.FINISHED;
                    Debug.Log(listBalls[tmpBall.X, tmpBall.Y].BallState.ToString());
                    GetMatch(tmpBall, tmpBall.X, tmpBall.Y);
                }
                else if (listBalls[tmpBall.X, tmpBall.Y] != null && listBalls[tmpBall.X, tmpBall.Y].BallState == BallState.FINISHED)
                {
                    Destroy(tmpBall.gameObject);
                }
            }

            int tmpX, tmpY, count;
            if (numberOfQueueBalls < maxBall - ballCounter)
                count = numberOfQueueBalls;
            else
                count = maxBall - ballCounter;
            tmpX = Random.Range(0, xDim);
            tmpY = Random.Range(0, yDim);

            while (count > 0)
            {
                if (listBalls[tmpX, tmpY] == null)
                {
                    ballQueue.Enqueue(SpawnNewBall(tmpX, tmpY, BallType.NORMAL, BallState.QUEUED));
                     
                    listBalls[tmpX, tmpY].ColorComponent.SetColor((BallColor.ColorType)Random.Range(0,
                            listBalls[tmpX, tmpY].ColorComponent.NumColors));

                    count--;
                    //Debug.Log("New ball at " + tmpX + " " + tmpY);

                }
                else
                {
                    tmpX = UnityEngine.Random.Range(0, xDim);
                    tmpY = UnityEngine.Random.Range(0, yDim);
                }

            }

            endTurn = false;

        }
    }

    void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                Tile spawnedTile = Instantiate(backgroundPrefab, GetWorldPosition(x,y), Quaternion.identity);

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);
                spawnedTile.X = x;
                spawnedTile.Y = y;
                spawnedTile.GridRef = this;

                tiles[new Vector2(x, y)] = spawnedTile;
                spawnedTile.transform.parent = transform;
            }
        }
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    public Ball GetPosition(int x, int y)
    {
        if (listBalls[x, y] != null)
            return listBalls[x, y];
        else
            return null;
    }


    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(transform.position.x - xDim / 2.0f + x,
            transform.position.y + yDim / 3.0f - y);
    }

    public Ball SpawnNewBall(int x, int y, BallType type, BallState state)
    {
        GameObject newBall = Instantiate(ballPrefabDict[type],
                                                        GetWorldPosition(x, y),
                                                        Quaternion.identity);

        newBall.transform.parent = transform;
        

        listBalls[x, y] = newBall.GetComponent<Ball>();
        listBalls[x, y].Init(x, y, this, type, state);
        ballCounter++;
        return listBalls[x, y];
    }

    //Destroy balls
    public void ExplodeBalls (List<Ball> hsBalls)
    {
        foreach (Ball ball in hsBalls)
        {
            Destroy(listBalls[ball.X, ball.Y].gameObject);
            listBalls[ball.X, ball.Y] = null;
            ballCounter--;
        } 
    }

    public bool IsGameOver()
    {
        if (ballCounter == 81)
        {
            gameOver = true;
            scoreTextGO.SetText("Score: " + score.ToString());
            highscoreTextGO.SetText("Highscore: " + PlayerPrefs.GetInt("highScore").ToString());
        }
        return gameOver;
    }

    public List<Ball> GetMatch(Ball ball, int newX, int newY)
    {
        BallColor.ColorType color = ball.ColorComponent.Color;
        
        List<Ball> matchingBalls = new List<Ball>();

        int[] u = { 0, 1, 1, 1 };
        int[] v = { 1, 0, -1, 1 };

        int i, j, k;
        for (int t = 0; t < 4; t++)
        {
            k = 0;
            i = newX;
            j = newY;
            while (true)
            {
                i += u[t];
                j += v[t];
                if (!PositionOnBoard(i, j))
                    break;
                if (listBalls[i, j] == null || listBalls[i, j] != null
                    && listBalls[i, j].BallState != BallState.QUEUED
                    && ball.ColorComponent.Color != listBalls[i, j].ColorComponent.Color)
                    break;
                k++;
            }
            i = newX;
            j = newY;
            
            while (true)
            {
                i -= u[t];
                j -= v[t];
                if (!PositionOnBoard(i, j))
                    break;
                if (listBalls[i, j] == null || listBalls[i, j] != null 
                    && listBalls[i, j].BallState != BallState.QUEUED 
                    && ball.ColorComponent.Color != listBalls[i, j].ColorComponent.Color)
                    break;
                k++;
            }
            k++;
            if (k >= 5)
                while (k-- > 0)
                {
                    i += u[t];
                    j += v[t];
                    if (listBalls[i, j] != null && listBalls[i, j].BallState != BallState.QUEUED &&
                        ball.ColorComponent.Color == listBalls[i, j].ColorComponent.Color
                        && i != newX || j != newY)
                        matchingBalls.Add(listBalls[i, j]);
                }
        }

        if (matchingBalls.Count > 0)
            matchingBalls.Add(listBalls[newX, newY]);
        else
            matchingBalls = null;
        return matchingBalls;
    }

    public void SetPosition(Ball obj)
    {
        Ball cm = obj.GetComponent<Ball>();
        cm.ColorComponent.SetColor(cm.ColorComponent.Color);

        cm.MoveableComponent.Move(cm.X, cm.Y, fillTime);
        //Overwrites either empty space or whatever was there
        listBalls[cm.X, cm.Y] = obj;

    }

    public ArrayList checkLines(Ball[,] a, int iCenter, int jCenter)
    {
        ArrayList list = new ArrayList();

        int[] u = { 0, 1, 1, 1 };
        int[] v = { 1, 0, -1, 1 };
        int i, j, k;
        for (int t = 0; t < 4; t++)
        {
            k = 0;
            i = iCenter;
            j = jCenter;
            while (true)
            {
                i += u[t];
                j += v[t];
                if (!PositionOnBoard(i, j))
                    break;
                if (a[i, j] != a[iCenter, jCenter])
                    break;
                k++;
            }
            i = iCenter;
            j = jCenter;
            while (true)
            {
                i -= u[t];
                j -= v[t];
                if (!PositionOnBoard(i, j))
                    break;
                if (a[i, j] != a[iCenter, jCenter])
                    break;
                k++;
            }
            k++;
            if (k >= 4)
                while (k-- > 0)
                {
                    i += u[t];
                    j += v[t];
                    if (i != iCenter || j != jCenter)
                        list.Add(new System.Drawing.Point(i, j));
                }
        }
        if (list.Count > 0) list.Add(new System.Drawing.Point(iCenter, jCenter));
        else list = null;
        return list;
    }

    //Helper function
    public void SetPositionEmpty(int x, int y)
    {
        listBalls[x, y] = null;
    }
    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= xDim || y >= yDim) return false;
        return true;
    }


}
