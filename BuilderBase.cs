using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoScout24.SMP.Common.Domain.TestHelper
{
    public class BuilderBase<T>
    {
        private readonly dynamic propertyBag = new ExpandoObject();

        protected dynamic PropertyBag
        {
            get
            {
                return propertyBag;
            }
        }

        public virtual T Build()
        {
            var fieldInfos = (IDictionary<string, object>)PropertyBag;
            T builtObject = ConstructObject(fieldInfos);

            // Fields that have not been yet set in the constructor.
            foreach (var fieldInfo in fieldInfos)
            {
                string name = fieldInfo.Key;
                var propertyInfo = typeof(T).GetProperty(name);
                if (propertyInfo == null || (!propertyInfo.CanWrite && propertyInfo.ReflectedType == propertyInfo.DeclaringType))
                {
                    Console.WriteLine("Missing property for field " + name);
                    continue;
                }

                try
                {
                    propertyInfo.SetValue(builtObject, fieldInfo.Value, null);
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Could not set property " + name, e);
                }
            }

            return builtObject;
        }
        
        protected virtual T ConstructObject(IDictionary<string, object> builderFieldInfos)
        {
            object builtObject;
            ConstructorInfo nonDefaultConstructor;
            try
            {
                nonDefaultConstructor = typeof(T).GetConstructors().Where(x => x.GetParameters().Length > 0).SingleOrDefault();
            }
            catch (InvalidOperationException e)
            {
                throw new ArgumentException(string.Format("The type '{0}' could not be builded by the builder because it contains more then one non default constructors",
                                            typeof(T).Name),
                                            e);
            }

            if (nonDefaultConstructor != null)
            {
                var parameterValues = new List<object>();
                foreach (var parameterInfo in nonDefaultConstructor.GetParameters())
                {
                    var parameterName = parameterInfo.Name;
                    if (!builderFieldInfos.Keys.Any(x => string.Compare(x, parameterName, StringComparison.OrdinalIgnoreCase) == 0))
                    {
                        parameterValues.Add(parameterInfo.ParameterType.IsClass ? null : Activator.CreateInstance(parameterInfo.ParameterType));
                    }
                    else
                    {
                        var fieldInfo = builderFieldInfos.First(x => string.Compare(x.Key, parameterName, StringComparison.OrdinalIgnoreCase) == 0);
                        parameterValues.Add(fieldInfo.Value);

                        // Removes the fields that have been allready set, so that they will be not longer used in the propertie setters
                        builderFieldInfos.Remove(fieldInfo);
                    }
                }

                try
                {
                    builtObject = nonDefaultConstructor.Invoke(parameterValues.ToArray());
                }
                catch (TargetInvocationException exception)
                {
                    if (exception.InnerException != null)
                    {
                        throw exception.InnerException;                        
                    }

                    throw;
                }
            }
            else
            {
                builtObject = CreateUsingDefaultConstructor();
            }

            return (T)builtObject;
        }

        private static object CreateUsingDefaultConstructor()
        {
            // From mongo driver
            const BindingFlags BindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            Expression expression;
            
            Type classType = typeof(T);
            var defaultConstructor = classType.GetConstructor(BindingFlags, null, new Type[0], null);
            if (defaultConstructor != null)
            {
                expression = Expression.New(defaultConstructor);
            }
            else
            {
                throw new InvalidOperationException(string.Format("The entity '{0}' has no default constructor", typeof(T).Name));
            }

            var lambda = Expression.Lambda<Func<object>>(expression);
            return lambda.Compile().Invoke();
        }
    }
}