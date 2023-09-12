using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Shape ghostShape = null;

    bool BHitBottom = false;

    public Color ghostColor = new Color(1f, 1f, 1f, 0.2f);

    public void DrawGhost(Shape oriShape, Board gameBoard)
    {
        if(!ghostShape)
        {
            ghostShape = Instantiate(oriShape, oriShape.transform.position, oriShape.transform.rotation) as Shape;
            ghostShape.gameObject.name = "GhostShape";

            // 색상변경
            SpriteRenderer[] allSprRends = ghostShape.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spr in allSprRends)
            {
                spr.color = ghostColor;
            }
        }
        else
        {
            ghostShape.transform.position = oriShape.transform.position;
            ghostShape.transform.rotation = oriShape.transform.rotation;
        }

        BHitBottom = false;
        while(!BHitBottom)
        {
            ghostShape.MoveDown();
            if(!gameBoard.IsValidPos(ghostShape))
            {
                ghostShape.MoveUp();
                BHitBottom = true;
            }
        }
    }

    public void Remove()
    {
        Destroy(ghostShape.gameObject);
    }
}
