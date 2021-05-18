#include "CModule2.h"
#include <math.h>
#include <tchar.h>
#include "CGlobal.h"

DWORD CModule2::threadProc(LPVOID param)
{
	CGlobal* pGlobal = CGlobal::instance();
	CModule2* pThis = (CModule2*)param;
	

	DWORD dwReaded;
	BLOCK_DATA block;
	HANDLE hFile = CreateFile(pGlobal->_inputFile, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, NULL);
	if (hFile == INVALID_HANDLE_VALUE)
	{
		pThis->_bRuning = FALSE;
		pGlobal->onError(-1);
		return 1;
	}

	DWORD fileSize = GetFileSize(hFile, NULL);
	int   blockSize = INPUT_BLOCK_HEADER_SIZE + INPUT_SAMPLE_DATA_SIZE * pGlobal->_inputChannelCount;

	int nBlocks = fileSize / blockSize;
	pGlobal->calcAndSetTotalBlocks(nBlocks);

	int iBlock = 0;
	int idxBlock = 0;
	while(pThis->_bRuning && iBlock < nBlocks)
	{
		//stuff to save memory
		if (pGlobal->_queue2.size() >= pGlobal->_nThreads * 4)
		{
			Sleep(100);
			continue;
		}

		if (ReadFile(hFile, &block, blockSize, &dwReaded, NULL) == FALSE ||
			dwReaded != blockSize)
			break;

		if (pGlobal->isNeedProcessBlock(iBlock))
		{
			CBlock* pBlock = pThis->createCBlock(&block, pGlobal->_inputChannelCount);
			pBlock->_idx = idxBlock;
			pGlobal->_queue2.append(pBlock);
			idxBlock++;
		}
		iBlock++;
	}
	CloseHandle(hFile);
	pThis->_bRuning = FALSE;
	return 0;
}

CBlock* CModule2::createCBlock(PBLOCK_DATA pBlock, int nChannels)
{
	CBlock* block = new CBlock();
	if (block == NULL)
		return block;

	block->_timeStmapArray[0] = pBlock->timeStampIndex[0];
	block->_timeStmapArray[1] = pBlock->timeStampIndex[1];
	block->_timeStmapArray[2] = pBlock->timeStampIndex[2];
	block->_adcVolt = calcVolt(pBlock->adcVolt);
	block->_temperature = calcTemperature(pBlock->rtd);

	byte* data = pBlock->data;
	for (int iCh = 0; iCh < nChannels; iCh++)
	{
		double* pData = new double[SAMPLE_DATA_SIZE];
		double  voltMin, voltMax, voltTotal = 0;
		for (int i = 0; i < SAMPLE_DATA_SIZE/2; i++)
		{
			byte b3 = data[i * 3 + 2];
			WORD num1 = ((WORD)data[i * 3]) + (b3 & 0xF) * 256;
			WORD num2 = ((WORD)data[i * 3 + 1]) + (b3 >> 4) * 256;
			double v1 = covertDataPointToVoltage((double)num1);
			double v2 = covertDataPointToVoltage((double)num2);
			pData[i * 2] = v1;
			pData[i * 2 + 1] = v2;
			voltTotal += v1;
			voltTotal += v2;
			if (i == 0)
			{
				voltMin = min(v1, v2);
				voltMax = max(v1, v2);
			}
			else
			{
				voltMin = min(voltMin, min(v1, v2));
				voltMax = max(voltMax, max(v1, v2));
			}
		}
		block->addSample(pData);
		block->_voltMins[iCh] = voltMin;
		block->_voltMaxs[iCh] = voltMax;
		block->_voltAvgs[iCh] = voltTotal/ SAMPLE_DATA_SIZE;
		data += INPUT_SAMPLE_DATA_SIZE;
	}
	return block;
}

float CModule2::calcTemperature(WORD rtd)
{
	float Temp = 0.0;
	if (rtd > 0) {
		float R = (rtd * RTD_REF) / ADC_TORES;
		//Conversion of RTD resistance to Temperature
		Temp = -RTD_RES * RTD_A + sqrt(RTD_RES * RTD_RES * RTD_A * RTD_A - 4 * RTD_RES * RTD_B * (RTD_RES - R));
		Temp = Temp / (2 * RTD_RES * RTD_B);
	}
	return Temp;
}
double CModule2::calcVolt(WORD adcVolt)
{
	return ((double)adcVolt) * 0.000806;
}

double CModule2::covertDataPointToVoltage(double value)
{
	CGlobal* pGlobal = CGlobal::instance();
	double voltage = (value * pGlobal->_settings._constants._adcLSB) - pGlobal->_settings._constants._adcHafVdd;
	if ((voltage > 0 && voltage < pGlobal->_settings._constants._UppderVoltageThreshold) ||
		(voltage < 0 && voltage > pGlobal->_settings._constants._LowerVoltageThreshold))
		voltage = 0;
	//else
		//voltage = round(voltage * 1000.0) / 1000.0;
	return voltage;
}

