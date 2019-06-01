using UnityEngine;

/// <summary>
/// Responsible for :
/// - Adjusting the UVs so that they don't strech.
/// - Copying UVs from base block to incoming block
/// - Readjusting UVs for the pieces so that they match with the cut.
/// </summary>
public class MeshUVAdjuster : MonoBehaviour
{
    public static float scaleFactor = 3;

    /// <summary>
    /// Copies the UVs
    /// </summary>
    /// <param name="copyFrom">Copy from.</param>
    /// <param name="copyTo">Copy to.</param>
    public static void CopyUVs(Transform copyFrom, Transform copyTo)
    {
        Mesh mesh1 = copyFrom.GetComponent<MeshFilter>().mesh;
        Mesh mesh2 = copyTo.GetComponent<MeshFilter>().mesh;
        mesh2.uv = mesh1.uv;
    }

    /// <summary>
    /// Adjusts the texture uniformly on the cube by tiling the texture according to cube's scale. Avoids texture tiling 
    /// </summary>
    public static void AdjustUVs(Transform _transform)
    {
        Mesh mesh = _transform.GetComponent<MeshFilter>().mesh;

        Vector2[] UVs = new Vector2[mesh.vertices.Length];

        //Top and bottom
        UVs[4] = UVs[12] = new Vector2(0, 0);
        UVs[5] = UVs[15] = new Vector2(0, _transform.localScale.x / scaleFactor);
        UVs[8] = UVs[13] = new Vector2(_transform.localScale.z / scaleFactor, 0);
        UVs[9] = UVs[14] = new Vector2(_transform.localScale.z / scaleFactor, _transform.localScale.x / scaleFactor);

        //Right and left
        UVs[16] = UVs[23] = new Vector2(0, _transform.localScale.z / scaleFactor);
        UVs[17] = UVs[22] = new Vector2(_transform.localScale.y / scaleFactor, _transform.localScale.z / scaleFactor);
        UVs[18] = UVs[21] = new Vector2(_transform.localScale.y / scaleFactor, 0);
        UVs[19] = UVs[20] = new Vector2(0, 0);

        //Front and back
        UVs[0] = UVs[6] = new Vector2(0, 0);
        UVs[1] = UVs[7] = new Vector2(0, _transform.localScale.x / scaleFactor);
        UVs[2] = UVs[10] = new Vector2(_transform.localScale.y / scaleFactor, 0);
        UVs[3] = UVs[11] = new Vector2(_transform.localScale.y / scaleFactor, _transform.localScale.x / scaleFactor);

        mesh.uv = UVs;
    }


    /// <summary>
    /// Ajusts the UVs for the pieces from x direction.
    /// </summary>
    /// <param name="baseBlock">Base block.</param>
    /// <param name="piece1">Piece1.</param>
    /// <param name="piece2">Piece2.</param>
    /// <param name="switchOrder">If set to <c>true</c> Switch order of piece1 and piece2.</param>
    public static void AjustUVsforpiecesFromX(Transform baseBlock, Transform piece1, Transform piece2, bool switchOrder=false)
    {
        Mesh baseCubeMesh = baseBlock.GetComponent<MeshFilter>().mesh;

        Transform firstPiece = switchOrder ? piece2.transform : piece1.transform;
        Mesh firstPieceMesh = switchOrder? piece2.GetComponent<MeshFilter>().mesh: piece1.GetComponent<MeshFilter>().mesh;
        Mesh secondPieceMesh = switchOrder ? piece1.GetComponent<MeshFilter>().mesh : piece2.GetComponent<MeshFilter>().mesh;

        Vector2[] UVs = baseCubeMesh.uv;

        float y = (UVs[5].y - UVs[4].y) / baseBlock.lossyScale.x * firstPiece.lossyScale.x + UVs[4].y;

        //for first piece
        //top and bottom
        UVs[5] = UVs[15] = new Vector2(UVs[5].x, y);
        UVs[9] = UVs[14] = new Vector2(UVs[9].x, y);
        //front and back
        UVs[1] = UVs[7] = new Vector2(UVs[1].x, y);
        UVs[3] = UVs[11] = new Vector2(UVs[3].x, y);
        firstPieceMesh.uv = UVs;

        //for second piece
        UVs = baseCubeMesh.uv;
        //top and bottom
        UVs[4] = UVs[12] = new Vector2(UVs[4].x, y);
        UVs[8] = UVs[13] = new Vector2(UVs[8].x, y);
        //front and back
        UVs[0] = UVs[6] = new Vector2(UVs[0].x, y);
        UVs[2] = UVs[10] = new Vector2(UVs[2].x, y);
        secondPieceMesh.uv = UVs;
    }


    /// <summary>
    ///  Ajusts the UVs for the pieces from z direction.
    /// </summary>
    /// <param name="baseBlock">Base block.</param>
    /// <param name="piece1">Piece1.</param>
    /// <param name="piece2">Piece2.</param>
    /// <param name="switchOrder">If set to <c>true</c> switch order of piece1 and piece2.</param>
    public static void AdjustUVsforpiecesFromZ(Transform baseBlock, Transform piece1, Transform piece2, bool switchOrder = false)
    {
        Mesh baseCubeMesh = baseBlock.GetComponent<MeshFilter>().mesh;

        Transform firstPiece = switchOrder ? piece2.transform : piece1.transform;
        Mesh firstPieceMesh = switchOrder ? piece2.GetComponent<MeshFilter>().mesh : piece1.GetComponent<MeshFilter>().mesh;
        Mesh secondPieceMesh = switchOrder ? piece1.GetComponent<MeshFilter>().mesh : piece2.GetComponent<MeshFilter>().mesh;

        Vector2[] uvs = baseCubeMesh.uv;

        float x = (uvs[8].x - uvs[4].x) / baseBlock.lossyScale.z * firstPiece.lossyScale.z + uvs[4].x;

        //For first piece
        //Top and bottom
        uvs[8] = uvs[13] = new Vector2(x, uvs[8].y);
        uvs[9] = uvs[14] = new Vector2(x, uvs[9].y);
        //Right and left
        uvs[17] = uvs[22] = new Vector2(uvs[17].x, x);
        uvs[16] = uvs[23] = new Vector2(uvs[16].x, x);
        firstPieceMesh.uv = uvs;

        uvs = baseCubeMesh.uv;

        //For second piece
        //Top and bottom
        uvs[4] = uvs[12] = new Vector2(x, uvs[4].y);
        uvs[5] = uvs[15] = new Vector2(x, uvs[5].y);
        //Right and left
        uvs[18] = uvs[21] = new Vector2(uvs[18].x, x);
        uvs[19] = uvs[20] = new Vector2(uvs[19].x, x);
        secondPieceMesh.uv = uvs;
    }
}
