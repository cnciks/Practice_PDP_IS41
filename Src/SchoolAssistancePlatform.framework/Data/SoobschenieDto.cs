namespace SchoolAssistancePlatform.framework.Data;

public class SoobschenieDto
{
	public int SoobschenieID { get; set; }

	public long OtpravitelID { get; set; }

	public string OtpravitelFIO { get; set; }

	public long PoluchatelID { get; set; }

	public string PoluchatelFIO { get; set; }

	public string TipPoluchatelya { get; set; }

	public string Tema { get; set; }

	public string Text { get; set; }

	public DateTime DataOtpravki { get; set; }

	public bool Prochitano { get; set; }
}
