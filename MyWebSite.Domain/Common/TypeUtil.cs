using MyWebSite.Domain.MixService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MyWebSite.Domain.Common
{
    /// <summary>
    /// Type类的处理工具类
    /// </summary>
    public class TypeUtil
    {
        /// <summary>
        /// 获取可空类型的实际类型
        /// </summary>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static Type GetUnNullableType(Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                //如果是泛型方法，且泛型类型为Nullable<>则视为可空类型
                //并使用NullableConverter转换器进行转换
                var nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return conversionType;
        }

        public static List<string> FindPrimaryKeyColNames(object obj)
        {
            Type t = obj.GetType();
            List<string> pk = new List<string>();
            //foreach (PropertyInfo pi in t.GetProperties())
            //{
            //    object[] atts = pi.GetCustomAttributes(true);
            //    foreach (object att in atts)
            //    {
            //        ColumnAttribute colAtt = att as ColumnAttribute;
            //        if (att is ColumnAttribute && (colAtt as ColumnAttribute).IsPrimaryKey)
            //            pk.Add(pi.Name);
            //    }
            //}
            return pk;
        }
        public static void ApplyChange(object target, object change)
        {
            Type t1 = target.GetType(); Type t2 = target.GetType();
            //检查类型是否相同     
            if (t1 != t2)
                throw new ApplicationException("更新类型不匹配!");
            //找出Primary Key    
            List<string> pk = FindPrimaryKeyColNames(target);
            if (pk.Count == 0)
                throw new ApplicationException("没有找到主键!");
            Dictionary<string, PropertyInfo> props = new Dictionary<string, PropertyInfo>();
            string removetype = "UserEvaluation";
            foreach (PropertyInfo pi in t1.GetProperties())
            {
                if ((pi.PropertyType).FullName.ToLower().IndexOf(removetype.ToLower()) < 0)//remove table
                {
                    props.Add(pi.Name, pi);        //加入所有列 
                }
            }
            foreach (string c in pk)
            {
                PropertyInfo pi = props[c];
                object v1 = pi.GetValue(target, null);
                object v2 = pi.GetValue(change, null);
                if (!v1.Equals(v2))
                    throw new ApplicationException("主键不同!");
            }
            //对比除主键外的值更新     
            foreach (PropertyInfo pi in props.Values)
            {
                if (pk.Contains(pi.Name))
                    continue;
                object v1 = pi.GetValue(target, null);
                object v2 = pi.GetValue(change, null);
                if (v1 != null)
                {
                    if (!v1.Equals(v2))
                    {
                        pi.SetValue(target, v2, null);
                    }
                }
                else
                {
                    if (v2 != null)
                    {
                        pi.SetValue(target, v2, null);
                    }
                }
            }
        }

        /// <summary>
        /// 对比两个相同的实体类中，数据是否相同[A：修改后的值,B：原始值]
        /// </summary>
        public static Dictionary<string, string> ContrastEntity(object A, object B)
        {
            Dictionary<string, string> Dict = new Dictionary<string, string>();
            PropertyInfo[] PropertyA = A.GetType().GetProperties();
            PropertyInfo[] PropertyB = B.GetType().GetProperties();
            foreach (var itemA in PropertyA)
            {
                foreach (var itemB in PropertyB)
                {
                    if (itemA.Name == itemB.Name && itemA.Name != "Issue" && itemA.Name != "IssueCPIPAttachment")
                    {
                        bool Is = false;
                        if (itemA.GetValue(A, null) == null || itemB.GetValue(B, null) == null)
                        {
                            if (itemA.GetValue(A, null) != itemB.GetValue(B, null))
                                Is = true;
                        }
                        else
                        {
                            if (!itemA.GetValue(A, null).Equals(itemB.GetValue(B, null)))
                                Is = true;
                        }

                        if (Is)
                        {
                            string field = GetDisplayName(itemA);
                            string ModelA = itemA.GetValue(A, null) == null ? string.Empty : itemA.GetValue(A, null).ToString();
                            string OriginalB = itemB.GetValue(B, null) == null ? string.Empty : itemB.GetValue(B, null).ToString();
                            if (!Dict.ContainsKey(field))
                                Dict.Add(field, ModelA + "|" + OriginalB + "|" + itemB.Name);
                            itemB.SetValue(B, itemA.GetValue(A, null), null);
                        }
                        break;
                    }
                }
            }
            return Dict;
        }

        /// <summary>
        /// 对比两个相同的实体类中，数据是否相同[A：修改后的值,B：原始值]
        /// </summary>
        public static Dictionary<string, string> ContrastEntityByIssue(object A, object B)
        {
            Dictionary<string, string> Dict = new Dictionary<string, string>();
            PropertyInfo[] PropertyA = A.GetType().GetProperties();
            PropertyInfo[] PropertyB = B.GetType().GetProperties();

            string[] strName = new string[]
            {
                "BasisBase", "QEOnwer", "Receiver", "Manger", "ReceiverBasisArea",
                "QESender", "CPIPQE", "BasisPart", "BasisIssueType",
                "Author", "BasisConfiguration", "BasisDefect", "BasisModule",
                "AuthorBasisArea", "BasisModel", "IssueAttachment", "IssueQeAttachment",
                "IssueQERemark", "IssueBreakPoint", "IssueContainment", "IssueOwnerAvailable",
                "IssueSolution", "IssueVerification", "IssueWhy", "IssueWorkHours","Issue",
                "OwnerManage"
            };

            foreach (var itemA in PropertyA)
            {
                foreach (var itemB in PropertyB)
                {
                    if (itemA.Name == itemB.Name && !strName.Contains(itemA.Name))
                    {
                        bool Is = false;
                        if (itemA.GetValue(A, null) == null || itemB.GetValue(B, null) == null)
                        {
                            if (itemA.GetValue(A, null) != itemB.GetValue(B, null))
                                Is = true;
                        }
                        else
                        {
                            if (!itemA.GetValue(A, null).Equals(itemB.GetValue(B, null)))
                                Is = true;
                        }

                        if (Is)
                        {
                            string field = GetDisplayName(itemA);
                            string ModelA = itemA.GetValue(A, null) == null ? string.Empty : itemA.GetValue(A, null).ToString();
                            string OriginalB = itemB.GetValue(B, null) == null ? string.Empty : itemB.GetValue(B, null).ToString();
                            if (!Dict.ContainsKey(field))
                                Dict.Add(field, ModelA + "|" + OriginalB + "|" + itemB.Name);
                            itemB.SetValue(B, itemA.GetValue(A, null), null);
                        }
                        break;
                    }
                }
            }
            return Dict;
        }

        /// <summary>
        /// 修改基础数据记录日志[Entity:修改后,Original：修改前]
        /// </summary>
        public static void EidtLog<T>(T Entity, T Original, string Account, string UserName)
        {
            PropertyInfo[] A = Entity.GetType().GetProperties();
            PropertyInfo[] B = Original.GetType().GetProperties();
            foreach (var itemA in A)
            {
                foreach (var itemB in B)
                {
                    if (itemB.PropertyType.Name == "Int32" || itemB.PropertyType.Name == "String" || itemB.PropertyType.Name == "Boolean" || itemB.PropertyType.Name == "DateTime" || itemB.PropertyType.Name == "Double" || itemB.PropertyType.Name == "Nullable`1")
                    {
                        if (itemA.Name == itemB.Name && itemA.Name != "LastUpdateUserID" && itemA.Name != "LastUpdateUserName" && itemA.Name != "LastUpdateDate" && itemA.Name != "CreateDate")
                        {
                            bool IsEquals = false;
                            if (itemA.GetValue(Entity, null) == null || itemB.GetValue(Original, null) == null)
                            {
                                if (itemA.GetValue(Entity, null) != itemB.GetValue(Original, null))
                                    IsEquals = true;
                            }
                            else
                            {
                                if (!itemA.GetValue(Entity, null).Equals(itemB.GetValue(Original, null)))
                                    IsEquals = true;
                            }
                            if (IsEquals)  //不相等记录日志
                            {
                                string field = GetDisplayName(itemA);
                                string ValueA = itemA.GetValue(Entity, null) == null ? string.Empty : itemA.GetValue(Entity, null).ToString();  //修改后
                                string ValueB = itemB.GetValue(Original, null) == null ? string.Empty : itemB.GetValue(Original, null).ToString();  //修改前
                                //记录日志
                                string Message = "用户帐号:" + Account + ".用户名字:" + UserName + " 修改了.表：" + Entity.GetType().Name + ".修改内容:" + field + ".原数据为：" + ValueB + ";现修改为:" + ValueA;
                                //LogHelper.Info(Message);

                                itemB.SetValue(Original, itemA.GetValue(Entity, null), null);
                                break;
                            }
                        }
                    }

                }
            }
        }

        public static void EidtAddField(string Account, string UserName, string TableName, string UpdateInfo, string UpdateValue)
        {
            //记录日志
            string Message = "用户帐号:" + Account + ".用户名字:" + UserName + " 修改了.表：" + TableName + ".修改内容:" + UpdateInfo + ".现修改为：" + UpdateValue;
            //LogHelper.Info(Message);
        }

        /// <summary>
        /// 删除基础数据记录日志
        /// </summary>
        public static void DeleteLog<T>(T Entity, string Account, string UserName)
        {
            string Str = string.Empty;
            PropertyInfo[] A = Entity.GetType().GetProperties();
            foreach (var itemA in A)
            {
                if (itemA.PropertyType.Name == "Int32" || itemA.PropertyType.Name == "String" || itemA.PropertyType.Name == "Boolean" || itemA.PropertyType.Name == "DateTime" || itemA.PropertyType.Name == "Double")
                {
                    string value = itemA.GetValue(Entity, null) == null ? string.Empty : itemA.GetValue(Entity, null).ToString();
                    Str += "[" + GetDisplayName(itemA) + ":" + value + "]";
                }
            }
            string Message = "用户帐号:" + Account + ".用户名字:" + UserName + " 删除了.表：" + Entity.GetType().Name + "." + Str;
            //LogHelper.Info(Message);
        }


        /// <summary>
        /// 对比两个相同的实体类中，数据是否相同
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="A">原始数据</param>
        /// <param name="B">修改后数据</param>
        /// <returns></returns>
        public static List<string> ContrastEntityList<T>(List<T> A, List<T> B)
        {
            List<string> list = new List<string>();
            string KeyValueA = string.Empty;
            string KeyValueB = string.Empty;
            for (int i = 0; i < A.Count; i++)
            {
                KeyValueA = GetKeyValue(A[i]);
                if (KeyValueA == "0")
                    break;//新增数据直接不做处理

                string field = SetFieldName(A[i]);

                for (int j = 0; j < B.Count; j++)
                {
                    KeyValueB = GetKeyValue(B[j]);

                    if (KeyValueA == KeyValueB)  //有相同的记录
                    {
                        Dictionary<string, string> NContrast = ContrastEntity(B[j], A[i]);   //反射修改后的字段
                        foreach (var DicItem in NContrast)
                        {
                            list.Add(field + "|" + DicItem.Key + "|" + DicItem.Value);
                        }
                        break;
                    }

                    if (j == B.Count - 1)  //删除过的记录
                        list.Add(field + "|原始数据|被删除|被删除");
                }

                if (B.Count == 0)
                {
                    list.Add(field + "|原始数据|被删除|被删除");
                }
            }
            return list;
        }

        /// <summary>
        /// 设置修改字段名称
        /// </summary>
        /// <returns></returns>
        public static string SetFieldName<T>(T Info)
        {
            string ClassName = Info.GetType().Name.Split('_')[0];
            string KeyName = string.Empty;
            switch (ClassName)
            {
                case "IssueWhy":
                    ClassName = "根本原因分析";
                    KeyName = "Question";
                    break;
                case "IssueBreakPoint":
                    foreach (var item in Info.GetType().GetProperties())
                    {
                        if (item.Name == "BreakPointType")
                        {
                            if (item.GetValue(Info, null).ToString() == "1")
                                ClassName = "抑制措施";
                            else if (item.GetValue(Info, null).ToString() == "2")
                                ClassName = "短期措施";
                            else if (item.GetValue(Info, null).ToString() == "3")
                                ClassName = "长期措施";
                            break;
                        }
                    }
                    KeyName = "Containment";
                    break;
                case "IssueWorkHours":
                    ClassName = "工时代码";
                    break;
                case "IssueQEIndexEntry":
                    foreach (var item in Info.GetType().GetProperties())
                    {
                        if (item.Name == "IndexType")
                        {
                            if (item.GetValue(Info, null).ToString() == "1")
                                ClassName = "指标类型-开启值";
                            else if (item.GetValue(Info, null).ToString() == "2")
                                ClassName = "指标类型-目标值";
                            else if (item.GetValue(Info, null).ToString() == "0")
                                ClassName = "月度指标类型";
                            break;
                        }
                    }
                    KeyName = "BasisTargetTypeID";
                    break;
                case "IssueSolveProgress":
                    ClassName = "问题解决进度";
                    KeyName = "Progress";
                    break;
            }
            foreach (var item in Info.GetType().GetProperties())
            {
                if (item.Name == KeyName && item.GetValue(Info, null) != null)
                {
                    KeyName = item.GetValue(Info, null).ToString();
                    break;
                }
            }
            return ClassName + ":" + KeyName;
        }

        /// <summary>
        /// 获得实体中的主键值
        /// </summary>
        public static string GetKeyValue(object T)
        {
            string KeyName = T.GetType().Name.Split('_')[0] + "ID";  //主键
            if (T.GetType().Name.Split('_')[0] == "IssueSolveProgress")
            {
                KeyName = "IssueProgressID";
            }
            string KeyValue = "0";
            var PropertA = T.GetType().GetProperties();
            foreach (var PPA in PropertA)
            {
                if (PPA.Name == KeyName && PPA.GetValue(T, null) != null)
                {
                    KeyValue = PPA.GetValue(T, null).ToString();
                }
            }
            return KeyValue;
        }
        public static List<int> GetIntList(string[] strlist)
        {
            List<int> newlist = new List<int>();
            foreach (string str in strlist)
            {
                int i = 0;
                if (int.TryParse(str, out i))
                {
                    newlist.Add(i);
                }
            }
            return newlist;
        }
        /// <summary>
        /// 获得实体中Display属性的值
        /// </summary>
        /// <returns></returns>
        public static string GetDisplayName(PropertyInfo Info)
        {
            string field = Info.Name;
            object[] objs = Info.GetCustomAttributes(true);
            if (objs.Length > 0)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i] is DisplayAttribute)
                    {
                        DisplayAttribute Display = objs[i] as DisplayAttribute;
                        field = Display.Name;
                    }
                }
            }
            return field;
        }

        public static void Clone<Tfrom, Tto>(Tfrom Entity, Tto Original)
        {
            PropertyInfo[] A = Entity.GetType().GetProperties(); //有值的
            PropertyInfo[] B = Original.GetType().GetProperties();//没值的
            foreach (var itemA in A)
            {
                foreach (var itemB in B)
                {
                    if (itemB.PropertyType.Name == "Int32" || itemB.PropertyType.Name == "Int64" || itemB.PropertyType.Name == "String" || itemB.PropertyType.Name == "Boolean" || itemB.PropertyType.Name == "DateTime" || itemB.PropertyType.Name == "Double" || itemB.PropertyType.Name == "Nullable`1")
                    {
                        if (itemA.Name == itemB.Name && itemA.Name != "LastUpdateUserID" && itemA.Name != "LastUpdateUserName" && itemA.Name != "LastUpdateDate" && itemA.Name != "CreateDate")
                        {
                            string field = GetDisplayName(itemA);
                            string ValueA = itemA.GetValue(Entity, null) == null ? string.Empty : itemA.GetValue(Entity, null).ToString();  //修改后
                            string ValueB = itemB.GetValue(Original, null) == null ? string.Empty : itemB.GetValue(Original, null).ToString();  //修改前
                            itemB.SetValue(Original, itemA.GetValue(Entity, null), null);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 深度克隆 ,相当于new,也就是在内存中重新创建  不受原来属性的改变而改变 
        /// </summary>
        /// <returns></returns>
        public static Object DeepClone(object obj)
        {
            //该方法要求  如果当前类的某个属性也是一个对象，则要求该属性对象要序列化
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Position = 0;
            return formatter.Deserialize(stream) as Object;
        }

    }
}
