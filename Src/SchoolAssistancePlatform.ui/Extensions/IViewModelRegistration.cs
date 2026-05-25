using System;
using System.Collections.Generic;

using Autofac.Builder;
using Autofac.Core;

namespace SchoolAssistancePlatform.UI.Extensions;

/// <summary>Регистрация представления в контейнере Autofac.</summary>
/// <typeparam name="TView">Тип представления.</typeparam>
/// <typeparam name="TActivator"></typeparam>
/// <typeparam name="TRegistrationStyle"></typeparam>
public interface IViewModelRegistration<out TView, out TActivator, out TRegistrationStyle>
{
	/// <summary>Задает тип модели представления.</summary>
	/// <typeparam name="TViewModel">Тип модели представления.</typeparam>
	/// <returns>Регистрация компонента Autofac для дальнейшей настройки.</returns>
	IRegistrationBuilder<TView, TActivator, TRegistrationStyle> OfType<TViewModel>();

	/// <summary>Задает тип модели представления.</summary>
	/// <typeparam name="TViewModel">Тип модели представления.</typeparam>
	/// <param name="parameters">Параметры, с которыми контейнер будет возвращать модель представления.</param>
	/// <returns>Регистрация компонента Autofac для дальнейшей настройки.</returns>
	IRegistrationBuilder<TView, TActivator, TRegistrationStyle> OfType<TViewModel>(IEnumerable<Parameter> parameters);

	/// <summary>Задает тип модели представления.</summary>
	/// <typeparam name="TViewModel">Тип модели представления.</typeparam>
	/// <param name="parameters">Параметры, с которыми контейнер будет возвращать модель представления.</param>
	/// <returns>Регистрация компонента Autofac для дальнейшей настройки.</returns>
	IRegistrationBuilder<TView, TActivator, TRegistrationStyle> OfType<TViewModel>(params Parameter[] parameters);

	/// <summary>Задает тип модели представления.</summary>
	/// <param name="viewModelType">Тип модели представления.</param>
	/// <returns>Регистрация компонента Autofac для дальнейшей настройки.</returns>
	IRegistrationBuilder<TView, TActivator, TRegistrationStyle> OfType(Type viewModelType);

	/// <summary>Задает тип модели представления.</summary>
	/// <param name="viewModelType">Тип модели представления.</param>
	/// <param name="parameters">Параметры, с которыми контейнер будет возвращать модель представления.</param>
	/// <returns>Регистрация компонента Autofac для дальнейшей настройки.</returns>
	IRegistrationBuilder<TView, TActivator, TRegistrationStyle> OfType(Type viewModelType, IEnumerable<Parameter> parameters);

	/// <summary>Задает тип модели представления.</summary>
	/// <param name="viewModelType">Тип модели представления.</param>
	/// <param name="parameters">Параметры, с которыми контейнер будет возвращать модель представления.</param>
	/// <returns>Регистрация компонента Autofac для дальнейшей настройки.</returns>
	IRegistrationBuilder<TView, TActivator, TRegistrationStyle> OfType(Type viewModelType, params Parameter[] parameters);

	/// <summary>Задает экземпляр модели представления.</summary>
	/// <param name="instance">Экземпляр модели представления.</param>
	/// <returns>Регистрация компонента Autofac для дальнейшей настройки.</returns>
	IRegistrationBuilder<TView, TActivator, TRegistrationStyle> Instance(object instance);
}

