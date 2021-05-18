#pragma once
#include "CBlock.h"
#include <complex>
#include <valarray>
#include <vector>

typedef std::complex<double> Complex;
typedef std::valarray<Complex> CArray;

class CModule4
{
public:
	CModule4() {
		_bRuning = FALSE;
		_hThread = NULL;
		InitializeCriticalSection(&_lock);
	};
	~CModule4() {
		stop();
		DeleteCriticalSection(&_lock);
		for (int i = 0; i < _orderingList.size(); i++)
			delete _orderingList[i];
		_orderingList.clear();
	};

protected:
	BOOL _bRuning;
	HANDLE _hThread;
	std::vector<HANDLE> _subThreads;

	void  onFinishFFT(CBlock4* block);
	CRITICAL_SECTION      _lock;
	std::vector<CBlock4*> _orderingList;
	int                   _lastPushed = -1;
public:
	void start(int nThread) {
		if (_bRuning) return;
		_bRuning = TRUE;
		_hThread = CreateThread(NULL, 0, threadProc, this, 0, NULL);

		for (int i = 0; i < nThread; i++)
		{
			HANDLE h = CreateThread(NULL, 0, subThreadProc, this, 0, NULL);
			_subThreads.push_back(h);
		}
	};

	void stop() {
		if (!_bRuning)return;
		TerminateThread(_hThread, 1);
		_bRuning = FALSE;
		_hThread = NULL;

		for (int i = 0; i < _subThreads.size(); i++)
			TerminateThread(_subThreads[i], 0);
		_subThreads.clear();
	};

	BOOL is_running() { return _bRuning; }
	static DWORD __stdcall threadProc(LPVOID param);
	static DWORD __stdcall subThreadProc(LPVOID param);

protected:
	static void fft(CArray& x);
	CBlock4* DoFFT(CBlock* block);
public:
	static void fft_proc(double* samples, double* reals, double* imgs);
	static double getFrequencyIntensity(double re, double im);
	static double getMagnitude_sqrd(double re, double im);
	static double getMagnitudeNormalized(double re, double im);
	static double getAmplitude(int BinIndex, double re, double im);
	static double index_to_frequency(int nIndex);
	static double getDecibels(double re, double im);
	static double getMagnitude(double Real, double Imaginary);
	static double getDecible10(double re, double im);
	static double squaredMagnitude(double Real, double Imaginary);
	static double getNoisePower(double re, double im);
	static double getPowerSpectrum(double re, double im);

};

