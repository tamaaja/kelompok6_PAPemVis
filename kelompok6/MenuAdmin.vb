Imports MySql.Data.MySqlClient

Public Class MenuAdmin

    ' Deklarasi variabel koneksi
    Dim CONN As MySqlConnection = DatabaseConnector.ConnectToDatabase()
    Dim cmd As MySqlCommand
    Dim reader As MySqlDataReader

    ' Variabel untuk menyimpan nilai awal
    Dim originalVarian As String
    Dim originalBentuk As String


    ' -------------------------------------- Program ------------------------------------------------------
    ' [1] Load varian, bentuk dan mengatur datagridvewnya
    Private Sub MenuAdmin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Mengatur mode pemilihan baris menjadi FullRowSelect
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        LoadVariants()
        LoadBentuk()

        txtbentuk.MaxLength = 30
        txtvarian.MaxLength = 30
        txtharga.MaxLength = 10

    End Sub

    '=====================================================================================================
    ' [2] Load data Varian ke datagridvew1
    Private Sub LoadVariants()

        Try
            Using CONN
                ' Mengaktifkan koneksi
                CONN.Open()

                ' Membuat dan mengeksekusi perintah
                Dim query As String = "SELECT * FROM tbvarian"
                cmd = New MySqlCommand(query, CONN)
                reader = cmd.ExecuteReader()

                ' Membaca database dan di masukan ke DataGridVew
                While reader.Read()
                    Dim varian As String = reader("nama_varian").ToString()
                    Dim harga As String = reader("harga").ToString()
                    DataGridView1.Rows.Add(varian, harga)
                End While
            End Using

            ' Menampilkan pesan error tanpa menghentikan program
        Catch ex As Exception
            MessageBox.Show("Error loading variants: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '=====================================================================================================
    ' [3] Load data Bentuk ke datagridvew2
    Private Sub LoadBentuk()
        Try
            Using CONN
                ' Mengaktifkan koneksi
                CONN.Open()

                ' Membuat perintah dan mengeksekusi perintah
                Dim query As String = "SELECT nama_bentuk FROM tbbentuk"
                cmd = New MySqlCommand(query, CONN)
                reader = cmd.ExecuteReader()

                ' Membaca database dan di masukan ke DataGridVew
                While reader.Read()
                    Dim bentuk As String = reader("nama_bentuk").ToString()
                    DataGridView2.Rows.Add(bentuk)
                End While
            End Using

            ' Menampilkan pesan error tanpa menghentikan program
        Catch ex As Exception
            MessageBox.Show("Error loading bentuk: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '=====================================================================================================
    ' [4] Validasi huruf untuk nama bentuk dan nama varian
    Private Sub txtbentuk_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbentuk.KeyPress
        ValidateKeyPress(e, txtbentuk)
    End Sub

    ' Untuk varian
    Private Sub txtvarian_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbentuk.KeyPress
        ValidateKeyPress(e, txtvarian)
    End Sub

    ' Untuk Bentuk
    Private Sub ValidateKeyPress(e As KeyPressEventArgs, targetTextBox As TextBox)

        Dim keyascii As Short = Asc(e.KeyChar)
        If (e.KeyChar Like "[a-zA-Z]" OrElse keyascii = Keys.Back OrElse keyascii = Keys.Space) Then
            keyascii = 0
        Else
            e.Handled = True
        End If

        ' Jika texbox di ketik maka fokus ke textbox lainnya
        If targetTextBox.Focused Then
            If targetTextBox Is txtbentuk Then
                txtvarian.Clear()
                txtharga.Clear()
            ElseIf targetTextBox Is txtvarian Or targetTextBox Is txt_harga Then
                txtbentuk.Clear()
            End If
        End If
    End Sub

    '=====================================================================================================
    ' [5] Validasi Harga
    Private Sub txtharga_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtharga.KeyPress
        Dim keyascii As Short = Asc(e.KeyChar)
        If (e.KeyChar Like "[0-9]" OrElse keyascii = Keys.Back) Then
            keyascii = 0
        Else
            e.Handled = CBool(keyascii)
        End If
    End Sub

    '=====================================================================================================
    ' [6] Mengatur DataGridVew varian dan bentuk

    ' Untuk varian
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            txtvarian.Text = row.Cells(0).Value.ToString()
            txtharga.Text = row.Cells(1).Value.ToString()
            originalVarian = row.Cells(0).Value.ToString()
        End If
    End Sub

    ' Untuk bentuk
    Private Sub DataGridView2_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridView2.Rows(e.RowIndex)
            txtbentuk.Text = row.Cells(0).Value.ToString()
            originalBentuk = row.Cells(0).Value.ToString()
        End If
    End Sub

    '=====================================================================================================
    ' [6] Proses Tambah data
    Private Sub btn_tambah_Click(sender As Object, e As EventArgs) Handles btn_tambah.Click

        ' Tampilkan pesan konfirmasi
        Dim result As DialogResult = MessageBox.Show("Yakin tambah data?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then
            Return
        End If

        Dim varian As String = txtvarian.Text

        Dim bentuk As String = txtbentuk.Text

        ' Mengisi hanya data varian atau bentuk
        If txtbentuk.Text <> "" Then
            txtvarian.Clear()
            txtharga.Clear()
        ElseIf txtvarian.Text <> "" Or txtharga.Text <> "" Then
            txtbentuk.Clear()
        End If

        Try
            ' Mengaktifkan koneksi
            Using CONN
                CONN.Open()

                ' Cek apakah data varian sudah ada di database
                If Not String.IsNullOrWhiteSpace(varian) Then
                    Dim checkVarianQuery As String = "SELECT COUNT(*) FROM tbvarian WHERE nama_varian = @nama_varian"
                    Using cmd As New MySqlCommand(checkVarianQuery, CONN)
                        cmd.Parameters.AddWithValue("@nama_varian", varian)
                        Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                        If count > 0 Then
                            MessageBox.Show("Varian sudah ada di database.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return
                        End If
                    End Using
                End If

                ' Cek apakah data bentuk sudah ada di database
                If Not String.IsNullOrWhiteSpace(bentuk) Then
                    Dim checkBentukQuery As String = "SELECT COUNT(*) FROM tbbentuk WHERE nama_bentuk = @nama_bentuk"
                    Using cmd As New MySqlCommand(checkBentukQuery, CONN)
                        cmd.Parameters.AddWithValue("@nama_bentuk", bentuk)
                        Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                        If count > 0 Then
                            MessageBox.Show("Bentuk sudah ada di database.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return
                        End If
                    End Using
                End If


                ' Jika data Varian terisi, masukan data varian
                If Not String.IsNullOrWhiteSpace(varian) Then
                    Dim harga As Integer = Convert.ToInt32(txtharga.Text)
                    InsertVarian(varian, harga)
                End If

                ' Jika data Bentuk terisi, masukan data bentuk
                If Not String.IsNullOrWhiteSpace(bentuk) Then
                    InsertBentuk(bentuk)
                End If

                MessageBox.Show("Berhasil menambah data!!", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using

            ' Menampilkan pesan error tanpa menghentikan program
        Catch ex As Exception
            MessageBox.Show("Error saat menambahkan data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub

    ' Proses menambah varian
    Private Sub InsertVarian(varian As String, harga As Integer)
        Dim queryVarian As String = "INSERT INTO tbvarian (nama_varian, harga) VALUES (@varian, @harga)"
        cmd = New MySqlCommand(queryVarian, CONN)
        cmd.Parameters.AddWithValue("@varian", varian)
        cmd.Parameters.AddWithValue("@harga", harga)
        cmd.ExecuteNonQuery()
        DataGridView1.Rows.Add(varian, harga)
    End Sub

    ' Proses menambah bentuk
    Private Sub InsertBentuk(bentuk As String)
        Dim queryBentuk As String = "INSERT INTO tbbentuk (nama_bentuk) VALUES (@bentuk)"
        cmd = New MySqlCommand(queryBentuk, CONN)
        cmd.Parameters.AddWithValue("@bentuk", bentuk)
        cmd.ExecuteNonQuery()
        DataGridView2.Rows.Add(bentuk)
    End Sub

    '=====================================================================================================
    ' [7] Proses Ubah data
    Private Sub btn_ubah_Click_1(sender As Object, e As EventArgs) Handles btn_ubah.Click

        ' Tampilkan pesan konfirmasi
        Dim result As DialogResult = MessageBox.Show("Yakin ubah data?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then
            Return
        End If

        Dim varian As String = txtvarian.Text
        Dim bentuk As String = txtbentuk.Text

        ' Mengisi hanya data varian atau bentuk
        If txtbentuk.Text <> "" Then
            txtvarian.Clear()
            txtharga.Clear()
        ElseIf txtvarian.Text <> "" Or txtharga.Text <> "" Then
            txtbentuk.Clear()
        End If


        Try
            ' Mengaktifkan koneksi
            Using CONN
                CONN.Open()

                ' Jika data Varian terisi, ubah data varian
                If Not String.IsNullOrWhiteSpace(varian) Then
                    Dim harga As Integer = txtharga.Text
                    UpdateVarian(varian, harga)
                End If

                ' Jika data Bentuk terisi, ubah data bentuk
                If Not String.IsNullOrWhiteSpace(bentuk) Then
                    UpdateBentuk(bentuk)
                End If
                MessageBox.Show("Berhasil ubah data!!", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using

            ' Menampilkan pesan error tanpa menghentikan program
        Catch ex As Exception
            MessageBox.Show("Error saat mengubah data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Untuk Varian
    Private Sub UpdateVarian(varian As String, harga As Integer)
        Dim queryVarian As String = "UPDATE tbvarian SET harga = @harga WHERE nama_varian = @originalVarian"
        cmd = New MySqlCommand(queryVarian, CONN)
        cmd.Parameters.AddWithValue("@varian", varian)
        cmd.Parameters.AddWithValue("@harga", harga)
        cmd.Parameters.AddWithValue("@originalVarian", originalVarian)
        cmd.ExecuteNonQuery()

        For Each row As DataGridViewRow In DataGridView1.Rows
            If row.Cells(0).Value.ToString() = originalVarian Then
                row.Cells(0).Value = varian
                row.Cells(1).Value = harga
                Exit For
            End If
        Next
    End Sub

    ' Untuk Bentuk
    Private Sub UpdateBentuk(bentuk As String)
        Dim queryBentuk As String = "UPDATE tbbentuk SET nama_bentuk = @bentuk WHERE nama_bentuk = @originalBentuk"
        cmd = New MySqlCommand(queryBentuk, CONN)
        cmd.Parameters.AddWithValue("@bentuk", bentuk)
        cmd.Parameters.AddWithValue("@originalBentuk", originalBentuk)
        cmd.ExecuteNonQuery()

        For Each row As DataGridViewRow In DataGridView2.Rows
            If row.Cells(0).Value.ToString() = originalBentuk Then
                row.Cells(0).Value = bentuk
                Exit For
            End If
        Next
    End Sub

    '=====================================================================================================
    ' [8] Proses Hapus data
    Private Sub btn_hapus_Click_1(sender As Object, e As EventArgs) Handles btn_hapus.Click

        ' Tampilkan pesan konfirmasi
        Dim result As DialogResult = MessageBox.Show("Yakin hapus data?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then
            Return
        End If

        Dim varian As String = txtvarian.Text
        Dim bentuk As String = txtbentuk.Text

        ' Mengisi hanya data varian atau bentuk
        If txtbentuk.Text <> "" Then
            txtvarian.Clear()
            txtharga.Clear()
        ElseIf txtvarian.Text <> "" Or txtharga.Text <> "" Then
            txtbentuk.Clear()
        End If


        Try
            ' Mengaktifkan koneksi
            Using CONN
                CONN.Open()

                ' Jika data Varian terisi, hapus data varian
                If Not String.IsNullOrWhiteSpace(varian) Then
                    DeleteVarian(varian)
                End If

                ' Jika data Bentuk terisi, hapus data bentuk
                If Not String.IsNullOrWhiteSpace(bentuk) Then
                    DeleteBentuk(bentuk)
                End If

                MessageBox.Show("Berhasil hapus data!!!", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using

            ' Menampilkan pesan error tanpa menghentikan program
        Catch ex As Exception
            MessageBox.Show("Error saat menghapus data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Untuk Varian
    Private Sub DeleteVarian(varian As String)
        Dim queryVarian As String = "DELETE FROM tbvarian WHERE nama_varian = @varian"
        cmd = New MySqlCommand(queryVarian, CONN)
        cmd.Parameters.AddWithValue("@varian", varian)
        cmd.ExecuteNonQuery()

        For Each row As DataGridViewRow In DataGridView1.Rows
            If row.Cells(0).Value.ToString() = varian Then
                DataGridView1.Rows.Remove(row)
                Exit For
            End If
        Next
    End Sub

    ' Untuk Bentuk
    Private Sub DeleteBentuk(bentuk As String)
        Dim queryBentuk As String = "DELETE FROM tbbentuk WHERE nama_bentuk = @bentuk"
        cmd = New MySqlCommand(queryBentuk, CONN)
        cmd.Parameters.AddWithValue("@bentuk", bentuk)
        cmd.ExecuteNonQuery()

        For Each row As DataGridViewRow In DataGridView2.Rows
            If row.Cells(0).Value.ToString() = bentuk Then
                DataGridView2.Rows.Remove(row)
                Exit For
            End If
        Next
    End Sub

    '=====================================================================================================
    ' [9] Tombol batal untuk membersihkan textbox
    Private Sub btn_batal_Click_1(sender As Object, e As EventArgs) Handles btn_batal.Click
        txtharga.Clear()
        txtbentuk.Clear()
        txtvarian.Clear()
    End Sub

    '=====================================================================================================
    ' [10] mengarahkan ke Form Data Pelanggan
    Private Sub btn_pesan_Click(sender As Object, e As EventArgs)
        DataPelanggan.Show()
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Tampilkan pesan konfirmasi
        Dim result As DialogResult = MessageBox.Show("Yakin keluar?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then
            Return
        End If
        FormLogin.Show()
        Me.Hide()
    End Sub

    Private Sub btn_pesan_Click_1(sender As Object, e As EventArgs) Handles btn_pesan.Click
        DataPelanggan.Show()
        Me.Hide()
    End Sub


 
End Class
