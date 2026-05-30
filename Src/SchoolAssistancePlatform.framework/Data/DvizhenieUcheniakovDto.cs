namespace SchoolAssistancePlatform.framework.Data;

public class DvizhenieUcheniakovDto
{
	public long DvizhenieID { get; set; }

	public long UchenikID { get; set; }

	public string FIOUchenika { get; set; }

	public string NomerKlassa { get; set; }

	public DateTime DataIzmeneniya { get; set; }

	public string TipDvizheniya { get; set; }

	public string Osnovanie { get; set; }

	public string Kommentariy { get; set; }
}
