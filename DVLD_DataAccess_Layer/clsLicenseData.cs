using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;

namespace DVLD_DataAccess_Layer
{
    public class clsLicenseData
    {
        public static bool GetLicenseInfoByID(int LicenseID, ref int LicenseClassID, ref int ApplicationID,
                                              ref int DriverID, ref int CreatedByUserID, 
                                              ref DateTime IssueDate, ref DateTime ExpirationDate,
                                              ref string Notes, ref float PaidFees, ref bool IsActive, ref short IssueReason)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From Licenses Where LicenseID = @LicenseID
                                                         ORDER BY LicenseID DESC

                            ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;

                    LicenseClassID = (int)Reader["LicenseClass"];
                    ApplicationID = (int)Reader["ApplicationID"];
                    DriverID = (int)Reader["DriverID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];

                    if (Reader["Notes"] != DBNull.Value)
                    {
                        Notes = (string)Reader["Notes"];
                    }
                    else
                    {
                        Notes = "";
                    }

                    PaidFees = Convert.ToSingle(Reader["PaidFees"]);
                    IsActive = (bool)Reader["IsActive"];

                    /*
                        1 - FirstTime,
                        2 - Renew,
                        3 - Replacement for Damaged,
                        4 - Replacement for Lost. 
                    */
                    IssueReason = Convert.ToInt16(Reader["IssueReason"]);


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


        public static bool GetOrdinaryDrivingLincese(ref int LicenseID, ref int LicenseClassID, ref int ApplicationID,
                                              int DriverID, ref int CreatedByUserID,
                                              ref DateTime IssueDate, ref DateTime ExpirationDate,
                                              ref string Notes, ref float PaidFees, ref bool IsActive, ref short IssueReason)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From Licenses Where DriverID = @DriverID And LicenseClass = 3";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@DriverID", DriverID);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;

                    LicenseClassID = (int)Reader["LicenseClass"];
                    ApplicationID = (int)Reader["ApplicationID"];
                    DriverID = (int)Reader["DriverID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];

                    if (Reader["Notes"] != DBNull.Value)
                    {
                        Notes = (string)Reader["Notes"];
                    }
                    else
                    {
                        Notes = "";
                    }

                    PaidFees = Convert.ToSingle(Reader["PaidFees"]);
                    IsActive = (bool)Reader["IsActive"];

                    /*
                        1 - FirstTime,
                        2 - Renew,
                        3 - Replacement for Damaged,
                        4 - Replacement for Lost. 
                    */
                    IssueReason = Convert.ToInt16(Reader["IssueReason"]);


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
        public static bool GetLicenseInfoByApplicationID(ref int LicenseID, ref int LicenseClassID, int ApplicationID,
                                              ref int DriverID, ref int CreatedByUserID,
                                              ref DateTime IssueDate, ref DateTime ExpirationDate,
                                              ref string Notes, ref float PaidFees, ref bool IsActive, ref short IssueReason)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From Licenses Where ApplicationID = @ApplicationID 
                                                         ORDER BY LicenseID DESC

                            ";

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

                    LicenseClassID = (int)Reader["LicenseClass"];
                    LicenseID = (int)Reader["LicenseID"];
                    DriverID = (int)Reader["DriverID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];

                    if (Reader["Notes"] != DBNull.Value)
                    {
                        Notes = (string)Reader["Notes"];
                    }
                    else
                    {
                        Notes = "";
                    }

                    PaidFees = Convert.ToSingle(Reader["PaidFees"]);
                    IsActive = (bool)Reader["IsActive"];

                    /*
                        1 - FirstTime,
                        2 - Renew,
                        3 - Replacement for Damaged,
                        4 - Replacement for Lost. 
                    */
                    IssueReason = Convert.ToInt16(Reader["IssueReason"]);


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
        public static bool GetLicenseInfoByDriverID(ref int LicenseID, int LicenseClassID, ref int ApplicationID,
                                              int DriverID, ref int CreatedByUserID,
                                              ref DateTime IssueDate, ref DateTime ExpirationDate,
                                              ref string Notes, ref float PaidFees, ref bool IsActive, ref short IssueReason)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From Licenses Where 
                             LicenseClass = @LicenseClassID 
                             AND DriverID = @DriverID
                             ORDER BY LicenseID DESC
                            ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;

                    LicenseID = (int)Reader["LicenseID"];
                    ApplicationID = (int)Reader["ApplicationID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];

                    if (Reader["Notes"] != DBNull.Value)
                    {
                        Notes = (string)Reader["Notes"];
                    }
                    else
                    {
                        Notes = "";
                    }

                    PaidFees = Convert.ToSingle(Reader["PaidFees"]);
                    IsActive = (bool)Reader["IsActive"];

                    /*
                        1 - FirstTime,
                        2 - Renew,
                        3 - Replacement for Damaged,
                        4 - Replacement for Lost. 
                    */
                    IssueReason = Convert.ToInt16(Reader["IssueReason"]);


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

        public static int AddNewLicense(int LicenseClassID, int ApplicationID,
                                        int DriverID, int CreatedByUserID,
                                        DateTime IssueDate, DateTime ExpirationDate,
                                        string Notes, float PaidFees, bool IsActive, short IssueReason)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            int LicenseID = -1;

            string Query = @"Insert Into Licenses(
                                            LicenseClass,
                                            ApplicationID,
                                            DriverID,
                                            CreatedByUserID,
                                            IssueDate,
                                            ExpirationDate,
                                            Notes,
                                            PaidFees,
                                            IsActive,
                                            IssueReason
                                         )
                                        Values
                                        (
                                            @LicenseClassID,
                                            @ApplicationID,
                                            @DriverID,
                                            @CreatedByUserID,
                                            @IssueDate,
                                            @ExpirationDate,
                                            @Notes,
                                            @PaidFees,
                                            @IsActive,
                                            @IssueReason
                                        );
                                            Select Scope_Identity();
                                        ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            Command.Parameters.AddWithValue("@Notes", Notes);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@IssueReason", IssueReason);

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
                    LicenseID = InsertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return LicenseID;
        }

        public static bool UpdateLicense(int LicenseID, int LicenseClassID, int ApplicationID,
                                         int DriverID, int CreatedByUserID,
                                         DateTime IssueDate, DateTime ExpirationDate,
                                         string Notes, float PaidFees, bool IsActive, short IssueReason)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update Licenses 
                                Set
                                    LicenseClass = @LicenseClassID,
                                    ApplicationID = @ApplicationID,
                                    DriverID = @DriverID,
                                    CreatedByUserID = @CreatedByUserID,
                                    IssueDate = @IssueDate,
                                    ExpirationDate = @ExpirationDate,
                                    Notes = @Notes,
                                    PaidFees = @PaidFees,
                                    IsActive = @IsActive,
                                    IssueReason = @IssueReason
                                Where
                                    LicenseID = @LicenseID;
                            ";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            Command.Parameters.AddWithValue("@Notes", Notes);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@IssueReason", IssueReason);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);


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

