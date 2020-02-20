using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using CRUD_Operations.General;

namespace CRUD_Operations
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        public int StudentID;

        private void Form1_Load(object sender, EventArgs e)
        {
            GetStudentsRecord();
        }

        private void GetStudentsRecord()
        {
            using (SqlConnection con = new SqlConnection(ApplicationConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Students_GetRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();
                    dt.Load(sdr);

                    StudentRecordDataGridView.DataSource = dt;
                }
            }
        }


        private bool IsValid()
        {
            if(txtStudentName.Text == string.Empty)
            {
                MessageBox.Show("Student Name is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ResetFormControls();
        }

        private void ResetFormControls()
        {
            StudentID = 0;
            txtStudentName.Clear();
            txtFatherName.Clear();
            txtRollNumber.Clear();
            txtMobile.Clear();
            txtAddress.Clear();

            txtStudentName.Focus();

            SaveStudentButton.Text = "Save Student";
        }

        private void StudentRecordDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            StudentID = Convert.ToInt32(StudentRecordDataGridView.SelectedRows[0].Cells[0].Value);
            txtStudentName.Text = StudentRecordDataGridView.SelectedRows[0].Cells[1].Value.ToString();
            txtFatherName.Text = StudentRecordDataGridView.SelectedRows[0].Cells[2].Value.ToString();
            txtRollNumber.Text = StudentRecordDataGridView.SelectedRows[0].Cells[3].Value.ToString();
            txtAddress.Text = StudentRecordDataGridView.SelectedRows[0].Cells[4].Value.ToString();
            txtMobile.Text = StudentRecordDataGridView.SelectedRows[0].Cells[5].Value.ToString();

            SaveStudentButton.Text = "Update Student";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SearchStudentByStudentName();
        }

        private void SearchStudentByStudentName()
        {
            using (SqlConnection con = new SqlConnection(ApplicationConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Students_SearchByName", con))
                {
                    DataTable dt = new DataTable();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentName", txtSearch.Text);
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();
                    dt.Load(sdr);

                    if (dt == null)
                    {
                        MessageBox.Show("No Student " + txtStudentName.Text + " is found in the database, Please search with correct name", "Not Found");
                    }
                    else
                    {
                        StudentRecordDataGridView.DataSource = dt;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                using (SqlConnection con = new SqlConnection(ApplicationConnection.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Students_SaveOrUpdate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", txtStudentName.Text);
                        cmd.Parameters.AddWithValue("@FatherName", txtFatherName.Text);
                        cmd.Parameters.AddWithValue("@Roll", txtRollNumber.Text);
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                        cmd.Parameters.AddWithValue("@StudentId", this.StudentID);


                        if (con.State != ConnectionState.Open)
                            con.Open();

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Student Information is successfully saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetStudentsRecord();
                ResetFormControls();
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (StudentID > 0)
            {
                using (SqlConnection con = new SqlConnection(ApplicationConnection.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Students_Delete", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StudentId", this.StudentID);

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Student is deleted from the system", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                GetStudentsRecord();
                ResetFormControls();
            }
            else
            {
                MessageBox.Show("Please Select an student to delete", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchStudentByStudentName();
            }
                
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text == string.Empty)
                GetStudentsRecord();
        }
    }
}
