using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Contest.Core.DataStore.Sql.EntityInfo;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public class SqlWhereClause<T>
        where T : class
    {
        private readonly LambdaExpression _expression;
        private const string NULL_VALUE = "NULL";

        private ISqlProviderStrategy ProviderStrategy { get; set; }
        private IEntityInfoFactory EntityInfoFactory { get; set; }

        public SqlWhereClause(ISqlProviderStrategy providerStrategy, IEntityInfoFactory entityInfoFactory, LambdaExpression exp)
        {
            ProviderStrategy = providerStrategy;
            EntityInfoFactory = entityInfoFactory;
            _expression = exp;
        }

        public string ToStatement()
        {
            // To manage null for sql server... (sql server doesn't support item = NULL but support item IS NULL
            return _expression != null ? ToStatement(_expression).Replace("= " + NULL_VALUE, "IS " + NULL_VALUE) : string.Empty;
        }

        private string ToStatement(Expression exp)
        {
            if (exp == null)
                return string.Empty;
            switch (exp.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    return ToStatement(exp.NodeType, (UnaryExpression)exp);
                case ExpressionType.Not:
                    return ToStatement(exp.NodeType, (UnaryExpression)exp);
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
                    return ToStatement(exp.NodeType, (BinaryExpression)exp);
                case ExpressionType.Constant:
                    return ToStatement((ConstantExpression)exp);
                case ExpressionType.MemberAccess:
                    return ToStatement((MemberExpression)exp);
                case ExpressionType.Lambda:
                    return ToStatement((LambdaExpression)exp);
                case ExpressionType.Parameter:
                    return ToStatement((ParameterExpression)exp);
                default:
                    throw new NotSupportedException(string.Format("Not supported expression type: '{0}'", exp.NodeType));
            }
        }

        protected virtual string ToStatement(ExpressionType type, UnaryExpression u)
        {
            return ToSqlStatement(type) + " " + ToStatement(u.Operand);
        }

        protected virtual string ToStatement(ExpressionType type, BinaryExpression b)
        {
            if (b.Left is ParameterExpression)
            {
                if (type == ExpressionType.Equal) return ToStatement(b.Right);
                throw new NotImplementedException();
            }
            if (b.Right is ParameterExpression)
            {
                if (type == ExpressionType.Equal) return ToStatement(b.Left);
                throw new NotImplementedException();
            }
            return ToStatement(b.Left) + " " + ToSqlStatement(type) + " " + ToStatement(b.Right);
        }

        protected virtual string ToStatement(ConstantExpression c)
        {
            return ToSqlValue(c.Value, null);
        }

        protected virtual string ToStatement(LambdaExpression l)
        {
            return l.Body.NodeType == ExpressionType.Constant ? string.Empty : "WHERE " + ToStatement(l.Body);
        }

        protected virtual string ToStatement(MemberExpression m)
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

            if (IsLambdaArgument(fieldName)) return GetColumnName(m);

            var objectMember = Expression.Convert(m, typeof(object));

            var getterLambda = Expression.Lambda<Func<object>>(objectMember);

            var getter = getterLambda.Compile();

            return ToSqlValue(getter(), null);
        }

        protected virtual string ToStatement(ParameterExpression p)
        {
            throw new NotImplementedException();
        }

        public string ToSqlValue(object obj, object[] customAttr)
        {
            // To manage null for sql server... (sql server doesn't support item = NULL but support item IS NULL
            if (obj == null) return NULL_VALUE;
            return ProviderStrategy.ToSqlValue(obj, customAttr);
        }

        private bool IsLambdaArgument(string fieldName)
        {
            return string.Equals(_expression.Parameters[0].Name, fieldName);
        }

        private static string ToSqlStatement(ExpressionType type)
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

        public string GetColumnName<TPropOrField>(Expression<Func<T, TPropOrField>> propertyorFieldExpression)
        {
            if (propertyorFieldExpression == null) throw new ArgumentNullException("propertyorFieldExpression");

            var body = propertyorFieldExpression.Body as MemberExpression;

            return GetColumnName(body);
        }

        public string GetColumnName(MemberExpression memberExpression)
        {
            if (memberExpression == null) throw new ArgumentNullException("memberExpression");

            var propertyWithSqlInfo = GetConcreteClassProperty(memberExpression.Member as PropertyInfo);
            return propertyWithSqlInfo.ColumnName;
        }

        private ISqlFieldInfo GetConcreteClassProperty(PropertyInfo property)
        {
            var requestedProperty = EntityInfoFactory.GetEntityInfo<T>().FieldList.FirstOrDefault(_ => _.PropertyInfo.Name == property.Name);

            if (requestedProperty != null) return requestedProperty;
            
            throw new NotSupportedException(string.Format("Type doesn't contains member expression property. Requested type:{0}. Property name:{1}.", typeof(T), property.Name));
        }
    }
}
