namespace Generator.XmlClasses.Entities.Gradebook.xml;

public interface IGradebookXmlGradeItem
{

		string CategoryId { get; set; }
		
		string ItemName { get; set; }
		
		string ItemType { get; set; }
		
		string ItemModule { get; set; }
		
		string ItemInstance { get; set; }
		
		string ItemNumber { get; set; }
		
		string ItemInfo { get; set; }
		
		string IdNumber { get; set; }
		
		string Calculation { get; set; }
		
		string GradeType { get; set; }
		
		string Grademax { get; set; }
		
		string Grademin { get; set; }
		
		string ScaleId { get; set; }
		
		string OutcomeId { get; set; }
		
		string Gradepass { get; set; }
		
		string Multfactor { get; set; }
		
		string Plusfactor { get; set; }
		
		string Aggregationcoef { get; set; }
		
		string Aggregationcoef2 { get; set; }
		
		string Weightoverride { get; set; }
		
		string Sortorder { get; set; }
		
		string Display { get; set; }
		
		string Decimals { get; set; }
		
		string Hidden { get; set; }
		
		string Locked { get; set; }
		
		string Locktime { get; set; }
		
		string Needsupdate { get; set; }
		
		string Timecreated { get; set; }
		
		string Timemodified { get; set; }

		string GradeGrades { get; set; }

		string Id { get; set; }
    
}