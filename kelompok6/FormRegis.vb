Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text

Public Class FormRegis

    ' -------------------------------------- Program ------------------------------------------------------
    ' [1] Tombol Register
    Private Sub btn_regis_Click(sender As Object, e As EventArgs) Handles btn_regis.Click
        Dim nama As String = txt_nama.Text.Trim()
        Dim alamat As String = txt_alamat.Text.Trim()
        Dim alamatemail As String = txt_email.Text.Trim()
        Dim katasandi As String = HashPassword(txt_password.Text.Trim())

        ' Mengecek isi textbox apakah NULL atau kosong, jika iya tampilkan peringatan dan keluar dari prosedur
        If String.IsNullOrEmpty(nama) Or String.IsNullOrEmpty(alamat) Or String.IsNullOrEmpty(alamatemail) Or String.IsNullOrEmpty(txt_password.Text) Then
            MessageBox.Show("Mohon isi semua kolom!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Validasi alamat email apakah sudah memenuhi syarat
        If Not IsValidEmail(alamatemail) Then
            MessageBox.Show("Alamat email tidak valid!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Tampilkan pesan konfirmasi
        Dim result As DialogResult = MessageBox.Show("Yakin data diri sudah benar?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then
            Return
        End If

        ' Membuat Koneksi ke database untuk menambahkan data yang di masukan pengguna
        Try
            Using connection As MySqlConnection = DatabaseConnector.ConnectToDatabase()
                ' Mengaktifkan koneksi
                connection.Open()

                ' Membuat perintah select untuk memeriksa keberadaan email dalam database
                Dim queryCheckEmail As String = "SELECT COUNT(*) FROM tblogin WHERE alamatemail = @alamatemail"
                ' Menjalankan perintah querry
                Using cmdCheckEmail As New MySqlCommand(queryCheckEmail, connection)
                    cmdCheckEmail.Parameters.AddWithValue("@alamatemail", alamatemail)
                    Dim count As Integer = Convert.ToInt32(cmdCheckEmail.ExecuteScalar())

                    ' Jika email sudah ada dalam database, tampilkan pesan kesalahan
                    If count > 0 Then
                        MessageBox.Show("Email sudah digunakan! Silakan gunakan email lain.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                End Using

                ' Membuat perintah insert untuk memasukan data inputan pengguna
                Dim query As String = "INSERT INTO tblogin (nama, alamat, alamatemail, katasandi) VALUES (@nama, @alamat, @alamatemail, @katasandi); SELECT LAST_INSERT_ID();"

                ' Menjalankan perintah querry
                Using cmd As New MySqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@nama", nama)
                    cmd.Parameters.AddWithValue("@alamat", alamat)
                    cmd.Parameters.AddWithValue("@alamatemail", alamatemail)
                    cmd.Parameters.AddWithValue("@katasandi", katasandi)

                    ' Mendapatkan ID terakhir yang di-generate oleh database
                    Dim idlogin As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                    ' Mengosongkan isian kolom setelah registrasi berhasil
                    txt_nama.Clear()
                    txt_alamat.Clear()
                    txt_email.Clear()
                    txt_password.Clear()

                    FormLogin.Show()
                    Me.Hide()
                End Using
            End Using

            ' Menangkap kesalahan yang terjadi
        Catch ex As Exception
            MessageBox.Show("Terjadi kesalahan: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '===================================================================================================================
    ' [2] Button untk pindah ke form login
    Private Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click
        FormLogin.Show()
        Me.Hide()
    End Sub

    '===================================================================================================================
    ' [3] Fungsi untuk melakukan hashing pada kata sandi
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





    ' ------------------------------------------- Error Handling ------------------------------------------------------
    ' [1] Verifikasi Email
    Private Function IsValidEmail(email As String) As Boolean

        ' Cek apakah alamat email mengandung "@" dan ".com"
        If email.Contains("@") AndAlso email.Contains(".com") Then

            ' Cari posisi dari "@" dan ".com"
            Dim atIndex As Integer = email.IndexOf("@")
            Dim dotComIndex As Integer = email.IndexOf(".com")

            ' Jika posisi "@" kurang dari posisi ".com" dan di antara keduanya ada setidaknya satu huruf
            If atIndex < dotComIndex AndAlso dotComIndex - atIndex > 1 Then
                Return True
            End If
        End If
        Return False
    End Function

    '===================================================================================================================
    ' [2] Validasi huruf
    Private Sub txt_nama_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_nama.KeyPress
        Dim keyAscii As Short = Asc(e.KeyChar)
        If Not (Char.IsLetter(e.KeyChar) OrElse
                e.KeyChar = Convert.ToChar(Keys.Back) OrElse
                e.KeyChar = Convert.ToChar(Keys.Space) OrElse
                e.KeyChar = Convert.ToChar(Keys.Enter) OrElse
                e.KeyChar = Convert.ToChar(Keys.Delete)) Then
            e.Handled = True
        End If
    End Sub

    Private Sub FormRegis_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txt_nama.MaxLength = 30
        txt_password.MaxLength = 10
    End Sub

    Private Sub btn_login_MouseEnter(sender As Object, e As EventArgs) Handles btn_login.MouseEnter
        btn_login.BackColor = Color.Wheat
    End Sub

    Private Sub btn_login_MouseLeave(sender As Object, e As EventArgs) Handles btn_login.MouseLeave
        btn_login.BackColor = Color.FromArgb(24, 0, 0)
    End Sub
End Class
