using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{

    public List<Sprite> sprites;
    [HideInInspector]	public int index = 0;

    void Start()
    {
        index = CharacterManager.Instance.index;
    }

    void Update()
    {
        CharacterManager.Instance.index = index;
        if (index < 0)
        {
            index = sprites.Count - 1;
        }
        if (index >= sprites.Count)
        {
            index = 0;
        }
        GetComponent<SpriteRenderer>().sprite = sprites[index];
        CharacterManager.Instance.character = sprites[index];
    }
}
