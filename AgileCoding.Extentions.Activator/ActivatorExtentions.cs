namespace AgileCoding.Extentions.Activators
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using AgileCoding.Extentions.Loggers;
    using AgileCoding.Library.Interfaces.Logging;

    public static class ActivatorExtentions
    {
        public static IReturnInstanceType CreateInstanceWithLogging<IReturnInstanceType>(this Type activatorType, ILogger logger, params object[] constructorParams)
        {
            try
            {
                logger.WriteVerbose($"{nameof(ActivatorExtentions)} - Trying to create Type {activatorType.Name}");

                if (activatorType == null)
                {
                    throw new ArgumentNullException($"{nameof(CreateInstanceWithLogging)} - Activator type was null. Unable to create instance");
                }

                return (IReturnInstanceType)Activator.CreateInstance(activatorType, constructorParams);
            }
            catch (MissingMethodException mme)
            {
                CreateConstructorInfoForMothodNotFoundException(activatorType, constructorParams, mme);
                throw;
            }
            catch (Exception ex)
            {
                var fullException = ex;
                logger.WriteError($"{nameof(ActivatorExtentions)} - Error Details:{Environment.NewLine}{fullException.ToStringAll()}");
                throw fullException;
            }
        }

        public static IReturnInstanceType CreateInstanceWithoutLogging<IReturnInstanceType>(this Type activatorType, params object[] constructorParams)
        {
            try
            {
                if (activatorType == null)
                {
                    throw new ArgumentNullException($"{nameof(CreateInstanceWithLogging)} - Activator type was null. Unable to create instance");
                }

                return (IReturnInstanceType)Activator.CreateInstance(activatorType, constructorParams);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                StringBuilder exceptionDetails = new StringBuilder(rtle.ToStringAll());

                if (rtle.LoaderExceptions.Length > 0)
                {
                    exceptionDetails.AppendLine($"Load Exception Count({rtle.LoaderExceptions.Length})");
                    rtle.LoaderExceptions
                        .ToList()
                        .ForEach((loaderException) => exceptionDetails.AppendLine(loaderException.ToStringAll()));
                }

                var ex2 = new Exception($"{nameof(ActivatorExtentions)} - Exception {Environment.NewLine}{rtle.ToString()}{Environment.NewLine}LoaderException : {exceptionDetails}");
                
                throw ex2;
            }
            catch (MissingMethodException mme)
            {
                CreateConstructorInfoForMothodNotFoundException(activatorType, constructorParams, mme);
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void CreateConstructorInfoForMothodNotFoundException<IGenericType>(IGenericType activatorType, object[] constructorParams, MissingMethodException mme) where IGenericType : Type
        {
            if (mme.Message.StartsWith("Constructor on type"))
            {
                var listOdTypeInConstructorParams = constructorParams.ToList().Select(x => x.GetType().Name).ToArray();
                var constructors = activatorType.GetConstructors();
                StringBuilder constructorsStrngBuilder = new StringBuilder();
                constructors
                    .ToList()
                    .ForEach((constructor) =>
                    {
                        var parametersList = constructor
                            .GetParameters()
                            .ToList()
                            .Select(x => $"{x.ParameterType.Name} {x.Name}")
                            .ToArray();

                        constructorsStrngBuilder.AppendLine($"{activatorType.Name}({string.Join(", ", parametersList)})");
                    }
                );
                MissingMethodException mme2 = new MissingMethodException($"Creating instance of type {activatorType.Name} with given params ({string.Join(",", listOdTypeInConstructorParams)})" +
                    $" does not match any of the constructor(s) on type. List  of constructors below : {Environment.NewLine}{constructorsStrngBuilder}", mme);

                throw mme2;
            }
        }
    }
}
