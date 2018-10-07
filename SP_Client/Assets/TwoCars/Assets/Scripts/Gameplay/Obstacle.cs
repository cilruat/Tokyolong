//  /*********************************************************************************
//   *********************************************************************************
//   *********************************************************************************
//   * Produced by Skard Games										                 *
//   * Facebook: https://goo.gl/5YSrKw											     *
//   * Contact me: https://goo.gl/y5awt4								             *
//   * Developed by Cavit Baturalp Gürdin: https://tr.linkedin.com/in/baturalpgurdin *
//   *********************************************************************************
//   *********************************************************************************
//   *********************************************************************************/

using UnityEngine;
using DG.Tweening;

namespace TwoCars
{
	public enum ObstacleType
	{
	    LET_GO,
	    HOLDER
	}

	public class Obstacle : MonoBehaviour {

	    public ObstacleType myType;

	    private Vector2 direction = Vector2.down;
	    private ParticleSystem _hitParticle;
	    private SpriteRenderer _spriteRenderer;
	    private Color _initColor;

	    void Start()
	    {
	        _hitParticle = GetComponentInChildren<ParticleSystem>();
	        _spriteRenderer = GetComponent<SpriteRenderer>();
	        _initColor = _spriteRenderer.color;
	    }

	    void Update()
	    {
			if (Managers.UI.isStop)
				return;

	        Move();
	    }

	    public void Move()
	    {
	        Vector2 movement = direction * Managers.Difficulty.obstacleSpeed * 1 / Managers.Difficulty.spawnInterval;

	        movement *= Time.deltaTime;

	        transform.Translate(movement);
	    }

	    void OnCollisionEnter2D(Collision2D coll)
	    {
	        if (myType == ObstacleType.LET_GO)
	        {
	            if (coll.gameObject.CompareTag("END"))
	            {
	                Managers.Game.SetState(typeof(GameOverState));
	                Destroy(this.gameObject);
	            }
	            else if (coll.gameObject.CompareTag("CAR"))
	            {
	                Managers.Audio.PlayCollectSound();
	                Managers.Score.OnScore(1);
	                Managers.Difficulty.IncreaseDifficulty();
	                Destroy(this.gameObject);
	            }
	        }
	        else if (myType == ObstacleType.HOLDER)
	        {
	            if (coll.gameObject.CompareTag("CAR"))
	            {
	                Managers.Game.SetState(typeof(GameOverState));
	               
	                _spriteRenderer.DOColor(new Color(_initColor.r, _initColor.g, _initColor.b, 0), 0.3f)
	                    .OnStart(() => { _hitParticle.Play();})
	                    .OnComplete(()=> { Destroy(this.gameObject); });                
	            }

	            else if (coll.gameObject.CompareTag("END"))
	            {
	                Destroy(this.gameObject);
	            }
	        }
	    }
	}
}