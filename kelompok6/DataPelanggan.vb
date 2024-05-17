Imports MySql.Data.MySqlClient

Public Class DataPelanggan
    Private CONN As MySqlConnection

    ' -------------------------------------- Program ------------------------------------------------------
    ' [1] Load data pesanan dan koneksi
    Private Sub DataPelanggan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CONN = DatabaseConnector.ConnectToDatabase()
        LoadOrderHistory()
    End Sub

    '=====================================================================================================
    ' [2] Proses load data pesanan ke DataGridView1
    Private Sub LoadOrderHistory(Optional searchQuery As String = "")
        Try
            Using CONN
                CONN.Open()
                Dim query As String = "SELECT id_pemesan, nama_pemesan, alamat_pemesan, total_harga FROM tbpesanan"
                Dim conditions As New List(Of String)

                If Not String.IsNullOrEmpty(searchQuery) Then
                    conditions.Add("(id_pemesan LIKE @search OR nama_pemesan LIKE @search OR alamat_pemesan LIKE @search)")
                End If
                

                If conditions.Count > 0 Then
                    query &= " WHERE " & String.Join(" AND ", conditions)
                End If

                Dim cmd As MySqlCommand = New MySqlCommand(query, CONN)
                If Not String.IsNullOrEmpty(searchQuery) Then
                    cmd.Parameters.AddWithValue("@search", "%" & searchQuery & "%")
                End If

                Dim adapter As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim table As DataTable = New DataTable()
                adapter.Fill(table)
                DataGridView1.DataSource = table
            End Using
        Catch ex As MySqlException
            MessageBox.Show("An error occurred while loading the data: " & ex.Message)
        End Try
    End Sub

    '=====================================================================================================
    ' [3] Tombol cari 
    Private Sub btn_cari_Click(sender As Object, e As EventArgs) Handles btn_cari.Click
        Dim searchQuery As String = TextBox1.Text
        LoadOrderHistory(searchQuery)
    End Sub

    '=====================================================================================================
    ' [4] Tombol exit
    Private Sub btn_exit_Click(sender As Object, e As EventArgs) Handles btn_exit.Click
        MenuAdmin.Show()
        Me.Hide()
    End Sub
End Class
