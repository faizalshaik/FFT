#pragma once
#include "CBlock.h"

class CModule2
{
public:
	CModule2() {
		_bRuning = FALSE;
		_hThread = NULL;
	};
	~CModule2() {
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

private:
	CBlock* createCBlock(PBLOCK_DATA pBlock, int nChannels);
	float calcTemperature(WORD rtd);
	double calcVolt(WORD adcVolt);
	double covertDataPointToVoltage(double value);
};

