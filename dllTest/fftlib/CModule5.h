#pragma once
#include "CBlock.h"

class CModule5
{
public:
	CModule5() {
		_bRuning = FALSE;
		_hThread = NULL;
	};
	~CModule5() {
		stop();
	};
protected:
	BOOL _bRuning;
	HANDLE _hThread;
public:
	void start() {
		if (_bRuning) return;
		_bRuning = TRUE;
		_hThread = CreateThread(NULL, 0, threadProc, this, 0, NULL);
	};
	void stop() {
		if (!_bRuning)return;
		TerminateThread(_hThread, 1);
		_bRuning = FALSE;
		_hThread = NULL;
	};
	BOOL is_running() { return _bRuning; }
	static DWORD __stdcall threadProc(LPVOID param);

protected:
	void processScale();
private:
	double scaleValue(double valueIn, double baseMin, double baseMax, double NewlimitMin, double NewlimitMax);
};

