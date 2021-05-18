Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary


Enum Channels
    channel_h2 = 0
    channel_h1
    channel_l2
    channel_l1
    channel_reserved1
    channel_reserved2
    channel_reserved3
    channel_reserved4
End Enum

Enum CalcFunctions
    magnitude = 0
    magnitude_normalized
    get_intensity
    get_amplitude
    get_decibels
    get_decible10
    get_power_spectrum
    get_noise_power
    calc_reserved_0
    calc_reserved_1
    calc_reserved_2
    calc_reserved_3
End Enum

Public Class FILTERSETTING
    Public windowSize As Integer
    Public windowType As Integer
    Public Sub read(reader As BinaryReader)
        windowSize = reader.ReadInt32
        windowType = reader.ReadInt32
    End Sub
End Class

Public Class CONSTANTS
    Public adcVdd As Double
    Public adcHafVdd As Double
    Public adcDaynamicRange As Double
    Public adcLSB As Double
    Public adcDcOffsetInVolts As Double
    Public adcDcOffsetInDecimal As Double
    Public dbRefPoint As Double
    Public fftFreqResolution As Double
    Public UppderVoltageThreshold As Double
    Public LowerVoltageThreshold As Double
    Public adcResoluationBits As Integer
    Public adcResoluation As Integer
    Public adcSamplingFrequencey As Integer
    Public adcWindowSize As Integer
    Public fftResultSize As Integer
    Public Sub read(reader As BinaryReader)
        adcVdd = reader.ReadDouble
        adcHafVdd = reader.ReadDouble
        adcDaynamicRange = reader.ReadDouble
        adcLSB = reader.ReadDouble
        adcDcOffsetInVolts = reader.ReadDouble
        adcDcOffsetInDecimal = reader.ReadDouble
        dbRefPoint = reader.ReadDouble
        fftFreqResolution = reader.ReadDouble
        UppderVoltageThreshold = reader.ReadDouble
        LowerVoltageThreshold = reader.ReadDouble
        adcResoluationBits = reader.ReadInt32
        adcResoluation = reader.ReadInt32
        adcSamplingFrequencey = reader.ReadInt32
        adcWindowSize = reader.ReadInt32
        fftResultSize = reader.ReadInt32
    End Sub
End Class


Public Class OutputFileHeader
    Public Const ChannelMaxCount = 8
    Public Const FunctionMaxCount = 12

    Public version As Integer
    Public filterSettings() As FILTERSETTING = New FILTERSETTING(ChannelMaxCount - 1) {}
    Public gains() As Double = New Double(ChannelMaxCount - 1) {}
    Public constants As CONSTANTS = New CONSTANTS()
    Public functions() As Byte = New Byte(FunctionMaxCount - 1) {}
    Public channels() As Byte = New Byte(ChannelMaxCount - 1) {}
    Public blockSize As UInt32
    Public blockCount As UInt32


    Public Sub read(reader As BinaryReader)
        version = reader.ReadInt32
        For i = 0 To filterSettings.Length - 1
            filterSettings(i) = New FILTERSETTING()
            filterSettings(i).read(reader)
        Next
        For i = 0 To ChannelMaxCount - 1
            gains(i) = reader.ReadDouble
        Next
        constants.read(reader)
        For i = 0 To FunctionMaxCount - 1
            functions(i) = reader.ReadByte
        Next
        For i = 0 To ChannelMaxCount - 1
            channels(i) = reader.ReadByte
        Next
        blockSize = reader.ReadUInt32
        blockCount = reader.ReadUInt32

        channelsInBlock = calcChannelsInBlock()
        functionsInBlock = calcFunctionsInBlock()
    End Sub

    Public channelsInBlock As Integer
    Public functionsInBlock As Integer

    Function calcChannelsInBlock() As Integer
        Dim nChns = 0
        For Each v In channels
            If v = 1 Then
                nChns = nChns + 1
            End If
        Next
        Return nChns
    End Function
    Function calcFunctionsInBlock() As Integer
        Dim nFns = 0
        For Each v In functions
            If v = 1 Then
                nFns = nFns + 1
            End If
        Next
        Return nFns
    End Function

    Public Function getChannelIndex(chNo As Integer) As Integer
        Dim idx = 0
        For i = 0 To chNo - 1
            If channels(i) = 1 Then idx = idx + 1
        Next
        Return idx
    End Function
    Public Function getFunctionIndex(fnNo As Integer) As Integer
        Dim idx = 0
        For i = 0 To fnNo - 1
            If functions(i) = 1 Then idx = idx + 1
        Next
        Return idx
    End Function
