#include "CModule4.h"
#include <math.h>
#include "CGlobal.h"
#include <tchar.h>

void  CModule4::onFinishFFT(CBlock4* block)
{
    CGlobal* pGlobal = CGlobal::instance();
    EnterCriticalSection(&_lock);
    if (block->_idx == _lastPushed + 1)
    {
        pGlobal->_queue4.append(block);
        _lastPushed++;
    }
    else
    {
        //insert 
        int idx = -1;
        if (_orderingList.size() == 0)
            idx = 0;
        else if (block->_idx < _orderingList[0]->_idx)
            idx = 0;
        else if (block->_idx > _orderingList[_orderingList.size()-1]->_idx)
            idx = _orderingList.size();
        else {
            for (int i = 0; i < _orderingList.size() - 1; i++)
            {
                if (block->_idx > _orderingList[i]->_idx &&
                    block->_idx < _orderingList[i + 1]->_idx)
                {
                    idx = i + 1;
                    break;
                }
            }
        }
        if (idx < 0)
        {
            //this is error
        }
        else
        {
            _orderingList.insert(_orderingList.begin() + idx, block);
        }
    }
    
    //check can push global queue
    while (_orderingList.size() > 0)
    {
        if (_orderingList[0]->_idx == _lastPushed + 1)
        {
            pGlobal->_queue4.append(_orderingList[0]);
            _orderingList.erase(_orderingList.begin());
            _lastPushed++;
        }
        else
            break;
    }
    LeaveCriticalSection(&_lock);
}

DWORD CModule4::threadProc(LPVOID param)
{
	int processed = 0, left;
    DWORD dwWritten;
	CGlobal* pGlobal = CGlobal::instance();
	CModule4* pThis = (CModule4*)param;
    std::vector<CBlock4*> results;
    OutputFileHeader  fileHeader;

    while (pThis->_bRuning && pGlobal->_totalBlockCount == 0)
    {
        Sleep(100);
    }

    HANDLE hFile = CreateFile(pGlobal->_outputFile, GENERIC_WRITE, FILE_SHARE_WRITE, NULL, CREATE_ALWAYS, 0, NULL);
    if (hFile == INVALID_HANDLE_VALUE)
    {
        pThis->_bRuning = FALSE;
        pGlobal->onError(-2);
        return 1;
    }

    //write header
    fileHeader._majorVer = MAJOR_VER;
    fileHeader._minorVer = MINOR_VER;
    memcpy(fileHeader._filterSettings, pGlobal->_settings._filterSettings,
        sizeof(FILTER_SETTING) * channel_max_count);
    memcpy(fileHeader._gains, pGlobal->_settings._gains,
        sizeof(double) * channel_max_count);
    memcpy(&fileHeader._constants, &pGlobal->_settings._constants, sizeof(CONSTANTS));
    memcpy(&fileHeader._functions, &pGlobal->_settings._functions, sizeof(byte) * calc_function_max_count);
    memcpy(&fileHeader._channels, &pGlobal->_settings._channels, sizeof(byte) * channel_max_count);
    fileHeader._blockSize = pGlobal->_settings.calcOutBlockSize(pGlobal->_inputChannelCount);
    fileHeader._blockCount = pGlobal->_totalBlockCount;
    int nSz = sizeof(OutputFileHeader);
    WriteFile(hFile, &fileHeader, sizeof(OutputFileHeader), &dwWritten, NULL);


	while (pThis->_bRuning)
	{
        if (pGlobal->_totalBlockCount > 0 && processed >= pGlobal->_totalBlockCount)
            break;

        left = pGlobal->_totalBlockCount - processed;
        if (left >= pGlobal->_nThreads && pGlobal->_queue4.size() < pGlobal->_nThreads)
        {
            Sleep(10);
            continue;
        }

        results.clear();
        pGlobal->_queue4.takeAll(results);

        //save results
        for (int i = 0; i < results.size(); i++)
        {
            results[i]->write(hFile);
            delete results[i];
        }
        processed += results.size();
        pGlobal->_processedBlockCount = processed;
	}
    CloseHandle(hFile);
    pThis->_bRuning = FALSE;
    pGlobal->onFinishedModule(4);
    return 0;
}

DWORD CModule4::subThreadProc(LPVOID param)
{
    CGlobal* pGlobal = CGlobal::instance();
    CModule4* pThis = (CModule4*)param;

    while (pThis->_bRuning)
    {
        //stuff to save memory
        if (pGlobal->_queue4.size() >= pGlobal->_nThreads * 2)
        {
            Sleep(100);
            continue;
        }

        CBlock* block = pGlobal->_queue3.take();
        if (block == NULL)
        {
            Sleep(100);
            continue;
        }
        CBlock4* newBlock = pThis->DoFFT(block);
        while (newBlock == NULL)
        {
            Sleep(10);
            newBlock = pThis->DoFFT(block);
        }
        delete block;

        pThis->onFinishFFT(newBlock);
        Sleep(10);
    }
    return 0;
}


