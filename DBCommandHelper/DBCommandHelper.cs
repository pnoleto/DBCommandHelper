using System;
using System.Collections.Generic;
using System.Data;

namespace DBCommandHelper
{
    public static class DBCommandHelper
    {
        private readonly static IDictionary<Type, DbType> typeMap = new Dictionary<Type, DbType>
        {
            [typeof(byte)] = DbType.Byte,
            [typeof(byte?)] = DbType.Byte,
            [typeof(sbyte?)] = DbType.SByte,
            [typeof(sbyte)] = DbType.SByte,
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
            //typeMap[typeof(Binary)] = DbType.Binary;
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
        /// <summary>
        /// Dicionario utilizado para resolver o DbType de um determinado tipo primitivo
        /// </summary>
        /// <param name="type">tipo do objeto que sera resolvido para DbType</param>
        /// TODO: How to map bynary type
        /// <returns></returns>
        private static DbType ReturnParameterType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return typeMap[type];
        }
        /// <summary>
        /// Cria um parametro no IDbCommand informado
        /// </summary>
        /// <param name="dbCommand">IDbCommand que sera adicionado o parametro</param>
        /// <param name="paramName">Nome do parametro que sera criado no IDbCommand</param>
        /// <param name="paramType">Tipo de dados DbType que sera passado no parametro</param>
        public static void CreateParamWithNameAndType(this IDbCommand dbCommand, string paramName, DbType paramType)
        {
            dbCommand.IfDbCommandNullThrowException();
            paramName.IfParamNameNullThrowException();

            IDataParameter param = dbCommand.CreateParameter();
            param.ParameterName = paramName;
            param.DbType = paramType;

            dbCommand.Parameters.Add(param);
        }

        /// <summary>
        /// Adiciona valor ao parametro que foi criado anteriormente
        /// </summary>
        /// <typeparam name="TType">Tipo do objeto que sera o valor do parametro</typeparam>
        /// <param name="dbCommand">IDbCommand que sera adicionado o parametro</param>
        /// <param name="paramName">Nome do parametro que sera criado no IDbCommand</param>
        /// <param name="paramValue">Valor que sera adicionado ao parametro</param>
        public static void SetParamValue<TType>(this IDbCommand dbCommand, string paramName, TType paramValue)
        {
            dbCommand.IfDbCommandNullThrowException();
            paramName.IfParamNameNullThrowException();

            ((IDataParameter)dbCommand.Parameters[paramName]).Value = IfNullReturnDbNullValue(paramValue);
        }
        /// <summary>
        /// Cria um parametro no IDbCommand informado com um determinado DbType nome e valor
        /// </summary>
        /// <typeparam name="TType">Tipo do objeto que sera o valor do parametro</typeparam>
        /// <param name="dbCommand">IDbCommand que sera adicionado o parametro</param>
        /// <param name="paramName">Nome do parametro que sera criado no IDbCommand</param>
        /// <param name="paramType">Tipo de dados DbType que sera passado no parametro</param>
        /// <param name="paramValue">Valor que sera adicionado ao parametro</param>
        public static void CreateParamWithNameTypeAndValue<TType>(this IDbCommand dbCommand, string paramName, DbType paramType, TType paramValue)
        {
            dbCommand.IfDbCommandNullThrowException();
            paramName.IfParamNameNullThrowException();

            CreateParamWithNameAndType(dbCommand, paramName, paramType);
            SetParamValue(dbCommand, paramName, IfNullReturnDbNullValue(paramValue));
        }

        /// <summary>
        /// Criar um parametro com valor no IBbCommand sem a necessidade de definir o DbType
        /// </summary>
        /// <param name="dbCommand">IDbCommand que sera adicionado o parametro com valor</param>
        /// <param name="paramName">Nome do parametro que sera criado no IDbCommand</param>
        /// <param name="paramValue">Valor que sera adicionado ao parametro</param>
        public static void CreateParamWithNameAndValue(this IDbCommand dbCommand, string paramName, object paramValue)
        {
            dbCommand.IfDbCommandNullThrowException();
            paramName.IfParamNameNullThrowException();

            CreateParamWithNameAndType(dbCommand, paramName, ReturnParameterType(paramValue.GetType()));
            SetParamValue(dbCommand, paramName, IfNullReturnDbNullValue(paramValue));
        }

        private static object IfNullReturnDbNullValue(object inputValue)
        {
            return inputValue ?? DBNull.Value;
        }
    }
}
