using Mapster;

using SchoolAssistancePlatform.Base.Entity.Auth;
using SchoolAssistancePlatform.Base.Entity.School;
using SchoolAssistancePlatform.framework;
using SchoolAssistancePlatform.framework.Data;

namespace SchoolAssistancePlatform.Base;

public static class MapsterConfig
{
	public static void Configure()
	{
		TypeAdapterConfig<UserEntity, UserDto>
			.NewConfig()			
			.PreserveReference(true);

		TypeAdapterConfig<RoleEntity, RoleDto>.NewConfig()
			.Map(dest => dest.Permissions, src =>
				src.Permissions.Select(p => (Permissions)p.PermissionValue).ToList());

		TypeAdapterConfig<RoleDto, RoleEntity>.NewConfig()
			.Map(dest => dest.Permissions, src =>
				src.Permissions.Select(perm => new RolePermissionEntity
				{
					PermissionValue = (long)perm
				}).ToList());

		TypeAdapterConfig<SotrudnikEntity, SotrudnikDto>
			.NewConfig()			
			.PreserveReference(true);

		TypeAdapterConfig<UchebniyPlanEntity, UchebniyPlanDto>
			.NewConfig()			
			.PreserveReference(true);

		TypeAdapterConfig<KlassEntity, KlassDto>
			.NewConfig()
			.Map(dest => dest.KlassRukovoditelFIO, src => src.Sotrudnik != null ? src.Sotrudnik.Familia + " " + src.Sotrudnik.Imya + " " + src.Sotrudnik.Otchestvo : null)
			.Map(dest => dest.PlanNazvanie, src => src.UchebniyPlan != null ? src.UchebniyPlan.Nazvanie : null);

		TypeAdapterConfig<UchenikEntity, UchenikDto>
			.NewConfig()
			
			.PreserveReference(true)
			.Map(dest => dest.NomerKlassa, src => src.Klass != null ? src.Klass.NomerKlassa : null)
			.Map(dest => dest.GodObucheniya, src => src.Klass != null ? src.Klass.GodObucheniya : 0);

		TypeAdapterConfig<UchebniyPredmetEntity, UchebniyPredmetDto>
			.NewConfig()			
			.PreserveReference(true);

		TypeAdapterConfig<RaspisanieEntity, RaspisanieDto>
			.NewConfig()
			
			.PreserveReference(true)
			.Map(dest => dest.NomerKlassa, src => src.Klass != null ? src.Klass.NomerKlassa : null)
			.Map(dest => dest.NazvaniePredmeta, src => src.UchebniyPredmet != null ? src.UchebniyPredmet.Nazvanie : null)
			.Map(dest => dest.SokrasheniePredmeta, src => src.UchebniyPredmet != null ? src.UchebniyPredmet.Sokrashenie : null)
			.Map(dest => dest.FIOPrepodaatelya, src => src.Sotrudnik != null ? $"{src.Sotrudnik.Familia} {src.Sotrudnik.Imya} {src.Sotrudnik.Otchestvo}".Trim() : null);

		TypeAdapterConfig<ZhurnalUspevaemostiEntity, ZhurnalUspevaemostiDto>
			.NewConfig()
			
			.PreserveReference(true)
			.Map(dest => dest.NomerKlassa, src => src.Raspisanie != null && src.Raspisanie.Klass != null ? src.Raspisanie.Klass.NomerKlassa : null)
			.Map(dest => dest.NazvaniePredmeta, src => src.Raspisanie != null && src.Raspisanie.UchebniyPredmet != null ? src.Raspisanie.UchebniyPredmet.Nazvanie : null)
			.Map(dest => dest.SokrasheniePredmeta, src => src.Raspisanie != null && src.Raspisanie.UchebniyPredmet != null ? src.Raspisanie.UchebniyPredmet.Sokrashenie : null)
			.Map(dest => dest.FIOUchenika, src => src.Uchenik != null ? $"{src.Uchenik.Familiia} {src.Uchenik.Imya} {src.Uchenik.Otchestvo}".Trim() : null);

		TypeAdapterConfig<SoobschenieEntity, SoobschenieDto>
			.NewConfig()
			
			.PreserveReference(true)
			.Map(dest => dest.OtpravitelFIO, src => src.Otpravitel != null ? $"{src.Otpravitel.Familia} {src.Otpravitel.Imya} {src.Otpravitel.Otchestvo}".Trim() : null)
			.Map(dest => dest.PoluchatelFIO, src => src.Poluchatel != null ? $"{src.Poluchatel.Familia} {src.Poluchatel.Imya} {src.Poluchatel.Otchestvo}".Trim() : null);

		TypeAdapterConfig<PrikazEntity, PrikazDto>
			.NewConfig()
			
			.PreserveReference(true)
			.Map(dest => dest.SotrudnikFIO, src => src.Sotrudnik != null ? $"{src.Sotrudnik.Familia} {src.Sotrudnik.Imya} {src.Sotrudnik.Otchestvo}".Trim() : null);

		TypeAdapterConfig<MatTehBazaEntity, MatTehBazaDto>
			.NewConfig()
			
			.PreserveReference(true)
			.Map(dest => dest.NomerPrikazaPostupleniya, src => src.PrikazPostupleniya != null ? src.PrikazPostupleniya.NomerPrikaza : null)
			.Map(dest => dest.NomerPrikazaSpisaniya, src => src.PrikazSpisaniya != null ? src.PrikazSpisaniya.NomerPrikaza : null);

		TypeAdapterConfig<FinansyEntity, FinansyDto>
			.NewConfig()
			
			.PreserveReference(true)
			.Map(dest => dest.FIOUchenika, src => src.Uchenik != null ? $"{src.Uchenik.Familiia} {src.Uchenik.Imya} {src.Uchenik.Otchestvo}".Trim() : null)
			.Map(dest => dest.NomerKlassa, src => src.Uchenik != null && src.Uchenik.Klass != null ? src.Uchenik.Klass.NomerKlassa : null);

		TypeAdapterConfig<DvizhenieUcheniakovEntity, DvizhenieUcheniakovDto>
			.NewConfig()
			
			.PreserveReference(true)
			.Map(dest => dest.FIOUchenika, src => src.Uchenik != null ? $"{src.Uchenik.Familiia} {src.Uchenik.Imya} {src.Uchenik.Otchestvo}".Trim() : null)
			.Map(dest => dest.NomerKlassa, src => src.Uchenik != null && src.Uchenik.Klass != null ? src.Uchenik.Klass.NomerKlassa : null);
	}
}
