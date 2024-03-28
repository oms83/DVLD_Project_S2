using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DVLD_DataAccess_Layer
{
    public class clsDetainedLicenseData
    {

        public static bool GetDetainedLicenseInfoByID(int DetainID, ref int LicenseID, ref int ReleasedByUserID,
                                              ref int ReleaseApplicationID, ref int CreatedByUserID,
                                              ref DateTime ReleaseDate, ref DateTime DetainDate,
                                              ref float FineFees, ref bool IsReleased)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From DetainedLicenses Where DetainID = @DetainID Order By LicenseID DESC";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@DetainID", DetainID);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;

                    LicenseID = (int)Reader["LicenseID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    DetainDate = (DateTime)Reader["DetainDate"];
                    IsReleased = (bool)Reader["IsReleased"];
                    FineFees = Convert.ToSingle(Reader["FineFees"]);


                    if (Reader["ReleaseApplicationID"] != DBNull.Value)
                    {
                        ReleaseApplicationID = (int)Reader["ReleaseApplicationID"];
                    }
                    else
                    {
                        ReleaseApplicationID = -1;
                    }

                    if (Reader["ReleaseDate"] != DBNull.Value)
                    {
                        ReleaseDate = (DateTime)Reader["ReleaseDate"];
                    }
                    else
                    {
                        ReleaseDate = DateTime.Now;
                    }

                    if (Reader["ReleasedByUserID"] != DBNull.Value)
                    {
                        ReleasedByUserID = (int)Reader["ReleasedByUserID"];
                    }
                    else
                    {
                        ReleasedByUserID = -1;
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

        public static bool GetDetainedLicenseInfoByLicenseID(ref int DetainID, int LicenseID, ref int ReleasedByUserID,
                                              ref int ReleaseApplicationID, ref int CreatedByUserID,
                                              ref DateTime ReleaseDate, ref DateTime DetainDate,
                                              ref float FineFees, ref bool IsReleased)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select TOP 1 * From DetainedLicenses Where LicenseID = @LicenseID Order By DetainID DESC";

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

                    DetainID = (int)Reader["DetainID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    DetainDate = (DateTime)Reader["DetainDate"];
                    IsReleased = (bool)Reader["IsReleased"];
                    FineFees = Convert.ToSingle(Reader["FineFees"]);


                    if (Reader["ReleaseApplicationID"] != DBNull.Value)
                    {
                        ReleaseApplicationID = (int)Reader["ReleaseApplicationID"];
                    }
                    else
                    {
                        ReleaseApplicationID = -1;
                    }

                    if (Reader["ReleaseDate"] != DBNull.Value)
                    {
                        ReleaseDate = (DateTime)Reader["ReleaseDate"];
                    }
                    else
                    {
                        ReleaseDate = DateTime.Now;
                    }

                    if (Reader["ReleasedByUserID"] != DBNull.Value)
                    {
                        ReleasedByUserID = (int)Reader["ReleasedByUserID"];
                    }
                    else
                    {
                        ReleasedByUserID = -1;
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
        
        public static int AddNewDetainedLicense(int LicenseID, DateTime DetainDate,
                                                float FineFees, int CreatedByUserID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            int DetainID = -1;

            string Query = @"Insert Into DetainedLicenses
                                         (
                                            LicenseID,
                                            DetainDate,
                                            FineFees,
                                            CreatedByUserID,
                                            IsReleased
                                         )
                                        Values
                                        (
                                            @LicenseID,
                                            @DetainDate, 
                                            @FineFees, 
                                            @CreatedByUserID,
                                            0
                                        );
                                            Select Scope_Identity();
                                        ";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);


            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@DetainDate", DetainDate);
            Command.Parameters.AddWithValue("@FineFees", FineFees);
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
                    DetainID = InsertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return DetainID;
        }

        public static bool UpdateDetainedLicense(int DetainID, int LicenseID, int ReleasedByUserID,
                                         int ReleaseApplicationID, int CreatedByUserID,
                                         DateTime ReleaseDate, DateTime DetainDate,
                                         float FineFees, bool IsReleased)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Update DetainedLicenses 
                                Set
                                    LicenseID = @LicenseID,
                                    ReleasedByUserID = @ReleasedByUserID,
                                    ReleaseApplicationID = @ReleaseApplicationID,
                                    CreatedByUserID = @CreatedByUserID,
                                    ReleaseDate = @ReleaseDate,
                                    DetainDate = @DetainDate,
                                    FineFees = @FineFees,
                                    IsReleased = @IsReleased
                                Where
                                    DetainID = @DetainID;
                            ";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            Command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
            Command.Parameters.AddWithValue("@DetainDate", DetainDate);
            Command.Parameters.AddWithValue("@FineFees", FineFees);
            Command.Parameters.AddWithValue("@IsReleased", IsReleased);
            Command.Parameters.AddWithValue("@DetainID", DetainID);


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

        public static bool DeleteDetainedLicense(int DetainID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Delete From DetainedLicenses Where DetainID = @DetainID";

            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DetainID", DetainID);

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

        public static bool ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE DetainedLicenses
                             SET
                                ReleaseDate = @ReleaseDate,
                                ReleaseApplicationID = @ReleaseApplicationID,
                                ReleasedByUserID = @ReleasedByUserID,
                                IsReleased = 1
                             WHERE DetainID = @DetainID
                            "
            ;

            SqlCommand command = new SqlCommand(Query, Connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);

            int rowsAffected = 0;
            
            try
            {
                Connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                Connection.Close();
            }

            return (rowsAffected > 0);
        }
        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dataTable = new DataTable();

            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT DetainedLicenses.DetainID, DetainedLicenses.LicenseID,
                             	    DetainedLicenses.DetainDate, DetainedLicenses.IsReleased,
                             	    DetainedLicenses.FineFees, DetainedLicenses.ReleaseDate,
                             	    People.NationalNo,
                             	    FullName = People.FirstName + ' ' + People.SecondName + ' ' +
                             		 	       ISNULL(People.ThirdName, '') + ' ' + People.LastName,
                             	    DetainedLicenses.ReleaseApplicationID
                             
                             FROM People
                             INNER JOIN Drivers ON Drivers.PersonID = People.PersonID
                             INNER JOIN Licenses ON Licenses.DriverID = Drivers.DriverID
                             INNER JOIN DetainedLicenses ON Licenses.LicenseID = DetainedLicenses.LicenseID
                             
                             ORDER BY  DetainedLicenses.IsReleased, DetainedLicenses.DetainDate,
                             		   Licenses.LicenseID DESC";

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

        public static bool IsDetainedLicenseExistByLicenseID(int LicenseID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "Select Found = 1 From DetainedLicenses Where LicenseID = @LicenseID Order By LicenseID DESC;";

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

        public static bool IsDetainedLicenseExistByDetainID(int DetainID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "Select Found = 1 From DetainedLicenses Where DetainID = @DetainID Order By LicenseID DESC;";

            // Command: It is used to execute queries to perform database operations
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DetainID", DetainID);

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
