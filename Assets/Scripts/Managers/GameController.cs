using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //���� ���带 �ҷ���
    Board gameBoard;

    // ��� ����(������)
    Spawner blockSpawner;

    //���� Ȱ��ȭ�� ���
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
        //���� �ð����� �ڵ����� �����Ǵ� ���
        //�����̸� ���� ���Ѿ� ��(���ϴ� �ð��� ������)

        if(Time.time > timeToDrop)
        {
            timeToDrop = Time.time + dropIntervalRate;
            if(activeShape)
            {
                activeShape.MoveDown();
                //���⼭ �迭 �ִ°� �����ؾ���
                if (!gameBoard.IsValidPos(activeShape))
                {
                    activeShape.MoveUp();
                    //��� �ױ� ����� �ҵ�?
                    gameBoard.InputBlocks(activeShape);
                    activeShape = blockSpawner.SpawnShape();
                }
            }
        }
    }


}
