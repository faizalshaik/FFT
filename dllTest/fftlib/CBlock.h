#pragma once
#include <Windows.h>
#include <math.h>
#include <vector>
#include "Enums.h"


#pragma pack(push, 1)
typedef struct _block_data {
	byte header[5];
	byte timeStampIndex[3];
	WORD adcVolt;
	WORD rtd;
	byte data[channel_max_count * INPUT_SAMPLE_DATA_SIZE];
}BLOCK_DATA, * PBLOCK_DATA;
#pragma pack(pop, 1)
#define INPUT_BLOCK_HEADER_SIZE  12


class CBlock
{	
public:
	CBlock() {
		_nChannels = 0;
		memset(_samples, 0, channel_max_count * sizeof(double*));
	};
	~CBlock() {
		for (int i = 0; i < _nChannels; i++)
			delete _samples[i];
	};
public:
	int     _idx;
	byte	_timeStmapArray[4];
	double  _adcVolt;
	double  _temperature;
	int     _nChannels;
	double  _voltMins[channel_max_count];
	double  _voltAvgs[channel_max_count];
	double  _voltMaxs[channel_max_count];
	double* _samples[channel_max_count];
	void    addSample(double* data)
	{
		if (_nChannels >= channel_max_count) return;
		_samples[_nChannels] = data;
		_nChannels++;
	}
};

class CBlock4
{
public:
	CBlock4() {
		_nDatas = 0;
		memset(_datas, 0, sizeof(float*) * channel_max_count * calc_function_max_count);
	};
	~CBlock4() {
		for (int i = 0; i < _nDatas; i++)
		{
			delete _datas[i];
		}
	};
	int     _idx;
	byte	_timeStmapArray[4];
	float   _adcVolt;
	float   _temperature;
	double  _voltMins[channel_max_count];
	double  _voltAvgs[channel_max_count];
	double  _voltMaxs[channel_max_count];

	int     _nChannels = 0;
	int     _nDatas = 0;
	float*  _datas[channel_max_count * calc_function_max_count];
	void addData(float* data)
	{
		if (_nDatas < channel_max_count * calc_function_max_count - 1)
		{
			_datas[_nDatas] = data;
			_nDatas++;
		}		
	}
	void write(HANDLE hFile)
	{
		DWORD dwWritten;
		WriteFile(hFile, &_idx, 4, &dwWritten, NULL);
		WriteFile(hFile, _timeStmapArray, 4, &dwWritten, NULL);
		WriteFile(hFile, &_adcVolt, 4, &dwWritten, NULL);
		WriteFile(hFile, &_temperature, 4, &dwWritten, NULL);
		WriteFile(hFile, _voltMins, sizeof(double) * _nChannels, &dwWritten, NULL);
		WriteFile(hFile, _voltMaxs, sizeof(double) * _nChannels, &dwWritten, NULL);
		WriteFile(hFile, _voltAvgs, sizeof(double) * _nChannels, &dwWritten, NULL);

		for (int i = 0; i < _nDatas; i++)
			WriteFile(hFile, _datas[i], sizeof(float) * FFT_RESULT_SIZE, &dwWritten, NULL);
	}
};
