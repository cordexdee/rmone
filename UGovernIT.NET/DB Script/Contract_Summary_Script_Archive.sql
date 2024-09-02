ALTER TABLE CRMProject_Archive
ADD Payments  varchar(max), SubContractorMarkUp  varchar(max), DisputeResolution varchar(max),
CertifyingAgency varchar(max), Insurance varchar(max), Retainage varchar(100),LiquidatedDamages varchar(max), Savings varchar(max),DiverseCertificationChoice varchar(100)

ALTER TABLE CRMProject_Archive
ADD GeneralConditionsDelay BIT, WaiverDamages BIT, LienWaiver BIT, BuilderRisk BIT, WaiverSubrogation BIT, DisputedWorkCap BIT, 
SubcontractorDefaultInsurance BIT, PaymentAndPerformanceBonds BIT, ExecutiveOrderRequirements BIT, Bonus BIT

ALTER TABLE Opportunity_Archive
ADD Payments  varchar(max), SubContractorMarkUp  varchar(max), DisputeResolution varchar(max),
CertifyingAgency varchar(max), Retainage varchar(100),ApprovedChangeOrders varchar(max), LiquidatedDamages varchar(max), Savings varchar(max),DiverseCertificationChoice varchar(100)

ALTER TABLE Opportunity_Archive
ADD GeneralConditionsDelay BIT, WaiverDamages BIT, LienWaiver BIT, BuilderRisk BIT, WaiverSubrogation BIT, DisputedWorkCap BIT, 
SubcontractorDefaultInsurance BIT, PaymentAndPerformanceBonds BIT, ExecutiveOrderRequirements BIT, Bonus BIT
