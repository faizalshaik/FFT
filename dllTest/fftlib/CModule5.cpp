#include "CModule5.h"
#include <tchar.h>
#include <math.h>
#include "CGlobal.h"

DWORD CModule5::threadProc(LPVOID param)
{
	CGlobal* pGlobal = CGlobal::instance();
	Settings* pSetting = &pGlobal->_settings;
	CModule5* pThis = (CModule5*)param;

	//while (pThis->_bRuning)
	//{
	//	if (pGlobal->_totalBlockCount==0)
	//	{
	//		Sleep(500);
	//		continue;
	//	}
	//	CBlock* block = pGlobal->getBlock(pGlobal->_totalBlockCount-1);
	//	if (block == NULL)
	//	{
	//		Sleep(500);
	//		continue;
	//	}
	//	if (pSetting->_scaleChNo < 0)
	//	{
	//		break;
	//	}

	//	pThis->processScale();
	//	break;
	//}
	//pThis->_bRuning = FALSE;
	//if (pSetting->_scaleChNo >= 0)
	//	pGlobal->onFinishedModule(5);
	return 0;
}

void CModule5::processScale()
{
	//CGlobal* pGlobal = CGlobal::instance();
	//Settings* pSetting = &pGlobal->_settings;
	//int chNo = pSetting->_scaleChNo;
	//double baseMin = pSetting->_scaleBaseMin;
	//double baseMax = pSetting->_scaleBaseMax;
	//double newMin = pGlobal->_minMaxs[chNo * 2];
	//double newMax = pGlobal->_minMaxs[chNo * 2+1];

	//std::vector<CBlock*>* blocks = pGlobal->blocks();
	//for (int i = 0; i < blocks->size(); i++)
	//{
	//	CBlock* block = (*blocks)[i];
	//	double* input = NULL, *output = NULL;
	//	switch (chNo)
	//	{
	//	case 0:
	//		input = block->_decblH2;
	//		output = block->_decblH2S;
	//		break;
	//	case 1:
	//		input = block->_decblH1;
	//		output = block->_decblH1S;
	//		break;
	//	case 2:
	//		input = block->_decblL2;
	//		output = block->_decblL2S;
	//		break;
	//	case 3:
	//		input = block->_decblL1;
	//		output = block->_decblL1S;
	//		break;
	//	}

	//	for (int j = 0; j < FFT_RESULT_SIZE; j++)
	//		output[j] = scaleValue(input[j], baseMin, baseMax, newMin, newMax);
	//	block->_bProcModule5 = TRUE;
	//}
}

double CModule5::scaleValue(double valueIn, double baseMin, double baseMax, double NewlimitMin, double NewlimitMax)
{
	return((NewlimitMax - NewlimitMin) * (valueIn - baseMin) / (baseMax - baseMin)) + NewlimitMin;
}

