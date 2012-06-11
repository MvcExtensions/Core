#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion
namespace MvcExtensions
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Linq;

    /// <summary>
    /// ExpressionUtil
    /// </summary>
    public class ExpressionVisitorHelper : ExpressionVisitor
    {
        private readonly List<string> properties;
        private MethodInfo info;

        /// <summary>
        /// Ctor
        /// </summary>
        public ExpressionVisitorHelper()
        {
            properties = new List<string>();
        }

        /// <summary>
        /// Ge mMethod from expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public MethodInfo GetMethod(Expression expression)
        {
            Visit(expression);
            return info;
        }

        /// <summary>
        /// Get property name
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string GetPropertyName(Expression expression)
        {
            Visit(expression);
            return properties.FirstOrDefault();
        }

        /// <summary>
        /// Get full property name
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string GetFullPropertyName(Expression expression)
        {
            Visit(expression);
            properties.Reverse();
            return string.Join(".", properties);
        }

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.MethodCallExpression"/>.
        /// </summary>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        /// <param name="node">The expression to visit.</param>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var test = (ConstantExpression)node.Arguments[2];
            info = (MethodInfo)test.Value;
            return base.VisitMethodCall(node);
        }

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.MemberExpression"/>.
        /// </summary>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        /// <param name="node">The expression to visit.</param>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.MemberType == MemberTypes.Property)
            {
                properties.Add(node.Member.Name);
            }

            return base.VisitMember(node);
        }
    }
}
