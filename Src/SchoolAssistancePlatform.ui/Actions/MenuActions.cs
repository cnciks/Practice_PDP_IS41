using System;

namespace SchoolAssistancePlatform.UI.Actions;

internal class MenuActions
{
	public event EventHandler<string>? ChangeMenu;

	public void SelectMenuByName(string name)
	{
		ChangeMenu?.Invoke(this, name);
	}
}
