using UnityEngine;
namespace GameBench
{
[ExecuteInEditMode]
public class ChangeSortingOrder : MonoBehaviour
{
    // Use this for initialization
    public int sortingOrder = 8;
    void Start()
    {
        GetComponent<Renderer>().sortingOrder = sortingOrder;
    }

    private void OnEnable()
    {
        Start();
    }
}
}