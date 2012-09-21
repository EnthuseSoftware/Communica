using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;


namespace StoringImages.Model
{
    public class dBHelper
    {
        // Declartion internal variables
        private SqliteConnection m_connection = null;
        private string m_connectionString = "";
        private SqliteDataAdapter m_dataAdapter = null;
        private DataSet m_dataSet = null;
        private string m_fieldNameID = "";

        // The DataSet is filled by the method LoadDataSet
        public DataSet DataSet
        {
            get { return m_dataSet; }
        }

        // Constructor -> ConnectionString is required
        public dBHelper(string connectionString)
        {
            m_connectionString = connectionString;
        }


        // Load the DataSet from the database
        public bool Load(string commandText, string fieldNameID)
        {
            // Save the variables
            m_fieldNameID = fieldNameID;        

            try
            {
                // Open the connection
                m_connection = new SqliteConnection(m_connectionString);
                m_connection.Open();

                // Make a DataAdapter using a select command and a connection string
                m_dataAdapter = new SqliteDataAdapter(commandText, m_connection);

                // Associate an event handler to the RowUpdated event of the DataAdapter
                //m_dataAdapter.RowUpdated += new SqlRowUpdatedEventHandler(m_dataAdapter_RowUpdated);
                m_dataAdapter.RowUpdated += m_dataAdapter_RowUpdated;
                m_dataSet = new DataSet();

                // Optionally, save--> Commands for creating
                if (!string.IsNullOrEmpty(fieldNameID))
                {
                    SqliteCommandBuilder commandBuilder = new SqliteCommandBuilder(m_dataAdapter);
					m_dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                    m_dataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
                    m_dataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                }
                // Fill the DataSet
                m_dataAdapter.Fill(m_dataSet);

                // We zijn hier, alles okay!
                return true;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // Altijd netjes sluiten
                m_connection.Close();
            }
        }


        // Laad the DataSet
        public bool Load(string commandText)
        {
            return Load(commandText, "");
        }

        // Save to the DataSet
        public bool Save()
        {
            // It can only store data in ID known
            if (m_fieldNameID.Trim().Length == 0)
            {
                return false;
            }

            try
            {
                // Open the connection
                m_connection.Open();

                /* BB 8/1/12 It appears that with Mono.data.sqlite you can update tables but not DataSets
                // Turn on the DataRow. This fires off the event OnRowUpdated
                  m_dataAdapter.Update(m_dataSet);
                 */
				DataTable table = m_dataSet.Tables[0];
				m_dataAdapter.Update (table);
                // We are here, everything is okay!
                return true;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // Always neatly close
                m_connection.Close();
            }
        }


        // Row is stored, determine, where appropriate, the new ID
        void m_dataAdapter_RowUpdated(object sender, System.Data.Common.RowUpdatedEventArgs e)
        {
            // The (newly obtained?) ID is only interesting to a new record 
            if (e.StatementType == StatementType.Insert)
            {
                // Bepaal het zojuist verkregen ID
                //SQLiteCommand command = new SQLiteCommand("SELECT @@IDENTITY", m_connection);
                SqliteCommand command = new SqliteCommand("SELECT last_insert_rowid() AS ID", m_connection);
                

                // Bepaal de nieuwe ID en sla deze op in het juiste veld
                object nieuweID = command.ExecuteScalar();

                // Bij evt. fouten geen ID --> Daarom testen
                if (nieuweID == System.DBNull.Value == false)
                {
                    // Zet de ID in de juiste kolom in de DataRow
                    e.Row[m_fieldNameID] = Convert.ToInt32(nieuweID);
                }
            }
        }

    } // close class dBHelper
}

/* The code is based on code written by Kribo and released under the CPOL (Code Project Open License)
 * CPOL: http://www.codeproject.com/info/cpol10.aspx
 * This code accompanied the article at: http://www.codeproject.com/Articles/196618/C-SQLite-Storing-Images
 */
