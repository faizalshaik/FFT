#pragma once

#include <Windows.h>
#include <vector>
#include "Enums.h"
#include "CBlock.h"
#include "Settings.h"
#include "CModule2.h"
#include "CModule3.h"
#include "CModule4.h"
#include "CModule5.h"
#include "CLockedQueue.h"


class CGlobal
{
public:
	typedef struct _CallBackParam {
		BOOL    bError;
		LPCTSTR strError;
	}CallBackParam;

public:
	CGlobal();
	~CGlobal();
	static CGlobal* m_s;
	static CGlobal* instance() {
		if (m_s == NULL)
			m_s = new CGlobal();
		return m_s;
	}	

	CLockedQueue<CBlock>  _queue2;
	CLockedQueue<CBlock>  _queue3;
	CLockedQueue<CBlock4> _queue4;
public:
	bool  _bError = false;
	int   _errorCode = 0;

	int   _nThreads = 100;
	TCHAR _inputFile[MAX_PATH];
	int   _inputChannelCount = 4;
	TCHAR _outputFile[MAX_PATH];

	double               _minMaxs[8];
	void updateMinMaxs(int chNo,  double val);
	int                  _totalBlockCount;
	int                  _processedBlockCount;

	std::vector<POINT>   _ranges;
	Settings _settings;
	void configureFilter(int chNo, int winType, int winSize)
	{
		if (chNo >=0 && chNo < channel_max_count)
		{
			_settings._filterSettings[chNo]._windowSize = winSize;
			_settings._filterSettings[chNo]._windowType = winType;
		}
	}

	void setGain(double* gains, int count)
	{
		for (int i = 0; i < min(count, channel_max_count); i++)
			_settings._gains[i] = gains[i];
	}
	void calcAndSetTotalBlocks(int totals)
	{
		if (_ranges.size() == 0)
		{
			_totalBlockCount = totals;
			return;
		}

		int nTot = 0;
		for (int i = 0; i < _ranges.size(); i++)
		{
			nTot += _ranges[i].y - _ranges[i].x + 1;
		}
		_totalBlockCount = nTot;
	}

	bool isNeedProcessBlock(int iBlock)
	{
		if (_ranges.size() == 0)
			return true;
		for (int i = 0; i < _ranges.size(); i++)
		{
			if (iBlock >= _ranges[i].x && iBlock <= _ranges[i].y)
				return true;
		}
		return false;
	}

public:
	CModule2* _module2;
	CModule3* _module3;
	CModule4* _module4;
	CModule5* _module5;


	bool start(LPCTSTR strfile, int channelCount, LPCTSTR strOutFile,  int nThreads);
	void pause();


	void onError(int errorCode);
	void onFinishedModule(int moduleNo);
	bool getStats(int& errorCode, int& total, int& processed);
};


