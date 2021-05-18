Imports System.IO

Public Class ToolDumpWriter

    Private binWriter As BinaryWriter
    Private outputFilename As String = "d:\ToolDump.bin"

    Public Sub New()
    End Sub
    Public Sub New(filename As String)
        outputFilename = filename
    End Sub

    Public Function CreateFile() As Boolean
        Try
            If File.Exists(outputFilename) Then
                File.Delete(outputFilename)
            End If
            binWriter = New BinaryWriter(File.Open(outputFilename, FileMode.Create))
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub Write(spi_Buffer() As Byte)
        binWriter.Write(spi_Buffer) ' keep repeating the same for every page
    End Sub

    Public Function CloseFile() As Boolean
        Try
            binWriter.Close()
            binWriter = Nothing
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

End Class


'' How to use:
''-------------------------------------------------
'' (1) Deleclare this public variables to download form
'Public spi_Buffer() As Byte = New Byte(4095) {} ' use this to receive page data from SPI FTDI cable
'Public ToolDumpWriter As ToolDumpWriter

'' (2) Add this to download button
'ToolDumpWriter = New ToolDumpWriter("d:\tooldumpfile.bin")
'ToolDumpWriter.CreateFile()

'' (3) add this to SPI receive and keep calling to save the data
'ToolDumpWriter.Write(spi_Buffer)

'' (4) call this when download is completed
'ToolDumpWriter.CloseFile()
