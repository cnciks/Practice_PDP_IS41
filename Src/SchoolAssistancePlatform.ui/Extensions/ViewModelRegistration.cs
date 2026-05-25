using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Builder;
using Autofac.Core;

using Avalonia.Controls;

namespace SchoolAssistancePlatform.UI.Extensions;

partial class AutofacMvvmExtensions
{
	private sealed class ViewModelRegistration<TView> : IViewModelRegistration<TView, ConcreteReflectionActivatorData, SingleRegistrationStyle>
		where TView : Control
	{
		private readonly IRegistrationBuilder<TView, ConcreteReflectionActivatorData, SingleRegistrationStyle> _registrationBuilder;

		public ViewModelRegistration(IRegistrationBuilder<TView, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder)
		{
			_registrationBuilder = registrationBuilder;
		}

		public IRegistrationBuilder<TView, ConcreteReflectionActivatorData, SingleRegistrationStyle> Instance(object instance)
		{
			return _registrationBuilder
				.AsSelf()
				.Keyed<Control>(instance.GetType())
				.OnActivated(h =>
				{
					h.Instance.DataContext = instance;
				});
		}

		public IRegistrationBuilder<TView, ConcreteReflectionActivatorData, SingleRegistrationStyle> OfType<TViewModel>()
		{
			return _registrationBuilder
				.AsSelf()
				.Keyed<Control>(typeof(TViewModel))
				.OnActivated(h =>
				{
					h.Instance.DataContext = FromParamsOrResolve<TViewModel>(h.Parameters, h.Context);
				});
		}

		public IRegistrationBuilder<TView, ConcreteReflectionActivatorData, SingleRegistrationStyle> OfType<TViewModel>(IEnumerable<Parameter> parameters)
		{
			return _registrationBuilder
				.AsSelf()
				.Keyed<Control>(typeof(TViewModel))
				.OnActivated(h =>
				{
					h.Instance.DataContext = FromParamsOrResolve<TViewModel>(parameters, h.Context);
				});
		}

		public IRegistrationBuilder<TView, ConcreteReflectionActivatorData, SingleRegistrationStyle> OfType<TViewModel>(Parameter[] parameters)
		{
			return _registrationBuilder
				.AsSelf()
				.Keyed<Control>(typeof(TViewModel))
				.OnActivated(h =>
				{
					h.Instance.DataContext = FromParamsOrResolve<TViewModel>(parameters, h.Context);
				});
		}

		public IRegistrationBuilder<TView, ConcreteReflectionActivatorData, SingleRegistrationStyle> OfType(Type viewModelType)
		{
			if(viewModelType.IsGenericTypeDefinition)
			{
				return _registrationBuilder
					.AsSelf()
					.Keyed<Control>(viewModelType)
					.WithMetadata(viewModelType.FullName, viewModelType)
					.OnActivated(h =>
					{
						if(h.Parameters.Any() && h.Parameters.First() is TypedParameter parameter && parameter.Type.GetGenericTypeDefinition() == viewModelType)
						{
							if(h.Component.Metadata.TryGetValue(viewModelType.FullName, out var value))
							{
								var arguments = h.Instance.GetType().GenericTypeArguments;
								var type = (Type)value;
								if(arguments.Length != 0)
								{
									type = type.MakeGenericType(arguments);
								}
								h.Instance.DataContext = FromParamsOrResolve(type, h.Parameters, h.Context);
							}
						}
						else
						{
						}
					});
			}
			return _registrationBuilder
				.AsSelf()
				.Keyed<Control>(viewModelType)
				.OnActivated(h =>
				{
					h.Instance.DataContext = FromParamsOrResolve(viewModelType, h.Parameters, h.Context);
				});
		}

		public IRegistrationBuilder<TView, ConcreteReflectionActivatorData, SingleRegistrationStyle> OfType(Type viewModelType, IEnumerable<Parameter> parameters)
		{
			return _registrationBuilder
				.AsSelf()
				.Keyed<Control>(viewModelType)
				.OnActivated(h =>
				{
					h.Instance.DataContext = FromParamsOrResolve(viewModelType, parameters, h.Context);
				});
		}

		public IRegistrationBuilder<TView, ConcreteReflectionActivatorData, SingleRegistrationStyle> OfType(Type viewModelType, Parameter[] parameters)
		{
			return _registrationBuilder
				.AsSelf()
				.Keyed<Control>(viewModelType)
				.OnActivated(h =>
				{
					h.Instance.DataContext = FromParamsOrResolve(viewModelType, parameters, h.Context);
				});
		}
	}

	/// <summary>Для View и VideoModel с Generic.</summary>
	private sealed class ViewModelRegistration : IViewModelRegistration<object, ReflectionActivatorData, DynamicRegistrationStyle>
	{
		private readonly IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> _registrationBuilder;

		/// <summary>Конструктор для generic.</summary>
		/// <param name="registrationBuilder"></param>
		public ViewModelRegistration(IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> registrationBuilder)
		{
			_registrationBuilder = registrationBuilder;
		}

		public IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> OfType<TViewModel>()
		{
			throw new NotImplementedException();
		}

		public IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> OfType<TViewModel>(IEnumerable<Parameter> parameters)
		{
			throw new NotImplementedException();
		}

		public IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> OfType<TViewModel>(params Parameter[] parameters)
		{
			throw new NotImplementedException();
		}

		public IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> OfType(Type viewModelType)
		{
			return _registrationBuilder
				.AsSelf()
				.WithMetadata(viewModelType.FullName, viewModelType)
				.OnActivated(h =>
				{
					if(h.Parameters.Any() && h.Parameters.First() is TypedParameter parameter && parameter.Type == viewModelType)
					{
						if(h.Instance is Control instance
						   && h.Component.Metadata.TryGetValue(viewModelType.FullName, out var value))
						{
							var arguments = h.Instance.GetType().GenericTypeArguments;
							var type = (Type)value;
							if(arguments.Length != 0)
							{
								type = type.MakeGenericType(arguments);
							}
							instance.DataContext = FromParamsOrResolve(type, h.Parameters, h.Context);
						}
					}
				});
		}

		public IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> OfType(Type viewModelType, IEnumerable<Parameter> parameters)
		{
			throw new NotImplementedException();
		}

		public IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> OfType(Type viewModelType, params Parameter[] parameters)
		{
			throw new NotImplementedException();
		}

		public IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> Instance(object instance)
		{
			throw new NotImplementedException();
		}
	}
}
