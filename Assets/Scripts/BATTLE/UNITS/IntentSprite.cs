using UnityEngine;
using UnityEngine.UI;


public class IntentSprite : MonoBehaviour
{
    // class to handle changing of enemy's intent sprite

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
