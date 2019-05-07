using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hammer
{
	public class GameManager : MonoBehaviour
	{
	    public Player p;
	    public SpriteRenderer spBG;
	    public Sprite[] bgSprites;
	    public Timer timer;
	    public GameObject tapObj;
	    public Text scoreText, scoreGameOver, higScoreGameOver;
	    public Image bgBar;
	    public Transform blocksParent;
	    public AudioClip hitClip, ahClip;
	    Block[] blocks;
	    [HideInInspector]
	    public AudioSource[] effectAudio = new AudioSource[5];

	    int _score;
	    public int Score
	    {
	        get
	        {
	            return _score;
	        }
	        set
	        {
	            _score = value;
	            //scoreText.text = _score.ToString();
				UIManager.Instance.SetScore(_score);
	        }
	    }

	    private void Start()
	    {
	        for (int i = 0; i < effectAudio.Length; i++)
	        {
	            effectAudio[i] = gameObject.AddComponent<AudioSource>();
	            effectAudio[i].playOnAwake = false;
	            effectAudio[i].loop = false;
	        }
	    }
	    BlockType GetRandomBlock
	    {
	        get
	        {
	            BlockType bt = BlockType.Simple;
	            //if (Random.Range(1, 100) > 5)
	            {
	                bt = Random.Range(1, 100) > 50 ? BlockType.LeftHazard : BlockType.RightHazard;
	            }
	            return bt;
	        }
	    }
	    void ResetAll()
	    {
	        p._anim.SetBool("death", false);
	        p._anim.Play("idle");
	        spBG.sprite = bgSprites[Random.Range(0, bgSprites.Length)];
	        totemsBroken = 0;
	        tapObj.SetActive(true);
	        Player.isInProcess = false;
	        timer.ResetTimer();
	        Player.firstTap = true;
	        Score = 0;
	        blocks = blocksParent.GetComponentsInChildren<Block>();
	        for (int i = 0; i < blocks.Length; i++)
	        {
	            blocks[i].ResetBlock(i - 1);
	            //print(string.Format("Block Name is {0} at index {1}", blocks[i].gameObject.name, i));
	        }
	    }
	    public void InitGame()
	    {
	        ResetAll();
	        for (int i = 1; i < 9; i++)
	        {
	            blocks[i].blockType = (blocks[i - 1].blockType == BlockType.Simple) ? GetRandomBlock :
	                BlockType.Simple;
	        }
	    }

	    public static Vector2 GetPos(int i)
	    {
	        return new Vector2(0, i * 1.1f);
	    }
	    public void SetBlockPos()
	    {
	        StopAllCoroutines();
	        for (int i = 0; i < 9; i++)
	        {
	            //print((i - 1) * 1.1f);
	            StartCoroutine(MoveToPosition(blocks[i].transform, GetPos(i), GetPos(i - 1), 0.2f));
	        }
	    }
	    public IEnumerator MoveToPosition(Transform t, Vector2 starPos, Vector2 newPos, float time)
	    {
	        float elapsedTime = 0.0f;
	        while (elapsedTime < time)
	        {
	            t.localPosition = Vector2.Lerp(starPos, newPos, elapsedTime / time);
	            elapsedTime += Time.deltaTime;
	            yield return new WaitForEndOfFrame();
	        }
	    }
	    int totemsBroken = 0;
	    public void PlaySmash()
	    {
	        PlayHit();
	        totemsBroken++;
	    }
	    public void RemoveBlock()
	    {
	        Player.isInProcess = false;
	        Score++;
	        blocks[0].ResetBlock(7);
	        Block temp = blocks[0];
	        for (int i = 0; i < 8; i++)
	        {
	            blocks[i] = blocks[i + 1];
	        }
	        blocks[8] = temp;
	        SetBlockPos();
	        blocks[8].blockType = blocks[7].blockType == BlockType.Simple ? GetRandomBlock : BlockType.Simple;

	    }

	    public bool IsNextBlockHazard(bool left)
	    {
	        bool result = false;
	        if (left && blocks[1].blockType == BlockType.LeftHazard)
	        {
	            result = true;
	        }
	        if (!left && blocks[1].blockType == BlockType.RightHazard)
	        {
	            result = true;
	        }
	        return result;
	    }
	    public bool IsCurrentBlockHazard(bool left)
	    {
	        bool result = false;

	        if (left && blocks[0].blockType == BlockType.LeftHazard)
	        {
	            result = true;
	        }
	        if (!left && blocks[0].blockType == BlockType.RightHazard)
	        {
	            result = true;
	        }
	        return result;
	    }
	    public void PlayAh()
	    {
	        if (UIManager.Instance.Volume)
	            AudioSource.PlayClipAtPoint(ahClip, Vector3.zero);
	    }
	    public void PlayHit()
	    {
	        foreach (var item in effectAudio)
	        {
	            if (!item.isPlaying)
	            {
	                item.clip = hitClip;
	                item.Play();
	                break;
	            }
	        }
	        //if (UIManager.Instance.Volume)
	        //    AudioSource.PlayClipAtPoint(hitClip, Vector3.zero);
	    }
	    public void GameOver()
	    {
	        UIManager.TotemsBroken += totemsBroken;
	    }
	    private static GameManager _instance;
	    public static GameManager Instance
	    {
	        get
	        {
	            if (_instance == null) _instance = GameObject.FindObjectOfType<GameManager>();
	            return _instance;
	        }
	    }
	}
}