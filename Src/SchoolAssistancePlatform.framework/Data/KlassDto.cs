namespace SchoolAssistancePlatform.framework.Data;

public class KlassDto
{
	public long KlassID { get; set; }

	public string NomerKlassa { get; set; }

	public int GodObucheniya { get; set; }

	public long KlassRukovoditelID { get; set; }

	public string KlassRukovoditelFIO { get; set; }

	public long PlanID { get; set; }

	public string PlanNazvanie { get; set; }
}
