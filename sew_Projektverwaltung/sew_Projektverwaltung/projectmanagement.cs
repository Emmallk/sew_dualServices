namespace projektverwaltung;

public enum EProjectState
{
    CREATED,
    IN_APPROVEMENT,
    CANCLED,
    APPROVED
}
public enum ELawType
{
    P_27,
    P_28,
    P_29
}
public class Facility
{
    public string FacilityName { get; set; }
}
public abstract class AProject
{
    public Facility Facility { get; set; }
    public String Title { get; set; }
    public String Description { get; set; }
    public EProjectState ProjectState { get; set; }
}
public class ManagementProject : AProject
{
    public ELawType LawType { get; set; }
}

public class RequestFundingProject : AProject
{
    public Boolean IsFWFFunded { get; set; }
    public Boolean IsFFGFunded { get; set; }
    public Boolean IsEUFunded { get; set; }
}