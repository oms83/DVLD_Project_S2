using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess_Layer
{
    public class clsTestAppointmentData
    {
        public static bool GetTestAppointmentInfoByID(int TestAppointmentID, ref int TestTypeID, 
                                                      ref int LocalDrivingLicenseApplicationID, ref int CreatedByUserID, 
                                                      ref DateTime AppointmentDate, ref float PaidFees,
                                                      ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From TestAppointments Where TestAppointmentID = @TestAppointmentID Order by TestAppointmentID Desc";

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

                    LocalDrivingLicenseApplicationID = (int)Reader["LocalDrivingLicenseApplicationID"];

                    if (Reader["RetakeTestApplicationID"] != System.DBNull.Value)
                        RetakeTestApplicationID = (int)Reader["RetakeTestApplicationID"];
                    else
                        RetakeTestApplicationID = -1; 

                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    TestTypeID = (int)Reader["TestTypeID"];
                    AppointmentDate = (DateTime)Reader["AppointmentDate"];
                    PaidFees = Convert.ToSingle(Reader["PaidFees"]);
                    IsLocked = (bool)Reader["IsLocked"];

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

        public static bool GetTestAppointmentInfoByLDLAppIDAndTestType(ref int TestAppointmentID, int TestTypeID,
                                                      int LocalDrivingLicenseApplicationID, ref int CreatedByUserID,
                                                      ref DateTime AppointmentDate, ref float PaidFees,
                                                      ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From TestAppointments Where 
                                 LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             And TestTypeID = @TestTypeID
                             Order by TestAppointmentID Desc";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;

                    LocalDrivingLicenseApplicationID = (int)Reader["LocalDrivingLicenseApplicationID"];

                    if (Reader["RetakeTestApplicationID"] != System.DBNull.Value)
                        RetakeTestApplicationID = (int)Reader["RetakeTestApplicationID"];
                    else
                        RetakeTestApplicationID = -1;

                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    TestTypeID = (int)Reader["TestTypeID"];
                    AppointmentDate = (DateTime)Reader["AppointmentDate"];
                    PaidFees = (float)Reader["PaidFees"];
                    IsLocked = (bool)Reader["IsLocked"];

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

        public static int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, 
                                            int CreatedByUserID, DateTime AppointmentDate, 
                                            float PaidFees, bool IsLocked, int RetakeTestApplicationID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            int TestAppointmentID = -1;

            string Query = @"Insert Into TestAppointments(
                                            TestTypeID,
                                            LocalDrivingLicenseApplicationID,
                                            CreatedByUserID,
                                            AppointmentDate,
                                            IsLocked,
                                            RetakeTestApplicationID,
                                            PaidFees
                                         )
                                        Values
                                        (
                                            @TestTypeID,
                                            @LocalDrivingLicenseApplicationID,
                                            @CreatedByUserID,
                                            @AppointmentDate,
                                            @IsLocked,
                                            @RetakeTestApplicationID,
                                            @PaidFees
                                        );
                                        Select Scope_Identity();
                                        ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@IsLocked", IsLocked);

            if (RetakeTestApplicationID > -1)
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
            else
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", System.DBNull.Value);

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
                    TestAppointmentID = InsertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return TestAppointmentID;
        }
        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID,
                                            int CreatedByUserID, DateTime AppointmentDate,
                                            float PaidFees, bool IsLocked, int RetakeTestApplicationID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update TestAppointments 
                                Set
                                    TestTypeID = @TestTypeID,
                                    LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                                    CreatedByUserID = @CreatedByUserID,
                                    AppointmentDate = @AppointmentDate,
                                    IsLocked = @IsLocked,
                                    RetakeTestApplicationID = @RetakeTestApplicationID,
                                    PaidFees = @PaidFees
                                Where
                                    TestAppointmentID = @TestAppointmentID;
                            ";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@IsLocked", IsLocked);

            if (RetakeTestApplicationID != -1)
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
            else
                Command.Parameters.AddWithValue("@RetakeTestApplicationID", System.DBNull.Value);

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
        public static bool DeleteTestAppointment(int TestAppointmentID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Delete From TestAppointments Where TestAppointmentID = @TestAppointmentID";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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

        public static DataTable GetAllTestAppointments()
        {
            DataTable dataTable = new DataTable();

            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From TestAppointments ORDER BY TestAppointmentID DESC";

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


        public static DataTable GetTestAppointmentByTestTypeIDAndLDLApp(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dataTable = new DataTable();

            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select TestAppointmentID, AppointmentDate, PaidFees, IsLocked
                             From   TestAppointments 
                             Where  TestTypeID = @TestTypeID
                             And    LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             ORDER BY TestAppointmentID DESC";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);
            
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

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

        public static DataTable GetTestAppointmentsByLDLIDAndTestTypeID(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dataTable = new DataTable();

            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT 
                            	TestAppointmentID, 
                            	AppointmentDate, 
                            	PaidFees, 
                            	IsLocked 
                            FROM  TestAppointments 
                            WHERE TestTypeID = @TestTypeID 
                            AND LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                            ORDER BY TestAppointmentID DESC
                            ";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID",  LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID",  TestTypeID);


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

        public static bool IsTestAppointmentExistByAppointmentID(int TestAppointmentID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "Select Found = 1 From TestAppointments Where TestAppointmentID = @TestAppointmentID;";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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

        public static int GetTestID(int TestAppointmentID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);



            string Query = @"Select TestID Where TestAppointmentID = @TestAppointmentID";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            /*
             
                ExecuteScalar() method in C# executes a SQL query against the database and returns the value of the first column 
                of the first row in the result set. It's commonly used for queries that return a single value, such as 
                aggregate functions (COUNT, MAX, MIN, SUM, AVG, etc.) or queries that return a single value.
             
                For example, you can use a query like SELECT COUNT(*) FROM TableName to get the total number of rows in a table, 
                and then use the ExecuteScalar() method to execute this query and retrieve the result. In this case, 
                the returned value will be the total row count.

            */

            int TestID = -1;
            try
            {
                Connection.Open();
                
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    TestID = (int)Reader["TestID"];
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

            return TestID;
        }

        public static bool IsThereAnActiveScheduleTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT Found = 1 
                             FROM LocalDrivingLicenseApplications 
                             INNER JOIN TestAppointments 
                             ON TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                             WHERE TestTypeID = @TestTypeID
                             AND IsLocked = 0
                             AND LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

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
