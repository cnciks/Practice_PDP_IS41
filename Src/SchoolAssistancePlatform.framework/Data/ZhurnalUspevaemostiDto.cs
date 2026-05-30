namespace SchoolAssistancePlatform.framework.Data;

public class ZhurnalUspevaemostiDto
{
	public long ZapisID { get; set; }

	public long RaspisanieID { get; set; }

	public string NomerKlassa { get; set; }

	public string NazvaniePredmeta { get; set; }

	public string SokrasheniePredmeta { get; set; }

	public long UchenikID { get; set; }

	public string FIOUchenika { get; set; }

	public DateTime DataUroka { get; set; }

	public int Ocenka { get; set; }

	public string TipOcenki { get; set; }

	public bool Poseschaemost { get; set; }

	public string PrichinaOtssutstviya { get; set; }
}
