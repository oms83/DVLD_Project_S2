using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess_Layer
{
    public class clsTestData
    {
        public static bool GetTestInfoByTestID(int TestID, ref int TestAppointmentID,
                                                       ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From Tests Where TestID = @TestID";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@TestID", TestID);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;

                    TestAppointmentID = (int)Reader["TestAppointmentID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    TestResult = (bool)Reader["TestResult"];
                    if (Reader["Notes"] != DBNull.Value)
                    {
                        Notes = (string)Reader["Notes"];
                    }
                    else
                    {
                        Notes = "";
                    }    

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

        public static bool GetTestInfoByTestAppointmentID(ref int TestID, int TestAppointmentID,
                                                       ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From Tests Where TestAppointmentID = @TestAppointmentID";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;

                    TestID = (int)Reader["TestID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    TestResult = (bool)Reader["TestResult"];
                    if (Reader["Notes"] != DBNull.Value)
                    {
                        Notes = (string)Reader["Notes"];
                    }
                    else
                    {
                        Notes = "";
                    }

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

        public static int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            int TestID = -1;

            string Query = @"   Update TestAppointments Set IsLocked = 1;
                                Insert Into Tests(
                                    TestAppointmentID,
                                    TestResult,
                                    CreatedByUserID,
                                    Notes
                                    )
                                Values
                                (
                                    @TestAppointmentID,
                                    @TestResult,
                                    @CreatedByUserID,
                                    @Notes                                             
                                );
                                            
                                Select Scope_Identity();
                                        ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestResult", TestResult);
            if (Notes == "" || Notes == null)
            {
                Command.Parameters.AddWithValue("@Notes", System.DBNull.Value);
            }
            else
            {
                Command.Parameters.AddWithValue("@Notes", Notes);
            }
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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
                    TestID = InsertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return TestID;
        }


        public static bool UpdateTest(int TestID, int TestAppointmentID,
                                          bool TestResult, string Notes, int CreatedByUserID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update Tests 
                                Set
                                    TestAppointmentID = @TestAppointmentID,
                                    TestResult = @TestResult,
                                    CreatedByUserID = @CreatedByUserID,
                                    Notes = @Notes
                                Where
                                    TestID = @TestID;
                            ";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestResult", TestResult);
            if (Notes == "" || Notes == null)
            {
                Command.Parameters.AddWithValue("@Notes", System.DBNull.Value);
            }
            else
            {
                Command.Parameters.AddWithValue("@Notes", Notes);
            }
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@TestID", TestID);

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

        public static bool DeleteTest(int TestID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Delete From Tests Where TestID = @TestID";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestID", TestID);
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

        public static DataTable GetAllTests()
        {
            DataTable dataTable = new DataTable();

            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From Tests";

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

        public static bool IsTestExistByTestID(int TestID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "Select Found = 1 From Tests Where TestID = @TestID;";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestID", TestID);

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
