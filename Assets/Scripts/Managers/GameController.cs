using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    //���� ���带 �ҷ���
    Board gameBoard;

    // ��� ����(������)
    Spawner blockSpawner;

    //���� Ȱ��ȭ�� ���
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
        //��� ������ ��ġ ������ ����
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
        //����ó��
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

        //Ű�Է�ó��
        TurnRight();
        MoveRight();
        MoveLeft();
        MoveFastDown();

        //���� �ð����� �ڵ����� �����Ǵ� ���
        //�����̸� ���� ���Ѿ� ��(���ϴ� �ð��� ������)
        if (Time.time > timeToDrop)
        {
            timeToDrop = Time.time + dropIntervalRate;
            if(activeShape)
            {
                activeShape.MoveDown();
                IsDrawGhost();
                //���⼭ �迭 �ִ°� �����ؾ���
                if (!gameBoard.IsValidPos(activeShape))
                {
                    activeShape.MoveUp();
                    //��� �ױ� ����� �ҵ�?
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
