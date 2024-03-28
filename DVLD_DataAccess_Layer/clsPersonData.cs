using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess_Layer
{
    public class clsPersonData
    {
        public static bool GetPersonInfoByPersonID(int PersonID, ref byte Gender, ref string NationalNo, ref int CountryID, ref string FirstName,
            ref string SecondName, ref string ThirdName, ref string LastName, ref string Email, 
            ref string Phone, ref string Address, ref string ImagePath, ref DateTime DateOfBirth)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From People Where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            bool isFound = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    isFound = true;
                    Gender = (byte)reader["Gender"];
                    CountryID = (int)reader["NationalityCountryID"];

                    NationalNo = (string)reader["NationalNo"];
                    Address = (string)reader["Address"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Phone = (string)reader["Phone"];

                    FirstName = (string)reader["FirstName"];
                    LastName = (string)reader["LastName"];
                    SecondName = (string)reader["SecondName"];
                    if (reader["ThirdName"] !=DBNull.Value)
                    {
                        ThirdName = (string)reader["ThirdName"];
                    }
                    else
                    {
                        ThirdName = "";
                    }
                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = (string)reader["Email"];
                    }
                    else
                    {
                        Email = "";
                    }
                    if (reader["ImagePath"]!=DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
                    }

                    reader.Close();

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

        public static bool GetPersonByNationalNo(ref int PersonID, string NationalNo, ref byte Gender, ref int CountryID, ref string FirstName,
            ref string SecondName, ref string ThirdName, ref string LastName, ref string Email,
            ref string Phone, ref string Address, ref string ImagePath, ref DateTime DateOfBirth)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select * From People Where NationalNo = @NationalNo";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);

            bool isFound = false;
            try
            {
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                if(reader.Read())
                {
                    isFound = true;

                    Gender = (byte)reader["Gender"];
                    PersonID = (int)reader["PersonID"];
                    CountryID = (int)reader["NationalityCountryID"];

                    Address = (string)reader["Address"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Phone = (string)reader["Phone"];

                    FirstName = (string)reader["FirstName"];
                    LastName = (string)reader["LastName"];
                    SecondName = (string)reader["SecondName"];
                    if (reader["ThirdName"] != DBNull.Value)
                    {
                        ThirdName = (string)reader["ThirdName"];
                    }
                    else
                    {
                        ThirdName = "";
                    }
                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = (string)reader["Email"];
                    }
                    else
                    {
                        Email = "";
                    }
                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Connection.Close();
            }
            return isFound;
        }

        public static int AddNewPerson(byte Gender, string NationalNo, int CountryID, string FirstName,
                          string SecondName, string ThirdName, string LastName, string Email,
                          string Phone, string Address, string ImagePath, DateTime DateOfBirth)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            int PersonID = -1;
            string Query = @"Insert Into People(
                                            Gender, 
                                            NationalityCountryID, 
                                            FirstName, 
                                            SecondName, 
                                            ThirdName, 
                                            LastName,
                                            NationalNo,
                                            Email, 
                                            Phone, 
                                            Address, 
                                            ImagePath, 
                                            DateOfBirth
                                        )
                                        Values
                                        (
                                            @Gender, 
                                            @NationalityCountryID, 
                                            @FirstName, 
                                            @SecondName, 
                                            @ThirdName, 
                                            @LastName,
                                            @NationalNo,
                                            @Email, 
                                            @Phone, 
                                            @Address, 
                                            @ImagePath, 
                                            @DateOfBirth
                                        );
                                        Select Scope_Identity();
                                        ";
            
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@Gender", Gender);
            Command.Parameters.AddWithValue("@NationalityCountryID", CountryID);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@Phone", Phone);
            Command.Parameters.AddWithValue("@Address", Address);

            if (Email != null || Email != "")
                Command.Parameters.AddWithValue("@Email", Email);
            else
                Command.Parameters.AddWithValue("@Email", System.DBNull.Value);
            if (ThirdName != null || ThirdName != "") 
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                Command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            if (ImagePath != null || ImagePath != "") 
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                Command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);

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
                    PersonID = InsertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }
            return PersonID;
        }
        
        public static bool UpdatePerson(int PersonID, byte Gender, string NationalNo, int CountryID, string FirstName,
                          string SecondName, string ThirdName, string LastName, string Email,
                          string Phone, string Address, string ImagePath, DateTime DateOfBirth)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"Update People 
                             Set
                                Gender = @Gender,
                                NationalityCountryID = @NationalityCountryID,
                                FirstName = @FirstName,
                                SecondName = @SecondName,
                                ThirdName = @ThirdName,
                                LastName = @LastName,
                                NationalNo = @NationalNo,
                                Email = @Email,
                                Phone = @Phone,
                                Address = @Address,
                                ImagePath = @ImagePath,
                                DateOfBirth = @DateOfBirth
                             Where
                             PersonID = @PersonID;
                            ";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@Gender", Gender);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@NationalityCountryID", CountryID);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@Phone", Phone);
            Command.Parameters.AddWithValue("@Address", Address);

            if (Email != null && Email != "")
                Command.Parameters.AddWithValue("@Email", Email);
            else
                Command.Parameters.AddWithValue("@Email", System.DBNull.Value);
            if (ThirdName != "" && ThirdName != null)
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                Command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            if (ImagePath != null && ImagePath != "")
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                Command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);

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
        
        public static bool DeletePerson(int PersonID)
        {
            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            
            string Query = @"Delete From People Where PersonID = @PersonID";
            
            // Command: It is used to execute queries to perform database operations.
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

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
    
        public static DataTable GetAllPeople()
        {
            DataTable dataTable = new DataTable();

            // Connection: It is used to establish a connection to a specific data source.
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Select PersonID, NationalNo, FirstName, SecondName, ThirdName, 
	                         LastName, DateOfBirth, Email, CountryName, Phone, CountryID, ImagePath,
	                         GenderCaption =
	                         Case 
		                        When Gender = 0 Then 'Male'
		                        When Gender = 1 Then 'Female'
	                            Else 'Unknown'
	                         End
                            From People
                            Inner Join Countries 
                            On Countries.CountryID = People.NationalityCountryID
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

        public static bool IsPersonExistByPersonID(int PersonID)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "Select Found = 1 From People Where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

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

        public static bool IsPersonExistByNationalNo(string NationalNo)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "Select Found = 1 From People Where NationalNo = @NationalNo";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);

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
