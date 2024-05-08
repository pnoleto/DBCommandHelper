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

        private static void IfParamTypeNullThrowException(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
        }

        private static DbType ReturnParameterType(Type type)
        {
            IfParamTypeNullThrowException(type);

            return typeMap[type];
        }

        public static void CreateParameter(this IDbCommand dbCommand, string parameterName, DbType parameterType)
        {
            IDataParameter param = dbCommand.CreateParameter();

            param.ParameterName = parameterName;
            param.DbType = parameterType;

            dbCommand.Parameters.Add(param);
        }

        public static void CreateParameter<TType>(this IDbCommand dbCommand, string parameterName, TType parameterValue)
        {
            CreateParameter(dbCommand, parameterName, ReturnParameterType(parameterValue.GetType()));

            dbCommand.SetParamValue(parameterName, IfNullReturnDbNullValue(parameterValue));
        }

        public static void CreateParameter<TType>(this IDbCommand dbCommand, string parameterName, DbType parameterType, TType parameterValue)
        {
            CreateParameter(dbCommand, parameterName, parameterType);
            dbCommand.SetParamValue(parameterName, IfNullReturnDbNullValue(parameterValue));
        }

        public static void SetParamValue<TType>(this IDbCommand dbCommand, string paramName, TType paramValue)
        {
            ((IDataParameter)dbCommand.Parameters[paramName]).Value = IfNullReturnDbNullValue(paramValue);
        }

        public static void ClearValues(this IDataParameterCollection parametersConnection)
        {
            foreach (IDataParameter param in parametersConnection) param.Value = DBNull.Value;
        }

        private static object IfNullReturnDbNullValue(object inputValue)
        {
            return inputValue ?? DBNull.Value;
        }
    }
}
