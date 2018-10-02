using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class WalkinVR_Mov12 : MonoBehaviour {

    public Text textview;
    public float force = 100.0f;
    Rigidbody rb;

    private struct WalkinData { public float x1, y1, x2, y2, vx1, vy1, vx2, vy2, vx, vy; }
    [DllImport("WalkinVR_SDK_Win64.dll")]
    extern private static int WalkinVR_Update();
    [DllImport("WalkinVR_SDK_Win64.dll")]
    extern private static void WalkinVR_GetData(ref WalkinData dst);
    
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	void Update () {

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

            textview.text = string.Format("V: ({0:0.###}, {1:0.###})", wdata.vx, wdata.vy);
            
            rb.AddForce(new Vector3(wdata.vx, 0, wdata.vy) * force);
        }
        else
        {
            // WalkinData has not changed because frame rate is faster than input
        }
    }
}
