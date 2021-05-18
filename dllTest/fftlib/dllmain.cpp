#include "CGlobal.h"

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}


_declspec(dllexport) void __stdcall ConfigureFilter(int chNo, int winType, int winSize)
{
    CGlobal::instance()->configureFilter(chNo, winType, winSize);
}
_declspec(dllexport) void __stdcall SetGains(LPSAFEARRAY* gains)
{
    CGlobal* pGlobal = CGlobal::instance();
    double* gainss = (double*)((*gains)->pvData);
    int   count = (*gains)->cDims;
    int nChns = min(count, channel_max_count);
    for (int i = 0; i < nChns; i++)
        pGlobal->_settings._gains[i] = gainss[i];
}

_declspec(dllexport) bool __stdcall Start(LPCTSTR strFile, int channelCount, LPCTSTR strOutFile, int nThreads)
{
    return CGlobal::instance()->start(strFile, channelCount, strOutFile, nThreads);
}

_declspec(dllexport) void __stdcall Pause()
{
    CGlobal::instance()->pause();
}

_declspec(dllexport) bool __stdcall GetStats(int& errorCode, int& total, int& processed)
{
    return CGlobal::instance()->getStats(errorCode, total, processed);
}

_declspec(dllexport) double __stdcall GetFrequencyIntensity(double re, double im)
{
    return CModule4::getFrequencyIntensity(re, im);
}
_declspec(dllexport) double __stdcall GetMagnitudeSqrd(double re, double im)
{
    return CModule4::getMagnitude_sqrd(re, im);
}
_declspec(dllexport) double __stdcall GetMagnitudeNormalized(double re, double im)
{
    return CModule4::getMagnitudeNormalized(re, im);
}
_declspec(dllexport) double __stdcall GetAmplitude(int BinIndex, double re, double im)
{
    return CModule4::getAmplitude(BinIndex, re, im);
}

_declspec(dllexport) double __stdcall IndexToFrequency(int nIndex)
{
    return CModule4::index_to_frequency(nIndex);    
}

_declspec(dllexport) double __stdcall GetDecibels(double re, double im)
{
    return CModule4::getDecibels(re, im);
}

_declspec(dllexport) double __stdcall GetMagnitude(double re, double im)
{
    return CModule4::getMagnitude(re, im);
}

_declspec(dllexport) double __stdcall GetDecible10(double re, double im)
{
    return CModule4::getDecible10(re, im);
}
_declspec(dllexport) double __stdcall SquaredMagnitude(double re, double im)
{
    return CModule4::squaredMagnitude(re, im);
}
_declspec(dllexport) double __stdcall GetNoisePower(double re, double im)
{
    return CModule4::getNoisePower(re, im);
}
_declspec(dllexport) double __stdcall GetPowerSpectrum(double re, double im)
{
    return CModule4::getPowerSpectrum(re, im);
}

_declspec(dllexport) bool __stdcall CalcFFT(LPSAFEARRAY* samples, LPSAFEARRAY* decbls)
{
    double real[SAMPLE_DATA_SIZE], imag[SAMPLE_DATA_SIZE];
    double* sampless = (double* )((*samples)->pvData);
    double* decblss = (double*)((*decbls)->pvData);

    CModule4::fft_proc(sampless, real, imag);
    for (int i = 0; i < FFT_RESULT_SIZE; i++)
        decblss[i] = CModule4::getDecibels(real[i], imag[i]);

    return true;
}

_declspec(dllexport) bool __stdcall SetChannels(LPSAFEARRAY* channels)
{
    CGlobal* pGlobal = CGlobal::instance();
    byte* channelss = (byte*)((*channels)->pvData);
    int   count = (*channels)->rgsabound->cElements;
    int nChns = min(count, channel_max_count);
    for (int i = 0; i < channel_max_count; i++)
        pGlobal->_settings._channels[i] = 0;

    for (int i = 0; i < nChns; i++)
        pGlobal->_settings._channels[i] = channelss[i];
    return true;
}

_declspec(dllexport) bool __stdcall SetFunctions(LPSAFEARRAY* functions)
{
    CGlobal* pGlobal = CGlobal::instance();
    byte* functionss = (byte*)((*functions)->pvData);
    for (int i = 0; i < calc_function_max_count; i++)
        pGlobal->_settings._functions[i] = 0;

    int   count = (*functions)->rgsabound->cElements;
    int nFns = min(count, calc_function_max_count);
    for (int i = 0; i < nFns; i++)
        pGlobal->_settings._functions[i] = functionss[i];
    return true;
}

_declspec(dllexport) bool __stdcall SetConstants(double uppderVoltageThreshold, 
    double lowerVoltageThreshold, double dbRefPoint, 
    int adcSamplingFrequencey, double adcVdd)
{
    CGlobal* pGlobal = CGlobal::instance();
    pGlobal->_settings.initConstants(uppderVoltageThreshold,
        lowerVoltageThreshold, dbRefPoint, adcSamplingFrequencey, adcVdd);
    return true;
}

_declspec(dllexport) bool __stdcall SetCalcRange(LPSAFEARRAY* ranges)
{
    CGlobal* pGlobal = CGlobal::instance();
    int* rangess = (int*)((*ranges)->pvData);
    int   count = (*ranges)->rgsabound->cElements;

    pGlobal->_ranges.clear();
    POINT rng;
    for (int i = 0; i < count / 2; i++)
    {
        rng.x = rangess[i * 2];
        rng.y = rangess[i * 2+1];
        if(rng.x >=0 && rng.y >=0 && rng.y >= rng.x)
            pGlobal->_ranges.push_back(rng);
    }
    return true;
}







