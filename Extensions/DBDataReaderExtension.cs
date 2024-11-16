using Microsoft.Data.SqlClient;
using System.Data.Common;


public static class DBDataReaderExtension
{
	public static string GetStringOrNull(this DbDataReader reader, string columnName)
	{
		int ordinal = reader.GetOrdinal(columnName);
		return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
	}

	public static int? GetIntOrNull(this DbDataReader reader, string columnName)
	{
		int ordinal = reader.GetOrdinal(columnName);
		return reader.IsDBNull(ordinal) ? (int?)null : reader.GetInt32(ordinal);
	}

	public static DateTime? GetDateTimeOrNull(this DbDataReader reader, string columnName)
	{
		int ordinal = reader.GetOrdinal(columnName);
		return reader.IsDBNull(ordinal) ? (DateTime?)null : reader.GetDateTime(ordinal);
	}
}

