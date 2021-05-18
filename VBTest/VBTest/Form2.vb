Imports System.IO
Imports MathNet.Numerics.Transformations

Public Class Form2



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim headerbytes(11) As Byte

        'Header ' 5 bytes - Header          ( Skip - not required )
        headerbytes(0) = 0
        headerbytes(1) = 0
        headerbytes(2) = 4
        headerbytes(3) = 0
        headerbytes(4) = 0

        'TimeStamp ' 3 bytes - timeStampIndex  ( Join three bytes to make long value) then add to TimeStampArray
        headerbytes(5) = 1
        headerbytes(6) = 2
        headerbytes(7) = 3



        'adcVolt  2 bytes - adcVolt         ( Join Two bytes to make doulble value and multiple by 0.000806  ) then add to adcVoltArray
        headerbytes(8) = 4
        headerbytes(9) = 5

        'rdd   2 bytes - rtd             ( Join Two bytes to make integer value and calcualte Temperature) then add to TemperatureArray
        headerbytes(10) = 6
        headerbytes(11) = 7







        Dim ChnH2_Bytes() As Byte = MakeChanneData(20000)
        Dim ChnH1_Bytes() As Byte = MakeChanneData(10000)

        Dim ChnL2_Bytes() As Byte = MakeChanneData(2000)
        Dim ChnL1_Bytes() As Byte = MakeChanneData(1000)



        Dim outputFilename As String = "d:\ToolDump.bin"
        If File.Exists(outputFilename) Then File.Delete(outputFilename)

        Dim binWriter As BinaryWriter
        binWriter = New BinaryWriter(File.Open(outputFilename, FileMode.Create))




        Dim ts As Byte()

        For j As Long = 0 To 174421

            ts = BitConverter.GetBytes(j)
            headerbytes(5) = ts(0)
            headerbytes(6) = ts(1)
            headerbytes(7) = ts(2)


            For Each value As Byte In headerbytes
                binWriter.Write(value)
            Next

            For Each value As Byte In ChnH2_Bytes
                binWriter.Write(value)
            Next

            For Each value As Byte In ChnH1_Bytes
                binWriter.Write(value)
            Next

            For Each value As Byte In ChnL2_Bytes
                binWriter.Write(value)
            Next

            For Each value As Byte In ChnL1_Bytes
                binWriter.Write(value)
            Next

        Next


        binWriter.Close()

        MsgBox("Done")

    End Sub


    'test restuls
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim sb As New System.Text.StringBuilder
        Dim binaryArraySize = 1024 / 2 * 3
        Dim bytesReadArray(binaryArraySize - 1) As Byte
        Dim samples(1023) As Double
        Dim ind = -1


        Dim reader As New BinaryReader(File.Open("d:\new binary file.bin", FileMode.Open))


        reader.ReadBytes(12)
        bytesReadArray = reader.ReadBytes(binaryArraySize)

        'here i read the new file bytes


        For i As Integer = 0 To 511
            Dim b3 As Byte = bytesReadArray(i * 3 + 2)
            Dim num1 As Integer = bytesReadArray(i * 3) + (b3 And &HF) * 256
            Dim num2 As Integer = bytesReadArray(i * 3 + 1) + (b3 >> 4) * 256

            ind += 1
            samples(ind) = num1
            ind += 1
            samples(ind) = num2

        Next


        'this is the signal i read from the file
        sb.Clear()
        For i As Integer = 0 To 1023
            sb.Append(samples(i) & vbCrLf)
        Next
        Clipboard.Clear()
        Clipboard.SetText(sb.ToString)
        Clipboard.Clear()


        'For i As Integer = 0 To bytesReadArray.Length - 3 Step 3
        '    Dim byte1 As Byte = bytesReadArray(i)
        '    Dim byte2 As Byte = bytesReadArray(i + 1)
        '    Dim byte3 As Byte = bytesReadArray(i + 2)

        '    Dim val1 = (byte1) + (byte3 And &HF) * 256
        '    Dim val2 = (byte2) + ((byte3 >> 4) * 256)


        '    ind += 1
        '    samples(ind) = val1
        '    ind += 1
        '    samples(ind) = val2
        'Next






        Dim LSB As Double = 3.3 / CDbl(CLng("&HFFF"))
        'here i calc volt for :samples()'' this is calc volrage part
        For i = 0 To 1023
            Dim v = (samples(i) * LSB) - (3.3 / 2)
            If (v > 0 And v < UppderVoltageThreshold) Or
                (v < 0 And v > LowerVoltageThreshold) Then
                v = 0
            End If
            samples(i) = v '/ Math.Round(v * 1000.0, 0) / 1000.0
        Next



        'here i apply window here i made it, k, plase calcVolt part

        For i = 0 To 1023
            samples(i) = Math.Round(samples(i) * (0.5 * (1 - Math.Cos((Math.PI * 2) * i / 1023))), 3)
        Next


        'here i make fft calc using dotnt lib.
        Dim n As Integer = 1024
        Dim FFTResultLength As Integer = n / 2
        Dim Decbl(n - 1) As Double
        Dim Freq(n - 1) As Double
        Dim RealFFT(n - 1), ImagFFT(n - 1) As Double
        Dim Real, Imag As Double

        'call fft function passing inputpoints and return res RealFFT(), ImagFFT()
        Dim fft As New RealFourierTransformation(TransformationConvention.NoScaling)
        fft.TransformForward(samples, RealFFT, ImagFFT)


        '
        sb.Clear()
        sb.Append("index" & vbTab & "Amp" & vbTab & "dbv" & vbTab & "Decbl" & vbCrLf)


        ' here i calc decbls.
        For i As Int16 = 0 To 512 - 1 Step 1

            Real = RealFFT(i)
            Imag = ImagFFT(i)

            Decbl(i) = Math.Round(GetDecibels(Real, Imag), 3)

            'ignore below
            '  Dim x = (Math.Sqrt(1) / 1024)

            Dim mag = Math.Sqrt((Real * Real) + (Imag * Imag))
            Dim dbv = 10 * Math.Log(mag)
            ' Dim am = mag / 512
            Dim am = mag * (Math.Sqrt(1) / 1024) * 2
            ' both calc are correct and gives same results
            'Dim dbv = 20 * Math.Log(Math.Sqrt(am))

            'here i write deble results to excel file
            sb.Append(i & vbTab & am & vbTab & dbv & vbTab & Decbl(i) & vbCrLf)

        Next

        'Dim XdbMax = adcDaynamicRange * -1
        'Dim Xdb As Single = Data(0).Average         'Peak dB magnitude
        'Dim Xdbn As Single = Xdb - XdbMax    'Normalize To 0DB peak
        'For i = 0 To Data(0).Length - 1
        '    Data(0)(i) = Data(0)(i) - Xdb
        '    If Data(0)(i) < adcDaynamicRange Then
        '        Data(0)(i) = Data(0)(i) - RoundUp(Data(0)(i) - adcDaynamicRange - 1, 1)
        '    End If

        'Next





        Clipboard.Clear()
        Clipboard.SetText(sb.ToString)
        'is it clear
        Me.Close()
    End Sub

    Private Function MakeChanneData(ToneFreq As Integer) As Byte()

        Dim sig() = GenerateTestData22(ToneFreq, 1, 120000, 1024, 3.3 / 2)
        Dim LSB As Double = 3.3 / CDbl(CLng("&HFFF"))

        Dim sb As New System.Text.StringBuilder
        For i As Integer = 0 To sig.Length - 1
            sig(i) = CInt(sig(i) / LSB)
            sb.Append(sig(i) & vbCrLf)
        Next
        Clipboard.Clear()
        Clipboard.SetText(sb.ToString)


        Dim arInd As Integer = 0
        Dim byte1 As Byte = 0
        Dim byte2 As Byte = 0
        Dim byte3 As Byte = 0
        Dim num1 As Integer = 0
        Dim num2 As Integer = 0

        Dim dataPointsCount As Integer = 1024
        Dim binaryArraySize = dataPointsCount / 2 * 3
        Dim bytesWriteArray(binaryArraySize - 1) As Byte

        arInd = -1
        For i As Integer = 0 To sig.Length - 2 Step 2

            byte1 = (sig(i) And &HFF)
            byte2 = (sig(i + 1) And &HFF)
            byte3 = ((sig(i) >> 8) And &HF) Or (((sig(i + 1) >> 8) And &HF) << 4)

            '' maybe this is there reason ,,, which by shoud save first to array bye3 or bye1
            ' then this is correct.
            arInd += 1
            bytesWriteArray(arInd) = (byte1)
            arInd += 1
            bytesWriteArray(arInd) = (byte2)
            arInd += 1
            bytesWriteArray(arInd) = (byte3)

        Next

        Return bytesWriteArray
    End Function

    Public Function GenerateTestData22(Frequencey As Single, Amplitude As Single, SamplingRate As Integer, WindoSize As Integer, DC_Offset As Single) As Double()
        ' example usage:  call GenerateTestData(1000, 1, 12000, 1024)

        Dim Tim = 1 / SamplingRate
        Dim ResArray(WindoSize - 1) As Double
        For i As Integer = 0 To WindoSize - 1
            If Amplitude = 0 Then
                ResArray(i) = (System.Math.Sin(2 * System.Math.PI * Frequencey * Tim * i) + DC_Offset)
            Else
                ResArray(i) = (Amplitude * System.Math.Sin(2 * System.Math.PI * Frequencey * Tim * i) + DC_Offset)
            End If
        Next
        Return ResArray

    End Function



End Class