using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class WalkinVR_Mov22 : MonoBehaviour
{
    public Text textview;
    public float speed = 0.1f; // movement speed (constant)
    public float boundary = 60.0f; // radius of dead zone

    private struct WalkinData { public float x1, y1, x2, y2, vx1, vy1, vx2, vy2, vx, vy; }
    [DllImport("WalkinVR_SDK_Win64.dll")]
    extern private static int WalkinVR_Update();
    [DllImport("WalkinVR_SDK_Win64.dll")]
    extern private static void WalkinVR_GetData(ref WalkinData dst);

    void Start()
    {

    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        int stat = WalkinVR_Update(); // You need to call this function periodically.
        if (stat < 0)
        {
            textview.text = "Walkin'VR disconnected";
        }
        else if (stat > 0)
        {
            WalkinData wdata = new WalkinData();
            WalkinVR_GetData(ref wdata);

            Vector2 p1 = new Vector2(wdata.x1, wdata.y1);
            Vector2 p2 = new Vector2(wdata.x2, wdata.y2);
            Vector2 dir = Vector2.zero;
            if (p1.magnitude >= boundary
                || p1.x * transform.forward.x + p1.y * transform.forward.z < 0) // if inner product is negative, the direction of p1 is opposite side of character forward.
                dir += p1;
            if (p2.magnitude >= boundary
                || p2.x * transform.forward.x + p2.y * transform.forward.z < 0)
                dir += p2;
            dir.Normalize();

            textview.text = string.Format("P1: ({0:0.###}, {1:0.###}), P2: ({2:0.###}, {3:0.###})", wdata.x1, wdata.y1, wdata.x2, wdata.y2);

            Vector3 v = new Vector3(dir.x, 0, dir.y) * speed;
            transform.Translate(v, Space.World);
        }
        else
        {
            // WalkinData has not changed because frame rate is faster than input
        }
    }
}
