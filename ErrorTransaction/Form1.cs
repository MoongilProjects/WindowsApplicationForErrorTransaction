using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ErrorTransaction
{
    public partial class Form1 : Form
    {
        SqlDataAdapter da;
        int pageNumber = 0;
        Int64 totalRows = 0;
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
            lblDateTime.Text = DateTime.Now.ToString();
            rdlError.Checked = true;
            DisplayData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString();
            timer1.Start();
        }
        private void DisplayData(int displayLength = 2, int displayStart = 0, int sortCol = 5, string sortDir = "desc")
        {
            try
            {

                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection("Data Source=LAPTOP-982TEVDR\\MSSQLSERVER01;Initial Catalog=StudentDataBase;Integrated Security=true;"))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetErrorTransactionDetails", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@DisplayLength", SqlDbType.Int).Value = displayLength;
                        command.Parameters.Add("@DisplayStart", SqlDbType.Int).Value = displayStart;
                        command.Parameters.Add("@SortCol", SqlDbType.Int).Value = sortCol;
                        command.Parameters.Add("@SortDir", SqlDbType.NVarChar).Value = sortDir;
                        command.Parameters.Add("@Search", SqlDbType.NVarChar).Value = txtSearch.Text.Trim();
                        command.Parameters.Add("@IsResolved", SqlDbType.BigInt).Value = rdlResolve.Checked;
                        con.Open();
                        da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;
                        con.Close();
                        if (dt.Rows.Count > 0)
                        {
                            totalRows = Convert.ToInt64(dt.Rows[0]["TotalCount"]);

                            lblTotalCount.Text = string.Format("Total Records : {0}", totalRows);
                            Int64 divNext = 1;
                            if (totalRows > displayLength)
                            {
                                divNext = (totalRows / 2);
                            }

                            lblPageNumber.Text = string.Format("page {0}/{1}", pageNumber + 1, divNext);
                            // dt.Columns.Remove("RowNum


                            if (divNext == 0)
                            {
                                btnNext.Enabled = false;
                                btnPrevious.Enabled = false;
                            }
                            else
                            {
                                if (totalRows > displayLength && pageNumber != 0)
                                {
                                    if (divNext == pageNumber + 1)
                                    {
                                        btnNext.Enabled = false;
                                        if (pageNumber != 0)
                                        {
                                            btnPrevious.Enabled = true;
                                        }
                                    }
                                    else
                                    {
                                        btnNext.Enabled = true;
                                    }
                                }
                                else
                                {
                                    btnPrevious.Enabled = false;
                                    if (pageNumber + 1 != divNext)
                                    {
                                        btnNext.Enabled = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("No Record Found");
                            txtSearch.Focus();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            pageNumber = 0;
            DisplayData(2, pageNumber);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            DisplayData(2, pageNumber--);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            DisplayData(2, pageNumber++);
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
