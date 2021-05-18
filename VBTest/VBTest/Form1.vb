Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text
Imports System.Threading





Public Class Form1


    Private Declare Sub ConfigureFilter Lib "fftlib.dll" (winType As Integer, winSize As Integer)
    Private Declare Sub SetGains Lib "fftlib.dll" (<MarshalAs(UnmanagedType.SafeArray)> ByRef gains() As Double)
    Private Declare Function SetChannels Lib "fftlib.dll" (<MarshalAs(UnmanagedType.SafeArray)> ByRef channels() As Byte) As Boolean
    Private Declare Function SetFunctions Lib "fftlib.dll" (<MarshalAs(UnmanagedType.SafeArray)> ByRef functions() As Byte) As Boolean
    Private Declare Function SetConstants Lib "fftlib.dll" (uppderVoltageThreshold As Double, lowerVoltageThreshold As Double, dbRefPoint As Double, adcSamplingFrequencey As Integer, adcVdd As Double)
    Private Declare Function Start Lib "fftlib.dll" (strFile As String, channelCount As Integer, strOutFile As String, nThread As Integer) As Boolean

    Private Declare Sub Pause Lib "fftlib.dll" ()
    Private Declare Function GetStats Lib "fftlib.dll" (ByRef errCode As Integer, ByRef total As Integer, ByRef processed As Integer) As Boolean

    Private Declare Function GetFrequencyIntensity Lib "fftlib.dll" (re As Double, im As Double) As Double
    Private Declare Function GetMagnitudeSqrd Lib "fftlib.dll" (re As Double, im As Double) As Double
    Private Declare Function GetMagnitudeNormalized Lib "fftlib.dll" (re As Double, im As Double) As Double
    Private Declare Function GetAmplitude Lib "fftlib.dll" (binIndex As Integer, re As Double, im As Double) As Double
    Private Declare Function IndexToFrequency Lib "fftlib.dll" (nIndex As Integer) As Double
    Private Declare Function GetDecibels Lib "fftlib.dll" (re As Double, im As Double) As Double
    Private Declare Function GetMagnitude Lib "fftlib.dll" (re As Double, im As Double) As Double
    Private Declare Function GetDecible10 Lib "fftlib.dll" (re As Double, im As Double) As Double
    Private Declare Function SquaredMagnitude Lib "fftlib.dll" (re As Double, im As Double) As Double
    Private Declare Function GetNoisePower Lib "fftlib.dll" (re As Double, im As Double) As Double
    Private Declare Function GetPowerSpectrum Lib "fftlib.dll" (re As Double, im As Double) As Double
    Private Declare Function CalcFFT Lib "fftlib.dll" (<MarshalAs(UnmanagedType.SafeArray)> ByRef samples() As Double, <MarshalAs(UnmanagedType.SafeArray)> ByRef decbls() As Double) As Boolean
    Private Declare Function SetCalcRange Lib "fftlib.dll" (<MarshalAs(UnmanagedType.SafeArray)> ByRef ranges() As Integer) As Boolean

    Private Sub Reconstruct_3BytesTo_1Number(ByRef result As Integer, ByVal byte1 As Integer, ByVal byte2 As Integer, ByVal byte3 As Integer)
        result = (byte3 << 16) Or ((byte2 And &HFF) << 8) Or (byte1 And &HFF)
    End Sub
    Private Sub Reconstruct_2BytesTo_1Number(ByRef result As Integer, ByVal highByte As Short, ByVal LowByte As Short)
        result = (highByte << 8) Or (LowByte And &HFF)
    End Sub



    Private Function ToBits(ByVal input As Integer) As Byte()
        Return Enumerable.Range(0, 8).[Select](Function(bitIndex) 1 << bitIndex).Select(Function(bitMask) CByte(If((input And bitMask) <> bitMask, 0, 1))).ToArray()
    End Function


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txtinput.Text = "" Or _txt_output.Text = "" Then
            Return
        End If

        'set channels
        Dim nChns = 0
        Dim channels() As Byte = New Byte(_list_Channels.Items.Count - 1) {}
        For i = 0 To _list_Channels.Items.Count - 1
            If _list_Channels.Items(i).Checked Then
                channels(i) = 1
                nChns = nChns + 1
            Else
                channels(i) = 0
            End If
        Next
        If nChns = 0 Then
            Return
        End If
        SetChannels(channels)

        'set functions
        Dim nFns = 0
        Dim functions() As Byte = New Byte(_list_Functions.Items.Count - 1) {}
        For i = 0 To _list_Functions.Items.Count - 1
            If _list_Functions.Items(i).Checked Then
                functions(i) = 1
                nFns = nFns + 1
            Else
                functions(i) = 0
            End If
        Next
        If nFns = 0 Then
            Return
        End If
        SetFunctions(functions)

        'SetChannels(Enumerable.Range(0, 8).[Select](Function(bitIndex) 1 << bitIndex).Select(Function(bitMask) CByte(If((15 And bitMask) <> bitMask, 0, 1))).ToArray())
        'SetFunctions(Enumerable.Range(0, 8).[Select](Function(bitIndex) 1 << bitIndex).Select(Function(bitMask) CByte(If((8 And bitMask) <> bitMask, 0, 1))).ToArray())

        'Set Ranges
        If _list_ranges.Items.Count > 0 Then
            Dim ranges As New List(Of Integer)
            For i = 0 To _list_ranges.Items.Count - 1
                Dim rangeStr = CStr(_list_ranges.Items(i))
                Dim rangeArr = Split(rangeStr, ",")
                ranges.Add(Convert.ToInt32(rangeArr(0)))
                ranges.Add(Convert.ToInt32(rangeArr(1)))
            Next
            SetCalcRange(ranges.ToArray())
        End If


        If File.Exists(_txt_output.Text) Then
            File.Delete(_txt_output.Text)
        End If
        _elapsed = 0
        ProgressBar1.Value = 0

        Dim nThread As Integer = _num_nThreads.Text
        Dim res = Start(txtinput.Text, _num_inputChannels.Value, _txt_output.Text, nThread)
        If res Then
            Timer1.Start()
        End If
    End Sub



    Private Function CalcTemperature(RTD As Integer) As Single

        ' Calculate Temperature() using rtd integer value
        Dim RTD_RES = 1000
        Dim RTD_REF = 4300
        Dim ADC_TORES = 32768
        Dim RTD_A = 0.0039083
        Dim RTD_B = -0.0000005775
        Dim RTD_C = -0.000000418301

        If (RTD > 0) Then
            Dim R As Single = (RTD * RTD_REF) / ADC_TORES
            Dim Temp As Single = -RTD_RES * RTD_A + Math.Sqrt(RTD_RES * RTD_RES * RTD_A * RTD_A - 4 * RTD_RES * RTD_B * (RTD_RES - R))
            Temp = Temp / (2 * RTD_RES * RTD_B)
            Return Temp
        Else
            Return 0
        End If

    End Function



    Private Sub Deconstruct_2NumbersTo_3Bytes(ByVal val1 As Integer, ByVal val2 As Integer, ByRef byte1 As Byte, ByRef byte2 As Byte, ByRef byte3 As Byte)
        byte1 = (val1 And &HFF)
        byte2 = (val2 And &HFF)
        byte3 = ((val1 >> 8) And &HF) Or ((val2 >> 8) And &HF0)
    End Sub



    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim res = OpenFileDialog1.ShowDialog()
        If res = Windows.Forms.DialogResult.OK Then
            txtinput.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Dim _elapsed As Integer = 0

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        _elapsed = _elapsed + 1
        _lbl_elapsed.Text = _elapsed.ToString()

        Dim errCode = 0
        Dim total = 0
        Dim proced = 0
        Dim res = GetStats(errCode, total, proced)
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = total
        ProgressBar1.Value = proced

        If total > 0 And proced >= total Then
            Timer1.Stop()
            MsgBox("Done")
        End If

    End Sub


    Private Sub Output_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim res = SaveFileDialog1.ShowDialog()
        If res = DialogResult.OK Then
            _txt_output.Text = SaveFileDialog1.FileName
        End If
    End Sub


    Private WithEvents oReader As OutPutFileReader
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim watch As Stopwatch = Stopwatch.StartNew()
        oReader = New OutPutFileReader(_txt_output.Text)


        Dim resH2 = oReader.GetChannelVoltage(0)
        Dim resH1 = oReader.GetChannelVoltage(1)
        Dim resL2 = oReader.GetChannelVoltage(2)
        Dim resL1 = oReader.GetChannelVoltage(3)




        Dim nBlocks = oReader.BlocksCount
        'Dim block = oReader.ReadBlock(0)
        'block = oReader.ReadBlock(4)
        'Dim blockHdrs = oReader.GetBlockHeaders()


        Dim data = oReader.GetCalcResult(Channels.channel_h2, CalcFunctions.get_decibels)
        watch.Stop()
        'Debug.Print(data.Item(0)(0) & Space(10) & data.Item(0)(data.Item(0).Count - 1))
        MsgBox(watch.Elapsed.TotalMilliseconds / 1000)
        oReader.Dispose()

        Debug.Print(adcDaynamicRange)

        'Dim XdbMax = adcDaynamicRange * -1
        'Dim Xdb As Single = data(0).Average         'Peak dB magnitude
        'Dim Xdbn As Single = Xdb - XdbMax    'Normalize To 0DB peak
        'For i = 0 To data(0).Length - 1
        '    data(0)(i) = data(0)(i) - Xdb
        '    If data(0)(i) < adcDaynamicRange Then
        '        data(0)(i) = data(0)(i) - RoundUp(data(0)(i) - adcDaynamicRange - 1, 1)
        '    End If

        'Next


        Dim sb As New StringBuilder
        For i As Integer = 0 To data(0).Length - 1
            sb.Append(data(0)(i) & vbCrLf)
        Next
        Clipboard.Clear()
        Clipboard.SetText(sb.ToString)


    End Sub

    Private Function RoundUp(value As Double, decimals As Integer) As Double

        Return Math.Ceiling(value * (10 ^ decimals)) / (10 ^ decimals)

    End Function

    Private Sub MyBackgroundThread()
    End Sub

    Private Sub SetProgressBarMax(value As Integer)
        'If ProgressBar1.InvokeRequired Then
        '    ProgressBar1.Invoke(New Action(Of Object)(AddressOf myreader_ProgressChanged), value)
        'Else
        '    ProgressBar1.Maximum = value
        'End If
        ProgressBar1.Maximum = value
    End Sub
    Private Sub myreader_ProgressChanged(Position As Integer) Handles oReader.ProgressChanged
        'If ProgressBar1.InvokeRequired Then
        '    ProgressBar1.Invoke(New Action(Of Object)(AddressOf myreader_ProgressChanged), Position)
        'Else
        '    ProgressBar1.Value = Position
        'End If

        '    ProgressBar1.Value = Position
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False

        Dim labels = System.Enum.GetValues(GetType(Channels))
        For i = 0 To 7
            Dim item = _list_Channels.Items.Add(labels(i).ToString())
            If i < 4 Then
                item.Checked = True
            End If
        Next

        labels = System.Enum.GetValues(GetType(CalcFunctions))
        For i = 0 To 7
            Dim item = _list_Functions.Items.Add(labels(i).ToString())
            If i = 4 Then
                item.Checked = True
            End If
        Next

    End Sub

    Private Sub _btn_add_range_Click(sender As Object, e As EventArgs) Handles _btn_add_range.Click
        If _num_ragen_start.Value > _num_ragen_stop.Value Then
            _num_ragen_start.Value = 0
            _num_ragen_stop.Value = 0
            Return
        End If
        _list_ranges.Items.Add(_num_ragen_start.Value.ToString() + "," + _num_ragen_stop.Value.ToString())
    End Sub

    Private Sub _btn_clear_ranges_Click(sender As Object, e As EventArgs) Handles _btn_clear_ranges.Click
        _list_ranges.Items.Clear()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Form2.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub
End Class

