using System;
using System.Data;
using System.Collections.Generic;

namespace DBCommandHelper
{
    public static class DBCommandHelper
    {
        private readonly static IDictionary<Type, DbType> typeMap = new Dictionary<Type, DbType>
        {
            [typeof(byte)] = DbType.Byte,
            [typeof(byte?)] = DbType.Byte,
            [typeof(sbyte)] = DbType.SByte,
            [typeof(sbyte?)] = DbType.SByte,
            [typeof(byte[])] = DbType.Binary,
            [typeof(short)] = DbType.Int16,
            [typeof(short?)] = DbType.Int16,
            [typeof(ushort)] = DbType.UInt16,
            [typeof(ushort?)] = DbType.UInt16,
            [typeof(int)] = DbType.Int32,
            [typeof(int?)] = DbType.Int32,
            [typeof(uint)] = DbType.UInt32,
            [typeof(uint?)] = DbType.UInt32,
            [typeof(long)] = DbType.Int64,
            [typeof(long?)] = DbType.Int64,
            [typeof(ulong)] = DbType.UInt64,
            [typeof(ulong?)] = DbType.UInt64,
            [typeof(float)] = DbType.Single,
            [typeof(float?)] = DbType.Single,
            [typeof(double)] = DbType.Double,
            [typeof(double?)] = DbType.Double,
            [typeof(decimal)] = DbType.Decimal,
            [typeof(decimal?)] = DbType.Decimal,
            [typeof(bool)] = DbType.Boolean,
            [typeof(bool?)] = DbType.Boolean,
            [typeof(string)] = DbType.String,
            [typeof(char)] = DbType.StringFixedLength,
            [typeof(char?)] = DbType.StringFixedLength,
            [typeof(Guid)] = DbType.Guid,
            [typeof(Guid?)] = DbType.Guid,
            [typeof(DateTime)] = DbType.DateTime,
            [typeof(DateTime?)] = DbType.DateTime,
            [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
            [typeof(DateTimeOffset?)] = DbType.DateTimeOffset
        };

        private static void IfDbCommandNullThrowException(this IDbCommand dbCommand)
        {
            if (dbCommand == null) throw new ArgumentNullException(nameof(dbCommand));
        }

        private static void IfParamNameNullThrowException(this string paramName)
        {
            if (paramName == null) throw new ArgumentNullException(nameof(paramName));

            if (string.IsNullOrEmpty(paramName.Trim())) throw new ArgumentNullException(nameof(paramName));
        }

        private static DbType ReturnParameterType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return typeMap[type];
        }

        public static void CreateParamWithNameAndType(this IDbCommand dbCommand, string paramName, DbType paramType)
        {
            dbCommand.IfDbCommandNullThrowException();
            paramName.IfParamNameNullThrowException();

            IDataParameter param = dbCommand.CreateParameter();
            param.ParameterName = paramName;
            param.DbType = paramType;

            dbCommand.Parameters.Add(param);
        }

        public static void SetParamValue<TType>(this IDbCommand dbCommand, string paramName, TType paramValue)
        {
            dbCommand.IfDbCommandNullThrowException();
            paramName.IfParamNameNullThrowException();

            ((IDataParameter)dbCommand.Parameters[paramName]).Value = IfNullReturnDbNullValue(paramValue);
        }

        public static void CreateParamWithNameTypeAndValue<TType>(this IDbCommand dbCommand, string paramName, DbType paramType, TType paramValue)
        {
            dbCommand.IfDbCommandNullThrowException();
            paramName.IfParamNameNullThrowException();

            dbCommand.CreateParamWithNameAndType(paramName, paramType);
            dbCommand.SetParamValue(paramName, IfNullReturnDbNullValue(paramValue));
        }

        /// <summary>
        /// Automatically resolve parameter DbType
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public static void CreateParamWithNameAndValue(this IDbCommand dbCommand, string paramName, object paramValue)
        {
            dbCommand.IfDbCommandNullThrowException();
            paramName.IfParamNameNullThrowException();

            dbCommand.CreateParamWithNameAndType(paramName, ReturnParameterType(paramValue.GetType()));
            dbCommand.SetParamValue(paramName, IfNullReturnDbNullValue(paramValue));
        }

        public static void ClearParamValue(this IDbCommand dbCommand, string paramName)
        {
            dbCommand.SetParamValue(paramName, DBNull.Value);
        }

        public static void CleanAllParamValues(this IDbCommand dbCommand)
        {
            for (int i = 0; i < dbCommand.Parameters.Count; i++) ((IDataParameter)dbCommand.Parameters[i]).Value = DBNull.Value;
        }

        private static object IfNullReturnDbNullValue(object inputValue)
        {
            return inputValue ?? DBNull.Value;
        }
    }
}
