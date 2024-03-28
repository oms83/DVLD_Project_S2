using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess_Layer
{
    public class clsApplicationData
    {
        public static bool GetApplicationInfoByApplicationID(int ApplicationID, ref int ApplicantPersonID, ref int ApplicationTypeID,
                                                      ref int CreatedByUserID, ref DateTime ApplicationDate, ref DateTime LastStatusDate, 
                                                      ref byte ApplicationStatus, ref float PaidFees)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From Applications Where ApplicationID = @ApplicationID";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;

                    ApplicantPersonID = (int)Reader["ApplicantPersonID"];
                    ApplicationTypeID = (int)Reader["ApplicationTypeID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    ApplicationDate = (DateTime)Reader["ApplicationDate"];
                    LastStatusDate = (DateTime)Reader["LastStatusDate"];
                    ApplicationStatus = (byte)Reader["ApplicationStatus"];
                    PaidFees = Convert.ToSingle(Reader["PaidFees"]);

                    Reader.Close();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetApplicationInfoByApplicantPersonID(ref int ApplicationID, int ApplicantPersonID, ref int ApplicationTypeID,
                                                      ref int CreatedByUserID, ref DateTime ApplicationDate, ref DateTime LastStatusDate,
                                                      ref byte ApplicationStatus, ref float PaidFees)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From Applications Where ApplicantPersonID = @ApplicantPersonID";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;

                    ApplicationID = (int)Reader["ApplicationID"];
                    ApplicationTypeID = (int)Reader["ApplicationTypeID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    ApplicationDate = (DateTime)Reader["ApplicationDate"];
                    LastStatusDate = (DateTime)Reader["LastStatusDate"];
                    ApplicationStatus = (byte)Reader["ApplicationStatus"];
                    PaidFees = (float)Reader["PaidFees"];

                    Reader.Close();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        public static int AddNewApplication(int ApplicantPersonID, int ApplicationTypeID, int CreatedByUserID, 
                                                DateTime ApplicationDate, DateTime LastStatusDate, 
                                                byte ApplicationStatus, float PaidFees)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            int ApplicationID = -1;

            string Query = @"Insert Into Applications(
                                            ApplicantPersonID,
                                            ApplicationTypeID,
                                            CreatedByUserID,
                                            ApplicationDate,
                                            LastStatusDate,
                                            ApplicationStatus,
                                            PaidFees
                                         )
                                        Values
                                        (
                                            @ApplicantPersonID,
                                            @ApplicationTypeID,
                                            @CreatedByUserID,
                                            @ApplicationDate,
                                            @LastStatusDate,
                                            @ApplicationStatus,
                                            @PaidFees
                                        );
                                        Select Scope_Identity();
                                        ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);

            /*
             
                ExecuteScalar() method in C# executes a SQL query against the database and returns the value of the first column 
                of the first row in the result set. It's commonly used for queries that return a single value, such as 
                aggregate functions (COUNT, MAX, MIN, SUM, AVG, etc.) or queries that return a single value.
             
                For example, you can use a query like SELECT COUNT(*) FROM TableName to get the total number of rows in a table, 
                and then use the ExecuteScalar() method to execute this query and retrieve the result. In this case, 
                the returned value will be the total row count.

            */

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int InsertedID))
                {
                    ApplicationID = InsertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return ApplicationID;
        }
        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, int ApplicationTypeID, int CreatedByUserID,
                                                DateTime ApplicationDate, DateTime LastStatusDate,
                                                byte ApplicationStatus, float PaidFees)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update Applications 
                                Set
                                    ApplicantPersonID = @ApplicantPersonID,
                                    ApplicationTypeID = @ApplicationTypeID,
                                    CreatedByUserID = @CreatedByUserID,
                                    ApplicationDate = @ApplicationDate,
                                    LastStatusDate = @LastStatusDate,
                                    ApplicationStatus = @ApplicationStatus,
                                    PaidFees = @PaidFees
                                Where
                                    ApplicationID = @ApplicationID;
                            ";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);


            int AffectedRows = -1;
            try
            {
                Connection.Open();
                AffectedRows = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Connection.Close();
            }
            return (AffectedRows > 0);
        }
        public static bool DeleteApplication(int ApplicationID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Delete From Applications Where ApplicationID = @ApplicationID";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            int AffectedRows = -1;
            try
            {
                Connection.Open();
                AffectedRows = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return (AffectedRows > 0);
        }

        public static DataTable GetAllApplications()
        {
            DataTable dataTable = new DataTable();

            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From Applications";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();

                // DataReader: It is used to read data from data source.
                // The DbDataReader is a base class for all DataReader objects.

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    dataTable.Load(Reader);
                }

                Reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return dataTable;
        }

        public static bool IsApplicationExistByApplicationID(int ApplicationID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "Select Found = 1 From Applications Where ApplicationID = @ApplicationID;";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            bool IsExist = false;

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                IsExist = Reader.HasRows;

                Reader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return IsExist;
        }

        public static bool IsApplicationExistByApplicantPersonID(int ApplicantPersonID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "Select Found = 1 From Applications Where ApplicantPersonID = @ApplicantPersonID;";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);

            bool IsExist = false;

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                IsExist = Reader.HasRows;

                Reader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return IsExist;
        }

        public static bool ChangeStatus(int ApplicationID, byte ApplicationStatus)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update Applications 
                                Set
                                    ApplicationStatus = @ApplicationStatus
                                Where
                                    ApplicationID = @ApplicationID;
                            ";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);


            int AffectedRows = -1;
            try
            {
                Connection.Open();
                AffectedRows = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return (AffectedRows > 0);
        }


    }
}
