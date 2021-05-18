#pragma once
typedef enum _FFTWindowConstants
{
    None = 0,            //No windowing Is applied
    Triangle = 1,        //Apply a basic triangle window
    Hanning = 2,         //Apply the Hanning window
    Hamming = 3,         //Apply the Hamming window
    Welch = 4,           //Apply the Welch window
    Gaussian = 5,        //Apply the Gaussian window
    Blackman = 6,        //Apply the Blackman window
    Parzen = 7,          //Apply the Parzen window
    Bartlett = 8,        //Apply the Bartlett window
    Connes = 9,          //Apply the Cones window
    KaiserBessel = 10,   //Apply the KaiserBessel window
    BlackmanHarris = 11, //Apply the BlackmanHarris window
}FFTWindowConstants;

typedef enum _FFTSizeConstants {
    FFTs128 = 128,//128 bands
    FFTs256 = 256,//256 bands
    FFTs512 = 512,//512 bands
    FFTs1024 = 1024,//1024 bands
    FFTs2048 = 2048,//2048 bands
    FFTs4096 = 4096,//4096 bands
}FFTSizeConstants;

typedef enum _channels {
    channel_h2 = 0,
    channel_h1,
    channel_l2,
    channel_l1,
    channel_reserved1,
    channel_reserved2,
    channel_reserved3,
    channel_reserved4,
    channel_max_count
}channels;

typedef enum _calc_functions {
    magnitude = 0,
    magnitude_normalized,
    get_intensity,
    get_amplitude,
    get_decibels,
    get_decible10,
    get_power_spectrum,
    get_noise_power,
    calc_reserved_0,
    calc_reserved_1,
    calc_reserved_2,
    calc_reserved_3,
    calc_function_max_count
}calc_functions;


#define FFT_RESULT_SIZE  512
#define SAMPLE_DATA_SIZE 1024
#define INPUT_SAMPLE_DATA_SIZE  (SAMPLE_DATA_SIZE * 3 / 2)

#define RTD_RES		((float)1000.0)
#define RTD_REF		((float)4300.0)
#define ADC_TORES   ((float)32768.0)
#define RTD_A      ((float) 3.9083e-3) 
#define RTD_B      ((float) -5.775e-7)   
#define RTD_C      ((float) -418301e-12)

#define OUTPUT_BLOCK_HEADER  16



#define MAJOR_VER 1
#define MINOR_VER 1