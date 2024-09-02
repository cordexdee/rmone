

alter table Opportunity drop constraint DF__Opportuni__Oppor__3732A735 

ALTER TABLE Opportunity
ADD CONSTRAINT df_OpportunityTargetChoice
DEFAULT 'Project' FOR OpportunityTargetChoice; 

update Opportunity set OpportunityTargetChoice = 'Project' where OpportunityTargetChoice <> 'Project' or OpportunityTargetChoice is null