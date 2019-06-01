using UnityEngine;
using System.Collections;

namespace GameBench
{
    public class SpinScroller : MonoBehaviour
    {
        public GameObject particleAnim;
        public ListAnimator listAnim;
        public AudioClip pointHitSound;
        public Transform chunksParent;
        public bool spinning;
        int _selectReward;
        AudioSource[] audSource;
        ChunkSlice[] chunkSlices;

        public ChunkSlice chunkSlice_prefab;

        private void Awake()
        {
            SetupSpinWheel();
        }

        internal float unitXSize = 7.5f;
        GameObject obj2;

        void SetupSpinWheel()
        {
            audSource = new AudioSource[5];
            for (int i = 0; i < 5; i++)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = false;
                audSource[i] = source;
            }
            int totalItem = Config.Instance.rewarItem.Length;
            chunkSlices = new ChunkSlice[totalItem];
            for (int i = 0; i < totalItem; i++)
            {
                ChunkSlice item = Instantiate(chunkSlice_prefab, chunksParent) as ChunkSlice;
                item.name = string.Format("Cunk Value {0}", i);
                chunkSlices[i] = item;
            }
            float leftPoint = -(unitXSize * totalItem / 2);
            for (int i = 0; i < Config.Instance.rewarItem.Length; i++)
            {
                chunkSlices[i].SetValues(Config.Instance.rewarItem[i], leftPoint + (i * unitXSize));
            }
            listAnim.tileSizeX = unitXSize * totalItem;
            obj2 = Instantiate(chunksParent.gameObject, listAnim.transform) as GameObject;
            obj2.transform.localPosition = new Vector3(listAnim.tileSizeX, 0, 0);

            ChunkSlice[] slices2 = obj2.GetComponentsInChildren<ChunkSlice>();
            for (int i = 0; i < Config.Instance.rewarItem.Length; i++)
            {
                slices2[i].SetType_Count(Config.Instance.rewarItem[i]);
            }
            particleAnim.SetActive(false);
        }

        public void StartSpin()
        {
            spinning = true;
            listAnim.StartSpin();
        }

        public void PlayHitClip()
        {
            for (int i = 0; i < audSource.Length; i++)
            {
                if (!audSource[i].isPlaying)
                {
                    audSource[i].clip = pointHitSound;
                    audSource[i].Play();
                    break;
                }
            }
        }

        private static SpinScroller _instance;

        public static SpinScroller Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<SpinScroller>();
                }
                return _instance;
            }
        }

        [HideInInspector]
        internal ChunkSlice currentReward;

        public void RewardPlayer()
        {
            spinning = false;
            Debug.LogFormat("{0} === {1}", currentReward.rewardType, currentReward.rewardC);
            bool isCoin = currentReward.rewardType == RewardType.Coin;
            if (isCoin)
            {
                UIManager.Instance.UpdateCoins(currentReward.rewardC);
            }
            particleAnim.SetActive(true);
            currentReward.endRewardObj.PlayAnim(isCoin);
        }

        internal void HitStart(SpriteRenderer sp)
        {
            PlayHitClip();
            sp.sprite = Config.Instance.pointSprite[1];
        }

        internal void HitEnd(SpriteRenderer sp)
        {
            sp.sprite = Config.Instance.pointSprite[0];
        }
    }
}