End Class


Public Class Block4
    Public Const BlockHeaderSize = 16
    Public Const FftResultSize = 512

    Public index As Integer
    Public timeStamp As Integer
    Public adcVolt As Single
    Public temperature As Single
    Public voltMins As New List(Of Double)
    Public voltMaxs As New List(Of Double)
    Public voltAvgs As New List(Of Double)
    Public results As New List(Of Single())

    Public channels As Integer
    Public functions As Integer
    Public Sub New(channelCount As Integer, functionCount As Integer)
        channels = channelCount
        functions = functionCount
    End Sub
    Public Sub read(reader As BinaryReader, onlyHeader As Boolean)
        index = reader.ReadInt32
        timeStamp = reader.ReadInt32
        adcVolt = reader.ReadSingle
        temperature = reader.ReadSingle

        For i = 0 To channels - 1
            voltMins.Add(reader.ReadDouble)
        Next
        For i = 0 To channels - 1
            voltMaxs.Add(reader.ReadDouble)
        Next
        For i = 0 To channels - 1
            voltAvgs.Add(reader.ReadDouble)
        Next

        If onlyHeader = False Then
            Dim nDatas = channels * functions
            For i = 0 To nDatas - 1
                Dim data() As Single = New Single(FftResultSize - 1) {}
                For ii = 0 To FftResultSize - 1
                    data(ii) = reader.ReadSingle
                Next
                results.Add(data)
            Next
        End If
    End Sub
End Class

