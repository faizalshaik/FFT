Imports System.IO
Imports System.Runtime.InteropServices

Public Class Block4
    Public index As Integer
    Public timeStamps As Long
    Public adcVolt As Single
    Public temperature As Single
    Public decblH2() As Single = New Single(512) {}
    Public decblH1() As Single = New Single(512) {}
    Public decblL2() As Single = New Single(512) {}
    Public decblL1() As Single = New Single(512) {}

    Public realH2() As Single = New Single(512) {}
    Public realH1() As Single = New Single(512) {}
    Public realL2() As Single = New Single(512) {}
    Public realL1() As Single = New Single(512) {}

    Public imagH2() As Single = New Single(512) {}
    Public imagH1() As Single = New Single(512) {}
    Public imagL2() As Single = New Single(512) {}
    Public imagL1() As Single = New Single(512) {}
End Class

Public Class Form1
    Private Declare Sub ConfigureFilter Lib "fftlib.dll" (chNo As Integer, winType As Integer, winSize As Integer)
    Private Declare Sub SetGain Lib "fftlib.dll" (Ch1Gain As Double, Ch2Gain As Double, Ch3Gain As Double, Ch4Gain As Double)
    Private Declare Sub SetScale Lib "fftlib.dll" (chNo As Integer, baseMin As Double, baseMax As Double)
    Private Declare Sub SetFftFunction Lib "fftlib.dll" (fnNo As Integer)
    Private Declare Function Start Lib "fftlib.dll" (strFile As String, strOutFile As String, nThread As Integer) As Boolean
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


    Dim _elapsed = 0

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim samples() As Double = New Double(1023) {}
        Dim decbls() As Double = New Double(511) {}
        samples(0) = 100
        samples(1) = 201
        CalcFFT(samples, decbls)

        'readTest()
        'If _txt_file.Text = "" Or _txt_output.Text = "" Then
        '    Return
        'End If
        'Dim nThread As Integer = _num_nThreads.Value
        'Dim res = Start(_txt_file.Text, _txt_output.Text, nThread)
        'If res Then
        '    Timer1.Start()
        'End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Pause()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim res = OpenFileDialog1.ShowDialog()
        If res = Windows.Forms.DialogResult.OK Then
            _txt_file.Text = OpenFileDialog1.FileName
        End If
    End Sub

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
        End If
    End Sub

    Private Sub Output_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim res = SaveFileDialog1.ShowDialog()
        If res = DialogResult.OK Then
            _txt_output.Text = SaveFileDialog1.FileName
        End If


    End Sub

    Private Sub readTest()
        Using reader As New BinaryReader(File.Open("d:\\1.bin", FileMode.Open))
            Dim nBlocks = reader.BaseStream.Length / 24592
            For i = 0 To nBlocks - 1
                Dim block = readBlock(reader)
            Next
        End Using
    End Sub

    Private Function readBlock(reader As BinaryReader) As Block4
        Dim block = New Block4()
        block.index = reader.ReadInt32()
        block.timeStamps = reader.ReadInt32()
        block.adcVolt = reader.ReadSingle()
        block.temperature = reader.ReadSingle()
        For i = 0 To 511
            block.decblH2(i) = reader.ReadSingle()
        Next
        For i = 0 To 511
            block.decblH1(i) = reader.ReadSingle()
        Next
        For i = 0 To 511
            block.decblL2(i) = reader.ReadSingle()
        Next
        For i = 0 To 511
            block.decblL1(i) = reader.ReadSingle()
        Next

        For i = 0 To 511
            block.realH2(i) = reader.ReadSingle()
        Next
        For i = 0 To 511
            block.realH1(i) = reader.ReadSingle()
        Next
        For i = 0 To 511
            block.realL2(i) = reader.ReadSingle()
        Next
        For i = 0 To 511
            block.realL1(i) = reader.ReadSingle()
        Next

        For i = 0 To 511
            block.imagH2(i) = reader.ReadSingle()
        Next
        For i = 0 To 511
            block.imagH1(i) = reader.ReadSingle()
        Next
        For i = 0 To 511
            block.imagL2(i) = reader.ReadSingle()
        Next
        For i = 0 To 511
            block.imagL1(i) = reader.ReadSingle()
        Next

        Return block
    End Function

End Class


