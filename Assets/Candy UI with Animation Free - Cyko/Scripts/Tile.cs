using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private GameObject highlight;

    public GameObject Highlight
    {
        get { return highlight; }
    }

    private int x;
    private int y;

    private GridGenerator gridRef;
    public GridGenerator GridRef
    {
        set { gridRef = value; }
    }
    public int X
    {
        get { return x; }
        set
        {
            x = value;
        }
    }

    public int Y
    {
        get { return y; }
        set
        {
            y = value;
        }
    }

    private void Start()
    {
        highlight.SetActive(false);
    }
    public void Init(bool isOffset)
    {
        if (isOffset)
            GetComponent<Renderer>().material.color = offsetColor;
        else
            GetComponent<Renderer>().material.color = baseColor;
    }

    void OnMouseEnter()
    {
        if (!PauseMenu.gameIsPaused)
            highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        if (!PauseMenu.gameIsPaused)
            highlight.SetActive(false);
    }

    private void OnMouseUp()
    {
        if (!PauseMenu.gameIsPaused)
        {
            moveTo(x, y);
            if (gridRef.IsGameOver())
                Debug.Log("GAME OVER");
        }
    }
    public void moveTo(int newCol, int newRow)
    {
        if (gridRef.SelectedBall == null)
            return;
        
        ArrayList list = Ball.havePath(gridRef.ListBalls, gridRef.SelectedBall.X, gridRef.SelectedBall.Y, newCol, newRow);
        if (list != null)
        {
            gridRef.SetPositionEmpty(gridRef.SelectedBall.X, gridRef.SelectedBall.Y);
            System.Drawing.Point p = (System.Drawing.Point)list[list.Count - 1];
            gridRef.SelectedBall.X = p.X;
            gridRef.SelectedBall.Y = p.Y;
            gridRef.SetPosition(gridRef.SelectedBall);
            List<Ball> listBalls = gridRef.GetMatch(gridRef.SelectedBall, gridRef.SelectedBall.X, gridRef.SelectedBall.Y);
            if (listBalls != null && listBalls.Count >= 5)
            {
                /*foreach (Ball ball in listBalls)
                    Debug.Log(ball.ColorComponent.Color.ToString());*/
                gridRef.ExplodeBalls(listBalls);
                gridRef.EndTurn = false;
                gridRef.Score += listBalls.Count;
                gridRef.scoreText.SetText("Score: " + gridRef.Score.ToString());
                if (gridRef.Score > PlayerPrefs.GetInt("highScore"))
                {
                    PlayerPrefs.SetInt("highScore", gridRef.Score);
                    gridRef.highscoreText.SetText("Highscore: " + gridRef.Score.ToString());
                }
            }

            else
                gridRef.EndTurn = true;

            gridRef.SelectedBall = null;
           
        }
        else
        {
            //Play sound
            gridRef.EndTurn = false;
        }
    }

}
