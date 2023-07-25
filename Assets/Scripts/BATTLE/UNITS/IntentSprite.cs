using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IntentSprite : MonoBehaviour
{
    public Sprite[] SpriteList;

    public void SetSprite(Move.Type type,Image intentIcon)
    {
        switch (type)
        {
            case Move.Type.ATTACK:
                intentIcon.sprite = SpriteList[0];
                break;
            case Move.Type.DEFEND:
                intentIcon.sprite = SpriteList[1];
                break;
            case Move.Type.DEBUFF:
                intentIcon.sprite = SpriteList[3];
                break;
            case Move.Type.BUFF:
                break;
        }
    }
}
