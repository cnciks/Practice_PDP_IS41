using System;

namespace SchoolAssistancePlatform.UI.Views.WorkSpace;

internal class Sotrudnik
{
	public long SotrudnikID { get; set; }

	public string Familia { get; set; }

	public string Imya { get; set; }

	public string Otchestvo { get; set; }

	public DateTime DataRozhdeniya { get; set; }

	public string Dolzhnost { get; set; }

	public string Email { get; set; }

	public string Telefon { get; set; }

	public DateTime DataPriema { get; set; }

	public string Status { get; set; }
}
