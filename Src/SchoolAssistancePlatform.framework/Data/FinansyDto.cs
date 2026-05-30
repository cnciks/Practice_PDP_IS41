namespace SchoolAssistancePlatform.framework.Data;

public class FinansyDto
{
	public long PlatezhID { get; set; }

	public long UchenikID { get; set; }

	public string FIOUchenika { get; set; }

	public string NomerKlassa { get; set; }

	public DateTime DataPlatezha { get; set; }

	public decimal Summa { get; set; }

	public string Naznachenie { get; set; }

	public string TipOperacii { get; set; }
}
