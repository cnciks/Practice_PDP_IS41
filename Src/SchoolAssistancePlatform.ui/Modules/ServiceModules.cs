using Autofac;

using SchoolAssistancePlatform.framework.Interfaces;
using SchoolAssistancePlatform.UI.Actions;
using SchoolAssistancePlatform.UI.Services;

namespace SchoolAssistancePlatform.UI.Modules;

internal class ServiceModules : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder
			.RegisterType<AccountService>()
			.AsSelf()
			.As<IInitializer>()
			.SingleInstance();

		builder
			.RegisterType<EmployeeService>()
			.AsSelf()
			.SingleInstance();

		builder
			.RegisterType<StudentService>()
			.AsSelf()
			.SingleInstance();

		builder
			.RegisterType<ClassService>()
			.AsSelf()
			.SingleInstance();

		builder
			.RegisterType<StatisticsService>()
			.AsSelf()
			.SingleInstance();

		builder
			.RegisterType<GradeJournalService>()
			.AsSelf()
			.SingleInstance();

		builder
			.RegisterType<ScheduleService>()
			.AsSelf()
			.SingleInstance();

		builder
			.RegisterType<CurriculumService>()
			.AsSelf()
			.SingleInstance();

		builder
			.RegisterType<FinanceService>()
			.AsSelf()
			.SingleInstance();

		builder
			.RegisterType<CommandsService>()
			.AsSelf()
			.SingleInstance();

		builder
			.RegisterType<MessagesService>()
			.AsSelf()
			.SingleInstance();

		builder
			.RegisterType<AdministrationService>()
			.AsSelf()
			.SingleInstance();

		builder
			.RegisterType<MenuActions>()
			.AsSelf()
			.SingleInstance();
	}
}
