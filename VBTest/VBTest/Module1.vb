Public Module Module1

    Public Enum dBReferencePoint
        dBSPL = 0.00002
        dBFS = 1.6425
        dBv = 1.0
        dBu = 0.775
    End Enum

    Public adcVdd As Single = 3.3                                               ' ADC Power Supply
    Public adcHafVdd As Single = adcVdd / 2                                     ' ADC Voltage Reference half VDD
    Public adcResoluationBits As Integer = 12                                   ' 12 bits ADC
    Public adcResoluation As Integer = Math.Pow(2, adcResoluationBits) - 1      ' 4095
    Public adcDaynamicRange = (6.021 * adcResoluationBits + (adcVdd / 2)) * -1  ' DR= 6.021*N + 1.763 dB
    Public adcLSB As Single = adcVdd / adcResoluation
    Public adcSamplingFrequencey As Integer = 120000                            ' ADC Sampling Frequence of ADC
    Public adcWindowSize As Integer = 1024                                      ' Number of Data Points per sample
    Public adcDcOffsetInVolts As Single = adcVdd / 2
    Public adcDcOffsetInDecimal As Single = adcResoluation / 2

    Public dbRefPoint As dBReferencePoint = dBReferencePoint.dBv
    Public fftResultSize As Integer = adcWindowSize / 2
    Public fftFreqResolution As Double = adcSamplingFrequencey / adcWindowSize


    Public UppderVoltageThreshold As Single = 0.0195
    Public LowerVoltageThreshold As Single = -0.0195


    Public Function GetDecibels(ByVal re As Double, ByVal im As Double) As Double

        Dim magn As Double = 0

        If re = 0 And im = 0 Then
            magn = Math.Abs((2 * Math.Sqrt(re * re + im * im) / 1024))
            If magn <= 0 Then magn = 0.0000316227619087529           ''''' 1.0E-12;   // To prevent a problem In dB()
        Else
            magn = Math.Abs((2 * Math.Sqrt(re * re + im * im) / 1024))
        End If
        magn = Math.Abs(magn)
        If magn <= 0 Then magn = 0.0000316227619087529
        ' Dim x = (Math.Sqrt(1) / 1024)
        '  dbRefPoint = dBReferencePoint.dBv
        Dim res As Single = 20 * Math.Log10(magn) '/ dbRefPoint               ''''' convert normalized  magnitude to dB value
        If res = Double.NegativeInfinity Or res = Double.PositiveInfinity Then res = adcDaynamicRange

        Return res

    End Function


End Module



'End Class

