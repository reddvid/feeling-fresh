using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Collections;
using System.Data.Common;
using System.Windows.Controls.Primitives;
using Models;

namespace FeelingFreshWPF.Helpers
{
	public class DBHelper
	{
		// WARNING: This is a BAD practice - use a more secure way to hold Connection String
		private const string CONNECTION_STRING = "Data Source=YOUR_DATABASE_SERVER;Database=DB_NAME;User Id=DB_USERNAME;Password=DB_PASSWORD;";

		public static async Task AddApp(string appName)
		{
			try
			{
				string query = "INSERT INTO AppsList VALUES ('" + appName + "')";
				using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
				{
					using (SqlCommand cmdRead = new SqlCommand(query, connection))
					{
						connection.Open();
						await cmdRead.ExecuteReaderAsync();
					}

					connection?.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error while adding value\n" + ex);
			}
		}

		public static async Task<ObservableCollection<Win32App>> GetApps()
		{
			var list = new ObservableCollection<Win32App>();

			try
			{
				string query = "SELECT * FROM AppsList";
				using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
				{
					using (SqlCommand cmdRead = new SqlCommand(query, connection))
					{
						connection.Open();
						using (SqlDataReader reader = await cmdRead.ExecuteReaderAsync())
						{
							if (reader != null)
							{
								while (reader.Read() && !string.IsNullOrWhiteSpace(reader.GetString(0)))
								{
									var app = new Win32App();
									app.AppName = reader.GetString(0);
									app.Id = reader.GetInt32(1);
									list.Add(app);
								}
							}
						}
					}

					connection?.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error while loading apps\n" + ex);
			}

			return list;
		}

		public static async Task RemoveApp(string appName)
		{
			try
			{
				string query = "DELETE FROM AppsList WHERE AppName='" + appName + "'";
				using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
				{
					using (SqlCommand cmdRead = new SqlCommand(query, connection))
					{
						connection.Open();
						await cmdRead.ExecuteReaderAsync();
					}

					connection?.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error while removing value\n" + ex);
			}
		}

		public static async Task UpdateApp(string originalAppName, string newAppName)
		{
			try
			{
				string query = "UPDATE AppsList SET AppName='" + newAppName + "' WHERE AppName=('" + originalAppName + "')";
				using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
				{
					using (SqlCommand cmdRead = new SqlCommand(query, connection))
					{
						connection.Open();
						await cmdRead.ExecuteReaderAsync();
					}

					connection?.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error while updating value\n" + ex);
			}
		}
	}
}
