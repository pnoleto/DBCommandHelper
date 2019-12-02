using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace Domain.Helpers.Infrastructure
{
    public class DBCommandHelper
    {
        /// <summary>
        /// Dicionario utilizado para resolver o DbType de um determinado tipo primitivo
        /// </summary>
        /// <param name="type">tipo do objeto que sera resolvido para DbType</param>
        /// <returns></returns>
        private static DbType ObterTipoDoParametro(Type type)
        {
            var typeMap = new Dictionary<Type, DbType>();
            typeMap[typeof(byte)] = DbType.Byte;
            typeMap[typeof(sbyte)] = DbType.SByte;
            typeMap[typeof(short)] = DbType.Int16;
            typeMap[typeof(ushort)] = DbType.UInt16;
            typeMap[typeof(int)] = DbType.Int32;
            typeMap[typeof(uint)] = DbType.UInt32;
            typeMap[typeof(long)] = DbType.Int64;
            typeMap[typeof(ulong)] = DbType.UInt64;
            typeMap[typeof(float)] = DbType.Single;
            typeMap[typeof(double)] = DbType.Double;
            typeMap[typeof(decimal)] = DbType.Decimal;
            typeMap[typeof(bool)] = DbType.Boolean;
            typeMap[typeof(string)] = DbType.String;
            typeMap[typeof(char)] = DbType.StringFixedLength;
            typeMap[typeof(Guid)] = DbType.Guid;
            typeMap[typeof(DateTime)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            typeMap[typeof(byte[])] = DbType.Binary;
            typeMap[typeof(byte?)] = DbType.Byte;
            typeMap[typeof(sbyte?)] = DbType.SByte;
            typeMap[typeof(short?)] = DbType.Int16;
            typeMap[typeof(ushort?)] = DbType.UInt16;
            typeMap[typeof(int?)] = DbType.Int32;
            typeMap[typeof(uint?)] = DbType.UInt32;
            typeMap[typeof(long?)] = DbType.Int64;
            typeMap[typeof(ulong?)] = DbType.UInt64;
            typeMap[typeof(float?)] = DbType.Single;
            typeMap[typeof(double?)] = DbType.Double;
            typeMap[typeof(decimal?)] = DbType.Decimal;
            typeMap[typeof(bool?)] = DbType.Boolean;
            typeMap[typeof(char?)] = DbType.StringFixedLength;
            typeMap[typeof(Guid?)] = DbType.Guid;
            typeMap[typeof(DateTime?)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset?)] = DbType.DateTimeOffset;
            //typeMap[typeof(Binary)] = DbType.Binary;
            return typeMap[type];
        }
        /// <summary>
        /// Cria um parametro no IDbCommand informado
        /// </summary>
        /// <param name="DbCommand">IDbCommand que sera adicionado o parametro</param>
        /// <param name="NomeParametro">Nome do parametro que sera criado no IDbCommand</param>
        /// <param name="TipoParametro">Tipo de dados DbType que sera passado no parametro</param>
        public static void CriarParametro(IDbCommand DbCommand, string NomeParametro, DbType TipoParametro)
        {
            IDataParameter param = DbCommand.CreateParameter();
            param.ParameterName = NomeParametro;
            param.DbType = TipoParametro;

            DbCommand.Parameters.Add(param);
        }

        /// <summary>
        /// Adiciona valor ao parametro que foi criado anteriormente
        /// </summary>
        /// <typeparam name="TTipo">Tipo do objeto que sera o valor do parametro</typeparam>
        /// <param name="DbCommand">IDbCommand que sera adicionado o parametro</param>
        /// <param name="NomeParametro">Nome do parametro que sera criado no IDbCommand</param>
        /// <param name="Valor">Valor que sera adicionado ao parametro</param>
        public static void AdicionarValorAoParametro<TTipo>(IDbCommand DbCommand, string NomeParametro, TTipo Valor)
        {
            ((IDataParameter)DbCommand.Parameters[NomeParametro]).Value = Valor;
        }
        /// <summary>
        /// Cria um parametro no IDbCommand informado com um determinado DbType nome e valor
        /// </summary>
        /// <typeparam name="TTipo">Tipo do objeto que sera o valor do parametro</typeparam>
        /// <param name="DbCommand">IDbCommand que sera adicionado o parametro</param>
        /// <param name="NomeParametro">Nome do parametro que sera criado no IDbCommand</param>
        /// <param name="TipoParametro">Tipo de dados DbType que sera passado no parametro</param>
        /// <param name="Valor">Valor que sera adicionado ao parametro</param>
        public static void CriarParametroComValor<TTipo>(IDbCommand DbCommand, string NomeParametro, DbType TipoParametro, TTipo Valor)
        {
            CriarParametro(DbCommand, NomeParametro, TipoParametro);
            AdicionarValorAoParametro(DbCommand, NomeParametro, Valor);
        }

        /// <summary>
        /// Criar um parametro com valor no IBbCommand sem a necessidade de definir o DbType
        /// </summary>
        /// <param name="DbCommand">IDbCommand que sera adicionado o parametro com valor</param>
        /// <param name="NomeParametro">Nome do parametro que sera criado no IDbCommand</param>
        /// <param name="Valor">Valor que sera adicionado ao parametro</param>
        public static void CriarParametroComValor(IDbCommand DbCommand, string NomeParametro, object Valor)
        {
            CriarParametro(DbCommand, NomeParametro, ObterTipoDoParametro(Valor.GetType()));
            AdicionarValorAoParametro(DbCommand, NomeParametro, Valor);
        }
    }
}
