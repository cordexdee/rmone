ALTER TABLE ResourceAllocationMonthly
ADD attachments NVARCHAR (2000) DEFAULT ('') NULL;

ALTER TABLE ResourceAllocationMonthly
ADD Deleted BIT             DEFAULT ((0)) NULL;

ALTER TABLE ResourceUsageSummaryMonthWise
ADD attachments NVARCHAR (2000) DEFAULT ('') NULL;

ALTER TABLE ResourceUsageSummaryMonthWise
ADD Deleted BIT             DEFAULT ((0)) NULL;

ALTER TABLE ResourceUsageSummaryWeekWise
ADD attachments NVARCHAR (2000) DEFAULT ('') NULL;

ALTER TABLE ResourceUsageSummaryWeekWise
ADD Deleted BIT             DEFAULT ((0)) NULL;

update ResourceAllocationMonthly set Deleted=0
update ResourceUsageSummaryMonthWise set Deleted=0
update ResourceUsageSummaryWeekWise set Deleted=0
