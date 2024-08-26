
using Agent_Management_Server.models;

public enum status_enum_agent 
{
    Dormant,
    busy,
    Active
}
public enum status_enum_target
{    
    Alive,
    eliminated,
    busy
}
public enum status_enum_direction
{
     
     n,
    ne,
    nw,
    w,
    e,
    s,
    sw,
    se

}
public enum status_enum_mission
{
    Active,
    Waiting_for_the_command,
    false_
}

