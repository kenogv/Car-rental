using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KenanPROJEKT
{
    public partial class Check : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.SearchCustomers();
            }
        }

        private void SearchCustomers()
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["RentCarConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        string sql = "SELECT * FROM Reservations " +
                            "INNER JOIN " +
                            "Cars ON Reservations.ID = Cars.ID";
                        if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                        {
                            sql += " WHERE Reservations.Last_Name LIKE @Last_Name + '%' OR Reservations.First_Name LIKE @First_Name+ '%' OR Reservations.Phone_Number LIKE @Phone_Number+ '%'";
                            cmd.Parameters.AddWithValue("@Last_Name", txtSearch.Text.Trim());
                            cmd.Parameters.AddWithValue("@First_Name", txtSearch.Text.Trim());
                            cmd.Parameters.AddWithValue("@Phone_Number", txtSearch.Text.Trim());
                        }
                        cmd.CommandText = sql;
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            gvCustomers.DataSource = dt;
                            gvCustomers.DataBind();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string er = ex.ToString();

            }
        }
        protected void Search(object sender, EventArgs e)
        {
            this.SearchCustomers();
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            this.SearchCustomers();
        }
    }
}