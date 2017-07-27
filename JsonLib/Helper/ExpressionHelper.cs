using System;
using System.Linq.Expressions;
using System.Reflection;

namespace com.github.zvreifnitz.JsonLib.Helper
{
    public static class ExpressionHelper
    {
        public static Func<TClass> CreateDefaultConstructor<TClass>()
        {
            var body = Expression.New(typeof(TClass));
            var lambda = Expression.Lambda<Func<TClass>>(body);
            return lambda.Compile();
        }

        public static string GetPropertyName<TClass, TProp>(Expression<Func<TClass, TProp>> property)
        {
            return GetProperty(property).Name;
        }

        public static Action<TClass, TProp> CreateSetter<TClass, TProp>(
            Expression<Func<TClass, TProp>> property)
        {
            ParameterExpression instance = Expression.Parameter(typeof(TClass), "instance");
            ParameterExpression parameter = Expression.Parameter(typeof(TProp), "param");
            var body = Expression.Call(instance, GetProperty(property).SetMethod, parameter);
            var parameters = new[] {instance, parameter};
            return Expression.Lambda<Action<TClass, TProp>>(body, parameters).Compile();
        }

        public static Func<TClass, TProp> CreateGetter<TClass, TProp>(
            Expression<Func<TClass, TProp>> property)
        {
            ParameterExpression instance = Expression.Parameter(typeof(TClass), "instance");
            var body = Expression.Call(instance, GetProperty(property).GetMethod);
            var parameters = new[] {instance};
            return Expression.Lambda<Func<TClass, TProp>>(body, parameters).Compile();
        }

        private static PropertyInfo GetProperty<TClass, TProp>(Expression<Func<TClass, TProp>> expression)
        {
            var member = GetMemberExpression(expression).Member;
            var property = member as PropertyInfo;
            if (property == null)
            {
                throw new InvalidOperationException(
                    string.Format("Member with Name '{0}' is not a property.", member.Name));
            }
            return property;
        }

        private static MemberExpression GetMemberExpression<TClass, TProp>(
            Expression<Func<TClass, TProp>> expression)
        {
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }
            if (memberExpression == null)
            {
                throw new ArgumentException("Not a member access", "expression");
            }
            return memberExpression;
        }
    }
}