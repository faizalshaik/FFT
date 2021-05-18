#pragma once
#include <Windows.h>
#include "Enums.h"
#include <math.h>

#define M_PI       3.14159265358979323846   // pi
#define M_E        2.71828182845904523536   // e
#define PI2 ((double)(2.0 * M_PI))
#define dBReferencePoint_dBSPL  ((double)0.00002)
#define dBReferencePoint_dBFS  ((double)1.6425)
#define dBReferencePoint_dBv  ((double)1.0)
#define dBReferencePoint_dBu  ((double)0.775)



typedef struct _filter_setting
{
    int _windowSize = FFTSizeConstants::FFTs1024;
    int _windowType = FFTWindowConstants::Hanning;
}FILTER_SETTING;

typedef struct _constants {
    //double _adcVdd = 3.3;                                              // ADC Power Supply
    //double _adcHafVdd = _adcVdd / 2;                                    // ADC Voltage Reference half VDD
    //double _adcDaynamicRange = (6.021 * _adcResoluationBits + (_adcVdd / 2)) * -1; // DR= 6.021*N + 1.763 dB
    //double _adcLSB = _adcVdd / _adcResoluation;                          //
    //double _adcDcOffsetInVolts = _adcVdd / 2;
    //double _adcDcOffsetInDecimal = _adcResoluation / 2;
    //double _dbRefPoint = 1.0;
    //double _fftFreqResolution = _adcSamplingFrequencey / _adcWindowSize;
    //double _UppderVoltageThreshold = 0.0195;
    //double _LowerVoltageThreshold = -0.0195;
    //int    _adcResoluationBits = 12;                                  // 12 bits ADC
    //int    _adcResoluation = pow(2, _adcResoluationBits) - 1;          // 4095
    //int    _adcSamplingFrequencey = 120000;                           // ADC Sampling Frequence of ADC
    //int    _adcWindowSize = 1024;                                     // Number of Data Points per sample
    //int    _fftResultSize = _adcWindowSize / 2;
    double _adcVdd;                                              // ADC Power Supply
    double _adcHafVdd;                                    // ADC Voltage Reference half VDD
    double _adcDaynamicRange; // DR= 6.021*N + 1.763 dB
    double _adcLSB;                          //
    double _adcDcOffsetInVolts;
    double _adcDcOffsetInDecimal;
    double _dbRefPoint;
    double _fftFreqResolution;
    double _UppderVoltageThreshold;
    double _LowerVoltageThreshold;
    int    _adcResoluationBits;                                  // 12 bits ADC
    int    _adcResoluation;          // 4095
    int    _adcSamplingFrequencey;                           // ADC Sampling Frequence of ADC
    int    _adcWindowSize;                                     // Number of Data Points per sample
    int    _fftResultSize;

}CONSTANTS;

class Settings
{
public:
    Settings() {
        //init constants
        initConstants(0.0195, -0.0195, 1.0, 120000, 3.3);

        //init filters
        for (int i = 0; i < channel_max_count; i++)
        {
            _filterSettings[i]._windowSize = FFTSizeConstants::FFTs1024;
            _filterSettings[i]._windowType = FFTWindowConstants::Hanning;
        }
        //init gains
        for (int i = 0; i < channel_max_count; i++)
        {
            _gains[i] = 1.0;
            _channels[i] = 1;
        }

        //init functions
        for (int i = 0; i < calc_function_max_count; i++)
            _functions[i] = 0;
        _functions[calc_functions::get_decibels] = 1;

        //init channels
        for (int i = 0; i < channel_max_count; i++)
            _channels[i] = 0;
        _channels[0] = 1;        
    };
    ~Settings() {};

    FILTER_SETTING  _filterSettings[channel_max_count];
    double          _gains[channel_max_count];
    CONSTANTS       _constants;
    byte            _channels[channel_max_count];
    byte            _functions[calc_function_max_count];

    void            initConstants(double uppderVoltageThreshold,
        double lowerVoltageThreshold, double dbRefPoint,
        int adcSamplingFrequencey, double adcVdd)
    {
        _constants._adcVdd = adcVdd;
        _constants._adcSamplingFrequencey = adcSamplingFrequencey;
        _constants._adcHafVdd = adcVdd / 2;
        _constants._adcResoluationBits = 12;
        _constants._adcDaynamicRange = (6.021 * _constants._adcResoluationBits + (adcVdd / 2)) * -1;
        _constants._adcResoluation = pow(2, _constants._adcResoluationBits) - 1;
        _constants._adcLSB = adcVdd / _constants._adcResoluation;
        _constants._adcDcOffsetInVolts = adcVdd / 2;
        _constants._adcDcOffsetInDecimal = _constants._adcResoluation / 2;
        _constants._dbRefPoint = dbRefPoint;
        _constants._adcWindowSize = 1024;
        _constants._fftResultSize = _constants._adcWindowSize / 2;
        _constants._fftFreqResolution = adcSamplingFrequencey / _constants._adcWindowSize;
        _constants._UppderVoltageThreshold = uppderVoltageThreshold;
        _constants._LowerVoltageThreshold = lowerVoltageThreshold;
    }

    int   outChannelCount(int inputChannelCount)
    {
        int outChns = 0;
        int nChns = min(inputChannelCount, channel_max_count);
        for (int i = 0; i < nChns; i++)
        {
            if (_channels[i] == 1)
                outChns++;
        }
        return outChns;
    }

    int   outFunctionsCount()
    {
        int outFns = 0;
        for (int i = 0; i <= calc_functions::get_noise_power; i++)
        {
            if (_functions[i] == 1)
                outFns++;
        }
        return outFns;
    }


    DWORD calcOutBlockSize(int inputChannelCount)
    {
        int outChns = outChannelCount(inputChannelCount);
        DWORD blockSize = OUTPUT_BLOCK_HEADER + 
            outChns * sizeof(double) * 3 + 
            outChns * outFunctionsCount() * FFT_RESULT_SIZE * sizeof(float);
        return blockSize;
    }
};


typedef struct _outputFileHeader {
    DWORD   _majorVer : 16,
        _minorVer : 16;
    FILTER_SETTING _filterSettings[channel_max_count];
    double         _gains[channel_max_count];
    CONSTANTS      _constants;
    byte           _functions[calc_function_max_count];
    byte           _channels[channel_max_count];
    DWORD          _blockSize;
    DWORD          _blockCount;
}OutputFileHeader;
