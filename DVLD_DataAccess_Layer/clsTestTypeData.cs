using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess_Layer
{
    public class clsTestTypeData
    {
        public static bool GetTestTypeInfoByTestTypeID(int TestTypeID, ref string TestTypeTitle, 
                                                             ref string TestTypeDescription, ref float TestTypeFees)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From TestTypes Where TestTypeID = @TestTypeID";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;

                    TestTypeTitle = (string)Reader["TestTypeTitle"];
                    TestTypeDescription = (string)Reader["TestTypeDescription"];
                    TestTypeFees = Convert.ToSingle(Reader["TestTypeFees"]);

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

        public static int AddNewTestType(string TestTypeTitle,
                                            string TestTypeDescription, float TestTypeFees)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            int TestTypeID = -1;

            string Query = @"Insert Into TestTypes(
                                            TestTypeTitle,
                                            TestTypeDescription,
                                            TestTypeFees
                                         )
                                        Values
                                        (
                                            @TestTypeTitle,
                                            @TestTypeDescription,
                                            @TestTypeFees                                              
                                        );
                                            
                                        Select Scope_Identity();
                                        ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            Command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            Command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);


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
                    TestTypeID = InsertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return TestTypeID;
        }
        public static bool UpdateTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, float TestTypeFees)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update TestTypes 
                                Set
                                    TestTypeTitle = @TestTypeTitle,
                                    TestTypeDescription = @TestTypeDescription,
                                    TestTypeFees = @TestTypeFees
                                Where
                                    TestTypeID = @TestTypeID;
                            ";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            Command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            Command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


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
        public static bool DeleteTestType(int TestTypeID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Delete From TestTypes Where TestTypeID = @TestTypeID";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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

        public static DataTable GetAllTestTypes()
        {
            DataTable dataTable = new DataTable();

            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From TestTypes";

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

        public static bool IsTestTypeExistByTestTypeID(int TestTypeID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "Select Found = 1 From TestTypes Where TestTypeID = @TestTypeID;";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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
