using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Adjusts The grid layout to fit the items according to the aspect ratio and number of columns for any screen resolution.
/// </summary>
[RequireComponent(typeof(GridLayoutGroup))]
public class DynamicGridFitter : MonoBehaviour {
    RectTransform container;

    [Tooltip("Desired width divided by desired height")]
    public float aspectRatio;
    public int noOfColumns;

    GridLayoutGroup gridLayoutGroup;

    private void Awake()
    {
        container = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }
   
    void OnEnable()
    {
        StartCoroutine(SetGridLayout());
    }

    /// <summary>
    /// Sets the Grid layout according to the given no of columns and aspectRatio
    /// </summary>
    IEnumerator SetGridLayout()
    {
        //wait a frame for UI update
        yield return new WaitForEndOfFrame();

        //calculate the already used sapce which is the padding + spacing between columns
        float alreayUsedSpace = gridLayoutGroup.padding.left + gridLayoutGroup.padding.right + (noOfColumns - 1) * gridLayoutGroup.spacing.x;

        //calculate cell width from the remaining space and apply
        float cellWidth = (container.rect.width - alreayUsedSpace) / noOfColumns;
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellWidth * aspectRatio);
    }
}
