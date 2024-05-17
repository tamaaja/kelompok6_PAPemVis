Imports MySql.Data.MySqlClient
Imports System.Globalization
Imports System.Text
Imports System.Drawing.Printing

Public Class MenuUser
    ' Deklarasi koneksi dan variabel lainnya
    Dim CONN As MySqlConnection = DatabaseConnector.ConnectToDatabase()
    Dim cmd As MySqlCommand
    Dim reader As MySqlDataReader

    ' Menambahkan field untuk menyimpan email dan total belanja
    Private email As String
    Dim totalBelanja As Decimal = 0

    ' Menambahkan field untuk menyimpan nama pemesan dan alamat pemesan dari tblogin
    Private namaPemesan As String
    Private alamatPemesan As String

    ' Menambahkan field untuk menyimpan struk yang akan di cetak
    Private struk As String

    ' -------------------------------------- Program ------------------------------------------------------
    ' [1] Modifikasi konstruktor untuk menerima email sebagai parameter
    Public Sub New(ByVal userEmail As String)
        ' Digunakan untuk menginisialisasi komponen antar muka
        InitializeComponent()
        email = userEmail
        ' Mendapatkan nama dan alamat pemesan dari tabel tblogin
        GetUserData()
        ' Menghubungkan event handler
        AddHandler cbvarian.SelectedIndexChanged, AddressOf cbvarian_SelectedIndexChanged
    End Sub

    '=====================================================================================================
    ' [2] Fungsi untuk mendapatkan nama dan alamat pemesan dari tabel tblogin
    Private Sub GetUserData()
        Try
            ' Mengaktifkan koneksi
            Using CONN
                CONN.Open()

                ' Membuat printah dan mengeksekusinya
                Dim query As String = "SELECT nama, alamat FROM tblogin WHERE alamatemail = @email"
                cmd = New MySqlCommand(query, CONN)
                cmd.Parameters.AddWithValue("@email", email)
                reader = cmd.ExecuteReader()
                If reader.Read() Then
                    namaPemesan = reader("nama").ToString()
                    alamatPemesan = reader("alamat").ToString()
                End If
            End Using

            ' Menampilkan pesan error tanpa menghentikan program
        Catch ex As Exception
            MessageBox.Show("Error saat mendapatkan data pengguna: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Fungsi untuk dapat id
    Private Function GetUserId() As Integer
        Dim userId As Integer = 0
        Try
            Dim query As String = "SELECT idlogin FROM tblogin WHERE alamatemail = @email"
            cmd = New MySqlCommand(query, CONN)
            cmd.Parameters.AddWithValue("@email", email)
            Dim result = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                userId = Convert.ToInt32(result)
            End If
        Catch ex As Exception
            MessageBox.Show("Error saat mendapatkan ID pengguna: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return userId
    End Function

    '=====================================================================================================
    ' [3] Load data varian, bentuk, cart data
    Private Sub MenuUser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadVariants()
        LoadBentuk()
        LoadCartData()
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

    '=====================================================================================================
    ' [4] Load data keranjang ke datagridview
    Private Sub LoadCartData()

        Try
            ' Mengaktifkan koneksi
            Using CONN
                CONN.Open()

                ' Membuat dan mengeksekusi perintah
                Dim id_pemesan As Integer = GetUserId()
                Dim query As String = "SELECT bentuk, varian, ukuran, jumlah, totalHarga FROM tbkeranjang WHERE id_pemesan = @id_pemesan"
                cmd = New MySqlCommand(query, CONN)
                cmd.Parameters.AddWithValue("@id_pemesan", id_pemesan)
                reader = cmd.ExecuteReader()

                Dim cartTable As New DataTable()
                cartTable.Load(reader)

                totalBelanja = 0 ' Reset totalBelanja

                For Each row As DataRow In cartTable.Rows
                    Dim bentuk As String = row("bentuk").ToString()
                    Dim varian As String = row("varian").ToString()
                    Dim ukuran As String = row("ukuran").ToString()
                    Dim jumlah As Integer = Convert.ToInt32(row("jumlah"))
                    Dim totalHargaItem As Decimal = Convert.ToDecimal(row("totalHarga"))

                    ' Menambahkan total harga dari item ini ke totalBelanja
                    totalBelanja += totalHargaItem

                    ' Menambahkan item ke DataGridView
                    DataGridView1.Rows.Add(bentuk, varian, ukuran, jumlah, totalHargaItem.ToString("C2"))
                Next

                ' Memperbarui total belanja di textbox
                txt_totalBelanja.Text = totalBelanja.ToString("C2")
            End Using

            ' Memberi pesan error tanpa menghentikan program
        Catch ex As Exception
            MessageBox.Show("Error saat memuat data keranjang: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If reader IsNot Nothing Then
                reader.Close()
            End If
        End Try
    End Sub


    '=====================================================================================================
    ' [5] Load data varian ke combobox
    Private Sub LoadVariants()
        Try
            cbvarian.Items.Clear()

            ' Mengaktifkan koneksi
            Using CONN

                ' Membuat dan mengeksekusi perintah
                CONN.Open()
                Dim query As String = "SELECT nama_varian FROM tbvarian"
                cmd = New MySqlCommand(query, CONN)
                reader = cmd.ExecuteReader()

                While reader.Read()
                    Dim namaVarian As String = reader("nama_varian").ToString()
                    cbvarian.Items.Add(namaVarian)
                End While
                reader.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading variants: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '=====================================================================================================
    ' [6] Event handler untuk cbvarian SelectedIndexChanged
    Private Sub cbvarian_SelectedIndexChanged(sender As Object, e As EventArgs)
        If cbvarian.SelectedItem IsNot Nothing Then
            Dim varian As String = cbvarian.SelectedItem.ToString()
            Dim hargaVarian As Decimal = 0

            ' Ambil harga varian dari database
            Try
                ' Mengaktifkan koneksi
                Using CONN
                    CONN.Open()

                    ' Membuat dan menjalankan perintah
                    Dim query As String = "SELECT harga FROM tbvarian WHERE nama_varian = @varian"
                    cmd = New MySqlCommand(query, CONN)
                    cmd.Parameters.AddWithValue("@varian", varian)
                    reader = cmd.ExecuteReader()

                    If reader.Read() Then
                        hargaVarian = Decimal.Parse(reader("harga").ToString())
                        txtharga.Text = hargaVarian.ToString("C2")
                    End If
                    reader.Close()
                End Using

                ' Mengambil pesan kesalahan tanpa menghentikan program
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    '=====================================================================================================
    ' [7] Load data bentuk ke combobox
    Private Sub LoadBentuk()
        Try
            cbbentuk.Items.Clear()

            ' Mengaktifkan koneksi
            Using CONN
                CONN.Open()
                Dim query As String = "SELECT nama_bentuk FROM tbbentuk"
                cmd = New MySqlCommand(query, CONN)
                reader = cmd.ExecuteReader()

                ' Membuat dan mengeksekusi perintah
                While reader.Read()
                    cbbentuk.Items.Add(reader("nama_bentuk").ToString())
                End While
                reader.Close()
            End Using

            ' Mengambil pesan kesalahan tanpa menghentikan program
        Catch ex As Exception
            MessageBox.Show("Error loading bentuk: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '=====================================================================================================
    ' [8] Proses menambah data ke keranjang
    Private Sub btn_tambah_Click(sender As Object, e As EventArgs) Handles btn_tambah.Click

        If cbvarian.SelectedItem Is Nothing Then
            MessageBox.Show("Harap pilih varian.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If cbbentuk.SelectedItem Is Nothing Then
            MessageBox.Show("Harap pilih bentuk.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not (RadioButton1.Checked Or RadioButton2.Checked Or RadioButton3.Checked) Then
            MessageBox.Show("Harap pilih ukuran.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim jumlah As Integer
        If String.IsNullOrWhiteSpace(txtjumlah.Text) OrElse Not Integer.TryParse(txtjumlah.Text, jumlah) OrElse jumlah <= 0 Then
            MessageBox.Show("Harap masukkan angka positif yang valid untuk jumlah.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If DataGridView1.Rows.Count >= 5 Then
            MessageBox.Show("Hanya diperbolehkan 5 data saja.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Tampilkan pesan konfirmasi
        Dim result As DialogResult = MessageBox.Show("Yakin tambah data?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then
            Return
        End If

        ' Ambil nilai dari input pengguna
        Dim varian As String = cbvarian.SelectedItem.ToString()
        Dim bentuk As String = cbbentuk.SelectedItem.ToString()
        Dim ukuran As String = ""
        Dim hargaVarian As Decimal = txtharga.Text

        ' Tentukan ukuran berdasarkan radio button yang dipilih
        If RadioButton1.Checked Then
            ukuran = "Small"
        ElseIf RadioButton2.Checked Then
            ukuran = "Medium"
        ElseIf RadioButton3.Checked Then
            ukuran = "Large"
        End If

        ' Hitung harga berdasarkan ukuran
        Dim harga As Decimal = hargaVarian
        Select Case ukuran
            Case "Medium"
                harga *= 2
            Case "Large"
                harga *= 3
        End Select

        ' Hitung total harga untuk item ini
        Dim totalHarga As Decimal = harga * jumlah

        ' Tambahkan total harga ke totalBelanja
        totalBelanja += totalHarga

        ' Tampilkan total belanja di textbox
        txt_totalBelanja.Text = totalBelanja.ToString("C2")


        ' Masukan data
        Try
            ' Mengaktifkan koneksi
            Using CONN
                CONN.Open()

                ' Membuat dan mengeksekusi perintah
                Dim id_pemesan As Integer = GetUserId()
                Dim query As String = "INSERT INTO tbkeranjang (id_pemesan, varian, bentuk, ukuran, jumlah, totalHarga) VALUES (@id_pemesan, @varian, @bentuk, @ukuran, @jumlah, @totalHarga)"
                cmd = New MySqlCommand(query, CONN)
                cmd.Parameters.AddWithValue("@id_pemesan", id_pemesan)
                cmd.Parameters.AddWithValue("@varian", varian)
                cmd.Parameters.AddWithValue("@bentuk", bentuk)
                cmd.Parameters.AddWithValue("@ukuran", ukuran)
                cmd.Parameters.AddWithValue("@jumlah", jumlah)
                cmd.Parameters.AddWithValue("@totalHarga", totalHarga)
                cmd.ExecuteNonQuery()

                MessageBox.Show("Item berhasil ditambahkan ke keranjang.", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using

            ' Menampilkan pesan error tanpa menghentikan program
        Catch ex As MySqlException
            MessageBox.Show("Error menambahkan item ke keranjang: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' Tambahkan data ke DataGridView
        DataGridView1.Rows.Add(bentuk, varian, ukuran, jumlah, totalHarga.ToString("C2"))
    End Sub

    '=====================================================================================================
    ' [9] Proses ubah data keranjang
    Private Sub btn_ubah_Click(sender As Object, e As EventArgs) Handles btn_ubah.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Mengambil data dari input
            Dim varian As String = cbvarian.SelectedItem.ToString()
            Dim bentuk As String = cbbentuk.SelectedItem.ToString()
            Dim ukuran As String = If(RadioButton1.Checked, "Small", If(RadioButton2.Checked, "Medium", "Large"))
            Dim jumlah As Integer = Integer.Parse(txtjumlah.Text)
            Dim hargaVarian As Decimal = txtharga.Text
            Dim totalHargaLama As Decimal = Decimal.Parse(selectedRow.Cells(4).Value.ToString(), NumberStyles.Currency)

            ' Tampilkan pesan konfirmasi
            Dim result As DialogResult = MessageBox.Show("Yakin ubah data?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.No Then
                Return
            End If

            ' Hitung harga berdasarkan ukuran
            Dim harga As Decimal = hargaVarian
            Select Case ukuran
                Case "Medium"
                    harga *= 2
                Case "Large"
                    harga *= 3
            End Select

            ' Hitung total harga baru
            Dim totalHargaBaru As Decimal = harga * jumlah

            ' Update total belanja
            totalBelanja -= totalHargaLama
            totalBelanja += totalHargaBaru
            txt_totalBelanja.Text = totalBelanja.ToString("C2")

            ' Update data di DataGridView
            selectedRow.Cells(0).Value = bentuk
            selectedRow.Cells(1).Value = varian
            selectedRow.Cells(2).Value = ukuran
            selectedRow.Cells(3).Value = jumlah
            selectedRow.Cells(4).Value = totalHargaBaru.ToString("C2")

            ' Update data di database
            Try
                ' Mengaktifkan koneksi
                Using CONN
                    CONN.Open()

                    ' Membuat dan mengeksekusi perintah
                    Dim id_pemesan As Integer = GetUserId()
                    Dim query As String = "UPDATE tbkeranjang SET bentuk = @bentuk, varian = @varian, ukuran = @ukuran, jumlah = @jumlah, totalHarga = @totalHarga WHERE id_pemesan = @id_pemesan AND bentuk = @bentuk_old AND varian = @varian_old AND ukuran = @ukuran_old"
                    cmd = New MySqlCommand(query, CONN)
                    cmd.Parameters.AddWithValue("@id_pemesan", id_pemesan)
                    cmd.Parameters.AddWithValue("@bentuk", bentuk)
                    cmd.Parameters.AddWithValue("@varian", varian)
                    cmd.Parameters.AddWithValue("@ukuran", ukuran)
                    cmd.Parameters.AddWithValue("@jumlah", jumlah)
                    cmd.Parameters.AddWithValue("@totalHarga", totalHargaBaru)
                    cmd.Parameters.AddWithValue("@bentuk_old", selectedRow.Cells(0).Value.ToString())
                    cmd.Parameters.AddWithValue("@varian_old", selectedRow.Cells(1).Value.ToString())
                    cmd.Parameters.AddWithValue("@ukuran_old", selectedRow.Cells(2).Value.ToString())
                    cmd.ExecuteNonQuery()

                    MessageBox.Show("Item berhasil diperbarui.", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Using

                ' Menampilkan pesan kesalahan tanpa menghentikan program
            Catch ex As Exception
                MessageBox.Show("Error saat memperbarui item: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("Silakan pilih baris untuk diperbarui.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    '=====================================================================================================
    ' [10] Proses hapus data keranjang
    Private Sub btn_hapus_Click(sender As Object, e As EventArgs) Handles btn_hapus.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Tampilkan pesan konfirmasi
            Dim result As DialogResult = MessageBox.Show("Yakin hapus data?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.No Then
                Return
            End If

            ' Mengurangi total harga item yang dihapus dari totalBelanja
            Dim totalHargaItem As Decimal = Decimal.Parse(selectedRow.Cells(4).Value.ToString(), NumberStyles.Currency)
            totalBelanja -= totalHargaItem
            txt_totalBelanja.Text = totalBelanja.ToString("C2")

            ' Hapus data dari DataGridView
            DataGridView1.Rows.Remove(selectedRow)

            ' Hapus data dari database
            Try
                Using CONN
                    CONN.Open()
                    Dim id_pemesan As Integer = GetUserId()
                    Dim query As String = "DELETE FROM tbkeranjang WHERE id_pemesan = @id_pemesan AND bentuk = @bentuk AND varian = @varian AND ukuran = @ukuran"
                    cmd = New MySqlCommand(query, CONN)
                    cmd.Parameters.AddWithValue("@id_pemesan", id_pemesan)
                    cmd.Parameters.AddWithValue("@bentuk", selectedRow.Cells(0).Value.ToString())
                    cmd.Parameters.AddWithValue("@varian", selectedRow.Cells(1).Value.ToString())
                    cmd.Parameters.AddWithValue("@ukuran", selectedRow.Cells(2).Value.ToString())
                    cmd.ExecuteNonQuery()

                    MessageBox.Show("Item berhasil dihapus.", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Using
            Catch ex As Exception
                MessageBox.Show("Error saat menghapus item: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("Silakan pilih baris untuk dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    '=====================================================================================================
    ' [11] Mengatur data gridview agar saat di pilih datanya akan masuk ke form yang ada
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 AndAlso e.RowIndex < DataGridView1.RowCount - 1 Then
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            cbbentuk.Text = row.Cells(0).Value.ToString()
            cbvarian.Text = row.Cells(1).Value.ToString()
            txtjumlah.Text = row.Cells(3).Value.ToString()
            ' Tentukan ukuran berdasarkan nilai dari DataGridView
            Dim ukuran As String = row.Cells(2).Value.ToString()
            Select Case ukuran
                Case "Small"
                    RadioButton1.Checked = True
                Case "Medium"
                    RadioButton2.Checked = True
                Case "Large"
                    RadioButton3.Checked = True
            End Select
        End If

    End Sub

    '=====================================================================================================
    ' [12] Tombol batal untuk membersihkan inputan
    Private Sub btn_batal_Click(sender As Object, e As EventArgs) Handles btn_batal.Click
        ' Mengosongkan pilihan ComboBox
        cbvarian.SelectedIndex = -1
        cbbentuk.SelectedIndex = -1

        ' Mengosongkan teks TextBox
        txtjumlah.Text = ""
        txtharga.Text = ""

        ' Mengosongkan pilihan RadioButton
        RadioButton1.Checked = False
        RadioButton2.Checked = False
        RadioButton3.Checked = False
    End Sub


    '=====================================================================================================
    ' [14] Tombol pesan
    Private Sub btn_pesan_Click(sender As Object, e As EventArgs) Handles btn_pesan.Click

        ' Pastikan DataGridView tidak kosong
        If DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("Keranjang belanja kosong. Silakan tambahkan item terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Mendapatkan total harga dari keranjang
        Dim total_harga As Decimal = totalBelanja
        Dim detailPesanan As New DataTable()

        ' Membuat struktur DataTable sesuai dengan DataGridView
        For Each column As DataGridViewColumn In DataGridView1.Columns
            ' Pastikan ValueType tidak null
            If column.ValueType IsNot Nothing Then
                detailPesanan.Columns.Add(column.Name, column.ValueType)
            Else
                detailPesanan.Columns.Add(column.Name, GetType(String))
            End If
        Next

        ' Menyalin data dari DataGridView ke DataTable
        For Each row As DataGridViewRow In DataGridView1.Rows
            If Not row.IsNewRow Then
                Dim newRow As DataRow = detailPesanan.NewRow()
                For Each column As DataGridViewColumn In DataGridView1.Columns
                    newRow(column.Name) = row.Cells(column.Name).Value
                Next
                detailPesanan.Rows.Add(newRow)
            End If
        Next

        ' Mendapatkan tanggal pesanan saat ini
        Dim tgl_pesanan As String = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)

        Dim id_pemesan As Integer
        ' Mendapatkan id_pemesan
        Using CONN
            CONN.Open()
            id_pemesan = GetUserId()
        End Using


        ' Membuka form Struk dan mengirimkan data
        Dim formStruk As New Struk(id_pemesan, namaPemesan, alamatPemesan, tgl_pesanan, total_harga, detailPesanan)
        formStruk.Show()



        ' Memasukkan pesanan ke dalam database
        Try
            Using CONN
                CONN.Open()
                ' Mendapatkan id_pemesan
                Dim query As String = "INSERT INTO tbpesanan (id_pemesan, nama_pemesan, alamat_pemesan, tgl_pesanan, total_harga) VALUES (@id_pemesan, @nama_pemesan, @alamat_pemesan, @tgl_pesanan, @total_harga)"
                cmd = New MySqlCommand(query, CONN)
                cmd.Parameters.AddWithValue("@id_pemesan", id_pemesan)
                cmd.Parameters.AddWithValue("@nama_pemesan", namaPemesan)
                cmd.Parameters.AddWithValue("@alamat_pemesan", alamatPemesan)
                cmd.Parameters.AddWithValue("@tgl_pesanan", tgl_pesanan)
                cmd.Parameters.AddWithValue("@total_harga", total_harga)
                cmd.ExecuteNonQuery()

                ' Menampilkan pesan berhasil
                MessageBox.Show("Pesanan berhasil diproses.", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)


                ' Hapus data dari tabel tbkeranjang setelah pesanan berhasil diproses
                Dim queryH = "DELETE FROM tbkeranjang WHERE id_pemesan = @id_pemesan"
                cmd = New MySqlCommand(queryH, CONN)
                cmd.Parameters.AddWithValue("@id_pemesan", id_pemesan)
                cmd.ExecuteNonQuery()
                MessageBox.Show("Keranjang berhasil dikosongkan.", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Bersihkan keranjang setelah pemesanan
                DataGridView1.Rows.Clear()
                txt_totalBelanja.Text = "0.00"
            End Using
        Catch ex As Exception
            MessageBox.Show("Error saat memproses pesanan: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        formStruk.Print()
    End Sub

    '=====================================================================================================
    ' [15] Tombol exit
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Tampilkan pesan konfirmasi
        Dim result As DialogResult = MessageBox.Show("Yakin mau keluar?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then
            Return
        End If
        FormLogin.Show()
        Me.Hide()
    End Sub

    '=====================================================================================================
    ' [16] Membatasi data girdview
    Private Sub DataGridView1_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles DataGridView1.RowsAdded
        If DataGridView1.Rows.Count > 5 Then
            ' Hapus baris yang baru saja ditambahkan jika melebihi 5 baris
            DataGridView1.Rows.RemoveAt(DataGridView1.Rows.Count - 1)
            MessageBox.Show("Hanya diperbolehkan 5 data saja.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub


    Private Sub txtjumlah_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtjumlah.KeyPress
        Dim keyascii As Short = Asc(e.KeyChar)
        If (e.KeyChar Like "[0-9]" OrElse keyascii = Keys.Back) Then
            keyascii = 0
        Else
            e.Handled = CBool(keyascii)
        End If
    End Sub


End Class


