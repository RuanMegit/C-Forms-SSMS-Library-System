using System;
using System.Data;
using System.Data.SqlClient;

namespace LibraryDBVS
{
    public class Loan
    {
        public int LoanID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int BookID { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }

        private string connString = @"Server=.\SQLEXPRESS;Database=LibraryDB;Integrated Security=True;";

        public void Add()
        {
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_AddLoan", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pFirstName", FirstName);
            cmd.Parameters.AddWithValue("@pLastName", LastName);
            cmd.Parameters.AddWithValue("@pPhone", Phone);
            cmd.Parameters.AddWithValue("@pBookID", BookID);
            cmd.Parameters.AddWithValue("@pLoanDate", LoanDate);
            cmd.Parameters.AddWithValue("@pDueDate", DueDate);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void Update()
        {
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_UpdateLoan", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pLoanID", LoanID);
            cmd.Parameters.AddWithValue("@pFirstName", FirstName);
            cmd.Parameters.AddWithValue("@pLastName", LastName);
            cmd.Parameters.AddWithValue("@pPhone", Phone);
            cmd.Parameters.AddWithValue("@pBookID", BookID);
            cmd.Parameters.AddWithValue("@pLoanDate", LoanDate);
            cmd.Parameters.AddWithValue("@pDueDate", DueDate);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void Delete()
        {
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_DeleteLoan", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pLoanID", LoanID);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public DataTable GetAll()
        {
            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("sp_GetAllLoans", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable Search(string term)
        {
            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("sp_SearchLoans", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pSearchTerm", term);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }
}