        public static bool DeleteLicense(int LicenseID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Delete From Licenses Where LicenseID = @LicenseID";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

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

        public static DataTable GetAllLicenses()
        {
            DataTable dataTable = new DataTable();

            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From Licenses";

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

        public static DataTable GetDriverLicenses(int DriverID)
        {
            DataTable dataTable = new DataTable();

            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT     
                           Licenses.LicenseID,
                           ApplicationID,
		                   LicenseClasses.ClassName, Licenses.IssueDate, 
		                   Licenses.ExpirationDate, Licenses.IsActive
                           FROM Licenses INNER JOIN
                                LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                            where DriverID=@DriverID
                            Order By IsActive Desc, ExpirationDate Desc";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DriverID", DriverID);

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
        public static bool IsLicenseExistByLicenseID(int LicenseID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "Select Found = 1 From Licenses Where LicenseID = @LicenseID;";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

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

        public static bool IsLicenseExistByApplicationID(int ApplicationID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "Select Found = 1 From Licenses Where ApplicationID = @ApplicationID;";

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

        public static bool IsLicenseExistByDriverIDAndLicenseClassID(int DriverID, int LicenseClassID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select Found = 1 From Licenses 
                             Where DriverID = @DriverID 
                             AND LicenseClass = @LicenseClassID;";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DriverID", DriverID);
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

        public static bool DeactiveteLicense(int LicenseID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update Licenses 
                                Set
                                    IsActive = 0
                                Where
                                    LicenseID = @LicenseID;
                            ";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);


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

        public static bool ActiveteLicense(int LicenseID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update Licenses 
                                Set
                                    IsActive = 1
                                Where
                                    LicenseID = @LicenseID;
                            ";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);


            Command.Parameters.AddWithValue("@LicenseID", LicenseID);


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
        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            /*
            string Query = @"Select Found = 1 from Applications 
                            Inner Join LocalDrivingLicenseApplications 
                            On LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                            Inner Join Licenses 
                            On Licenses.ApplicationID = Applications.ApplicationID
                            Where Applications.ApplicantPersonID = @PersonID
                            And LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                            And Applications.ApplicationTypeID = @ApplicationTypeID
                             ";
            */

            string Query = @"Select Found = 1 From Licenses
                             Inner Join Drivers On Drivers.DriverID = Licenses.DriverID
                             Where Licenses.LicenseClass = @LicenseClassID, 
                             And Drivers.PersonID = @PersonID";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
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

        public static int GetAnActiveLicenseByPersonID(int PersonID, int LicenseClassID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select Licenses.LicenseID
                             From Licenses 
                             Inner Join Drivers On Drivers.DriverID = Licenses.DriverID
                             Where Licenses.IsActive = 1 And Drivers.PersonID = @PersonID And Licenses.LicenseClass = @LicenseClassID
                             ";
            /*
                
                Select Licenses.LicenseID
                From People 
                Inner join Applications On People.PersonID = Applications.ApplicantPersonID 
                Inner join Licenses On Licenses.ApplicationID = Applications.ApplicationID
                Inner join LicenseClasses On Licenses.LicenseClass = LicenseClasses.LicenseClassID
                Where Licenses.IsActive = 1 And Applications.ApplicantPersonID = 1 And LicenseClasses.LicenseClassID = 1
 
            */

            SqlCommand Command = new SqlCommand(Query, Connection);

            int LicenseID = -1;

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int Inserted))
                {
                    LicenseID = Inserted;
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return LicenseID;
        }

    }
}
