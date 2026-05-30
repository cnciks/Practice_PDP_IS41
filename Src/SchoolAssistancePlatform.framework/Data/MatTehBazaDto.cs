namespace SchoolAssistancePlatform.framework.Data;

public class MatTehBazaDto
{
	public int InventarID { get; set; }

	public string Naimenovanie { get; set; }

	public string Tip { get; set; }

	public string Kabinet { get; set; }

	public string InvNomer { get; set; }

	public string Status { get; set; }

	public int PrikazPostupleniyaID { get; set; }

	public string NomerPrikazaPostupleniya { get; set; }

	public int? PrikazSpisaniyaID { get; set; }

	public string NomerPrikazaSpisaniya { get; set; }
}
