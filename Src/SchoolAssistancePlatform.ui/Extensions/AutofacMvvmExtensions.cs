using System;
using System.Collections.Generic;

using Autofac;
using Autofac.Builder;
using Autofac.Core;

using Avalonia.Controls;

namespace SchoolAssistancePlatform.UI.Extensions;

/// <summary>Расширения для Autofac. (Avalonia)</summary>
public static partial class AutofacMvvmExtensions
{
	/// <summary>Присоединяет модель представления к виду <typeparamref name="TView" />.</summary>
	/// <typeparam name="TView">Тип вида.</typeparam>
	/// <param name="registrationBuilder">Регистрация компонента вида.</param>
	/// <returns>Объект регистрации модели представления для указанного вида.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="registrationBuilder" /> == <c>null</c>.</exception>
	public static IViewModelRegistration<TView, ConcreteReflectionActivatorData, SingleRegistrationStyle> WithViewModel<TView>(this IRegistrationBuilder<TView, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder)
		where TView : Control
	{
		return new ViewModelRegistration<TView>(registrationBuilder);
	}

	/// <summary>Присоединяет модель представления к generic-представлению.</summary>
	/// <param name="registrationBuilder"></param>
	/// <returns></returns>
	public static IViewModelRegistration<object, ReflectionActivatorData, DynamicRegistrationStyle> WithViewModel(this IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> registrationBuilder)
	{
		return new ViewModelRegistration(registrationBuilder);
	}

	/// <summary>Возвращает экземпляр объекта тип <typeparamref name="T" /> из параметров или контейнера.</summary>
	/// <typeparam name="T">Тип возвращаемого объекта.</typeparam>
	/// <param name="parameters">Список параметров.</param>
	/// <param name="componentContext">Контекст операции разрешения зависимости.</param>
	/// <returns>Объект типа <typeparamref name="T" />.</returns>
	private static T FromParamsOrResolve<T>(IEnumerable<Parameter> parameters, IComponentContext componentContext)
	{
		if(parameters != null)
		{
			foreach(var p in parameters)
			{
				if(p is TypedParameter typed &&
				   (typed.Type.IsGenericType && typed.Type.GetGenericTypeDefinition() == typeof(T)
					|| typed.Type == typeof(T)))
				{
					return (T)typed.Value;
				}
			}
		}
		return componentContext.Resolve<T>(parameters);
	}

	private static object FromParamsOrResolve(Type type, IEnumerable<Parameter> parameters, IComponentContext componentContext)
	{
		if(parameters != null)
		{
			foreach(var p in parameters)
			{
				if(p is TypedParameter typed &&
				   (typed.Type.IsGenericType && typed.Type.GetGenericTypeDefinition() == type
					|| typed.Type == type))
				{
					return typed.Value;
				}
			}
		}
		return componentContext.Resolve(type, parameters);
	}
}

