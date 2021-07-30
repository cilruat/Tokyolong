using UnityEngine;

public class FlyDeko : MonoBehaviour {

    public float lookSpeed = 15.0f;
    public float moveSpeed = 10.0f;

    public float rotationX = 0.0f;
    public float rotationY = 0.0f;

    // Update is called once per frame
    void Update () {
        Vector3 move_vec = Vector3.zero;

        rotationX += Input.GetAxis("Mouse X") * lookSpeed;
        rotationY += Input.GetAxis("Mouse Y") * lookSpeed;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        move_vec += transform.forward * Input.GetAxis("Vertical");
        move_vec += transform.right * Input.GetAxis("Horizontal");

        GetComponent<Rigidbody>().AddForce(move_vec * moveSpeed);


    }
}



