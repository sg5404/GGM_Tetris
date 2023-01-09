using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //게임 보드를 불러옴
    Board gameBoard;

    // 블록 생성(스포너)
    Spawner blockSpawner;

    //현재 활성화한 블록
    Shape activeShape;

    [Range(0.02f, 1f)] public float dropIntervalRate = 0.8f;
    float timeToDrop = 0f;

    private void Awake()
    {
        gameBoard = GameObject.FindObjectOfType<Board>();
        if (gameBoard == null)
        {
            Debug.LogWarning("GameBoard is null");
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
        //일정 시간마다 자동으로 생생되는 블록
        //딜레이를 적용 시켜야 함(원하는 시간의 딜레이)

        if(Time.time > timeToDrop)
        {
            timeToDrop = Time.time + dropIntervalRate;
            if(activeShape)
            {
                activeShape.MoveDown();
                //여기서 배열 넣는거 생각해야함
                if (!gameBoard.IsValidPos(activeShape))
                {
                    activeShape.MoveUp();
                    //블록 쌓기 해줘야 할듯?
                    gameBoard.InputBlocks(activeShape);
                    activeShape = blockSpawner.SpawnShape();
                }
            }
        }
    }


}
