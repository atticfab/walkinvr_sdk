using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class WalkinVR_Mov34 : MonoBehaviour
{
    public Text textview;
    public float multiplier = 0.1f;
    public float damping = 0.9f;
    public float max_spd = 1.0f;
    public float min_spd = 0.01f;
    public float speed = 0.1f;
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

    // coordinates of foots (previous frame)
    private float tap_spd = 0;
    private Vector3 prev_p1 = Vector3.zero;
    private Vector3 prev_p2 = Vector3.zero;
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

            Vector3 p1 = new Vector3(wdata.x1, 0, wdata.y1);
            Vector3 p2 = new Vector3(wdata.x2, 0, wdata.y2);
            Vector3 v = Vector3.zero;
            float accel = 0;
            if (p1.magnitude >= boundary) // step back
                v -= p1;
            else if (!p1.Equals(Vector3.zero) && prev_p1.Equals(Vector3.zero)) // step in
                accel += multiplier;
            if (p2.magnitude >= boundary) // step back
                v -= p2;
            else if (!p2.Equals(Vector3.zero) && prev_p2.Equals(Vector3.zero)) // step in
                accel += multiplier;
            v.Normalize();
            v *= speed;
            prev_p1 = p1;
            prev_p2 = p2;

            textview.text = string.Format("P1: ({0:0.###}, {1:0.###}), P2: ({2:0.###}, {3:0.###})", wdata.x1, wdata.y1, wdata.x2, wdata.y2);

            tap_spd += accel;
            v += transform.forward * tap_spd;
            if (v.magnitude > max_spd)
                transform.Translate(v.normalized * max_spd, Space.World);
            else
                transform.Translate(v, Space.World);
            tap_spd *= damping;
            if (tap_spd < min_spd)
                tap_spd = 0;
        }
        else
        {
            // WalkinData has not changed because frame rate is faster than input
        }
    }
}
