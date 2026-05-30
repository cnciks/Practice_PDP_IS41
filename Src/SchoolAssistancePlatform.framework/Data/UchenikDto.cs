namespace SchoolAssistancePlatform.framework.Data;

public class UchenikDto
{
	public long UchenikID { get; set; }

	public string Familiia { get; set; }

	public string Imya { get; set; }

	public string Otchestvo { get; set; }

	public DateTime DataRozhdeniya { get; set; }

	public string AdresProzhivaniya { get; set; }

	public string FIORoditeley { get; set; }

	public string TelefonRoditelya { get; set; }

	public DateTime DataZachisleniya { get; set; }

	public long KlassID { get; set; }

	public string NomerKlassa { get; set; }

	public int GodObucheniya { get; set; }
}