void CModule4::fft(CArray& x)
{
    // DFT
    unsigned int N = x.size(), k = N, n;
    double thetaT = 3.14159265358979323846264338328L / N;
    Complex phiT = Complex(cos(thetaT), -sin(thetaT)), T;
    while (k > 1)
    {
        n = k;
        k >>= 1;
        phiT = phiT * phiT;
        T = 1.0L;
        for (unsigned int l = 0; l < k; l++)
        {
            for (unsigned int a = l; a < N; a += n)
            {
                unsigned int b = a + k;
                Complex t = x[a] - x[b];
                x[a] += x[b];
                x[b] = t * T;
            }
            T *= phiT;
        }
    }
    // Decimate
    unsigned int m = (unsigned int)log2(N);
    for (unsigned int a = 0; a < N; a++)
    {
        unsigned int b = a;
        // Reverse bits
        b = (((b & 0xaaaaaaaa) >> 1) | ((b & 0x55555555) << 1));
        b = (((b & 0xcccccccc) >> 2) | ((b & 0x33333333) << 2));
        b = (((b & 0xf0f0f0f0) >> 4) | ((b & 0x0f0f0f0f) << 4));
        b = (((b & 0xff00ff00) >> 8) | ((b & 0x00ff00ff) << 8));
        b = ((b >> 16) | (b << 16)) >> (32 - m);
        if (b > a)
        {
            Complex t = x[a];
            x[a] = x[b];
            x[b] = t;
        }
    }
}
void CModule4::fft_proc(double* samples, double* reals, double* imgs)
{
    Complex data[SAMPLE_DATA_SIZE];
    for (int i = 0; i < SAMPLE_DATA_SIZE; i++)
        data[i] = samples[i];
    CArray dataArry(data, SAMPLE_DATA_SIZE);
    fft(dataArry);
    for (int i = 0; i < SAMPLE_DATA_SIZE; i++)
    {
        reals[i] = dataArry[i]._Val[0];
        imgs[i] = dataArry[i]._Val[1];
    }
}


CBlock4* CModule4::DoFFT(CBlock* block)
{
    //CBlock4* newBlock = new CBlock4();
    CBlock4* newBlock = new CBlock4();
    if (newBlock == NULL)
        return newBlock;

    newBlock->_idx = block->_idx;
    memcpy(newBlock->_timeStmapArray, block->_timeStmapArray, 4);
    newBlock->_adcVolt = (float)block->_adcVolt;
    newBlock->_temperature = (float)block->_temperature;
    newBlock->_nChannels = block->_nChannels;
    memcpy(newBlock->_voltMins, block->_voltMins, sizeof(double) * channel_max_count);
    memcpy(newBlock->_voltMaxs, block->_voltMaxs, sizeof(double) * channel_max_count);
    memcpy(newBlock->_voltAvgs, block->_voltAvgs, sizeof(double) * channel_max_count);

    CGlobal* pGlobal = CGlobal::instance();
    double RealFFT[1024], ImagFFT[1024];

    int nMaxChannels = min(block->_nChannels, channel_max_count);
    for (int iCh = 0; iCh < nMaxChannels; iCh++)
    {
        if (pGlobal->_settings._channels[iCh] == 0) 
            continue;
        fft_proc(block->_samples[iCh], RealFFT, ImagFFT);
        for (int iFn = 0; iFn < calc_function_max_count; iFn++)
        {
            if (pGlobal->_settings._functions[iFn] == 0)
                continue;
            float* data = new float[FFT_RESULT_SIZE];
            switch (iFn)
            {
            case calc_functions::magnitude:
                for (int i = 0; i < FFT_RESULT_SIZE; i++)
                    data[i] = (float)getMagnitude(RealFFT[i+1], ImagFFT[i+1]);
                break;
            case calc_functions::magnitude_normalized:
                for (int i = 0; i < FFT_RESULT_SIZE; i++)
                    data[i] = (float)getMagnitudeNormalized(RealFFT[i+1], ImagFFT[i+1]);
                break;
            case calc_functions::get_intensity:
                for (int i = 1; i < FFT_RESULT_SIZE; i++)
                    data[i] = (float)getFrequencyIntensity(RealFFT[i+1], ImagFFT[i+1]);
                break;
            case calc_functions::get_amplitude:
                for (int i = 0; i < FFT_RESULT_SIZE; i++)
                    data[i] = (float)getAmplitude(i, RealFFT[i+1], ImagFFT[i+1]);
                break;
            case calc_functions::get_decibels:
                for (int i = 0; i < FFT_RESULT_SIZE; i++)
                    data[i] = (float)getDecibels(RealFFT[i+1], ImagFFT[i+1]);
                break;
            case calc_functions::get_decible10:
                for (int i = 0; i < FFT_RESULT_SIZE; i++)
                    data[i] = (float)getDecible10(RealFFT[i+1], ImagFFT[i+1]);
                break;
            case calc_functions::get_power_spectrum:
                for (int i = 0; i < FFT_RESULT_SIZE; i++)
                    data[i] = (float)getPowerSpectrum(RealFFT[i+1], ImagFFT[i+1]);                
                break;
            case calc_functions::get_noise_power:
                for (int i = 0; i < FFT_RESULT_SIZE; i++)
                    data[i] = (float)getNoisePower(RealFFT[i+1], ImagFFT[i+1]);
                break;
            }
            newBlock->addData(data);
        }

    }
    return newBlock;
}

