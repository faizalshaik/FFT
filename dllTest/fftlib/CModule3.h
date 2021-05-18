#pragma once
#include "CBlock.h"

class CModule3
{
public:
	CModule3() {
		_bRuning = FALSE;
		_hThread = NULL;
	};
	~CModule3() {
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
	void applyFilter(CBlock* block);
	void applyGain(CBlock* block);

private:
	double applyWindow(int i, int windowSize, int windowType);
	double bessel(double x);
	int fact(int x);
};

