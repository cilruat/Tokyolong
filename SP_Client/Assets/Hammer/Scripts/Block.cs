using UnityEngine;

namespace Hammer
{
	public class Block : MonoBehaviour
	{
	    public Hazard leftHazard, rightHazard;
	    BlockType _blockType;
	    public BlockType blockType
	    {
	        get
	        {
	            return _blockType;
	        }
	        set
	        {
	            _blockType = value;
	            switch (_blockType)
	            {
	                case BlockType.Simple:
	                    leftHazard.gameObject.SetActive(false);
	                    rightHazard.gameObject.SetActive(false);
	                    break;
	                case BlockType.LeftHazard:
	                    leftHazard.gameObject.SetActive(true);
	                    rightHazard.gameObject.SetActive(false);
	                    break;
	                case BlockType.RightHazard:
	                    leftHazard.gameObject.SetActive(false);
	                    rightHazard.gameObject.SetActive(true);
	                    break;
	                default:
	                    break;
	            }

	        }
	    }
	    public void ResetBlock(int i)
	    {
	        blockType = BlockType.Simple;
	        transform.localPosition = GameManager.GetPos(i);
	    }
	}

	public enum BlockType
	{
	    Simple,
	    LeftHazard,
	    RightHazard
	}
}