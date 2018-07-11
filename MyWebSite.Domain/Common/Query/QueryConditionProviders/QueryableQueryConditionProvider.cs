using MyWebSite.Domain.Common.Query.TransformProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MyWebSite.Domain.Common.Query.QueryConditionProviders
{
    public class QueryableQueryConditionProvider : IQueryConditionTransformProvider
    {
        private Type _ModelType = null;
        private List<ITransformProvider> _TransformProviders;

        protected virtual void Init()
        {
            _TransformProviders = new List<ITransformProvider>
            {
                new LikeTransformProvider(),
                new BetweenTransformProvider(),
                new InTransformProvider()
            };
        }

        public IQueryable<T> Search<T>(IQueryable<T> table, IEnumerable<ConditionItem> items)
        {
            _ModelType = typeof(T);
            //构建 c=>Body中的c
            ParameterExpression param = Expression.Parameter(typeof(T), "c");

            //判断ConditionItem的值是否为空,如果为空,则删除条件
            IList<ConditionItem> items2 = new List<ConditionItem>();
            foreach (var item in items)
            {
                if (item.Method != QueryMethod.In && item.Method != QueryMethod.ExIn && !string.IsNullOrEmpty(item.Value.ToString()))
                {
                    items2.Add(item);
                }
                else if (item.Method == QueryMethod.In || item.Method == QueryMethod.ExIn)
                {
                    Type type = item.Value.GetType();
                    if (type.FullName == "System.String")
                    {
                        if (item.Value != null && !string.IsNullOrEmpty(item.Value.ToString()))
                        {
                            items2.Add(item);
                        }
                    }
                    else if (type.IsGenericType)
                    {
                        var list = item.Value as IList<object>;
                        if (list != null && list.Any(x => x != null && !string.IsNullOrEmpty(x.ToString())) && string.Join(",", list).Length > 0)
                        {
                            items2.Add(item);
                        }
                    }
                    else if (type.IsArray)
                    {
                        var list = item.Value as IList<object>;
                        if (item.Value is Int32[])
                        {
                            var arr = item.Value as ICollection;
                            if (arr != null && arr.Count > 0)
                            {
                                items2.Add(item);
                            }
                        }
                        else if (list != null && list.Any() && string.Join(",", list).Length > 0)
                        {
                            items2.Add(item);
                        }
                    }
                }
            }
            if (items2.Any())
            {
                items = items2.AsEnumerable();
            }

            //构建c=>Body中的Body
            var body = GetExpressoinBody(param, items);
            //将二者拼为c=>Body
            var expression = Expression.Lambda<Func<T, bool>>(body, param);
            //传到Where中当做参数，类型为Expression<Func<T,bool>>
            return table.Where(expression);
        }

        private Expression GetExpressoinBody(ParameterExpression param, IEnumerable<ConditionItem> items)
        {
            var list = new List<Expression>();
            //OrGroup为空的情况下，即为And组合
            var andList = items.Where(c => string.IsNullOrEmpty(c.OrGroup));
            //将And的子Expression以AndAlso拼接
            if (andList.Count() != 0)
            {
                list.Add(GetGroupExpression(param, andList, Expression.AndAlso));
            }
            //其它的则为Or关系，不同Or组间以And分隔
            var orGroupByList = items.Where(c => !string.IsNullOrEmpty(c.OrGroup)).GroupBy(c => c.OrGroup);
            //拼接子Expression的Or关系
            foreach (IGrouping<string, ConditionItem> group in orGroupByList)
            {
                if (group.Count() != 0)
                    list.Add(GetGroupExpression(param, group, Expression.OrElse));
            }
            //将这些Expression再以And相连
            return list.Aggregate(Expression.AndAlso);
        }

        private Expression GetGroupExpression(ParameterExpression param, IEnumerable<ConditionItem> items, Func<Expression, Expression, Expression> func)
        {
            //获取最小的判断表达式
            var list = items.Where(item => item.Value != null && !string.IsNullOrEmpty(item.Value.ToString()) && item.Value.ToString().Any()).Select(item => GetExpression(param, item));
            //再以逻辑运算符相连
            return list.Aggregate(func);
        }

        private Expression GetExpression(ParameterExpression param, ConditionItem item)
        {
            //属性表达式
            LambdaExpression exp = GetPropertyLambdaExpression(item, param);
            //如果有特殊类型处理，则进行处理，暂时不关注
            foreach (var provider in _TransformProviders)
            {
                if (provider.Match(item, exp.Body.Type))
                {
                    return GetGroupExpression(param, provider.Transform(item, exp.Body.Type), Expression.AndAlso);
                }
            }
            //常量表达式
            var constant = ChangeTypeToExpression(item, exp.Body.Type);
            //以判断符或方法连接
            return ExpressionDict[item.Method](exp.Body, constant);
        }

        private LambdaExpression GetPropertyLambdaExpression(ConditionItem item, ParameterExpression param)
        {
            //获取每级属性如c.Users.Proiles.UserId
            var props = item.Field.Split('.');
            Expression propertyAccess = param;
            var typeOfProp = _ModelType;
            int i = 0;
            do
            {
                PropertyInfo property = typeOfProp.GetProperty(props[i]);
                if (property == null) return null;
                typeOfProp = property.PropertyType;
                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                i++;
            } while (i < props.Length);

            return Expression.Lambda(propertyAccess, param);
        }

        #region ChangeType

        /// <summary>
        /// 类型转换，支持非空类型与可空类型之间的转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversionType)
        {
            if (value == null) return null;
            return Convert.ChangeType(value, TypeUtil.GetUnNullableType(conversionType));
        }

        /// <summary>
        /// 转换SearchItem中的Value的类型，为表达式树
        /// </summary>
        /// <param name="item"></param>
        /// <param name="conversionType">目标类型</param>
        public static Expression ChangeTypeToExpression(ConditionItem item, Type conversionType)
        {
            if (item.Value == null) return Expression.Constant(item.Value, conversionType);

            #region 数组
            if (item.Method == QueryMethod.ExIn || item.Method == QueryMethod.In)
            {
                if (item.Value is String)
                {
                    item.Value = item.Value.ToString().Split(',');
                }

                var arr = (item.Value as Array);
                var expList = new List<Expression>();
                //确保可用
                if (arr != null)
                    for (var i = 0; i < arr.Length; i++)
                    {
                        //构造数组的单元Constant
                        var newValue = ChangeType(arr.GetValue(i), conversionType);
                        expList.Add(Expression.Constant(newValue, conversionType));
                    }
                //构造inType类型的数组表达式树，并为数组赋初值
                return Expression.NewArrayInit(conversionType, expList);
            }
            #endregion

            var elementType = TypeUtil.GetUnNullableType(conversionType);
            var value = Convert.ChangeType(item.Value, elementType);
            return Expression.Constant(value, conversionType);
        }

        #endregion

        #region SearchMethod 操作方法

        private static readonly Dictionary<QueryMethod, Func<Expression, Expression, Expression>> ExpressionDict =
            new Dictionary<QueryMethod, Func<Expression, Expression, Expression>>
                {
                    {
                        QueryMethod.Equal,
                        (left, right) => { return Expression.Equal(left, right); }
                        },
                    {
                        QueryMethod.NotEqual,
                        (left, right) => { return Expression.NotEqual(left, right); }
                        },
                    {
                        QueryMethod.LessThan,
                        (left, right) => { return Expression.LessThan(left, right); }
                        },
                    {
                        QueryMethod.LessThanOrEqual,
                        (left, right) => { return Expression.LessThanOrEqual(left, right); }
                        },
                    {
                        QueryMethod.GreaterThan,
                        (left, right) => { return Expression.GreaterThan(left, right); }
                        },
                    {
                        QueryMethod.GreaterThanOrEqual,
                        (left, right) => { return Expression.GreaterThanOrEqual(left, right); }
                        },

                    {
                        QueryMethod.Like,
                        (left, right) =>
                            {
                                if (left.Type != typeof (string)) return null;
                                return Expression.Call(left, typeof (string).GetMethod("Contains"), right);
                            }
                        },
                    {
                        QueryMethod.In,
                        (left, right) =>
                            {
                                if (!right.Type.IsArray) return null;
                                //调用Enumerable.Contains扩展方法
                                MethodCallExpression resultExp =
                                    Expression.Call(
                                        typeof (Enumerable),
                                        "Contains",
                                        new[] {left.Type},
                                        right,
                                        left);

                                return resultExp;
                            }
                        },
                    {
                        QueryMethod.ExIn,
                        (left, right) =>
                            {
                                if (!right.Type.IsArray) return null;
                                //调用Enumerable.Contains扩展方法
                                MethodCallExpression resultExp =
                                    Expression.Call(
                                        typeof (Enumerable),
                                        "Contains",
                                        new[] {left.Type},
                                        right,
                                        left);

                                return resultExp;
                            }
                        },
                    {
                        QueryMethod.StartsWith,
                        (left, right) =>
                            {
                                if (left.Type != typeof (string)) return null;
                                return Expression.Call(left, typeof (string).GetMethod("StartsWith", new[] {typeof (string)}), right);

                            }
                        },
                    {
                        QueryMethod.EndsWith,
                        (left, right) =>
                            {
                                if (left.Type != typeof (string)) return null;
                                return Expression.Call(left, typeof (string).GetMethod("EndsWith", new[] {typeof (string)}), right);
                            }
                        }
                };

        #endregion
    }
}
