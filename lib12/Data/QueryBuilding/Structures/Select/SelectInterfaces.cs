﻿using System;
using System.Collections.Generic;

namespace lib12.Data.QueryBuilding.Structures.Select
{
    public interface ISelect
    {
        /// <summary>
        /// Adds * representing all fields to SELECt statement
        /// </summary>
        IFields AllFields { get; }

        /// <summary>
        /// Adds set of fields to SELECT statement
        /// </summary>
        /// <param name="fields">The fields to add</param>
        /// <returns></returns>
        IFields Fields(IEnumerable<SelectField> fields);

        /// <summary>
        /// Adds set of fields to SELECT statement
        /// </summary>
        /// <param name="fields">The fields to add</param>
        /// <returns></returns>
        IFields Fields(params string[] fields);

        /// <summary>
        /// Adds set of fields to SELECT statement with optional alias
        /// </summary>
        /// <param name="withAlias">if set to <c>true</c> [with alias].</param>
        /// <param name="fields">The fields to add</param>
        /// <returns></returns>
        IFields Fields(bool withAlias, params string[] fields);
    }

    public interface IFields : ITop
    {
        /// <summary>
        /// Adds TOP statement to SELECT
        /// </summary>
        /// <param name="count">The top count</param>
        /// <returns></returns>
        ITop Top(int count);
    }

    public interface ITop
    {
        /// <summary>
        /// Adds FROM statement to SELECT
        /// </summary>
        /// <param name="table">The table name for FROM</param>
        /// <returns></returns>
        ISelectFrom From(string table);

        /// <summary>
        /// Adds FROM statement to SELECT with alias
        /// </summary>
        /// <param name="table">The table name for FROM</param>
        /// <param name="alias">The alias for table</param>
        /// <returns></returns>
        ISelectFrom From(string table, string alias);
    }

    #region Possibilites


    public interface ISelectWherePossible
    {
        /// <summary>
        /// Adds the WHERE statement to query
        /// </summary>
        /// <param name="cnd">The condition</param>
        /// <returns></returns>
        ISelectWhere Where(Condition cnd);

        /// <summary>
        /// Adds the WHERE statement to query
        /// </summary>
        /// <param name="field">The field on which comparison occurs</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="argument">The argument for comparison</param>
        /// <returns></returns>
        ISelectWhere Where(string field, Compare comparison, object argument);

        /// <summary>
        /// Adds the WHERE statement to query
        /// </summary>
        /// <param name="condition">The condition as text</param>
        /// <returns></returns>
        ISelectWhere Where(string condition);

        /// <summary>
        /// Adds the WHERE BETWEEN statement to query
        /// </summary>
        /// <param name="field">The field on which comparison occurs</param>
        /// <param name="argument1">The first argument for between</param>
        /// <param name="argument2">The second argument for between</param>
        /// <returns></returns>
        ISelectWhere WhereBetween(string field, object argument1, object argument2);

        /// <summary>
        /// Adds the WHERE IS NULL statement to query
        /// </summary>
        /// <param name="field">The field to check</param>
        /// <returns></returns>
        ISelectWhere WhereIsNull(string field);

        /// <summary>
        /// Adds the WHERE IS NOT NULL statement to query
        /// </summary>
        /// <param name="field">The field to check</param>
        /// <returns></returns>
        ISelectWhere WhereIsNotNull(string field);
    }

    public interface ISelectBracketPossible
    {
        /// <summary>
        /// Opens the bracket.
        /// </summary>
        /// <returns></returns>
        IOpenSelectBracket OpenBracket();
    }

    public interface ISelectConcatPossible
    {
        /// <summary>
        /// Adds AND statement
        /// </summary>
        ISelectConcat And { get; }

        /// <summary>
        /// Adds OR statement
        /// </summary>
        ISelectConcat Or { get; }
    }

    public interface IGroupByPossible
    {
        /// <summary>
        /// Adds the GROUP BY statement to select
        /// </summary>
        /// <param name="field">The field to GROUP BY on</param>
        /// <returns></returns>
        IGroupBy GroupBy(string field);
    }

    public interface IOrderByPossible
    {
        /// <summary>
        /// Adds the ORDER BY statement to select
        /// </summary>
        /// <param name="field">The field to order on</param>
        /// <returns></returns>
        IOrderBy OrderBy(string field);

        /// <summary>
        /// Adds the ORDER BY statement to select
        /// </summary>
        /// <param name="orderBy">The order by data</param>
        /// <returns></returns>
        IOrderBy OrderBy(OrderBy orderBy);

        /// <summary>
        /// Adds the ORDER BY DESC statement to select
        /// </summary>
        /// <param name="field">The field to order on</param>
        /// <returns></returns>
        IOrderBy OrderByDesc(string field);
    }
    #endregion

    public interface ISelectFrom : ISelectBracketPossible, ISelectWherePossible, IGroupByPossible, IOrderByPossible, IBuild
    {
        /// <summary>
        /// Adds JOIN statement to SELECT
        /// </summary>
        /// <param name="join">The join object</param>
        /// <returns></returns>
        ISelectFrom Join(Join join);

        /// <summary>
        /// Adds JOIN statement to SELECT
        /// </summary>
        /// <param name="rightTable">The right table to join</param>
        /// <param name="rightTableAlias">The right table alias</param>
        /// <param name="leftField">The left field to join on</param>
        /// <param name="rightField">The right field to join on</param>
        /// <returns></returns>
        ISelectFrom Join(string rightTable, string rightTableAlias, string leftField, string rightField);

        /// <summary>
        /// Adds JOIN statement to SELECT
        /// </summary>
        /// <param name="rightTable">The right table to join</param>
        /// <param name="rightTableAlias">The right table alias</param>
        /// <param name="leftField">The left field to join on</param>
        /// <param name="rightField">The right field to join on</param>
        /// <param name="type">The type of JOIN</param>
        /// <returns></returns>
        ISelectFrom Join(string rightTable, string rightTableAlias, string leftField, string rightField, JoinType type);
    }

    public interface IOpenSelectBracket : ISelectWherePossible
    {

    }

    public interface ICloseSelectBracket : ISelectConcatPossible, IGroupByPossible, IOrderByPossible, IBuild
    {

    }

    public interface ISelectConcat : ISelectBracketPossible, ISelectWherePossible
    {

    }

    public interface ISelectWhere : ISelectConcatPossible, IGroupByPossible, IOrderByPossible, IBuild
    {
        /// <summary>
        /// Closes the bracket.
        /// </summary>
        /// <returns></returns>
        ICloseSelectBracket CloseBracket();
    }

    public interface IGroupBy : IGroupByPossible, IOrderByPossible, IBuild
    {
        /// <summary>
        /// Ads HAVING statement to SELECT
        /// </summary>
        /// <param name="cnd">The condition for HAVING</param>
        /// <returns></returns>
        IHaving Having(string cnd);
    }

    public interface IHaving : IOrderByPossible, IBuild
    {

    }

    public interface IOrderBy : IBuild
    {

    }
}
