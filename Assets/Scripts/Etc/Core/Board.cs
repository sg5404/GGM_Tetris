using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public Transform GhostBlock;

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
        return !arrayGrid[x, y];
    }

    public bool IsValidPos(Shape shape)
    {
        foreach (Transform block in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(block.position);

            if (!IsInBoard((int)pos.x, (int)pos.y))
                return false;

            if (!IsNoBlock((int)pos.x, (int)pos.y))
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
            //Debug.Log(arrayGrid[(int)pos.x, (int)pos.y].position);
        }
    }

    public void ClearCheck()
    {
        for (int y = 0; y < blankSpriteHeight - blankHeader; y++)
        {
            for (int x = 0; x < blankSpriteWidth; x++)
            {
                if (!arrayGrid[x, y]) break;
                if(x == blankSpriteWidth - 1)
                {
                    ClearLine(y);
                    DownLine(y);
                    y--;
                }
            }
        }
    }

    void ClearLine(int y)
    {
        for(int x = 0; x < blankSpriteWidth; x++)
        {
            Destroy(arrayGrid[x, y].gameObject);
            arrayGrid[x, y] = null;
        }
    }

    void DownLine(int num)
    {
        for (int y = num; y < blankSpriteHeight - blankHeader; y++)
        {
            if (y + 1 == blankSpriteHeight - blankHeader) break;

            for (int x = 0; x < blankSpriteWidth; x++)
            {
                if(arrayGrid[x,y] != null)
                {
                    arrayGrid[x, y].position += new Vector3(0, -1, 0);
                }

                arrayGrid[x, y] = arrayGrid[x, y + 1];
            }
        }
    }

    public bool EndCheck()
    {
        for (int y = blankSpriteHeight - blankHeader; y < blankSpriteHeight; y++)
        {
            for (int x = 0; x < blankSpriteWidth; x++)
            {
                if (arrayGrid[x, y])
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void ResetBoard()
    {
        for (int y = 0; y < blankSpriteHeight; y++)
        {
            for (int x = 0; x < blankSpriteWidth; x++)
            {
                if (arrayGrid[x, y])
                    Destroy(arrayGrid[x, y].gameObject);
                arrayGrid[x, y] = null;
            }
        }
    }

    public void UpLine()
    {
        for (int y = 0; y < blankSpriteHeight - blankHeader; y++)
        {
            for (int x = 0; x < blankSpriteWidth; x++)
            {
                if (arrayGrid[x, y])
                {
                    arrayGrid[x, y].position += new Vector3(0, 1, 0);
                }


                if (y - 1 < 0)
                    arrayGrid[x, y] = null;
                else
                    arrayGrid[x, y] = arrayGrid[x, y - 1];
            }
        }
        MakeGhostBrick();
    }

    void MakeGhostBrick()
    {
        int num = Random.Range(0, blankSpriteWidth);

        for (int x = 0; x < blankSpriteWidth; x++)
        {
            if (x == num)
                continue;

            arrayGrid[x, 0] = Instantiate(GhostBlock, new Vector3(x,0,-1), Quaternion.identity);
        }
    }
}
