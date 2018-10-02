using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class WalkinVR_Mov14 : MonoBehaviour
{
    public Text textview;
    public float multiplier = 0.02f;
    public float damping = 0.8f;
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

    private Vector2 velo = Vector2.zero;
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

            textview.text = string.Format("V: ({0:0.###}, {1:0.###})", wdata.vx, wdata.vy);

            Vector2 accel = new Vector2(wdata.vx, wdata.vy) * multiplier;
            velo += accel;
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