Public Class OutPutFileReader
    Public Const HeaderSize = 260

    'Declare the event as so...
    Public Event ProgressChanged(ByVal Position As Integer)
    Private _binaryFile As FileStream
    Private _reader As BinaryReader
    Private _totalReadBytes As Long = 0
    Private _inputFileSize As Long = 0

    Private _indexes As New List(Of Integer)
    Private _timeStamps As New List(Of Integer)
    Private _adcVoltages As New List(Of Single)
    Private _temperatures As New List(Of Single)
    Private _header As OutputFileHeader = New OutputFileHeader()


    Public ReadOnly Property Header() As OutputFileHeader
        Get
            Return _header
        End Get
    End Property


    Public ReadOnly Property GetIndex() As Integer()
        Get
            Return _indexes.ToArray()
        End Get
    End Property
    Public ReadOnly Property GetTimeStamp() As Integer()
        Get
            Return _timeStamps.ToArray
        End Get
    End Property
    Public ReadOnly Property GetVoltage() As Single()
        Get
            Return _adcVoltages.ToArray
        End Get
    End Property
    Public ReadOnly Property GetTemperature() As Single()
        Get
            Return _temperatures.ToArray
        End Get
    End Property
    Public ReadOnly Property BlocksCount() As Long
        Get
            Return _header.blockCount
        End Get
    End Property


    Public Sub New(InputFile As String)
        SetInputFile(InputFile)
    End Sub
    Public Sub SetInputFile(InputFile As String)
        _binaryFile = New FileStream(InputFile, FileMode.Open, FileAccess.Read)
        _reader = New BinaryReader(_binaryFile)
        _inputFileSize = _reader.BaseStream.Length

        'Read file header
        _header.read(_reader)

        'Read block headers
        For i = 0 To _header.blockCount - 1
            Dim offset = HeaderSize + i * _header.blockSize
            _binaryFile.Seek(offset, SeekOrigin.Begin)
            _indexes.Add(_reader.ReadInt32)
            _timeStamps.Add(_reader.ReadInt32)
            _adcVoltages.Add(_reader.ReadSingle)
            _temperatures.Add(_reader.ReadSingle)
        Next
    End Sub
    Public Sub Dispose()
        _binaryFile.Close()
        _reader.Close()
        _indexes = Nothing
        _timeStamps = Nothing
        _adcVoltages = Nothing
        _temperatures = Nothing
        _binaryFile = Nothing
        _reader = Nothing
    End Sub

    Public Function ReadBlock(iBock As Integer) As Block4
        If iBock >= _header.blockCount Then
            Return Nothing
        End If
        Dim offset = HeaderSize + iBock * _header.blockSize
        _binaryFile.Seek(offset, SeekOrigin.Begin)
        Dim block = New Block4(_header.channelsInBlock, _header.functionsInBlock)
        block.read(_reader, False)
        Return block
    End Function

    Public Function GetCalcResult(channelNo As Integer, functionNo As Integer) As List(Of Single())

        If channelNo >= OutputFileHeader.ChannelMaxCount Or functionNo >= OutputFileHeader.FunctionMaxCount Then
            Return Nothing
        End If
        If _header.channels(channelNo) = 0 Or _header.functions(functionNo) = 0 Then
            Return Nothing
        End If

        Dim offset As Long = 0
        Dim buff() As Byte = New Byte(_header.constants.fftResultSize * 4) {}
        Dim data() As Single = New Single(Block4.FftResultSize - 1) {}
        Dim list = New List(Of Single())
        Dim idxChn = _header.getChannelIndex(channelNo)
        Dim idxFn = _header.getFunctionIndex(functionNo)

        For iBock = 0 To _header.blockCount - 1
            offset = HeaderSize + Block4.BlockHeaderSize + (iBock * _header.blockSize)
            offset = offset + _header.channelsInBlock * 8 * 3
            offset = offset + (idxChn * _header.functionsInBlock + idxFn) * (Block4.FftResultSize * 4)
            _binaryFile.Seek(offset, SeekOrigin.Begin)
            'For i = 0 To Block4.FftResultSize - 1
            '    data(i) = _reader.ReadSingle
            'Next
            _binaryFile.Read(buff, 0, buff.Length)
            Buffer.BlockCopy(buff, 0, data, 0, buff.Length - 1)
            list.Add(data)
        Next

        Return list
    End Function

    Public Function GetBlockHeaders() As List(Of Block4)
        Dim offset As Long = 0
        Dim list = New List(Of Block4)
        For iBock = 0 To _header.blockCount - 1
            offset = HeaderSize + (iBock * _header.blockSize)
            Dim block = New Block4(_header.channelsInBlock, _header.functionsInBlock)
            _binaryFile.Seek(offset, SeekOrigin.Begin)
            block.read(_reader, True)
            list.Add(block)
        Next
        Return list
    End Function



    Public Function GetChannelVoltage(ChannelNo As Integer) As List(Of Double)
        If ChannelNo >= OutputFileHeader.ChannelMaxCount Then
            Return Nothing
        End If
        If _header.channels(ChannelNo) = 0 Then
            Return Nothing
        End If

        Dim idx = _header.getChannelIndex(ChannelNo)

        Dim headers As List(Of Block4) = GetBlockHeaders()
        Dim res As New List(Of Double)
        For Each h In headers
            res.Add(h.voltMins.Item(idx))
            res.Add(h.voltMaxs.Item(idx))
            res.Add(h.voltAvgs.Item(idx))
        Next
        headers = Nothing
        Return res
    End Function



    'Private Function ProcessAtInex(iBockIndex As Long, idxChn As Integer, idxFn As Integer)
    '    Dim offset As Long = 0
    '    Dim buff() As Byte = New Byte(_header.constants.fftResultSize * 4) {}
    '    Dim data() As Single = New Single(Block4.FftResultSize - 1) {}

    '    offset = HeaderSize + Block4.BlockHeaderSize + (iBockIndex * _header.blockSize)
    '    offset = offset + (idxChn * _header.functionsInBlock + idxFn) * (Block4.FftResultSize * 4)
    '    _binaryFile.Seek(offset, SeekOrigin.Begin)

    '    _binaryFile.Read(buff, 0, buff.Length - 1)
    '    Buffer.BlockCopy(buff, 0, data, 0, buff.Length - 1)

    '    Return data
    'End Function

End Class
