namespace MTGView.Core.Enums;

/// <summary>
/// The Severity of a given incident
/// </summary>
public enum Severity
{
    ///<value>
    /// Simple incident to resolve
    /// </value>
    Minor,
    ///<value>
    /// Required a minor level of elevation
    /// </value>
    BlackShirt,
    ///<value>
    /// Required elevation to the team's captain
    /// </value>
    Captain,
    ///<value>
    /// Required elevation to the administrators
    /// </value>
    Admin,
    ///<value>
    /// Required elevation to the Assistant Department Head
    /// </value>
    Adh,
    ///<value>
    /// Required elevation to the department head
    /// </value>
    Dh,
    ///<value>
    /// Required Police Interaction or a Police report
    /// </value>
    Police,
    ///<value>
    /// Incident was Resolved
    /// </value>
    Resolved
}