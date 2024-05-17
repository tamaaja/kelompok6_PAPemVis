Imports MySql.Data.MySqlClient

Module DatabaseConnector
    Public Function ConnectToDatabase() As MySqlConnection
        Dim connectionString As String = "server=localhost;userid=root;password=;database=kelompok6"
        Return New MySqlConnection(connectionString)
    End Function
End Module

