
ALTER TABLE ResourceUsageSummaryMonthWise
DROP CONSTRAINT DF__ResourceU__Attac__5E169ED5,CONSTRAINT DF__ResourceU__Delet__5D227A9C;

ALTER TABLE ResourceUsageSummaryMonthWise
DROP COLUMN attachments, COLUMN deleted;

ALTER TABLE ResourceAllocationMonthly
DROP CONSTRAINT DF__ResourceA__Attac__1AEAA2EB, CONSTRAINT DF__ResourceA__Delet__19F67EB2

ALTER TABLE ResourceAllocationMonthly
DROP COLUMN attachments, COLUMN deleted;

ALTER TABLE ResourceUsageSummaryWeekWise
ALTER COLUMN ManagerLookup nvarchar(128);

ALTER TABLE ResourceUsageSummaryWeekWise
ALTER COLUMN ResourceUser nvarchar(128);

ALTER TABLE ResourceUsageSummaryMonthWise
ALTER COLUMN ResourceUser nvarchar(128);

ALTER TABLE ResourceUsageSummaryMonthWise
ALTER COLUMN ManagerLookup nvarchar(128);