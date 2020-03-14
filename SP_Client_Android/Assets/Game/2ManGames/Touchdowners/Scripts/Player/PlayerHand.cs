using UnityEngine;

namespace Touchdowners
{

	[RequireComponent(typeof(HingeJoint2D))]
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(BoxCollider2D))]
	public class PlayerHand : MonoBehaviour
	{
		[Header("- Player type -")]
		[SerializeField] private PlayerType _playerType;

		[Header("- Ball settings -")]
		[SerializeField] private Transform _ballPosition;
		[SerializeField] private Vector2 _throwSpeed;

		private float _handRotationSpeed = 700;

		private Collider2D _collider2D;
		private Rigidbody2D _rb2D;

		private IPlayerInput _input;

		private Ball _ball;

		public PlayerType HandType { get { return _playerType; } }
		public Transform BallPosition { get { return _ballPosition; } }

		#region MonoBehaviour

		private void Awake()
		{
			_collider2D = GetComponent<Collider2D>();
			_rb2D = GetComponent<Rigidbody2D>();

			_collider2D.isTrigger = true;

			_input = transform.root.GetComponent<Player>().input;
		}

		private void Update()
		{
			if (_input.ThrowBallPressed() && _ball != null)
			{
				ThrowBall();
			}
		}

		private void FixedUpdate()
		{
			// Hand rotation
			if (_ball == null)
			{
				if (_input.MoveLeftPressed())
					SetHandAngularVelocity(_handRotationSpeed, 5);
				else if (_input.MoveRightPressed())
					SetHandAngularVelocity(-_handRotationSpeed, 5);
				else
					SetHandAngularVelocity(0, 0.2f);
			}
			else
			{
				if (_input.MoveLeftPressed())
					_rb2D.angularVelocity = _handRotationSpeed;
				else if (_input.MoveRightPressed())
					_rb2D.angularVelocity = -_handRotationSpeed;
				else
					SetHandAngularVelocity(0, 1f);
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag(Tags.Ball))
			{
				Ball triggeredBall = collision.GetComponent<Ball>();

				if (triggeredBall.CanInteract && _playerType != triggeredBall.PlayerHolderType)
				{
					AttachBall(triggeredBall);
				}
			}
		}

		#endregion

		private void SetHandAngularVelocity(float velocity, float lerpSpeed)
		{
			_rb2D.angularVelocity = Mathf.Lerp(_rb2D.angularVelocity, velocity, Time.deltaTime * lerpSpeed);
		}

		public void AttachBall(Ball ball)
		{
			ball.AttachToHand(this);
			_ball = ball;

			// In order to collide with ground
			_collider2D.isTrigger = false;
		}

		public void DetachBall()
		{
			_ball = null;

			// In order to not to collider with ground
			_collider2D.isTrigger = true;
		}

		public void ThrowBall()
		{
			_ball.Throw(_throwSpeed);

			DetachBall();
		}

	}

}