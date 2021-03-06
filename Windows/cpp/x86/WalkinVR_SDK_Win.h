typedef struct
{
	// 피사체의 좌표
	float x1, y1;
	float x2, y2;
	// 피사체의 속도
	float vx1, vy1;
	float vx2, vy2;
	// 피사체의 속도 평균
	float vx, vy;
} WalkinData;

extern "C" __declspec(dllimport) int WalkinVR_Update();
extern "C" __declspec(dllimport) void WalkinVR_GetData(WalkinData *dst);