using System;
using System.Collections.Generic;
using System.Data;

namespace Domain.Helpers.Infrastructure
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
            if (dbCommand == null) throw new ArgumentNullException(nameof(IDbCommand));
        }
        /// <summary>
        /// Dicionario utilizado para resolver o DbType de um determinado tipo primitivo
        /// </summary>
        /// <param name="type">tipo do objeto que sera resolvido para DbType</param>
        /// TODO: How to map bynary type
        /// <returns></returns>
        private static DbType ReturnParameterType(Type type)
        {
            return typeMap[type];
        }
        /// <summary>
        /// Cria um parametro no IDbCommand informado
        /// </summary>
        /// <param name="dbCommand">IDbCommand que sera adicionado o parametro</param>
        /// <param name="paramName">Nome do parametro que sera criado no IDbCommand</param>
        /// <param name="paramType">Tipo de dados DbType que sera passado no parametro</param>
        public static void CreateParamWithName(this IDbCommand dbCommand, string paramName, DbType paramType)
        {
            dbCommand.IfDbCommandNullThrowException();

            IDataParameter param = dbCommand.CreateParameter();
            param.ParameterName = paramName;
            param.DbType = paramType;

            dbCommand.Parameters.Add(param);
        }

        /// <summary>
        /// Adiciona valor ao parametro que foi criado anteriormente
        /// </summary>
        /// <typeparam name="TTipo">Tipo do objeto que sera o valor do parametro</typeparam>
        /// <param name="dbCommand">IDbCommand que sera adicionado o parametro</param>
        /// <param name="paramName">Nome do parametro que sera criado no IDbCommand</param>
        /// <param name="paramValue">Valor que sera adicionado ao parametro</param>
        public static void SetParamValue<TTipo>(this IDbCommand dbCommand, string paramName, TTipo paramValue)
        {
            dbCommand.IfDbCommandNullThrowException();

            ((IDataParameter)dbCommand.Parameters[paramName]).Value = IfNullThenReturnDbNullValue(paramValue);
        }
        /// <summary>
        /// Cria um parametro no IDbCommand informado com um determinado DbType nome e valor
        /// </summary>
        /// <typeparam name="TTipo">Tipo do objeto que sera o valor do parametro</typeparam>
        /// <param name="dbCommand">IDbCommand que sera adicionado o parametro</param>
        /// <param name="paramName">Nome do parametro que sera criado no IDbCommand</param>
        /// <param name="paramType">Tipo de dados DbType que sera passado no parametro</param>
        /// <param name="paramValue">Valor que sera adicionado ao parametro</param>
        public static void CreateParamWithNameTypeAndValue<TTipo>(this IDbCommand dbCommand, string paramName, DbType paramType, TTipo paramValue)
        {
            dbCommand.IfDbCommandNullThrowException();

            CreateParamWithName(dbCommand, paramName, paramType);
            SetParamValue(dbCommand, paramName, IfNullThenReturnDbNullValue(paramValue));
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

            CreateParamWithName(dbCommand, paramName, ReturnParameterType(paramValue.GetType()));
            SetParamValue(dbCommand, paramName, IfNullThenReturnDbNullValue(paramValue));
        }

        private static object IfNullThenReturnDbNullValue(object inputValue)
        {
            return inputValue ?? DBNull.Value;
        }
    }
}
