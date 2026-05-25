using Autofac;

using SchoolAssistancePlatform.UI.Extensions;
using SchoolAssistancePlatform.UI.Interfaces;
using SchoolAssistancePlatform.UI.Views.Auth;
using SchoolAssistancePlatform.UI.Views.Pages.AdministrationPage;
using SchoolAssistancePlatform.UI.Views.Pages.ClassesPage;
using SchoolAssistancePlatform.UI.Views.Pages.CommandsPage;
using SchoolAssistancePlatform.UI.Views.Pages.CurriculumPage;
using SchoolAssistancePlatform.UI.Views.Pages.EmployeesPage;
using SchoolAssistancePlatform.UI.Views.Pages.FinancePage;
using SchoolAssistancePlatform.UI.Views.Pages.JournalPage;
using SchoolAssistancePlatform.UI.Views.Pages.MainPage;
using SchoolAssistancePlatform.UI.Views.Pages.MessagesPage;
using SchoolAssistancePlatform.UI.Views.Pages.ReportsPage;
using SchoolAssistancePlatform.UI.Views.Pages.SchedulePage;
using SchoolAssistancePlatform.UI.Views.Pages.StudentsPage;
using SchoolAssistancePlatform.UI.Views.WorkSpace;
using SchoolAssistancePlatform.UI.Windows;

namespace SchoolAssistancePlatform.UI.Modules;

internal class ViewModules : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder
			.RegisterType<MainWindowViewModel>()
			.AsSelf()
			.SingleInstance();

		builder
			.RegisterType<MainWindow>()
			.WithViewModel()
			.OfType<MainWindowViewModel>()
			.SingleInstance();

		builder
			.RegisterType<WorkSpaceViewModel>()
			.AsSelf()
			.SingleInstance();

		builder
			.RegisterType<WorkSpaceView>()
			.WithViewModel()
			.OfType<WorkSpaceViewModel>()
			.SingleInstance();

		builder
			.RegisterType<AuthViewModel>()
			.AsSelf()
			.SingleInstance();

		builder
			.RegisterType<AuthView>()
			.WithViewModel()
			.OfType<AuthViewModel>()
			.SingleInstance();

		builder
			.RegisterType<MainPageViewModel>()
			.AsSelf()
			.As<IWorkSpacePage>()
			.SingleInstance();

		builder
			.RegisterType<MainPageView>()
			.WithViewModel()
			.OfType<MainPageViewModel>()
			.SingleInstance();

		builder
			.RegisterType<EmployeesPageViewModel>()
			.AsSelf()
			.As<IWorkSpacePage>()
			.SingleInstance();

		builder
			.RegisterType<EmployeesPageView>()
			.WithViewModel()
			.OfType<EmployeesPageViewModel>()
			.SingleInstance();

		builder
			.RegisterType<StudentsPageViewModel>()
			.AsSelf()
			.As<IWorkSpacePage>()
			.SingleInstance();

		builder
			.RegisterType<StudentsPageView>()
			.WithViewModel()
			.OfType<StudentsPageViewModel>()
			.SingleInstance();

		builder
			.RegisterType<ClassesPageViewModel>()
			.AsSelf()
			.As<IWorkSpacePage>()
			.SingleInstance();

		builder
			.RegisterType<ClassesPageView>()
			.WithViewModel()
			.OfType<ClassesPageViewModel>()
			.SingleInstance();

		builder
			.RegisterType<SchedulePageViewModel>()
			.AsSelf()
			.As<IWorkSpacePage>()
			.SingleInstance();

		builder
			.RegisterType<SchedulePageView>()
			.WithViewModel()
			.OfType<SchedulePageViewModel>()
			.SingleInstance();

		builder
			.RegisterType<CurriculumPageViewModel>()
			.AsSelf()
			.As<IWorkSpacePage>()
			.SingleInstance();

		builder
			.RegisterType<CurriculumPageView>()
			.WithViewModel()
			.OfType<CurriculumPageViewModel>()
			.SingleInstance();

		builder
			.RegisterType<JournalPageViewModel>()
			.AsSelf()
			.As<IWorkSpacePage>()
			.SingleInstance();

		builder
			.RegisterType<JournalPageView>()
			.WithViewModel()
			.OfType<JournalPageViewModel>()
			.SingleInstance();

		builder
			.RegisterType<FinancePageViewModel>()
			.AsSelf()
			.As<IWorkSpacePage>()
			.SingleInstance();

		builder
			.RegisterType<FinancePageView>()
			.WithViewModel()
			.OfType<FinancePageViewModel>()
			.SingleInstance();

		builder
			.RegisterType<CommandsPageViewModel>()
			.AsSelf()
			.As<IWorkSpacePage>()
			.SingleInstance();

		builder
			.RegisterType<CommandsPageView>()
			.WithViewModel()
			.OfType<CommandsPageViewModel>()
			.SingleInstance();

		builder
			.RegisterType<MessagesPageViewModel>()
			.AsSelf()
			.As<IWorkSpacePage>()
			.SingleInstance();

		builder
			.RegisterType<MessagesPageView>()
			.WithViewModel()
			.OfType<MessagesPageViewModel>()
			.SingleInstance();

		builder
			.RegisterType<ReportsPageViewModel>()
			.AsSelf()
			.As<IWorkSpacePage>()
			.SingleInstance();

		builder
			.RegisterType<ReportsPageView>()
			.WithViewModel()
			.OfType<ReportsPageViewModel>()
			.SingleInstance();

		builder
			.RegisterType<AdministrationPageViewModel>()
			.AsSelf()
			.As<IWorkSpacePage>()
			.SingleInstance();

		builder
			.RegisterType<AdministrationPageView>()
			.WithViewModel()
			.OfType<AdministrationPageViewModel>()
			.SingleInstance();
	}
}
