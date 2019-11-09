using UnityEngine;

public class VectorHelper  {

    public enum Vector3Coord{
        x,
        y,
        z
    }

    /// <summary>
    /// Returns a vector with components zeroed out except X 
    /// </summary>
    public static Vector3 GetXVector(Vector3 vector)
    {
        return  new Vector3(vector.x, 0, 0);
    }


    ///<summary>
    /// Returns a vector with components zeroed out except Z
    /// </summary>
    public static Vector3 GetZVector(Vector3 vector)
    {
        return new Vector3( 0, 0, vector.z);
    }

    /// <summary>
    /// Returns a vector with the passed coordinate modified with the passed value
    /// </summary>
    public static Vector3 GetVectorWith( Vector3Coord coordToModify, Vector3 vectorToModify, float value){
        Vector3 result = vectorToModify;

        switch(coordToModify){
            case Vector3Coord.x:
                result.x = value;
                break;
            case Vector3Coord.y:
                result.y = value;
                break;
            case Vector3Coord.z:
                result.z = value;
                break;
        }
        return result;
    }

    /// <summary>
    /// Copies the position, rotation and scale of one transform to another
    /// </summary>
    public static void CopyTransformProperties(Transform to, Transform from){
        to.transform.position = from.transform.position;
        to.transform.rotation = from.transform.rotation;
        to.transform.localScale = from.transform.localScale;
    }

}
