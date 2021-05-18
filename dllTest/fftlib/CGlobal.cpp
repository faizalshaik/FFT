#include "CGlobal.h"
#include <tchar.h>
CGlobal* CGlobal::m_s = NULL;

CGlobal::CGlobal()
{
	_nThreads = 100;
	_totalBlockCount = 0;
	_processedBlockCount = 0;
	memset(_minMaxs, INFINITY, sizeof(double) * 8);

	_module2 = new CModule2();
	_module3 = new CModule3();
	_module4 = new CModule4();
	_module5 = new CModule5();
}
CGlobal::~CGlobal()
{
	_module2->stop();
	_module3->stop();
	_module4->stop();
	_module5->stop();
	delete _module2;
	delete _module3;
	delete _module4;
	delete _module5;
}

void CGlobal::updateMinMaxs(int chNo, double val)
{
	if (_minMaxs[chNo * 2] == INFINITY || val < _minMaxs[chNo * 2])
		_minMaxs[chNo * 2] = val;
	if (_minMaxs[chNo * 2+1] == INFINITY || val > _minMaxs[chNo * 2+1])
		_minMaxs[chNo * 2+1] = val;
}

bool CGlobal::start(LPCTSTR strFile, int channelCount, LPCTSTR strOutFile, int nThreads)
{
	if (_module2->is_running() || _module3->is_running() ||
		_module4->is_running() || _module5->is_running())
		return false;
	delete _module2;
	delete _module3;
	delete _module4;
	delete _module5;

	_inputChannelCount = channelCount;
	if(_settings.outChannelCount(channelCount) == 0 || _settings.outFunctionsCount() ==0)
		return false;

	_queue2.clear();
	_queue3.clear();
	_queue4.clear();

	_totalBlockCount = 0;	
	_processedBlockCount = 0;
	memset(_minMaxs, INFINITY, sizeof(double) * 8);
	_module2 = new CModule2();
	_module3 = new CModule3();
	_module4 = new CModule4();
	_module5 = new CModule5();

	_bError = false;
	_errorCode = 0;
	_nThreads = nThreads;
	_tcscpy_s(_inputFile, MAX_PATH, strFile);
	_tcscpy_s(_outputFile, MAX_PATH, strOutFile);
	_module2->start();
	_module3->start();
	_module4->start(_nThreads);
	_module5->start();
	return true;
}
void CGlobal::pause()
{
	_module2->stop();
	_module3->stop();
	_module4->stop();
	_module5->stop();
}

void CGlobal::onError(int errorCode)
{
	pause();
	_bError = true;
	_errorCode = errorCode;
}
void CGlobal::onFinishedModule(int moduleNo)
{
	switch (moduleNo)
	{
	case 4:
		_errorCode = 0;
		break;
	case 5:
		_errorCode = 0;
		break;
	}
}


bool CGlobal::getStats(int& errorCode, int& total, int& processed)
{
	errorCode = _errorCode;
	total = _totalBlockCount;
	processed = _processedBlockCount;
	return _bError;
}

