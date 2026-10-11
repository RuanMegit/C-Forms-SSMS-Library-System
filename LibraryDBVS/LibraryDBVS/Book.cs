using System;
using System.Data;
using System.Data.SqlClient;

namespace LibraryDBVS
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int TotalCopies { get; set; }

        private string connString = @"Server=.\SQLEXPRESS;Database=LibraryDB;Integrated Security=True;";

        public void Add()
        {
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_AddBook", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pTitle", Title);
            cmd.Parameters.AddWithValue("@pAuthor", Author);
            cmd.Parameters.AddWithValue("@pGenre", Genre);
            cmd.Parameters.AddWithValue("@pTotalCopies", TotalCopies);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void Update()
        {
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_UpdateBook", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pBookID", BookID);
            cmd.Parameters.AddWithValue("@pTitle", Title);
            cmd.Parameters.AddWithValue("@pAuthor", Author);
            cmd.Parameters.AddWithValue("@pGenre", Genre);
            cmd.Parameters.AddWithValue("@pTotalCopies", TotalCopies);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void Delete()
        {
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_DeleteBook", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pBookID", BookID);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public DataTable GetAll()
        {
            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("sp_GetAllBooks", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }
}