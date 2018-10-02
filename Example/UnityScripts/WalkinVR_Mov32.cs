using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class WalkinVR_Mov32 : MonoBehaviour
{
    public Text textview;
    public float multiplier = 0.1f;
    public float damping = 0.9f;
    public float max_spd = 1.0f;
    public float min_spd = 0.01f;

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
    private Vector2 velo = Vector2.zero;
    private Vector2 prev_p1 = Vector2.zero;
    private Vector2 prev_p2 = Vector2.zero;
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
            Vector2 accel = Vector2.zero;
            if (!p1.Equals(Vector2.zero) && prev_p1.Equals(Vector2.zero)) // step in
                accel += p1.normalized;
            if (!p2.Equals(Vector2.zero) && prev_p2.Equals(Vector2.zero)) // step in
                accel += p2.normalized;
            accel.Normalize();
            accel *= multiplier;
            prev_p1 = p1;
            prev_p2 = p2;

            textview.text = string.Format("P1: ({0:0.###}, {1:0.###}), P2: ({2:0.###}, {3:0.###})", wdata.x1, wdata.y1, wdata.x2, wdata.y2);
            
            velo += accel;
            if (velo.magnitude > max_spd)
                transform.Translate((velo.normalized * max_spd).x, 0, (velo.normalized * max_spd).y, Space.World);
            else
                transform.Translate(velo.x, 0, velo.y, Space.World);
            velo *= damping;
            if (velo.magnitude < min_spd)
                velo = Vector2.zero;
        }
        else
        {
            // WalkinData has not changed because frame rate is faster than input
        }
    }
}
