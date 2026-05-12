using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolAssistancePlatform.Base.Entity;

[Table("Sample", Schema = "School")]
public class SampleEntity
{
	/// <summary> Идентификатор. </summary>
	[Key]
	public long Id { get; set; }


}
