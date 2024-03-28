using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess_Layer
{
    public class clsLicenseClassData
    {
        public static bool GetLicenseClassInfoByID(int LicenseClassID, ref string ClassName, 
                                                    ref string ClassDescription, ref byte MinimumAllowedAge, 
                                                    ref byte DefaultValidityLength, ref float ClassFees)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From LicenseClasses Where LicenseClassID = @LicenseClassID";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;

                    ClassName = (string)Reader["ClassName"];
                    ClassDescription = (string)Reader["ClassDescription"];
                    MinimumAllowedAge = (byte)Reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)Reader["DefaultValidityLength"];
                    ClassFees = Convert.ToSingle(Reader["ClassFees"]);
                    

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
        public static bool GetLicenseClassInfoByClassName(ref int LicenseClassID, string ClassName,
                                                   ref string ClassDescription, ref byte MinimumAllowedAge,
                                                   ref byte DefaultValidityLength, ref float ClassFees)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From LicenseClasses Where ClassName = @ClassName";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@ClassName", ClassName);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;

                    LicenseClassID = (int)Reader["LicenseClassID"];
                    ClassDescription = (string)Reader["ClassDescription"];
                    MinimumAllowedAge = (byte)Reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)Reader["DefaultValidityLength"];
                    ClassFees = Convert.ToSingle(Reader["ClassFees"]);


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

        public static int AddNewLicenseClass(string ClassName, string ClassDescription,
                                            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            int LicenseClassID = -1;

            string Query = @"Insert Into LicenseClasses(
                                            ClassName,
                                            ClassDescription,
                                            MinimumAllowedAge,
                                            DefaultValidityLength,
                                            ClassFees
                                         )
                                        Values
                                        (
                                            @ClassName,
                                            @ClassDescription,
                                            @MinimumAllowedAge,
                                            @DefaultValidityLength,
                                            @ClassFees
                                        );
                                        Select Scope_Identity();
                                        ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ClassName", ClassName);
            Command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            Command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
            Command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            Command.Parameters.AddWithValue("@ClassFees", ClassFees);

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
                    LicenseClassID = InsertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return LicenseClassID;
        }

        public static bool UpdateLicenseClass(int LicenseClassID, string ClassName, string ClassDescription,
                                              byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update LicenseClasses 
                                Set
                                    ClassName = @ClassName,
                                    ClassDescription = @ClassDescription,
                                    MinimumAllowedAge = @MinimumAllowedAge,
                                    DefaultValidityLength = @DefaultValidityLength,
                                    ClassFees = @ClassFees
                                Where
                                    LicenseClassID = @LicenseClassID;
                            ";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@ClassName", ClassName);
            Command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            Command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
            Command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            Command.Parameters.AddWithValue("@ClassFees", ClassFees);


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

        public static bool DeleteLicenseClass(int LicenseClassID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Delete From LicenseClasses Where LicenseClassID = @LicenseClassID";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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

        public static DataTable GetAllLicenseClasses()
        {
            DataTable dataTable = new DataTable();

            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From LicenseClasses";

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

        public static bool IsLicenseClassExistByLicenseClassID(int LicenseClassID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "Select Found = 1 From LicenseClasses Where LicenseClassID = @LicenseClassID;";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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
