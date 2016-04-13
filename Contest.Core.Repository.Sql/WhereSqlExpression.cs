using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;

namespace Contest.Core.Repository.Sql
{
    public class WhereSqlExpression<T, TI>
        where T : class, TI
        where TI : class
    {
        private readonly Expression<Func<TI, bool>> _expression;

        public WhereSqlExpression(Expression<Func<TI, bool>> exp)
        {
            _expression = exp;
        }

        public string ToStatement(out IList<SqlField> arg)
        {
            arg = new List<SqlField>();
            return ToStatement(_expression, arg);
        }

        private string ToStatement(Expression exp, IList<SqlField> arg)
        {
            if (exp == null)
                return string.Empty;
            switch (exp.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                    return ToStatement(exp.NodeType, (UnaryExpression)exp, arg);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Equal:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.NotEqual:
                    return ToStatement(exp.NodeType, (BinaryExpression)exp, arg);
                case ExpressionType.Constant:
                    return ToStatement((ConstantExpression)exp, arg);
                case ExpressionType.MemberAccess:
                    return ToStatement((MemberExpression)exp, arg);
                case ExpressionType.Lambda:
                    return ToStatement((LambdaExpression)exp, arg);
                case ExpressionType.Parameter:
                    return ToStatement((ParameterExpression)exp, arg);
                default:
                    throw new NotSupportedException(string.Format("Not supported expression type: '{0}'", exp.NodeType));
            }
        }

        protected virtual string ToStatement(ExpressionType type, UnaryExpression u, IList<SqlField> arg)
        {
            return ToSqlStatement(type) + " " + ToStatement(u.Operand, arg);
        }

        protected virtual string ToStatement(ExpressionType type, BinaryExpression b, IList<SqlField> arg)
        {
            if (b.Left is ParameterExpression)
            {
                if (type == ExpressionType.Equal) return ToStatement(b.Right, arg);
                throw new NotImplementedException();
            }
            if (b.Right is ParameterExpression)
            {
                if (type == ExpressionType.Equal) return ToStatement(b.Left, arg);
                throw new NotImplementedException();
            }
            return ToStatement(b.Left, arg) + " " + ToSqlStatement(type) + " " + ToStatement(b.Right, arg);
        }

        protected virtual string ToStatement(ConstantExpression c, IList<SqlField> arg)
        {
            return ToSqlValue(c.Value, null, arg);
        }

        protected virtual string ToStatement(LambdaExpression l, IList<SqlField> arg)
        {
            return l.Body.NodeType == ExpressionType.Constant ? string.Empty : "WHERE " + ToStatement(l.Body, arg);
        }

        protected virtual string ToStatement(MemberExpression m, IList<SqlField> arg)
        {
            string fieldName = null;
            if (m.Expression is ParameterExpression) fieldName = ((ParameterExpression)m.Expression).Name;
            else if (m.Expression is MemberExpression) fieldName = ((MemberExpression)m.Expression).Member.Name;
            else if (m.Expression is ConstantExpression) fieldName = ((ConstantExpression)m.Expression).Value.ToString();
            else if (m.Expression is UnaryExpression)
            {
                var unary = (UnaryExpression)m.Expression;
                if (unary.Operand is ParameterExpression) fieldName = ((ParameterExpression)unary.Operand).Name;
            }
            else throw new NotSupportedException(string.Format("Type not supported. Type:{0}.", m.Expression.GetType()));

            if (IsLambdaArgument(fieldName)) return m.ColumnName<T>();

            var objectMember = Expression.Convert(m, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();

            return ToSqlValue(getter(), null, arg);
        }

        private bool IsLambdaArgument(string fieldName)
        {
            return string.Equals(_expression.Parameters[0].Name, fieldName);
        }

        protected virtual string ToStatement(ParameterExpression p, IList<SqlField> arg)
        {
            throw new NotImplementedException();
        }

        public string ToSqlValue(object obj, object[] customAttr, IList<SqlField> arg)
        {
            var newArg = SqlField.Create("P" + arg.Count.ToString(CultureInfo.InvariantCulture), obj, customAttr);
            arg.Add(newArg);
            return newArg.MarkerValue;
        }

        protected string ToSqlStatement(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    return "-";
                case ExpressionType.Not:
                    return "NOT";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Modulo:
                    return "MOD";
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return "AND";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return "OR";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "<>";
                default:
                    throw new Exception(string.Format("Unhandled expression type: '{0}'", type));
            }
        }
    }
}
