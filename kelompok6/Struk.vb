Imports System.Drawing.Printing

Public Class Struk
    Private formImage As Bitmap
    Private captureWidth As Integer = 700
    Private captureHeight As Integer = 550

    Public Sub New(ByVal idPemesan As Integer, ByVal namaPemesan As String, ByVal alamatPemesan As String, ByVal tglPesanan As String, ByVal totalHarga As Decimal, ByVal detailPesanan As DataTable)
        InitializeComponent()

        ' Atur data ke label

        lbl_namaPemesan.Text = namaPemesan
        lbl_alamatPemesan.Text = alamatPemesan
        lbl_tglPemesanan.Text = tglPesanan
        lbl_totalHarga.Text = totalHarga.ToString("C2")

        ' Atur data ke DataGridView
        DataGridView1.DataSource = detailPesanan
    End Sub

    ' Fungsi untuk melakukan pencetakan
    Public Sub Print()
        formImage = New Bitmap(captureWidth, captureHeight)
        Using mg As Graphics = Graphics.FromImage(formImage)
            mg.CopyFromScreen(Me.Location, Point.Empty, New Size(captureWidth, captureHeight))
        End Using
        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.ShowDialog()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        e.Graphics.DrawImage(formImage, 0, 0)
    End Sub

    Private Sub Struk_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
