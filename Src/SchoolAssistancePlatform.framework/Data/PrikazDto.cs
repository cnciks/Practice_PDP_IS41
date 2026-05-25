namespace SchoolAssistancePlatform.framework.Data;

public class PrikazDto
{
	public int PrikazID { get; set; }

	public string NomerPrikaza { get; set; }

	public DateTime DataPrikaza { get; set; }

	public string TipPrikaza { get; set; }

	public string Soderzhanie { get; set; }

	public string FileLink { get; set; }

	public long SotrudnikID { get; set; }

	public SotrudnikDto Sotrudnik { get; set; }
}
