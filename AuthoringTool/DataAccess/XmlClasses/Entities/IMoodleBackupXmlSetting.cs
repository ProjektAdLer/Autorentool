namespace AuthoringTool.DataAccess.XmlClasses;

public interface IMoodleBackupXmlSetting
{
	public void SetParametersSetting(string? level, string? name, string? value);

	public void SetParametersSection(string? level, string? section, string? name, string? value);

	public void SetParametersActivity(string? level, string? activity, string? name, string? value);
}