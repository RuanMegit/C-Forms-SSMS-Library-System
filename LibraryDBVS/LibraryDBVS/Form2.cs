using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LibraryDBVS
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadBooks();
            LoadLoans();
            LoadBookLoan();
        }

        // Books

        //Loading all records to datagridview
        private void LoadBooks()
        {
            Book b = new Book();
            dgvBooks.DataSource = b.GetAll();
        }

        //when clicking an row of a book data, the data will be filled into the textbox to do operations on
        private void dgvBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvBooks.Rows[e.RowIndex];
            txtBookID.Text = row.Cells["BookID"].Value.ToString();
            txtTitle.Text = row.Cells["Title"].Value.ToString();
            txtAuthor.Text = row.Cells["Author"].Value.ToString();
            txtGenre.Text = row.Cells["Genre"].Value.ToString();
            txtTotalCopies.Text = row.Cells["TotalCopies"].Value.ToString();
        }

        //Adding a new book information to DB table with the requirement that it must contain Title, Author, TCopies and TCopies must be above 0.
        private void btnAddBook_Click(object sender, EventArgs e)
        {

            int totalCopies;

            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtAuthor.Text) || string.IsNullOrWhiteSpace(txtTotalCopies.Text))
            {
                MessageBox.Show("Please fill in Title, Author and Total Copies.");
                return;
            }

            if (!int.TryParse(txtTotalCopies.Text, out totalCopies) ||
                totalCopies < 0)
            {
                MessageBox.Show("Total Copies must be a number above 0");
                return;
            }


            Book b = new Book();
            b.Title = txtTitle.Text;
            b.Author = txtAuthor.Text;
            b.Genre = txtGenre.Text;
            b.TotalCopies = int.Parse(txtTotalCopies.Text);
            b.Add();
            LoadBooks();
            LoadBookLoan();
            ClearBookTxtBox();
        }

        //Update an existing Book by clicking on a row and editing the data and submiting that to DB table
        private void btnUpdateBook_Click(object sender, EventArgs e)
        {
            if (txtBookID.Text == "")
            {
                MessageBox.Show("Click a book in the grid first.");
                return;
            }

            int totalCopies;
            if (!int.TryParse(txtTotalCopies.Text, out totalCopies) ||
                totalCopies < 0)
            {
                MessageBox.Show("Total Copies must be a number above 0");
                return;
            }

            Book b = new Book();
            b.BookID = int.Parse(txtBookID.Text);
            b.Title = txtTitle.Text;
            b.Author = txtAuthor.Text;
            b.Genre = txtGenre.Text;
            b.TotalCopies = int.Parse(txtTotalCopies.Text);
            b.Update();

            LoadBooks();
            LoadBookLoan();
            ClearBookTxtBox();
        }

        private void btnDeleteBook_Click(object sender, EventArgs e)
        {
            if (txtBookID.Text == "")
            {
                MessageBox.Show("Click a book in the grid first.");
                return;
            }

            DialogResult answer = MessageBox.Show("Delete this book?", "Confirm", MessageBoxButtons.YesNo);
            if (answer == DialogResult.No) return;

            try
            {
                Book b = new Book();
                b.BookID = int.Parse(txtBookID.Text);
                b.Delete();

                LoadBooks();
                LoadBookLoan();
                ClearBookTxtBox();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    MessageBox.Show("Cannot delete - this book has loans.");
                else
                    MessageBox.Show(ex.Message);
            }
        }

        private void btnClearBook_Click(object sender, EventArgs e)
        {
            ClearBookTxtBox();
        }

        private void ClearBookTxtBox()
        {
            txtBookID.Text = "";
            txtTitle.Text = "";
            txtAuthor.Text = "";
            txtGenre.Text = "";
            txtTotalCopies.Text = "";
        }





        // Loans

        private void LoadLoans()
        {
            Loan l = new Loan();
            dgvLoans.DataSource = l.GetAll();
        }

        private void LoadBookLoan()
        {
            Book b = new Book();
            cmbBooks.DataSource = b.GetAll();
            cmbBooks.DisplayMember = "Title";
            cmbBooks.ValueMember = "BookID";
        }

        private void dgvLoans_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvLoans.Rows[e.RowIndex];
            txtLoanID.Text = row.Cells["LoanID"].Value.ToString();
            txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
            txtLastName.Text = row.Cells["LastName"].Value.ToString();
            txtPhone.Text = row.Cells["Phone"].Value.ToString();
            cmbBooks.SelectedValue = row.Cells["BookID"].Value;
            dtpLoanDate.Value = Convert.ToDateTime(row.Cells["LoanDate"].Value);
            dtpDueDate.Value = Convert.ToDateTime(row.Cells["DueDate"].Value);
        }

        private void btnAddLoan_Click(object sender, EventArgs e)
        {
            if (txtFirstName.Text == "" || txtLastName.Text == "")
            {
                MessageBox.Show("Fill in FirstName and LastName.");
                return;
            }
            if (cmbBooks.SelectedValue == null)
            {
                MessageBox.Show("Select a book.");
                return;
            }
            if (dtpDueDate.Value.Date <= dtpLoanDate.Value.Date)
            {
                MessageBox.Show("Due date must be after loan date.");
                return;
            }

            Loan l = new Loan();
            l.FirstName = txtFirstName.Text;
            l.LastName = txtLastName.Text;
            l.Phone = txtPhone.Text;
            l.BookID = (int)cmbBooks.SelectedValue;
            l.LoanDate = dtpLoanDate.Value;
            l.DueDate = dtpDueDate.Value;
            l.Add();

            LoadLoans();
            ClearLoan();
        }

        private void btnUpdateLoan_Click(object sender, EventArgs e)
        {
            if (txtLoanID.Text == "")
            {
                MessageBox.Show("Click a loan in the grid first.");
                return;
            }

            Loan l = new Loan();
            l.LoanID = int.Parse(txtLoanID.Text);
            l.FirstName = txtFirstName.Text;
            l.LastName = txtLastName.Text;
            l.Phone = txtPhone.Text;
            l.BookID = (int)cmbBooks.SelectedValue;
            l.LoanDate = dtpLoanDate.Value;
            l.DueDate = dtpDueDate.Value;
            l.Update();

            LoadLoans();
            ClearLoan();
        }

        private void btnDeleteLoan_Click(object sender, EventArgs e)
        {
            if (txtLoanID.Text == "")
            {
                MessageBox.Show("Click a loan in the grid first.");
                return;
            }

            DialogResult answer = MessageBox.Show("Delete this loan?", "Confirm", MessageBoxButtons.YesNo);
            if (answer == DialogResult.No) return;

            Loan l = new Loan();
            l.LoanID = int.Parse(txtLoanID.Text);
            l.Delete();

            LoadLoans();
            ClearLoan();
        }

        private void btnClearLoan_Click(object sender, EventArgs e)
        {
            ClearLoan();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Loan l = new Loan();
            dgvLoans.DataSource = l.Search(txtSearch.Text);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadLoans();
        }

        private void ClearLoan()
        {
            txtLoanID.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtPhone.Text = "";
            dtpLoanDate.Value = DateTime.Now;
            dtpDueDate.Value = DateTime.Now.AddDays(14);
        }

        private void txtBookID_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAuthor_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtGenre_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTotalCopies_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLoanID_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbBooks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtpLoanDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpDueDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
