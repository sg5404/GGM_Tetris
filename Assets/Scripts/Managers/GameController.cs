using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    //게임 보드를 불러옴
    Board gameBoard;

    // 블록 생성(스포너)
    Spawner blockSpawner;

    //현재 활성화한 블록
    Shape activeShape;

    Ghost ghostShape;

    [Range(0.02f, 1f)] public float dropIntervalRate = 0.8f;
    float timeToDrop = 0f;

    float timer_Horizontal = 0f;
    float timer_Turning = 0f;
    float temp = 0f;
    float timer_Score;
    int Score;

    public GameObject GameOverPanel;
    public Button RestartButton;
    public TextMeshProUGUI ScoreText;

    private void Awake()
    {
        RestartButton.onClick.AddListener(Restart);
        GameOverPanel.SetActive(false);
        gameBoard = GameObject.FindObjectOfType<Board>();
        if (gameBoard == null)
        {
            Debug.LogWarning("GameBoard is null");
            return;
        }

        ghostShape = GameObject.FindObjectOfType<Ghost>();
        if (gameBoard == null)
        {
            Debug.LogWarning("ghostShape is null");
            return;
        }

        blockSpawner = GameObject.FindObjectOfType<Spawner>();
        if (blockSpawner == null)
        {
            Debug.LogWarning("blockSpawner is null");
            return;
        }
        //블록 스포너 위치 정수로 고정
        blockSpawner.transform.position = Vector3Int.RoundToInt(blockSpawner.transform.position);
    }

    private void Start()
    {
        if (activeShape == null)
        {
            activeShape = blockSpawner.SpawnShape();
        }
    }

    private void Update()
    {
        //예외처리
        if (gameBoard == null || blockSpawner == null || activeShape == null) return;

        timer_Horizontal += Time.deltaTime;
        timer_Turning += Time.deltaTime;
        timer_Score += Time.deltaTime;

        if(timer_Score > 1f)
        {
            Score += 1;
            timer_Score = 0f;
            string str = Score.ToString("D5");
            ScoreText.text = str;
        }

        //키입력처리
        TurnRight();
        MoveRight();
        MoveLeft();
        MoveFastDown();

        //일정 시간마다 자동으로 생생되는 블록
        //딜레이를 적용 시켜야 함(원하는 시간의 딜레이)
        if (Time.time > timeToDrop)
        {
            timeToDrop = Time.time + dropIntervalRate;
            if(activeShape)
            {
                activeShape.MoveDown();
                IsDrawGhost();
                //여기서 배열 넣는거 생각해야함
                if (!gameBoard.IsValidPos(activeShape))
                {
                    activeShape.MoveUp();
                    //블록 쌓기 해줘야 할듯?
                    gameBoard.InputBlocks(activeShape);
                    gameBoard.ClearCheck();
                    if(gameBoard.EndCheck())
                    {
                        Time.timeScale = 0f;
                        GameOverPanel.SetActive(true);
                        activeShape.CanMove = false;
                    }
                    activeShape = blockSpawner.SpawnShape();
                }
            }
        }
    }

    void IsDrawGhost()
    {
        if (ghostShape)
        {
            if (ghostShape.ghostShape)
                ghostShape.Remove();
            ghostShape.DrawGhost(activeShape, gameBoard);
        }
    }

    void MoveLeft()
    {
        if (Input.GetKey("left"))
        {
            if (Input.GetKeyDown("left"))
            {
                Left();
                return;
            }

            if (timer_Horizontal < 0.05f) return;
            Left();
        }
    }

    void Left()
    {
        timer_Horizontal = 0f;
        activeShape.MoveLeft();
        if (!gameBoard.IsValidPos(activeShape))
        {
            activeShape.MoveRight();
        }

        IsDrawGhost();
    }

    void MoveRight()
    {
        if (Input.GetKey("right"))
        {
            if(Input.GetKeyDown("right"))
            {
                //gameBoard.UpLine();
                Right();
                return;
            }

            if (timer_Horizontal < 0.05f) return;
            Right();
        }
    }

    void Right()
    {
        timer_Horizontal = 0f;
        activeShape.MoveRight();

        if (!gameBoard.IsValidPos(activeShape))
            activeShape.MoveLeft();

        IsDrawGhost();
    }

    void MoveFastDown()
    {
        if(Input.GetKeyDown("down"))
        {
            temp = dropIntervalRate;
            dropIntervalRate = 0.03f;
        }

        if(Input.GetKeyUp("down"))
        {
            dropIntervalRate = temp;
        }
    }

    void TurnRight()
    {
        if (Input.GetKey("up"))
        {
            if (Input.GetKeyDown("up"))
            {
                Rotate();
                return;
            }

            if (timer_Turning < 0.1f) return;
            Rotate();
        }
    }

    void Rotate()
    {
        timer_Turning = 0f;
        activeShape.RotateRight();

        if (!gameBoard.IsValidPos(activeShape))
            activeShape.RotateLeft();

        IsDrawGhost();
    }

    void Restart()
    {
        Destroy(activeShape.gameObject);
        gameBoard.ResetBoard();
        GameOverPanel.SetActive(false);
        activeShape = blockSpawner.SpawnShape();
        Time.timeScale = 1;
    }
}
