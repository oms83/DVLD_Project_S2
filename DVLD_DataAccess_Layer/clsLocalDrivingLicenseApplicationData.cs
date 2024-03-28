using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess_Layer
{
    public class clsLocalDrivingLicenseApplicationData
    {
        public static bool GetLDLAppInfoByLDLAppID(int LocalDrivingLicenseApplicationID, 
                                                                                ref int ApplicationID, ref int LicenseClassID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From LocalDrivingLicenseApplications Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;

                    ApplicationID = (int)Reader["ApplicationID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];
  
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
        public static bool GetLDLAppInfoByApplicationID(ref int LocalDrivingLicenseApplicationID, int ApplicationID, 
                                                             ref int LicenseClassID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From LocalDrivingLicenseApplications Where ApplicationID = @ApplicationID";

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

                    LocalDrivingLicenseApplicationID = (int)Reader["LocalDrivingLicenseApplicationID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];

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
        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            int LocalDrivingLicenseApplicationID = -1;

            string Query = @"Insert Into LocalDrivingLicenseApplications(
                                            ApplicationID,
                                            LicenseClassID
                                         )
                                        Values
                                        (
                                            @ApplicationID,
                                            @LicenseClassID
                                        );
                                        Select Scope_Identity();
                                        ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);


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
                    LocalDrivingLicenseApplicationID = InsertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return LocalDrivingLicenseApplicationID;
        }

        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID,
                                                             int LicenseClassID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update LocalDrivingLicenseApplications 
                                Set
                                    ApplicationID = @ApplicationID,
                                    LicenseClassID = @LicenseClassID
                                Where
                                    LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;
                            ";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
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
        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Delete From LocalDrivingLicenseApplications Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

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

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dataTable = new DataTable();

            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT  
	                            LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID,
	                            LicenseClasses.ClassName,
	                            People.NationalNo,
	                            FullName = People.FirstName + ' ' + People.SecondName + ' ' + ISNULL(People.ThirdName, '') + ' ' + People.LastName,
	                            Applications.ApplicationDate,
	                            PassedTest = (
					                            SELECT COUNT(Tests.TestResult) 
					                            FROM TestAppointments 
					                            INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
					                            INNER JOIN TestTypes ON TestTypes.TestTypeID = TestAppointments.TestTypeID
					                            WHERE Tests.TestResult = 1 
					                            AND   LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
				                             ),
	                            Status = 
	                            CASE 
		                            WHEN Applications.ApplicationStatus = 1 THEN 'New'
		                            WHEN Applications.ApplicationStatus = 2 THEN 'Canceled'
		                            WHEN Applications.ApplicationStatus = 3 THEN 'Complated'
		                            ELSE 'Unknown'
	                            END
                                                        
                                FROM People 
                                INNER JOIN Applications ON People.PersonID = Applications.ApplicantPersonID
                                INNER JOIN LocalDrivingLicenseApplications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                                INNER JOIN LicenseClasses ON LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
                                ORDER BY LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID DESC


";

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

        public static bool IsLocalDrivingLicenseApplicationExistByID(int LocalDrivingLicenseApplicationID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "Select Found = 1 From LocalDrivingLicenseApplications Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

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


        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT TestResult 
                             FROM TestAppointments
                             INNER JOIN Tests 
                             ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                             Where Tests.TestResult = 1 
                             AND   TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                             AND   TestAppointments.TestTypeID = @TestTypeID ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
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

        public static bool DoesPersonPassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT TestResult 
                             FROM TestAppointments
                             INNER JOIN Tests 
                             ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                             Where Tests.TestResult = 1 
                             AND   TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                             ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

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

            return IsExist ;
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT COUNT(TestResult) 
                             FROM TestAppointments
                             INNER JOIN Tests 
                             ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                             Where Tests.TestResult = 1 
                             AND   TestAppointments.LocalDrivingLicenseApplicationID =  @LocalDrivingLicenseApplicationID
                             ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            byte CountOfTestPassed = 0;

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && byte.TryParse(Result.ToString(), out byte NumberOfTests))
                {
                    CountOfTestPassed = NumberOfTests;
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return CountOfTestPassed;
        }

        public static bool DoesPersonHasAnActiveApplication(int PersonID, int ApplicationTypeID, int LicenseClassID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select Found = 1 from Applications 
                             Inner Join LocalDrivingLicenseApplications 
                             On LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                             Where Applications.ApplicationStatus = 1
                             And Applications.ApplicantPersonID = @PersonID
                             And LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                             And Applications.ApplicationTypeID = @ApplicationTypeID 
                             ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static bool DoesPersonHasComplatedApplication(int PersonID, int ApplicationTypeID, int LicenseClassID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select Found = 1 from Applications 
                             Inner Join LocalDrivingLicenseApplications 
                             On LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                             Where Applications.ApplicationStatus = 3
                             And Applications.ApplicantPersonID = @PersonID
                             And LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                             And Applications.ApplicationTypeID = @ApplicationTypeID 
                             ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@PersonID", PersonID);

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
        public static byte TotalTrialPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT 
                             TotalTrail = (
                             				 SELECT COUNT(TestResult) 
                             				 FROM TestAppointments 
                             				 INNER JOIN Tests 
                             				 ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID 
                             				 WHERE TestTypeID = @TestType
                             				 And TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                             			 )
                             FROM LocalDrivingLicenseApplications
                             Where LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 


                             ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestType", TestTypeID);

            byte Trails = 0;

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && byte.TryParse(Result.ToString(), out byte NumberOfTrails))
                {
                    Trails = NumberOfTrails;
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return Trails;
        }

        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT top 1 Found=1
                             FROM LocalDrivingLicenseApplications 
                             INNER JOIN
                             TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                             Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID

                             WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             AND   TestAppointments.TestTypeID = @TestTypeID
                             ORDER BY TestAppointments.TestAppointmentID desc";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            bool IsExist = false;

            try
            {
                Connection.Open();

                object result = Command.ExecuteScalar();

                if (result != null)
                {
                    IsExist = true;
                }

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
