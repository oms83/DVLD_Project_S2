using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess_Layer
{
    public class clsApplicationTypeData
    {
        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID, ref string ApplicationTypeTitle, ref float ApplicationFees)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From ApplicationTypes Where ApplicationTypeID = @ApplicationTypeID";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;
                    ApplicationTypeTitle = (string)Reader["ApplicationTypeTitle"];
                    ApplicationFees = Convert.ToSingle(Reader["ApplicationFees"]);

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

        public static int AddNewApplicationType(string ApplicationTypeTitle, float ApplicationFees)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            int ApplicationTypeID = -1;

            string Query = @"Insert Into ApplicationTypes(
                                            ApplicationTypeTitle,
                                            ApplicationFees
                                         )
                                        Values
                                        (
                                            @ApplicationTypeTitle,
                                            @ApplicationFees
                                        );
                                        Select Scope_Identity();
                                        ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            Command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);

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
                    ApplicationTypeID = InsertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }
            return ApplicationTypeID;
        }

        public static bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, float ApplicationFees)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update ApplicationTypes 
                             Set
                                ApplicationTypeTitle = @ApplicationTypeTitle,
                                ApplicationFees = @ApplicationFees
                             Where
                             ApplicationTypeID = @ApplicationTypeID;
                            ";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);
            Command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);


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

        public static bool DeleteApplicationType(int ApplicationTypeID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Delete From ApplicationTypes Where ApplicationTypeID = @ApplicationTypeID";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

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

        public static DataTable GetAllApplicationTypes()
        {
            DataTable dataTable = new DataTable();

            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From ApplicationTypes";

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

        public bool IsApplicationTypeExistByID(int ApplicationTypeID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "Select Found = 1 From ApplicationTypes Where ApplicationTypeID = @ApplicationTypeID";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

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
    }
}
