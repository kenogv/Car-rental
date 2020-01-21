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
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlCar.DataBind();
                ddlCar.SelectedIndex = 0;
                getInfo();
            }
        }
        string er = "";
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["RentCarConnectionString"].ConnectionString;
            try
            {
                if (String.IsNullOrEmpty(txtFname.Value) || String.IsNullOrEmpty(txtLname.Value) || String.IsNullOrEmpty(txtEmail.Value) || String.IsNullOrEmpty(txtPhone.Value))
                {
                    lblFillAll.Visible = true;
                }
                else
                {
                    using (SqlConnection connection = new SqlConnection(constr))
                    {
                        String query = "INSERT INTO Reservations (First_Name, Last_Name, Email, Phone_Number, CarID, Date_Start, Date_End) VALUES (@firstname, @lastname, @email, @phone, @carid, @start, @end)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@firstname", txtFname.Value);
                            command.Parameters.AddWithValue("@lastname", txtLname.Value);
                            command.Parameters.AddWithValue("@email", txtEmail.Value);
                            command.Parameters.AddWithValue("@phone", txtPhone.Value);
                            command.Parameters.AddWithValue("@carid", ddlCar.SelectedValue);
                            command.Parameters.AddWithValue("@start", datepicker.Value);
                            command.Parameters.AddWithValue("@end", datepicker2.Value);

                            connection.Open();
                            int result = command.ExecuteNonQuery();

                            // Check Error
                            if (result < 0)
                            {
                                lblFillAll.Text = "Error inserting data into Database!";
                                lblFillAll.Visible = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                er = ex.ToString();
            }
            finally
            {
                txtFname.Value = "";
                txtLname.Value = "";
                txtEmail.Value = "";
                txtPhone.Value = "";
                datepicker.Value = "";
                datepicker2.Value = "";
                if (String.IsNullOrEmpty(er))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Car has been booked in your name!')", true);
            }
        }

        protected void ddlCar_SelectedIndexChanged(object sender, EventArgs e)
        {
            getInfo();
        }
        protected void getInfo()
        {
            string constr = ConfigurationManager.ConnectionStrings["RentCarConnectionString"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    String query = "SELECT Model, Year FROM Cars WHERE ID='" + ddlCar.SelectedValue + "'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.HasRows)
                            {
                                // Read advances to the next row.
                                while (reader.Read())
                                {

                                    lblModel.Text = (reader["Model"].ToString());
                                    lblYear.Text = (reader["Year"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string er = ex.ToString();
            }
            finally
            {

            }
        }
    }
}