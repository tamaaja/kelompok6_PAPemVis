Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text

Public Class FormLogin

    ' ------------------------------------ Desain ------------------------------------------------------
    ' [1] Mouse berada di button membuat background jadi putih
    Private Sub btn_regis_MouseEnter(sender As Object, e As EventArgs) Handles btn_regis.MouseEnter
        btn_regis.BackColor = Color.Wheat
    End Sub

    '=====================================================================================================
    ' [2] Mouse meninggalkan button membuat background balik ke warna asal
    Private Sub btn_regis_MouseLeave(sender As Object, e As EventArgs) Handles btn_regis.MouseLeave
        btn_regis.BackColor = Color.FromArgb(24, 0, 0)
    End Sub





    ' -------------------------------------- Program ------------------------------------------------------
    ' [1] Tombol Exit
    Private Sub btn_exit_Click(sender As Object, e As EventArgs) Handles btn_exit.Click
        MessageBox.Show("Terima kasih sudah menggunakan program ini!!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End
    End Sub

    '=====================================================================================================
    ' [2] Tombol yang mengarahkan ke form regis
    Private Sub btn_regis_Click(sender As Object, e As EventArgs) Handles btn_regis.Click
        FormRegis.Show()
        Me.Hide()
    End Sub

    '=====================================================================================================
    ' [3] Tombol login
    Private Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click
        Dim alamatemail As String = txt_email.Text.Trim()
        ' Hash kata sandi sebelum membandingkan dengan yang disimpan di database
        Dim password As String = HashPassword(txt_password.Text.Trim())

        'Kondisi untuk login admin
        If alamatemail = "kelompok6@gmail.com" And password = HashPassword("kelompok6") Then
            MenuAdmin.Show()
            Me.Hide()
            Return
        End If

        ' Validasi jika email dan password kosong
        If String.IsNullOrEmpty(alamatemail) Or String.IsNullOrEmpty(password) Then
            MessageBox.Show("Silakan masukkan username dan password!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Koneksi ke database untuk mengambil data email dan password untuk di bandingkan
        Try
            Using connection As MySqlConnection = DatabaseConnector.ConnectToDatabase()

                ' Mengaktifkan koneksi
                connection.Open()

                ' Membuat perintah untuk menghitung jumlah baris yang memenuhi syarat
                Dim query As String = "SELECT COUNT(*) FROM tblogin WHERE alamatemail=@alamatemail AND katasandi=@katasandi"

                ' Eksekusi perintah
                Using cmd As New MySqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@alamatemail", alamatemail)
                    cmd.Parameters.AddWithValue("@katasandi", password)
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                    ' Jika di temukan baris yang sesuai maka tampilkan pesan sukses dan di arahkan ke form 
                    If count > 0 Then
                        MessageBox.Show("Login berhasil!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Me.Hide()

                        Dim menuUserForm As New MenuUser(alamatemail)
                        menuUserForm.Show()

                        ' Jika salah maka akan menampilkan pesan kesalahan
                    Else
                        MessageBox.Show("Username atau password salah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        txt_email.Clear()
                        txt_password.Clear()
                        txt_email.Focus()
                    End If
                End Using
            End Using

            ' Menangkap error tanpa menghentikan program
        Catch ex As Exception
            MessageBox.Show("Terjadi kesalahan: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '=====================================================================================================
    ' [4] Fungsi untuk melakukan hashing pada kata sandi
    Private Function HashPassword(password As String) As String

        ' Membuat objek untuk algoritma hash password SHA256
        Using Hash As SHA256 = SHA256.Create()
            Dim hashedBytes As Byte() = Hash.ComputeHash(Encoding.UTF8.GetBytes(password))
            Dim passHass As New StringBuilder()
            For i As Integer = 0 To hashedBytes.Length - 1
                passHass.Append(hashedBytes(i).ToString("x2"))
            Next
            Return passHass.ToString()
        End Using
    End Function

    Private Sub FormLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txt_password.MaxLength = 10
    End Sub



End Class
