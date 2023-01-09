using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // 빈 블록 이미지 프리팹
    [SerializeField] private Transform blankSpritePrefab;

    [SerializeField] private int blankSpriteHeight = 30;

    [SerializeField] private int blankSpriteWidth = 10;

    //헤더 부분을 미리 선언한다
    [SerializeField] private int blankHeader = 8;

    //게임의 불록 공간 전체를 저장하는 용도
    Transform[,] arrayGrid;

    private void Awake()
    {
        arrayGrid = new Transform[blankSpriteWidth, blankSpriteHeight];
    }

    private void Start()
    {
        DrawEmptyCells();
    }

    void DrawEmptyCells()
    {
        if(blankSpriteHeight != null)
        {
            for(int y = 0; y < blankSpriteHeight - blankHeader; y++)
            {
                for(int x = 0; x < blankSpriteWidth; x++)
                {
                    Transform clone;
                    clone = Instantiate(blankSpritePrefab, new Vector3(x, y, -1), Quaternion.identity) as Transform;
                    clone.name = string.Format("Board Space(x={0},y={1})", x, y).ToString();
                    clone.transform.parent = this.transform;

                }
            }
        }
        else
        {
            Debug.LogWarning("blankSpritePrefab is null");
        }
    }

    bool IsInBoard(int x, int y)
    {
        return (x >= 0 && x < blankSpriteWidth && y >= 0);
    }

    bool IsNoBlock(int x, int y)
    {
        return arrayGrid[x, y] == null;
    }

    public bool IsValidPos(Shape shape)
    {
        foreach (Transform block in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(block.position);

            if (!IsInBoard((int)block.position.x, (int)block.position.y))
                return false;

            if (!IsNoBlock((int)block.position.x, (int)block.position.y))
                return false;
        }
        return true;
    }

    public void InputBlocks(Shape shape)
    {
        if (shape == null) return;

        foreach (Transform block in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(block.position);
            arrayGrid[(int)pos.x, (int)pos.y] = block;
        }
    }
}
