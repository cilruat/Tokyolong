using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Festival
{
    /// NPC의 행동을 제어하는 컴포넌트
    public class NpcBehaviour : MonoBehaviour
    {
        private Transform tr;
        private Rigidbody2D ri;

        /// NPC 정보
        public NPC info;

        /// 랜덤 움직임의 범위
        public Vector3 movePosRange;

        /// 방향
        int dir = -1;
        /// 움직이는 중인가?
        public bool isMove = false;

        /// 랜덤 무브의 기본 딜레이 시간
        public float moveDelay;

        /// 랜덤 무브의 최소 딜레이 시간
        public float moveDelayMin;

        /// 랜덤 무브의 최대 딜레이 시간
        public float moveDelayMax;
        /// 상태
        public NpcState state;

        /// 애니메이터
        public Animator ani;

        /// 대화 컴포턴트
        /// </summary>
        TalkBehaviour talkBehaviour;


        private PolyNavAgent _agent;
        /// <summary>
        /// 길찾기 AI
        /// </summary>
        public PolyNavAgent agent
        {
            get
            {
                if (!_agent)
                    _agent = GetComponent<PolyNavAgent>();
                return _agent;
            }
        }


        void Awake()
        {
            tr = GetComponent<Transform>();
            ri = GetComponent<Rigidbody2D>();
            talkBehaviour = GetComponent<TalkBehaviour>();
        }

        void Start()
        {
            ChangeState(NpcState.Move);
        }

        /// NPC의 행동을 제어합니다.
        /// <param name="_state">행동 값</param>
        public void ChangeState(NpcState _state)
        {
            agent.ClearDestinationEvent();

            state = _state;

            switch (state)
            {
                case NpcState.Move:
                    if (moveCoroutine != null)
                    {
                        StopCoroutine(moveCoroutine);
                    }
                    moveCoroutine = StartCoroutine(Move());
                    break;
                case NpcState.Talk:

                    break;
                default: break;
            }
        }

        Coroutine moveCoroutine = null;

        /// 랜덤한 범위 내로 이동합니다.
        IEnumerator Move()
        {
            isMove = true;
            agent.OnDestinationInvalid += Stop;
            agent.OnDestinationReached += Stop;
            agent.SetDestination(GetRandomMovePos());

            while (isMove)
            {
                MoveAnimate();
                Flip();
                yield return null;
            }
            MoveAnimate();

            yield return new WaitForSeconds(moveDelay + Random.Range(moveDelayMin, moveDelayMax));

            moveCoroutine = null;


            if (Random.Range(0f, 1f) < 0.5f)
            {
                switch (Random.Range(0, 1))
                {
                    case 0:
                        Say();
                        break;
                }
                ChangeState(NpcState.Move);
            }
            else
            {
                ChangeState(NpcState.Move);
            }
        }

        /// <summary>
        /// 특정 지점으로 이동합니다.
        /// </summary>
        /// <param name="movePos">도착 지점</param>
        /// <returns></returns>
        IEnumerator MoveTarget(Vector2 movePos)
        {
            isMove = true;
            agent.SetDestination(movePos);
            while (isMove)
            {
                MoveAnimate();
                Flip();
                yield return null;
            }
            MoveAnimate();
            moveCoroutine = null;
        }

        /// <summary>
        /// 특정 지점으로 이동하고 메소드를 실행합니다.
        /// </summary>
        /// <param name="movePos">도착 지점</param>
        /// <param name="_action">메소드</param>
        /// <returns></returns>
        IEnumerator MoveTargetAndAction(Vector2 movePos, System.Action _action)
        {
            isMove = true;
            agent.OnDestinationInvalid += ChangeStateToMove;
            agent.OnDestinationReached += _action;
            agent.SetDestination(movePos);
            while (isMove)
            {
                MoveAnimate();
                Flip();
                yield return null;
            }
            MoveAnimate();
            moveCoroutine = null;
        }

        /// <summary>
        /// 움직임을 멈춥니다.
        /// </summary>
        void Stop()
        {
            isMove = false;
        }

        /// <summary>
        /// 랜덤 무브 상태로 변경합니다.
        /// </summary>
        void ChangeStateToMove()
        {
            isMove = false;
            ChangeState(NpcState.Move);
        }

        /// <summary>
        /// 플레이어 주변으로 랜덤 범위내의 도착 지점을 얻습니다.
        /// </summary>
        /// <returns></returns>
        Vector2 GetRandomMovePos()
        {
            return new Vector2(tr.position.x + Random.Range(-movePosRange.x, movePosRange.x), tr.position.y + Random.Range(-movePosRange.y, movePosRange.y));
        }

        /// <summary>
        /// 움직임 애니메이션을 실행합니다.   여기도 손보아야할듯==///
        /// </summary>
        void MoveAnimate()
        {
            ani.SetFloat("Move", agent.GetSpeedRatio());
        }

        /// <summary>
        /// 캐릭터의 방향에 따라 스케일을 조정합니다.
        /// </summary>
        public void Flip()
        {
            if (agent.movingDirection.x < 0)
            {
                tr.localScale = new Vector3(-1, 1, 1);
                dir = -1;
            }
            else if (agent.movingDirection.x > 0)
            {
                tr.localScale = new Vector3(1, 1, 1);
                dir = 1;
            }
        }

        /// <summary>
        /// 랜덤하게 대화합니다.
        /// </summary>
        public void Say()
        {
            talkBehaviour.Talk();
        }

        /// <summary>
        /// NPC의 상태
        /// </summary>
        public enum NpcState
        {
            Move,
            Talk,
        }
    }
}