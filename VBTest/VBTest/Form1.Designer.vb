<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me._num_nThreads = New System.Windows.Forms.TextBox()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me._txt_output = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me._lbl_elapsed = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.txtinput = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me._num_inputChannels = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me._list_Channels = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me._list_Functions = New System.Windows.Forms.ListView()
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me._btn_clear_ranges = New System.Windows.Forms.Button()
        Me._btn_add_range = New System.Windows.Forms.Button()
        Me._num_ragen_stop = New System.Windows.Forms.NumericUpDown()
        Me._num_ragen_start = New System.Windows.Forms.NumericUpDown()
        Me._list_ranges = New System.Windows.Forms.ListBox()
        Me.Button5 = New System.Windows.Forms.Button()
        CType(Me._num_inputChannels, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me._num_ragen_stop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._num_ragen_start, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_num_nThreads
        '
        Me._num_nThreads.Location = New System.Drawing.Point(355, 132)
        Me._num_nThreads.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me._num_nThreads.Name = "_num_nThreads"
        Me._num_nThreads.Size = New System.Drawing.Size(100, 22)
        Me._num_nThreads.TabIndex = 0
        Me._num_nThreads.Text = "400"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(76, 513)
        Me.ProgressBar1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(648, 23)
        Me.ProgressBar1.TabIndex = 1
        '
        '_txt_output
        '
        Me._txt_output.Location = New System.Drawing.Point(65, 84)
        Me._txt_output.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me._txt_output.Name = "_txt_output"
        Me._txt_output.Size = New System.Drawing.Size(273, 22)
        Me._txt_output.TabIndex = 2
        Me._txt_output.Text = "D:\outputFile.bin"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        '_lbl_elapsed
        '
        Me._lbl_elapsed.AutoSize = True
        Me._lbl_elapsed.Location = New System.Drawing.Point(636, 491)
        Me._lbl_elapsed.Name = "_lbl_elapsed"
        Me._lbl_elapsed.Size = New System.Drawing.Size(88, 17)
        Me._lbl_elapsed.TabIndex = 3
        Me._lbl_elapsed.Text = "_lbl_elapsed"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'txtinput
        '
        Me.txtinput.Location = New System.Drawing.Point(65, 33)
        Me.txtinput.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtinput.Name = "txtinput"
        Me.txtinput.Size = New System.Drawing.Size(273, 22)
        Me.txtinput.TabIndex = 4
        Me.txtinput.Text = "D:\Binary File.bin"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(180, 565)
        Me.Button1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(125, 36)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Start"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(325, 565)
        Me.Button2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(117, 36)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "Pause"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(355, 33)
        Me.Button3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(120, 32)
        Me.Button3.TabIndex = 7
        Me.Button3.Text = "Open"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(355, 80)
        Me.Button4.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(120, 30)
        Me.Button4.TabIndex = 8
        Me.Button4.Text = "Save"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(469, 565)
        Me.Button6.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(117, 36)
        Me.Button6.TabIndex = 9
        Me.Button6.Text = "Read"
        Me.Button6.UseVisualStyleBackColor = True
        '
        '_num_inputChannels
        '
        Me._num_inputChannels.Location = New System.Drawing.Point(191, 133)
        Me._num_inputChannels.Margin = New System.Windows.Forms.Padding(4)
        Me._num_inputChannels.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
        Me._num_inputChannels.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me._num_inputChannels.Name = "_num_inputChannels"
        Me._num_inputChannels.Size = New System.Drawing.Size(68, 22)
        Me._num_inputChannels.TabIndex = 10
        Me._num_inputChannels.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(277, 135)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 17)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Threads"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(61, 135)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 17)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Input File Chnnels"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me._list_Channels)
        Me.GroupBox1.Location = New System.Drawing.Point(61, 182)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Size = New System.Drawing.Size(203, 283)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Out Channels"
        '
        '_list_Channels
        '
        Me._list_Channels.CheckBoxes = True
        Me._list_Channels.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me._list_Channels.FullRowSelect = True
        Me._list_Channels.GridLines = True
        Me._list_Channels.HideSelection = False
        Me._list_Channels.Location = New System.Drawing.Point(13, 28)
        Me._list_Channels.Margin = New System.Windows.Forms.Padding(4)
        Me._list_Channels.Name = "_list_Channels"
        Me._list_Channels.Size = New System.Drawing.Size(177, 238)
        Me._list_Channels.TabIndex = 0
        Me._list_Channels.UseCompatibleStateImageBehavior = False
        Me._list_Channels.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Channel No"
        Me.ColumnHeader1.Width = 100
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me._list_Functions)
        Me.GroupBox2.Location = New System.Drawing.Point(272, 182)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox2.Size = New System.Drawing.Size(237, 283)
        Me.GroupBox2.TabIndex = 12
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Out Functions"
        '
        '_list_Functions
        '
        Me._list_Functions.CheckBoxes = True
        Me._list_Functions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2})
        Me._list_Functions.FullRowSelect = True
        Me._list_Functions.GridLines = True
        Me._list_Functions.HideSelection = False
        Me._list_Functions.Location = New System.Drawing.Point(11, 28)
        Me._list_Functions.Margin = New System.Windows.Forms.Padding(4)
        Me._list_Functions.Name = "_list_Functions"
        Me._list_Functions.Size = New System.Drawing.Size(212, 238)
        Me._list_Functions.TabIndex = 0
        Me._list_Functions.UseCompatibleStateImageBehavior = False
        Me._list_Functions.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Functions"
        Me.ColumnHeader2.Width = 140
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me._btn_clear_ranges)
        Me.GroupBox3.Controls.Add(Me._btn_add_range)
        Me.GroupBox3.Controls.Add(Me._num_ragen_stop)
        Me.GroupBox3.Controls.Add(Me._num_ragen_start)
        Me.GroupBox3.Controls.Add(Me._list_ranges)
        Me.GroupBox3.Location = New System.Drawing.Point(520, 182)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox3.Size = New System.Drawing.Size(219, 284)
        Me.GroupBox3.TabIndex = 13
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Ranges"
        '
        '_btn_clear_ranges
        '
        Me._btn_clear_ranges.Location = New System.Drawing.Point(113, 237)
        Me._btn_clear_ranges.Margin = New System.Windows.Forms.Padding(4)
        Me._btn_clear_ranges.Name = "_btn_clear_ranges"
        Me._btn_clear_ranges.Size = New System.Drawing.Size(91, 31)
        Me._btn_clear_ranges.TabIndex = 2
        Me._btn_clear_ranges.Text = "Clear"
        Me._btn_clear_ranges.UseVisualStyleBackColor = True
        '
        '_btn_add_range
        '
        Me._btn_add_range.Location = New System.Drawing.Point(16, 237)
        Me._btn_add_range.Margin = New System.Windows.Forms.Padding(4)
        Me._btn_add_range.Name = "_btn_add_range"
        Me._btn_add_range.Size = New System.Drawing.Size(91, 31)
        Me._btn_add_range.TabIndex = 2
        Me._btn_add_range.Text = "Add"
        Me._btn_add_range.UseVisualStyleBackColor = True
        '
        '_num_ragen_stop
        '
        Me._num_ragen_stop.Location = New System.Drawing.Point(113, 206)
        Me._num_ragen_stop.Margin = New System.Windows.Forms.Padding(4)
        Me._num_ragen_stop.Maximum = New Decimal(New Integer() {1316134912, 2328, 0, 0})
        Me._num_ragen_stop.Name = "_num_ragen_stop"
        Me._num_ragen_stop.Size = New System.Drawing.Size(92, 22)
        Me._num_ragen_stop.TabIndex = 1
        '
        '_num_ragen_start
        '
        Me._num_ragen_start.Location = New System.Drawing.Point(16, 206)
        Me._num_ragen_start.Margin = New System.Windows.Forms.Padding(4)
        Me._num_ragen_start.Maximum = New Decimal(New Integer() {1000000000, 0, 0, 0})
        Me._num_ragen_start.Name = "_num_ragen_start"
        Me._num_ragen_start.Size = New System.Drawing.Size(92, 22)
        Me._num_ragen_start.TabIndex = 1
        '
        '_list_ranges
        '
        Me._list_ranges.FormattingEnabled = True
        Me._list_ranges.ItemHeight = 16
        Me._list_ranges.Location = New System.Drawing.Point(13, 30)
        Me._list_ranges.Margin = New System.Windows.Forms.Padding(4)
        Me._list_ranges.Name = "_list_ranges"
        Me._list_ranges.Size = New System.Drawing.Size(191, 164)
        Me._list_ranges.TabIndex = 0
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(565, 42)
        Me.Button5.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(117, 36)
        Me.Button5.TabIndex = 14
        Me.Button5.Text = "Make DataFile"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(753, 634)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me._num_inputChannels)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.txtinput)
        Me.Controls.Add(Me._lbl_elapsed)
        Me.Controls.Add(Me._txt_output)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me._num_nThreads)
        Me.DoubleBuffered = True
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me._num_inputChannels, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        CType(Me._num_ragen_stop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._num_ragen_start, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents _num_nThreads As TextBox
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents _txt_output As TextBox
    Friend WithEvents Timer1 As Timer
    Friend WithEvents _lbl_elapsed As Label
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents txtinput As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents _num_inputChannels As NumericUpDown
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents _list_Channels As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents _list_Functions As ListView
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents _btn_clear_ranges As Button
    Friend WithEvents _btn_add_range As Button
    Friend WithEvents _num_ragen_stop As NumericUpDown
    Friend WithEvents _num_ragen_start As NumericUpDown
    Friend WithEvents _list_ranges As ListBox
    Friend WithEvents Button5 As Button
End Class
