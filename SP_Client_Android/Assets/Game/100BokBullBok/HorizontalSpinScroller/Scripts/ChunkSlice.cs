using UnityEngine;
namespace GameBench
{
    public class ChunkSlice : MonoBehaviour
    {
        public SpriteRenderer spRend, iconSpRend;
        public TextMesh valueText;
        //public PointCollider[] pointCollider;
        public EndRewardObject endRewardObj;
        internal RewardType rewardType;
        int _reward;
        public int rewardC
        {
            get
            {
                print("Value is " + _reward);
                return _reward;
            }
            set
            {
                _reward = value;
                valueText.text = _reward.ToString();
            }
        }

        public void SetType_Count(RewardItem value)
        {
            rewardType = value.rewardType;
            rewardC = value.amountOrID;
        }

        public void SetValues(RewardItem value, float pos)
        {
            endRewardObj.SetActive(false);
            spRend.color = value.itemBgColor;
            iconSpRend.sprite = value.rewardSp;
            rewardType = value.rewardType;
            rewardC = value.amountOrID;
            SetPosition(pos);
            endRewardObj.AssignValues(value.itemBgColor, value.rewardSp, value.amountOrID);
            //Debug.LogWarningFormat("{0} =====> {1}", value.amountOrID, value.rewardType);
        }
        internal void SetPosition(float v)
        {
            transform.localPosition = new Vector3(v, 0, 0);
        }
        public float Pos { get { return transform.localPosition.x; } }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SpinScroller.Instance.currentReward = this;
        }
    }
}