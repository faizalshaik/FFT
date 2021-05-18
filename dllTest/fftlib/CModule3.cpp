#include "CModule3.h"
#include <math.h>
#include "CGlobal.h"

DWORD CModule3::threadProc(LPVOID param)
{
	int processed = 0;
	CGlobal* pGlobal = CGlobal::instance();
	CModule3* pThis = (CModule3*)param;

	while (pThis->_bRuning)
	{
		if (pGlobal->_totalBlockCount > 0 && processed >= pGlobal->_totalBlockCount)
			break;

		//stuff to save memory
		if (pGlobal->_queue3.size() >= pGlobal->_nThreads * 4)
		{
			Sleep(100);
			continue;
		}

		CBlock* block = pGlobal->_queue2.take();
		if (block == NULL)
		{
			Sleep(100);
			continue;
		}

		pThis->applyFilter(block);
		pThis->applyGain(block);
		pGlobal->_queue3.append(block);
		processed++;
	}
	pThis->_bRuning = FALSE;
	return 0;
}

void CModule3::applyFilter(CBlock* block)
{
	Settings* pSetting = &CGlobal::instance()->_settings;
	for (int iCh = 0; iCh < block->_nChannels; iCh++)
	{
		for (int i = 0; i < SAMPLE_DATA_SIZE; i++)
		{
			double dVal = applyWindow(i, pSetting->_filterSettings[iCh]._windowSize, pSetting->_filterSettings[iCh]._windowType);
			block->_samples[iCh][i] = round(block->_samples[iCh][i] * dVal * 1000.0) / 1000.0;
		}
	}
}

void CModule3::applyGain(CBlock* block)
{
	Settings* pSetting = &CGlobal::instance()->_settings;
	for (int iCh = 0; iCh < block->_nChannels; iCh++)
	{
		for (int i = 0; i < SAMPLE_DATA_SIZE; i++)
		{
			block->_samples[iCh][i] = block->_samples[iCh][i] * pSetting->_gains[iCh];
		}
	}
}

double CModule3::applyWindow(int i, int windowSize, int windowType)
{
	double w = (double)(windowSize - 1);
	double ii = (double)i;
	switch (windowType)
	{
	case FFTWindowConstants::None:
		return 1.0;
		break;
	case FFTWindowConstants::Triangle:
		return 1 - abs(1 - ((2 * ii) / w));
		break;
	case FFTWindowConstants::Hanning:
		return(0.5 * (1 - cos(PI2 * ii / w)));
		break;
	case FFTWindowConstants::Hamming:
		return 0.54 - 0.46 * cos(PI2 * ii / w);
		break;
	case FFTWindowConstants::Welch:
		return 1 - (ii - 0.5 * (w - 1)) / pow(0.5 * (w + 1), 2.0);
		break;
	case FFTWindowConstants::Gaussian:
		return pow(M_E, (pow(-6.25 * M_PI * ii, 2)/pow(w ,2)));
		break;
	case FFTWindowConstants::Blackman:
		return 0.42 - 0.5 * cos(PI2 * ii / w) + 0.08 * cos(2 * PI2 * ii / w);
		break;
	case FFTWindowConstants::Parzen:
		return 1 - abs((ii - 0.5 * w) / (0.5 * (w + 1)));
		break;
	case FFTWindowConstants::Bartlett:
		return 1 - abs(i) / w;
		break;
	case FFTWindowConstants::Connes:
		return pow((1 - pow(ii, 2) / pow(w, 2)), 2);
	case FFTWindowConstants::KaiserBessel:
		if (i >= 0 && ii <= w / 2)
			return bessel((w / 2) * pow(sqrt(1 - 2 * ii / w), 2)) / bessel(w / 2);
		else
			return 0;
		break;
	case FFTWindowConstants::BlackmanHarris:
		return 0.36 - 0.49 * cos(M_PI * i / w) + 0.14 * cos(PI2 * i / w) - 0.01 * cos(3 * M_PI * i / w);
		break;
	}
	return 0;
}

int CModule3::fact(int x)
{
	int n = 1;
	for (int i = 1; i <= x; i++)
	{
		n*= i;
	}
	return n;
}

double CModule3::bessel(double x)
{
	double r = 1;
	for (int i = 0; i < 3; i++)
	{
		r += pow((x / 2) , (2 * i)) / pow(fact(i), 2);
	}
	return r;
}
