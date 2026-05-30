namespace SchoolAssistancePlatform.framework.Data;

public class RaspisanieDto
{
	public long RaspisanieID { get; set; }

	public long KlassID { get; set; }

	public string NomerKlassa { get; set; }

	public long PredmetID { get; set; }

	public string NazvaniePredmeta { get; set; }

	public string SokrasheniePredmeta { get; set; }

	public long SotrudnikID { get; set; }

	public string FIOPrepodaatelya { get; set; }

	public long DenNedeli { get; set; }

	public short NomerUroka { get; set; }

	public string Kabinet { get; set; }
}