double CModule4::getFrequencyIntensity(double re, double im)
{
    return sqrt((re * re) + (im * im));
}
double CModule4::getMagnitude_sqrd(double re, double im)
{
    return(re * re + im * im);
}
double CModule4::getMagnitudeNormalized(double re, double im)
{
    Settings* pSetting = &CGlobal::instance()->_settings;
    return (2 * sqrt(re * re + im * im) / pSetting->_constants._adcWindowSize); // normalized  magnitude
}

double CModule4::getAmplitude(int BinIndex, double re, double im)
{
    Settings* pSetting = &CGlobal::instance()->_settings;
    double magn = abs(getMagnitude(re, im));
    double res = (20 * log10(magn)) / pSetting->_constants._adcWindowSize;
    if (res == -INFINITY || res == INFINITY)
        res = pSetting->_constants._adcDaynamicRange;
    return res;
}

double CModule4::index_to_frequency(int nIndex)
{
    Settings* pSetting = &CGlobal::instance()->_settings;
    int nBaseFreq = pSetting->_constants._adcSamplingFrequencey / pSetting->_constants._adcWindowSize;
    double Res = 0;
    if (nIndex >= pSetting->_constants._adcWindowSize)
        Res = 0.0;
    else if (nIndex <= pSetting->_constants._adcWindowSize / 2)
        Res = round(nIndex * nBaseFreq);
    else
        Res = round((pSetting->_constants._adcWindowSize - nIndex) * nBaseFreq);
    return Res;
}

double CModule4::getDecibels(double re, double im)
{
    Settings* pSetting = &CGlobal::instance()->_settings;
    double magn = 0;
    if (re == 0 && im == 0)
    {
        magn = abs((2 * sqrt(re * re + im * im) / 1024));
        if (magn <= 0)
            magn = 0.0000316227619087529;           //''''' 1.0E-12;   // To prevent a problem In dB()
    }
    else
        magn = abs((2 * sqrt(re * re + im * im) / 1024));
    magn = abs(magn);

    if (magn <= 0)
        magn = 0.0000316227619087529;

    double res = (20 * log10(magn / pSetting->_constants._dbRefPoint));                //''''' convert normalized  magnitude to dB value
    if (res == -INFINITY || res == INFINITY)
        res = pSetting->_constants._adcDaynamicRange;
    return res;
}

double CModule4::getMagnitude(double Real, double Imaginary)
{
    return sqrt(Real * Real + Imaginary * Imaginary);
}
double CModule4::getDecible10(double re, double im)
{
    return 10.0 * log10((getMagnitude_sqrd(re, im))) / 2;
}

double CModule4::squaredMagnitude(double Real, double Imaginary)
{
    double mag = getMagnitude(Real, Imaginary);
    return mag * mag;
}


double CModule4::getNoisePower(double re, double im)
{
    Settings* pSetting = &CGlobal::instance()->_settings;
    double magn = 0;
    if (re == 0 && im == 0)
    {
        magn = abs((2 * sqrt(re * re + im * im) / 1024));
        if (magn <= 0)
            magn = 0.0000316227619087529;           //''''' 1.0E-12;   // To prevent a problem In dB()
    }
    else
        magn = abs((2 * sqrt(re * re + im * im) / 1024));
    
    magn = abs(magn);
    if (magn <= 0)
        magn = 0.0000316227619087529;
    double res = (20 * log10(magn / pSetting->_constants._dbRefPoint));
    //res = Math.Sqrt(res * res)
    return res;
}


double CModule4::getPowerSpectrum(double re, double im)
{
    double magn = sqrt(re * re + im * im);
    return magn * 2 / 1024;
}

